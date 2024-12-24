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
                self.myRobot = new AncientBot(baseCreature.mainBodyChunk.pos, new Color(228f, 205f, 0f), self, true);
                self.room.AddObject(self.myRobot);
            }


        }

    }
}
