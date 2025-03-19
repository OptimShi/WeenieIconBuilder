using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeenieIconBuilder.Enums;
namespace WeenieIconBuilder.Db
{
    public class IconData
    {
        public int Icon;            // ICON_DID
        public int IconUnderlay;    // ICON_UNDERLAY_DID
        public int IconOverlay;     // ICON_OVERLAY_DID
        public int IconOverlay2;    // ICON_OVERLAY_SECONDARY_DID
        public int UiEffect;        // UI_EFFECTS_INT
        public int ItemType = 0;    // ITEM_TYPE_INT

        public int ClothingBase;    // CLOTHINGBASE_DID
        public int PaletteTemplate; // PALETTE_TEMPLATE_INT
        
        public bool IgnoreClothingBase = false; // IGNORE_CLO_ICONS_BOOL

        public static IconData GenerateFromWeenie(dbWeenie weenie)
        {
            var data = new IconData();

            if (weenie.DIDs.ContainsKey(PropertyDID.ICON_DID))
                data.Icon = weenie.DIDs[PropertyDID.ICON_DID];

            if (weenie.Ints.ContainsKey(PropertyInt.ITEM_TYPE_INT))
                data.ItemType = weenie.Ints[PropertyInt.ITEM_TYPE_INT];

            if (weenie.DIDs.ContainsKey(PropertyDID.ICON_UNDERLAY_DID))
                data.IconUnderlay = weenie.DIDs[PropertyDID.ICON_UNDERLAY_DID];
            else
                data.IconUnderlay = GetIconUnderlayFromItemType(data.ItemType);

            if (weenie.DIDs.ContainsKey(PropertyDID.ICON_OVERLAY_DID))
                data.IconOverlay = weenie.DIDs[PropertyDID.ICON_OVERLAY_DID];

            if (weenie.DIDs.ContainsKey(PropertyDID.ICON_OVERLAY_SECONDARY_DID))
                data.IconOverlay2 = weenie.DIDs[PropertyDID.ICON_OVERLAY_SECONDARY_DID];

            if (weenie.DIDs.ContainsKey(PropertyDID.CLOTHINGBASE_DID))
                data.ClothingBase = weenie.DIDs[PropertyDID.CLOTHINGBASE_DID];

            if (weenie.Ints.ContainsKey(PropertyInt.UI_EFFECTS_INT))
                data.UiEffect = weenie.Ints[PropertyInt.UI_EFFECTS_INT];

            if (weenie.Ints.ContainsKey(PropertyInt.PALETTE_TEMPLATE_INT))
                data.UiEffect = weenie.Ints[PropertyInt.PALETTE_TEMPLATE_INT];

            if (weenie.Bools.ContainsKey(PropertyBool.IGNORE_CLO_ICONS_BOOL))
                data.IgnoreClothingBase = weenie.Bools[PropertyBool.IGNORE_CLO_ICONS_BOOL];

            return data;

        }


        // Defined in 25000008
        public static int GetIconUnderlayFromItemType(int itemType)
        {
            switch (itemType)
            {
                case 1: return 0x060011cb; // MeleeWeapon
                case 2: return 0x060011cf; // Armor
                case 4: return 0x060011f3; // Clothing
                case 8: return 0x060011d5; // Jewelry
                case 16: return 0x060011d1; // Creature
                case 32: return 0x060011cc; // Food
                case 64: return 0x060011f4; // Money
                case 128: return 0x060011d4; // Misc
                case 256: return 0x060011d2; // MissileWeapon
                case 512: return 0x060011ce; // Container
                case 1024: return 0x060011d0; // Useless
                case 2048: return 0x060011d3; // Gem
                case 4096: return 0x060011cd; // SpellComponents
                case 1048576: return 0x06005e23; // Service
            }
            return 0x060011d4;
        }
    }
}
