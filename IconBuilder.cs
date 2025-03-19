using ACE.DatLoader;
using ACE.DatLoader.FileTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeenieIconBuilder.Db;

namespace WeenieIconBuilder
{
    public class IconBuilder
    {
        public IconBuilder() {

        }
        
        public static bool BuildIcon(string filename, IconData iconData)
        {
            if(iconData.Icon > 0)
            {
                var icon = DatManager.PortalDat.ReadFromDat<Texture>((uint)iconData.Icon);
            }

            // var item = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)ClothingBase);

            return true;
        }
    }
}
