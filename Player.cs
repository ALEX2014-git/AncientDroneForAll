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

namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {
        private void Player_UpdateMSC(On.Player.orig_UpdateMSC orig, Player self)
        {
            orig(self);
            AbstractCreature firstAlivePlayer = self.room.game.FirstAlivePlayer;
            if (self.room != null && !self.room.game.wasAnArtificerDream && self.room.game.session is StoryGameSession && ((self.AI == null && (self.room.game.session as StoryGameSession).saveState.hasRobo) || (self.AI != null && (self.playerState as PlayerNPCState).Drone)) && (self.myRobot == null || self.myRobot.slatedForDeletetion) && (!ModManager.CoopAvailable || (firstAlivePlayer != null && firstAlivePlayer.realizedCreature != null && firstAlivePlayer.realizedCreature == self)))
            {
                Creature baseCreature = (Creature)self;
                if (self.slugcatStats.name == SlugcatStats.Name.White)
                {
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 1f, 1f), self, true);
                }
                if (self.slugcatStats.name == SlugcatStats.Name.Yellow)
                {
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 0.92f, 0.016f), self, true);
                }
                if (self.slugcatStats.name == SlugcatStats.Name.Red)
                {
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 0f, 0f), self, true);
                }
                if (self.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
                {
                    self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(1f, 1f, 0f), self, true);
                }
                if (self.myRobot != null)
                {
                    self.room.AddObject(self.myRobot);
                }
            }


        }

    }
}
