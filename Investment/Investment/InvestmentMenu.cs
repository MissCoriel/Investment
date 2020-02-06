using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;

namespace Investment
{
    class InvestmentMenu : IClickableMenu
    {
        public InvestmentMenu(IDataHelper helper, string modDirectory) : base((int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport.Width, Game1.viewport.Height).X, (int)Utility.getTopLeftPositionForCenteringOnScreen(Game1.viewport.Width, Game1.viewport.Height).X, Game1.viewport.Width, Game1.viewport.Height, true)
        {

        }

    }
}
