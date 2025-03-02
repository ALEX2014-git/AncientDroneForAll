using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx.Logging;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using CoralBrain;
using Noise;

namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {
        Room myRoom;
        private void Room_Loaded(On.Room.orig_Loaded orig, Room self)
        {
            orig(self);
            if (AncientDroneForAll.hasDrone) return; //Preventing drone object placement logic if hasDrone
            if (self != null) myRoom = self;
            if (self.abstractRoom == null || self.game == null) return;
            if (self.abstractRoom.name == "UW_H01" && self.game.IsStorySession)
            {
                Logger.LogInfo("Loading Five Pebbles' rooftop. Attempting to create Custrom Drone Cutscene object. (White)");
                
                if (myPlayer.room.game.StoryCharacter == SlugcatStats.Name.White)
                self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(650f, 192f), new Color(1f, 1f, 1f), false));
            }
            if (self.abstractRoom.name == "SI_A21" && self.game.IsStorySession)
            {
                Logger.LogInfo("Loading Sky Islands Svac Merch Shop room. Attempting to create Custrom Drone Cutscene object. (Yellow)");

                if (myPlayer.room.game.StoryCharacter == SlugcatStats.Name.Yellow)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(490f, 267f), new Color(1f, 0.92f, 0.016f), false));
            }
            if (self.abstractRoom.name == "GW_A11" && self.game.IsStorySession)
            {
                Logger.LogInfo("Loading Garbage Wastes' popcorn shelter. Attempting to create Custrom Drone Cutscene object. (Red)");

                if (myPlayer.room.game.StoryCharacter == SlugcatStats.Name.Red)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(678f, 455f), new Color(1f, 0f, 0f), false));
            }
            if (self.abstractRoom.name == "VS_C03" && self.game.IsStorySession)
            {
                Logger.LogInfo("Loading Pipeyard's flood room. Attempting to create Custrom Drone Cutscene object. (Gourmand)");

                if (myPlayer.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(810f, 376f), new Color(1f, 1f, 0f), true));
            }
            if (self.abstractRoom.name == "SH_E04RIV" && self.game.IsStorySession)
            {
                Logger.LogInfo("Loading Pipeyard's flood room. Attempting to create Custrom Drone Cutscene object. (Riv)");

                if (myPlayer.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(1615f, 315f), new Color(0f, 1f, 1f), true));
            }
            if (self.abstractRoom.name == "VS_BASEMENT01" && self.game.IsStorySession)
            {
                Logger.LogInfo("Loading MSC Basement. Attempting to create Custrom Drone Cutscene object. (Inv UWU)");

                if (myPlayer.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel)
                    self.AddObject(new CustomCutsceneArtificerRobo(self, new Vector2(545f, 47f), new Color(0f, 0f, 1f), true));
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
