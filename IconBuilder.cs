/**
 * 
 * Some functionailty lifted from Lifetsoned
 * https://gitlab.com/Scribble/lifestoned/
 * 
 * Which I may have written initially. I can't remember anymore.
 * 
 **/
using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using ACE.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Effects;
using WeenieIconBuilder.Db;

namespace WeenieIconBuilder
{
    public class IconBuilder
    {
        private const uint ITEM_TYPE_MAP = 0x25000008;
        private const uint ITEM_EFFECT_MAP = 0x25000009;

        public static Bitmap BuildIcon(IconData iconData)
        {
            List<Bitmap> images = new List<Bitmap>();
            images.Add(GetImage(iconData.IconUnderlay));

            if (iconData.Icon > 0)
            {
                var icon = GetImage(iconData.Icon);
                if (iconData.UiEffect > 0)
                    images.Add(BlendEffect(GetImage(iconData.Icon), GetImage(iconData.UiEffect)));
                else
                    images.Add(icon);
            }
            if (iconData.IconOverlay > 0)
                images.Add(GetImage(iconData.IconOverlay));
            if (iconData.IconOverlay2 > 0)
                images.Add(GetImage(iconData.IconOverlay2));

            return CombineImageList(images);
        }

        private static Bitmap GetImage(int imageId)
        {
            if (DatManager.PortalDat.AllFiles.ContainsKey((uint)imageId))
            {
                var image = DatManager.PortalDat.ReadFromDat<Texture>((uint)imageId);
                return image.GetBitmap();
            }
            else
            {
                // return blank image
                var image = new Bitmap(32, 32);
                return image;
            }
        }

        private static Bitmap CombineImageList(List<Bitmap> imageBuffer)
        {
            Bitmap iconImage = new Bitmap(32, 32, PixelFormat.Format32bppRgb);
            // iconImage.MakeTransparent();
            using (Graphics g = Graphics.FromImage(iconImage))
            {
                Brush effectBrush = new SolidBrush(Color.Black);
                // Allow composting ontop?
                g.CompositingMode = CompositingMode.SourceOver;
                foreach (Bitmap image in imageBuffer)
                {
                    // image.MakeTransparent(Color.Transparent);
                    // add the bitmap
                    g.DrawImage(image, 0, 0);
                }
                return iconImage;
            }
        }

        private static Bitmap BlendEffect(Bitmap baseImage, Bitmap effect)
        {
            Bitmap iconImage = new Bitmap(32, 32, PixelFormat.Format32bppPArgb);

            iconImage.MakeTransparent();

            for (int y = 0; y < baseImage.Height; y++)
            {
                for (int x = 0; x < baseImage.Width; x++)
                {
                    var pixel = baseImage.GetPixel(x, y);

                    if (pixel.A > 0)
                    {
                        iconImage.SetPixel(x, y, pixel);
                    }

                    if (pixel.A == 255 && pixel.R == 255 && pixel.G == 255 && pixel.B == 255)
                    {
                        iconImage.SetPixel(x, y, effect.GetPixel(x, y));
                    }
                }
            }

            baseImage.Dispose();
            effect.Dispose();

            return iconImage;
        }

    }
}
