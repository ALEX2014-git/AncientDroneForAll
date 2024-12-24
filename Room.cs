using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using MoreSlugcats;
using RWCustom;
using UnityEngine;

namespace AncientDroneForAll
{

    public partial class AncientDroneForAll
    {
        public Room myRoom;
        private void Room_Loaded(On.Room.orig_Loaded orig, Room self)
        {
            orig(self);
            if (self != null) myRoom = self;
            if (self.abstractRoom == null || self.game == null) return;
            if (self.abstractRoom.name == "UW_H01" && self.game.IsStorySession && !self.game.GetStorySession.saveState.hasRobo)
            {
                Logger.LogInfo("Loading Five Pebbles' rooftop. Attempting to create Custrom Drone Cutscene object. (Yellow/White)");
                
                if (myPlayer.slugcatStats.name == SlugcatStats.Name.White || myPlayer.slugcatStats.name == SlugcatStats.Name.Yellow)
                self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(650f, 192f), (myPlayer.slugcatStats.name == SlugcatStats.Name.White) ? new Color(1f, 1f, 1f) : new Color(1f, 0.92f, 0.016f), false));
            }
            if (self.abstractRoom.name == "GW_A11" && self.game.IsStorySession && !self.game.GetStorySession.saveState.hasRobo)
            {
                Logger.LogInfo("Loading Garbage Wastes' popcorn shelter. Attempting to create Custrom Drone Cutscene object. (Red)");

                if (myPlayer.slugcatStats.name == SlugcatStats.Name.Red)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(678f, 455f), new Color(1f, 0f, 0f), false));
            }
            if (self.abstractRoom.name == "VS_C03" && self.game.IsStorySession && !self.game.GetStorySession.saveState.hasRobo)
            {
                Logger.LogInfo("Loading Pipeyard's flood room. Attempting to create Custrom Drone Cutscene object. (Gourmand)");

                if (myPlayer.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(810f, 376f), new Color(1f, 1f, 0f), true));
            }
        }

        public Player myPlayer
        {
            get
            {
                AbstractCreature firstAlivePlayer = myRoom.game.FirstAlivePlayer;
                if (myRoom.game.Players.Count > 0 && firstAlivePlayer != null && firstAlivePlayer.realizedCreature != null)
                {
                    return firstAlivePlayer.realizedCreature as Player;
                }
                return null;
            }
        }

    }
}
