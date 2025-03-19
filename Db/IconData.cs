using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.DatLoader.FileTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeenieIconBuilder.Enums;
namespace WeenieIconBuilder.Db
{
    public class IconData
    {
        private const int DEFAULT_ICON_UNDERLAY = 0x060011D4;
        private const int DEFAULT_UI_EFFECT = 0x060011C5;

        // These are all references to a texture (0x06000000)
        public int Icon;            // ICON_DID
        public int IconUnderlay;    // ICON_UNDERLAY_DID
        public int IconOverlay;     // ICON_OVERLAY_DID
        public int IconOverlay2;    // ICON_OVERLAY_SECONDARY_DID
        public int UiEffect;        // UI_EFFECTS_INT

        private int ItemType = 0;    // ITEM_TYPE_INT

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

            if (weenie.Ints.ContainsKey(PropertyInt.UI_EFFECTS_INT))
                data.UiEffect = GetUiEffect(weenie.Ints[PropertyInt.UI_EFFECTS_INT]);
            else
                data.UiEffect = DEFAULT_UI_EFFECT;

            bool ignoreClothingBase = false;
            if (weenie.Bools.ContainsKey(PropertyBool.IGNORE_CLO_ICONS_BOOL))
                ignoreClothingBase = weenie.Bools[PropertyBool.IGNORE_CLO_ICONS_BOOL];

            // Get the icon from the ClothingBase...
            if(ignoreClothingBase != true)
            {
                if (weenie.DIDs.ContainsKey(PropertyDID.CLOTHINGBASE_DID))
                {
                    int clothingBase = weenie.DIDs[PropertyDID.CLOTHINGBASE_DID];
                    uint palTemplateInt = 0;
                    if (weenie.Ints.ContainsKey(PropertyInt.PALETTE_TEMPLATE_INT))
                    {
                        palTemplateInt = (uint)weenie.Ints[PropertyInt.PALETTE_TEMPLATE_INT];
                    }

                    var cb = DatManager.PortalDat.ReadFromDat<ClothingTable>((uint)clothingBase);
                    CloSubPalEffect? subPal = null;
                    if (palTemplateInt != 0 && cb.ClothingSubPalEffects.ContainsKey(palTemplateInt))
                        subPal = cb.ClothingSubPalEffects[palTemplateInt];
                    else if(cb.ClothingSubPalEffects.Count > 0)
                       subPal = cb.ClothingSubPalEffects.First().Value;

                    if (subPal != null && subPal.Icon > 0)
                        data.Icon = (int)subPal.Icon;
                }
            }


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
            return DEFAULT_ICON_UNDERLAY;
        }

        private static int GetUiEffect(int UiEffect)
        {
            // Lookup the UIEffect ID from the 0x25000009 DIDMapper in the client_portal.dat file
            // Hard code for simplicity

            switch (UiEffect)
            {
                case 1: 
                    return 0x060011CA; // Magical
                case 2:
                    return 0x060011C6; // Poisoned
                case 4:
                    return 0x06001B05; // BoostHealth
                case 8:
                    return 0x060011CA; // BoostMana
                case 16:
                    return 0x06001B06; // BoostStamina
                case 32:
                    return 0x06001B2E; // FIRE
                case 64:
                    return 0x06001B2D; // LIGHTNING
                case 128:
                    return 0x06001B2F; // FROST
                case 256:
                    return 0x06001B2C; // Acid
                case 512:
                    return 0x060033C2; // SLASHING
                case 1024:
                    return 0x060033C3; // BLUDGEONING
                case 2048:
                    return 0x060033C4; // PIERCING

            }

            return DEFAULT_UI_EFFECT;
        }

    }
}
