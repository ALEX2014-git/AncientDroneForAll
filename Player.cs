using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using CoralBrain;
using Expedition;
using HUD;
using JollyCoop;
using JollyCoop.JollyMenu;
using MoreSlugcats;
using Noise;
using RWCustom;
using UnityEngine;
using System.Reflection;

namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {
        private void Player_UpdateMSC(On.Player.orig_UpdateMSC orig, Player self)
        {
            orig(self);

            if (self != null)
            {
                foreach (UpdatableAndDeletable obj in self.room.updateList)
                {
                    if (obj is CustomCutsceneArtificerRobo && (obj as CustomCutsceneArtificerRobo).scareObj != null)
                    {
                        Logger.LogMessage($"scareObj lifetime: {(obj as CustomCutsceneArtificerRobo).scareObj.lifeTime}");                       
                    }
                }


                foreach (AbstractCreature creature in self.room.abstractRoom.creatures)
                {
                    if (creature.realizedCreature != null &&
                        (creature.realizedCreature.Blinded || creature.realizedCreature.Deaf > 0))
                    {
                        Logger.LogMessage($"Creature {creature.realizedCreature} is blind for {creature.realizedCreature.blind}");
                        Logger.LogMessage($"Creature {creature.realizedCreature} is deaf for {creature.realizedCreature.deaf}");
                    }
                }
            }


            AbstractCreature firstAlivePlayer = self.room.game.FirstAlivePlayer;
            if (self.room != null && !self.room.game.wasAnArtificerDream && self.room.game.session is StoryGameSession && (self.AI == null && AncientDroneForAll.hasDrone) && (self.myRobot == null || self.myRobot.slatedForDeletetion) && (!ModManager.CoopAvailable || (firstAlivePlayer != null && firstAlivePlayer.realizedCreature != null && firstAlivePlayer.realizedCreature == self)))
            {
                Logger.LogInfo("Initialized room change procedure. Attempting to find drone template.");
                Creature baseCreature = (Creature)self;
                if (self.slugcatStats.name == SlugcatStats.Name.White)
                {
                    Logger.LogInfo("Room change in process. Current drone template is WHITE");
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 1f, 1f), self, true);
                }
                if (self.slugcatStats.name == SlugcatStats.Name.Yellow)
                {
                    Logger.LogInfo("Room change in process. Current drone template is YELLOW");
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 0.92f, 0.016f), self, true);
                }
                if (self.slugcatStats.name == SlugcatStats.Name.Red)
                {
                    Logger.LogInfo("Room change in process. Current drone template is RED");
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 0f, 0f), self, true);
                }
                if (self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                {
                    Logger.LogInfo("Room change in process. Current drone template is GOURMAND");
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 1f, 0f), self, true);
                }
                if (self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
                {
                    Logger.LogInfo("Room change in process. Current drone template is RIV");
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(0f, 1f, 1f), self, true);
                }
                if (self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel)
                {
                    Logger.LogInfo("Room change in process. Current drone template is Inv UWU");
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(0.1f, 0.1f, 0.1f), self, true);
                }
                if (self.myRobot != null)
                {
                    Logger.LogInfo("Respawning drone.");
                    self.room.AddObject(self.myRobot);
                }
            }
        }

    }
}
