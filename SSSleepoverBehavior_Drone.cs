using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AncientDroneForAll;
using MoreSlugcats;
using RWCustom;
using UnityEngine;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.Mathematics;

namespace AncientDroneForAll
{

    public class SSSleepoverBehavior_Drone : SSOracleBehavior.ConversationBehavior
    {
        public SSSleepoverBehavior_Drone(SSOracleBehavior owner) : base(owner, DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty, Conversation.ID.None)
        {
            this.lowGravity = -1f;
            if (!base.oracle.room.game.GetStorySession.saveState.deathPersistentSaveData.theMark)
            {
                owner.getToWorking = 0f;
                this.gravOn = false;
                this.firstMetOnThisCycle = true;
                owner.SlugcatEnterRoomReaction();
                this.owner.voice = base.oracle.room.PlaySound(SoundID.SL_AI_Talk_4, base.oracle.firstChunk);
                this.owner.voice.requireActiveUpkeep = true;
                owner.LockShortcuts();
                return;
            }
            if (this.owner.conversation != null)
            {
                this.owner.conversation.Destroy();
                this.owner.conversation = null;
                return;
            }
            this.owner.TurnOffSSMusic(true);
            owner.getToWorking = 1f;
            this.gravOn = true;
            if (base.oracle.ID == Oracle.OracleID.SS)
            {
                Custom.Log(new string[]
                {
                "SSAI convos had:",
                base.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad.ToString()
                });
                if (base.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad <= 3)
                {
                    base.dialogBox.NewMessage(base.Translate("Oh. It's you, why have you come back? Again."), 0);
                }
                else if (base.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad == 8)
                {
                    base.dialogBox.NewMessage(base.Translate("For what reason do you visit so often? There is nothing more I can do for you."), 0);
                    base.dialogBox.NewMessage(base.Translate(".  .  ."), 40);
                    base.dialogBox.NewMessage(base.Translate("I can only think that you somehow find me pleasant to listen to."), 0);
                }
                else if (UnityEngine.Random.value < 0.1f)
                {
                    base.dialogBox.NewMessage(base.Translate("Little creature, please leave. I would prefer to be alone."), 0);
                }
                else if (UnityEngine.Random.value < 0.3f)
                {
                    base.dialogBox.NewMessage(base.Translate("Have you brought something new this time?"), 0);
                }
                else if (UnityEngine.Random.value < 0.3f)
                {
                    base.dialogBox.NewMessage(base.Translate("Do you have something new, or have you come to just stare at me?"), 0);
                }
                else if (UnityEngine.Random.value < 0.3f)
                {
                    base.dialogBox.NewMessage(base.Translate("Hello again. I hope you have a reason to visit me."), 0);
                }
                else
                {
                    base.dialogBox.NewMessage(base.Translate(".  .  ."), 0);
                }
                base.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad++;
                return;
            }
        }

        public override void Activate(SSOracleBehavior.Action oldAction, SSOracleBehavior.Action newAction)
        {
            base.Activate(oldAction, newAction);
        }

        public override void NewAction(SSOracleBehavior.Action oldAction, SSOracleBehavior.Action newAction)
        {
            base.NewAction(oldAction, newAction);
            if (newAction == SSOracleBehavior.Action.ThrowOut_KillOnSight && this.owner.conversation != null)
            {
                this.owner.conversation.Destroy();
                this.owner.conversation = null;
            }
        }

        public override void Update()
        {
            base.Update();
            if (base.player == null)
            {
                return;
            }
        }

        private Vector2 holdPlayerPos
        {
            get
            {
                return new Vector2(668f, 268f + Mathf.Sin((float)base.inActionCounter / 70f * 3.14159274f * 2f) * 4f);
            }
        }

        public override bool Gravity
        {
            get
            {
                return this.gravOn;
            }
        }

        public override float LowGravity
        {
            get
            {
                return this.lowGravity;
            }
        }

        public bool holdPlayer;

        public bool gravOn;

        public bool firstMetOnThisCycle;

        public float lowGravity;

        public float lastGetToWork;

        public float tagTimer;

        public OraclePanicDisplay panicObject;

        public int timeUntilNextPanic;

        public int panicTimer;
    }
}