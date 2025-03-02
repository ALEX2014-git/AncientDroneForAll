using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreSlugcats;
using RWCustom;
using UnityEngine;

namespace AncientDroneForAll
{
        public class SSOracleMeetPlayer_Drone : SSOracleBehavior.ConversationBehavior
        {
            public SSOracleMeetPlayer_Drone(SSOracleBehavior owner) : base(owner, DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_MeetPlayer, Conversation.ID.None)
            {
                Custom.Log(new string[]
                {
            "Drone meet conversation made!"
                });
                owner.TurnOffSSMusic(true);
            }

            public override void Update()
            {
                base.Update();
                if (this.finishedFlag)
                {
                    this.owner.UnlockShortcuts();
                    return;
                }
                this.owner.LockShortcuts();
                base.oracle.marbleOrbiting = true;
                if (this.resyncObject != null)
                {
                    this.resyncObject.botSlider = this.botSlider;
                }
                if (this.player == null)
                {
                    this.botSlider = 0.9f;
                    this.player = (base.oracle.room.game.session.Players[0].realizedCreature as Player);
                    this.playerAng = Custom.Angle(this.player.firstChunk.pos, new Vector2(base.oracle.room.PixelWidth / 2f, base.oracle.room.PixelHeight / 2f));
                    if (this.player.myRobot == null)
                    {
                        AncientDroneForAll.hasDrone = true;
                        this.botSlider = 0.1f;
                        this.botSliderDestination = 0.1f;
                    }
                }
                if (this.player.room == base.oracle.room)
                {
                    if (this.player.myRobot != null)
                    {
                        this.botSlider = Mathf.Lerp(this.botSlider, this.botSliderDestination, 0.01f);
                    }
                    this.player.enteringShortCut = null;
                    this.playerAng += 0.4f;
                    Vector2 b = new Vector2(base.oracle.room.PixelWidth / 2f, base.oracle.room.PixelHeight / 2f) + Custom.DegToVec(this.playerAng) * 140f;
                    if (!base.oracle.room.game.GetStorySession.saveState.deathPersistentSaveData.theMark)
                    {
                        if (base.inActionCounter == 0)
                        {
                            this.owner.movementBehavior = SSOracleBehavior.MovementBehavior.KeepDistance;
                        }
                        if (base.inActionCounter == 40)
                        {
                            this.owner.NewAction(SSOracleBehavior.Action.General_GiveMark);
                            this.owner.afterGiveMarkAction = DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Talking;
                            this.botSliderDestination = 0.5f;
                            return;
                        }
                    }
                    else
                    {
                        this.player.firstChunk.pos = Vector2.Lerp(this.player.firstChunk.pos, b, 0.16f);
                        if (this.startedConversation && this.owner.conversation.slatedForDeletion)
                        {
                            Custom.Log(new string[]
                            {
                        "throw out"
                            });
                            this.owner.NewAction(SSOracleBehavior.Action.ThrowOut_ThrowOut);
                            this.player.myRobot.lockTarget = null;
                            base.oracle.marbleOrbiting = false;
                            this.Deactivate();
                        }
                        if (!this.startedConversation && base.inActionCounter == 10)
                        {
                            this.owner.movementBehavior = SSOracleBehavior.MovementBehavior.Talk;
                            this.owner.InitateConversation(DroneForAllEnums.ConversationID.Drone_MeetPlayer, this);
                            this.startedConversation = true;
                        }
                    }
                }
            }

            public override bool CurrentlyCommunicating
            {
                get
                {
                    return false;
                }
            }
            public override void Deactivate()
            {
                AncientDroneForAll.isDroneResynced = true;
                this.owner.UnlockShortcuts();
                this.finishedFlag = true;
            }

            private bool startedConversation;

            private new Player player;

            private float playerAng;

            private float botSlider;

            private float botSliderDestination;

            private bool finishedFlag;

            public OracleBotResync resyncObject;
        }
    }