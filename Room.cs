using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using MoreSlugcats;

namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {
        private void Room_Loaded(On.Room.orig_Loaded orig, Room self)
        {
            orig(self);
            if (self.abstractRoom.name == "UW_H01" && self.game.IsStorySession && !self.game.GetStorySession.saveState.hasRobo)
            {
                Logger.LogInfo("Loading Five Pebbles' rooftop. Attempting to create Custrom Drone Cutscene object");
                self.AddObject(new CustomCutsceneArtificerRobo(self));
            }
        }

    }
}
