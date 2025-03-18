using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeenieIconBuilder.Db
{
    class IconData
    {
        uint Icon;            // ICON_DID
        uint IconUnderlay;    // ICON_UNDERLAY_DID
        uint IconOverlay;     // ICON_OVERLAY_DID
        uint IconOverlay2;    // ICON_OVERLAY_SECONDARY_DID
        uint ClothingBase;    // CLOTHINGBASE_DID
        uint UiEffect;        // UI_EFFECTS_INT
        uint ItemType;        // ITEM_TYPE_INT
        uint PaletteTemplate; // PALETTE_TEMPLATE_INT
    }
}
