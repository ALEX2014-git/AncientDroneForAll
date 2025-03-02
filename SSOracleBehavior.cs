using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;
using System.Diagnostics;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using BepInEx;
using Debug = UnityEngine.Debug;
using System.Data.SqlClient;
using BepInEx.Logging;
using System.Globalization;
using CoralBrain;
using Expedition;
using HUD;
using JollyCoop;
using JollyCoop.JollyMenu;
using MoreSlugcats;
using Noise;
using static UpdatableAndDeletable;
using Menu;
using Rewired;
using UnityEngine.SocialPlatforms;
using UnityEngine.Rendering;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using Steamworks;
using IL.Menu.Remix.MixedUI;
using Menu.Remix.MixedUI;
using IL;
using On.Menu.Remix.MixedUI;
using System.Runtime.Remoting.Messaging;
using MonoMod.RuntimeDetour;
using System.Runtime.Remoting.Lifetime;
using System.Xml;
using System.Reflection;
using static MonoMod.InlineRT.MonoModRule;
using HarmonyLib.Tools;
using System.Runtime.Remoting.Contexts;
using Mono.Cecil.Cil;
using On;
using IL.Menu;
using On.MoreSlugcats;
using IL.MoreSlugcats;
using AncientDroneForAll;
using System.Text.RegularExpressions;

namespace AncientDroneForAll
{
    public partial class AncientDroneForAll
    {

        private void PebblesConversation_AddEvents(On.SSOracleBehavior.PebblesConversation.orig_AddEvents orig, SSOracleBehavior.PebblesConversation self)
        {
            orig(self);
            if (self.id == DroneForAllEnums.ConversationID.Drone_MeetPlayer)
            {
                if (self.owner.player.slugcatStats.name == SlugcatStats.Name.White)
                {
                    Logger.LogMessage("PebblesConversation_AddEvents: Attempting to load events from file 20141 for Drone_MeetPlayer scene");
                    self.LoadEventsFromFile(20141);
                    return;
                }
                if (self.owner.player.slugcatStats.name == SlugcatStats.Name.Yellow)
                {
                    Logger.LogMessage("PebblesConversation_AddEvents: Attempting to load events from file 20142 for Drone_MeetPlayer scene");
                    self.LoadEventsFromFile(20142);
                    return;
                }
            }
             /* if (self.id == Conversation.ID.Drone_ResyncScene)
              {
                  base.LoadEventsFromFile(Drone_ResyncScene);
                  return;
              }
              */
        }

        private void SSOracleBehavior_SpecialEvent(On.SSOracleBehavior.orig_SpecialEvent orig, SSOracleBehavior self, string eventName)
        {
            orig(self, eventName);
            if (ModManager.MSC)
            {
                if (eventName == "drone_resync")
                {
                    MoreSlugcats.OracleBotResync oracleBotResync = new MoreSlugcats.OracleBotResync(self.oracle);
                    self.oracle.room.AddObject(oracleBotResync);
                    if (self.currSubBehavior is SSOracleMeetPlayer_Drone)
                    {
                        (self.currSubBehavior as SSOracleMeetPlayer_Drone).resyncObject = oracleBotResync;
                    }
                }
            }
        }

        private void SSOracleBehavior_SeePlayer(On.SSOracleBehavior.orig_SeePlayer orig, SSOracleBehavior self)
        {
            Logger.LogMessage("Oracle saw player");
            Logger.LogMessage($"{self.oracle.ID} action: {self.action}");
            Logger.LogMessage($"{self.oracle.ID} subBehavior: {self.currSubBehavior}");
            Logger.LogMessage($"{self.oracle.ID} allSubBehaviors: {self.allSubBehaviors}");
            orig(self);
            //TODO: REWRITE THIS CODE TO PROPERLY CHECK FOR DRONE AND SETTINGS
            /*if (options.PebblesIntro.Value && AncientDroneForAll.hasDrone && !AncientDroneForAll.isDroneResynced)
            {
                if (self.oracle.room.game.StoryCharacter != MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Spear && self.oracle.room.game.StoryCharacter != SlugcatStats.Name.Red)
                {
                    Logger.LogMessage("Attempting to set Oracle's NewAction as Drone_MeetPlayer_Init");
                    self.NewAction(DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init);
                }
            }
            if (options.PacifyPebbles.Value && AncientDroneForAll.hasDrone && AncientDroneForAll.isDroneResynced)
            {
                if (self.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad > 0 && self.oracle.room.game.StoryCharacter != MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Spear && self.oracle.room.game.StoryCharacter != SlugcatStats.Name.Red)
                {
                    Logger.LogMessage("Attempting to set Oracle's NewAction as Drone_SlumberParty");
                    self.NewAction(DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty);
                }
            }
            ---------------------------------------------------
            MOVED THIS CODE TO THE IL-HOOK

            */ 
            Logger.LogMessage("Finishing SeePlayer iteration");
            Logger.LogMessage($"{self.oracle.ID} action: {self.action}");
            Logger.LogMessage($"{self.oracle.ID} subBehavior: {self.currSubBehavior}");
            Logger.LogMessage($"{self.oracle.ID} allSubBehaviors: {self.allSubBehaviors}");
        }

        private void SSOracleBehavior_NewAction1(On.SSOracleBehavior.orig_NewAction orig, SSOracleBehavior self, SSOracleBehavior.Action nextAction)
        {
            Logger.LogMessage($"NewAction called with nextAction: {nextAction}");
            Logger.LogMessage("Oracle requested NewAction method");
            try
            {
                Logger.LogMessage("Calling original method...");
                orig(self, nextAction);
                Logger.LogMessage("Original method completed.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                throw;
            }
            Logger.LogMessage("NewAction method completed");
        }


        private void SSOracleBehavior_SeePlayer1(ILContext il)
        {
            var c = new ILCursor(il);
            var d = new ILCursor(il);

            d.GotoNext(
                MoveType.After, // Переместиться за найденный блок
                x => x.MatchLdarg(0), // Первая инструкция первой последовательности
                x => x.MatchLdsfld(typeof(SSOracleBehavior.Action).GetField("ThrowOut_KillOnSight")), // Поле ThrowOut_KillOnSight
                x => x.MatchCall(typeof(SSOracleBehavior).GetMethod("NewAction", new[] { typeof(SSOracleBehavior.Action) })), // Вызов NewAction
                x => x.MatchBr(out _), // Переход на следующую инструкцию
                x => x.MatchLdarg(0), // Начало второй последовательности
                x => x.MatchLdsfld(typeof(SSOracleBehavior.Action).GetField("ThrowOut_SecondThrowOut")), // Поле ThrowOut_SecondThrowOut
                x => x.MatchCall(typeof(SSOracleBehavior).GetMethod("NewAction", new[] { typeof(SSOracleBehavior.Action) })), // Вызов NewAction
                x => x.MatchLdarg(0), // Продолжение второй последовательности
                x => x.MatchCall(typeof(SSOracleBehavior).GetMethod("SlugcatEnterRoomReaction")) // Завершающий вызов метода
            );
            ILLabel methodEnd = d.DefineLabel();
            d.MarkLabel(methodEnd);

            c.GotoNext(
                MoveType.After,
                x => x.MatchLdloc(0),
                x => x.MatchBrfalse(out _),
                x => x.MatchLdarg(0),
                x => x.MatchLdloc(0),
                x => x.MatchStfld(typeof(OracleBehavior).GetField("player"))
            );
            c.MoveAfterLabels();

            var secondIfCheck = c.DefineLabel();
            var standartCode = c.DefineLabel();

            c.Emit(OpCodes.Ldarg_0);
            bool SeePlayer_CheckPebblesIntro(SSOracleBehavior this_arg)
            {
                if (options.PebblesIntro.Value && AncientDroneForAll.hasDrone && !AncientDroneForAll.isDroneResynced)
                {
                    if (this_arg.oracle.room.game.StoryCharacter != MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Spear && this_arg.oracle.room.game.StoryCharacter != SlugcatStats.Name.Red)
                    {
                        Logger.LogMessage("Attempting to set Oracle's NewAction as Drone_MeetPlayer_Init");
                        return true;
                    }
                    return false;
                }
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior, bool>>(SeePlayer_CheckPebblesIntro); //Вызываем делегат проверки условий
            c.Emit(OpCodes.Brfalse, secondIfCheck); //Если условия не выполняются, то переходим ко второму if'у
            c.Emit(OpCodes.Ldarg_0);
            void SeePlayer_SetPebblesIntro(SSOracleBehavior this_arg)
            {
             Logger.LogMessage("Setting Oracle's NewAction to Drone_MeetPlayer_Init");
             this_arg.NewAction(DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init);
            }
            c.EmitDelegate<Action<SSOracleBehavior>>(SeePlayer_SetPebblesIntro);
            c.Emit(OpCodes.Br, methodEnd);

            c.MarkLabel(secondIfCheck);
            c.Emit(OpCodes.Ldarg_0);
            bool SeePlayer_CheckPebblesParty(SSOracleBehavior this_arg)
            {
                if ((options.PacifyPebbles.Value && AncientDroneForAll.hasDrone && AncientDroneForAll.isDroneResynced) || (!options.PacifyPebbles.Value && AncientDroneForAll.hasDrone))
                {
                    if (this_arg.oracle.room.game.GetStorySession.saveState.miscWorldSaveData.SSaiConversationsHad > 0 && this_arg.oracle.room.game.StoryCharacter != MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName.Spear && this_arg.oracle.room.game.StoryCharacter != SlugcatStats.Name.Red)
                    {
                        Logger.LogMessage("Attempting to set Oracle's NewAction as Drone_SlumberParty");
                        return true;
                    }
                    return false;
                }
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior, bool>>(SeePlayer_CheckPebblesParty); //Вызываем делегат проверки условий
            c.Emit(OpCodes.Brfalse, standartCode); //Если условия не выполняются, то переходим ко второму if'у
            c.Emit(OpCodes.Ldarg_0);
            void SeePlayer_SetPebblesParty(SSOracleBehavior this_arg)
            {
                Logger.LogMessage("Setting Oracle's NewAction to Drone_SlumberParty");
                this_arg.NewAction(DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty);
            }
            c.EmitDelegate<Action<SSOracleBehavior>>(SeePlayer_SetPebblesParty);
            c.Emit(OpCodes.Br, methodEnd);
            c.MarkLabel(standartCode);

        }


        private void ThrowOutBehavior_Update(ILContext il)
        {
            var c = new ILCursor(il);
            var d = new ILCursor(il);
            var e = new ILCursor(il);
            var f = new ILCursor(il);
            var myCodeEntry = c.DefineLabel();

            f.GotoNext(
                MoveType.After,
                x => x.MatchCall(typeof(SSOracleBehavior.SubBehavior).GetMethod("set_movementBehavior")),  // IL_0768: call
                x => x.MatchLdarg(0),  // IL_076d: ldarg.0
                x => x.MatchLdarg(0),  // IL_076e: ldarg.0
                x => x.MatchCall(typeof(SSOracleBehavior.SubBehavior).GetMethod("get_inActionCounter")),  // IL_076f: call
                x => x.MatchLdcI4(220),  // IL_0774: ldc.i4 220
                x => x.MatchCgt(),  // IL_0779: cgt
                x => x.MatchStfld(typeof(SSOracleBehavior.ThrowOutBehavior).GetField("telekinThrowOut")),  // IL_077b: stfld
                x => x.MatchLdsfld(typeof(ModManager).GetField("MSC")) // IL_0780: ldsfld
                );
            f.Remove();
            f.Emit(OpCodes.Brfalse, myCodeEntry);

            f.GotoNext(
                MoveType.After,
                x => x.MatchLdarg(0),  // IL_078a: ldarg.0
                x => x.MatchCall(typeof(SSOracleBehavior.SubBehavior).GetMethod("get_oracle")),  // IL_078b: call
                x => x.MatchLdfld(typeof(UpdatableAndDeletable).GetField("room")),  // IL_0790: ldfld
                x => x.MatchLdfld(typeof(Room).GetField("game")),  // IL_0795: ldfld
                x => x.MatchCallvirt(typeof(RainWorldGame).GetMethod("get_GetStorySession")),  // IL_079a: callvirt
                x => x.MatchLdfld(typeof(StoryGameSession).GetField("saveStateNumber")),  // IL_079f: ldfld
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName).GetField("Artificer")),  // IL_07a4: ldsfld
                x => x.MatchCall(typeof(ExtEnum<SlugcatStats.Name>).GetMethod("op_Equality"))  // IL_07a9: call
            );
            f.Remove();
            f.Emit(OpCodes.Brfalse, myCodeEntry);


            d.GotoNext(
                            MoveType.After,
                            x => x.MatchLdfld(typeof(SSOracleBehavior).GetField("throwOutCounter")), // IL_08B8: ldfld int32 SSOracleBehavior::throwOutCounter
                            x => x.MatchLdcI4(2900),                                                // IL_08BD: ldc.i4 2900
                            x => x.MatchBneUn(out _),                                               // IL_08C2: bne.un IL_0986
                            x => x.MatchLdarg(0),                                                   // IL_08C7: ldarg.0
                            x => x.MatchCall(typeof(SSOracleBehavior.TalkBehavior).GetMethod("get_dialogBox")), // IL_08C8: call instance class HUD.DialogBox SSOracleBehavior/TalkBehavior::get_dialogBox()
                            x => x.MatchLdarg(0),                                                   // IL_08CD: ldarg.0
                            x => x.MatchLdstr(out _), // IL_08CE: ldstr
                            x => x.MatchCall(typeof(SSOracleBehavior.TalkBehavior).GetMethod("Translate", new[] { typeof(string) })), // IL_08D3: call Translate
                            x => x.MatchLdcI4(0),                                                   // IL_08D8: ldc.i4.0
                            x => x.MatchCallvirt(typeof(HUD.DialogBox).GetMethod("Interrupt", new[] { typeof(string), typeof(int) })), // IL_08D9: callvirt Interrupt
                            x => x.MatchBr(out _)                                                   // IL_08DE: br IL_0986
                           );
            var origCode = d.DefineLabel();
            d.MarkLabel(origCode);

            e.GotoNext(
                        MoveType.After,
                        x => x.MatchCallvirt(typeof(HUD.DialogBox).GetMethod("Interrupt", new[] { typeof(string), typeof(int) })), // IL_095D: callvirt HUD.DialogBox::Interrupt
                        x => x.MatchBr(out _), // IL_0962: br.s IL_0986
                        x => x.MatchLdarg(0),  // IL_0964: ldarg.0
                        x => x.MatchLdfld(typeof(SSOracleBehavior.SubBehavior).GetField("owner")), // IL_0965: ldfld SSOracleBehavior/SubBehavior::owner
                        x => x.MatchLdfld(typeof(SSOracleBehavior).GetField("throwOutCounter")),   // IL_096A: ldfld SSOracleBehavior::throwOutCounter
                        x => x.MatchLdcI4(1780),  // IL_096F: ldc.i4 1780
                        x => x.MatchBle(out _),   // IL_0974: ble.s IL_0986
                        x => x.MatchLdarg(0),     // IL_0976: ldarg.0
                        x => x.MatchLdfld(typeof(SSOracleBehavior.SubBehavior).GetField("owner")), // IL_0977: ldfld SSOracleBehavior/SubBehavior::owner
                        x => x.MatchLdsfld(typeof(SSOracleBehavior.Action).GetField("ThrowOut_KillOnSight")), // IL_097C: ldsfld SSOracleBehavior/Action::ThrowOut_KillOnSight
                        x => x.MatchCallvirt(typeof(SSOracleBehavior).GetMethod("NewAction", new[] { typeof(SSOracleBehavior.Action) })) // IL_0981: callvirt SSOracleBehavior::NewAction
                    );
            var endCode = e.DefineLabel();
            e.MarkLabel(endCode);

            c.GotoNext(
                MoveType.After,
                x => x.MatchLdfld(typeof(SSOracleBehavior).GetField("throwOutCounter")), // IL_08B8: ldfld int32 SSOracleBehavior::throwOutCounter
                x => x.MatchLdcI4(2900),                                                // IL_08BD: ldc.i4 2900
                x => x.MatchBneUn(out _),                                               // IL_08C2: bne.un IL_0986
                x => x.MatchLdarg(0),                                                   // IL_08C7: ldarg.0
                x => x.MatchCall(typeof(SSOracleBehavior.TalkBehavior).GetMethod("get_dialogBox")), // IL_08C8: call instance class HUD.DialogBox SSOracleBehavior/TalkBehavior::get_dialogBox()
                x => x.MatchLdarg(0),                                                   // IL_08CD: ldarg.0
                x => x.MatchLdstr(out _), // IL_08CE: ldstr
                x => x.MatchCall(typeof(SSOracleBehavior.TalkBehavior).GetMethod("Translate", new[] { typeof(string) })), // IL_08D3: call Translate
                x => x.MatchLdcI4(0),                                                   // IL_08D8: ldc.i4.0
                x => x.MatchCallvirt(typeof(HUD.DialogBox).GetMethod("Interrupt", new[] { typeof(string), typeof(int) })), // IL_08D9: callvirt Interrupt
                x => x.MatchBr(out _)                                                   // IL_08DE: br IL_0986
                        );

            
            var secondCheck = c.DefineLabel();
            var thirdCheck = c.DefineLabel();
            var fourthCheck = c.DefineLabel();

            c.MarkLabel(myCodeEntry);
            bool CheckIfOracleCantKillOnSight()
            {
                if (options.PacifyPebbles.Value && AncientDroneForAll.hasDrone)
                {
                    return true;
                }
                return false;
            }
            c.EmitDelegate<Func<bool>>(CheckIfOracleCantKillOnSight);
            c.Emit(OpCodes.Brfalse, origCode);
            c.Emit(OpCodes.Ldarg_0);
            bool CantKOSDialogue1Check(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                if (this_arg.owner.throwOutCounter == 700)
                {
                    return true;
                }
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior.ThrowOutBehavior, bool>>(CantKOSDialogue1Check);
            c.Emit(OpCodes.Brfalse, secondCheck);
            c.Emit(OpCodes.Ldarg_0);
            void CantKOSDialogue1(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                this_arg.dialogBox.Interrupt(this_arg.Translate("Please leave."), 60);
                this_arg.dialogBox.NewMessage(this_arg.Translate("This is not a request. I have important work to do."), 0);
            }
            c.EmitDelegate<Action<SSOracleBehavior.ThrowOutBehavior>>(CantKOSDialogue1);
            c.Emit(OpCodes.Br, endCode);

            c.MarkLabel(secondCheck);
            c.Emit(OpCodes.Ldarg_0);
            bool CantKOSDialogue2Check(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                if (this_arg.owner.throwOutCounter == 1300)
                {
                    return true;
                }
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior.ThrowOutBehavior, bool>>(CantKOSDialogue2Check);
            c.Emit(OpCodes.Brfalse, thirdCheck);
            c.Emit(OpCodes.Ldarg_0);
            void CantKOSDialogue2(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                this_arg.dialogBox.Interrupt(this_arg.Translate("Unfortunately, my operations are encoded with a restriction that prevents<LINE>me from carrying out violent actions against my own citizens."), 0);
                this_arg.dialogBox.NewMessage(this_arg.Translate("Please do not take advantage of this. I do not have the patience for your continued presence here."), 0);
            }
            c.EmitDelegate<Action<SSOracleBehavior.ThrowOutBehavior>>(CantKOSDialogue2);
            c.Emit(OpCodes.Br, endCode);

            c.MarkLabel(thirdCheck);
            c.Emit(OpCodes.Ldarg_0);
            bool CantKOSDialogue3Check(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                if (this_arg.owner.throwOutCounter == 2100)
                {
                    return true;
                }
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior.ThrowOutBehavior, bool>>(CantKOSDialogue3Check);
            c.Emit(OpCodes.Brfalse, fourthCheck);
            c.Emit(OpCodes.Ldarg_0);
            void CantKOSDialogue3(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                this_arg.dialogBox.Interrupt(this_arg.Translate("Did you not register what I've said to you?"), 60);
                this_arg.dialogBox.NewMessage(this_arg.Translate("LEAVE."), 0);
            }
            c.EmitDelegate<Action<SSOracleBehavior.ThrowOutBehavior>>(CantKOSDialogue3);
            c.Emit(OpCodes.Br, endCode);

            c.MarkLabel(fourthCheck);
            c.Emit(OpCodes.Ldarg_0);
            bool CantKOSDialogue4Check(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                if (this_arg.owner.throwOutCounter == 2900)
                {
                    return true;
                }
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior.ThrowOutBehavior, bool>>(CantKOSDialogue4Check);
            c.Emit(OpCodes.Brfalse, endCode);
            c.Emit(OpCodes.Ldarg_0);
            void CantKOSDialogue4(SSOracleBehavior.ThrowOutBehavior this_arg)
            {
                this_arg.dialogBox.Interrupt(this_arg.Translate("I'm returning to my work. Unless you have anything productive<LINE>for me, I have nothing further to say to you."), 0);
            }
            c.EmitDelegate<Action<SSOracleBehavior.ThrowOutBehavior>>(CantKOSDialogue4);
            c.Emit(OpCodes.Br, endCode);

            

        }

        private void SSOracleBehavior_NewAction(ILContext il)
        {
            var c = new ILCursor(il);
            var d = new ILCursor(il);
            var e = new ILCursor(il);
            var f = new ILCursor(il);
            var g = new ILCursor(il);
            var h = new ILCursor(il);
            ILLabel IL_034A = null;
            ILLabel il_0350 = null;


            Logger.LogInfo("Attempting first SSOracleBehavior hook");

            d.GotoNext(
                MoveType.After,
                x => x.MatchLdsfld(typeof(ModManager).GetField(nameof(ModManager.MSC))), // Загрузка поля ModManager.MSC
                x => x.MatchBrfalse(out _), // Условие перехода, если false
                x => x.MatchLdarg(1), // Загрузка nextAction
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorAction).GetField("Rubicon")), // Загрузка Rubicon
                x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.Action>).GetMethod("op_Equality")), // Вызов op_Equality для сравнения
                x => x.MatchBrfalse(out _), // Условие перехода, если false
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("Rubicon")),
                x => x.MatchStloc(0),
                x => x.MatchBr(out _),
                x => x.MatchLdsfld(typeof(SSOracleBehavior.SubBehavior.SubBehavID).GetField("General")),
                x => x.MatchStloc(0)
                //x => x.MatchLdarg(0)
                        );
            il_0350 = d.DefineLabel();
            d.MarkLabel(il_0350);

            e.GotoNext(
                MoveType.After,
                x => x.MatchLdsfld(typeof(ModManager).GetField(nameof(ModManager.MSC))), // Загрузка поля ModManager.MSC
                x => x.MatchBrfalse(out _), // Условие перехода, если false
                x => x.MatchLdarg(1), // Загрузка nextAction
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorAction).GetField("Rubicon")), // Загрузка Rubicon
                x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.Action>).GetMethod("op_Equality")), // Вызов op_Equality для сравнения
                x => x.MatchBrfalse(out _), // Условие перехода, если false
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("Rubicon")),
                x => x.MatchStloc(0),
                x => x.MatchBr(out _)
                //x => x.MatchLdsfld(typeof(SSOracleBehavior.SubBehavior.SubBehavID).GetField("General"))
                        );
            IL_034A = e.DefineLabel();
            e.MarkLabel(IL_034A);

            // Переход к нужной позиции, где используется nextAction
            c.GotoNext(
                MoveType.After,
                x => x.MatchLdsfld(typeof(ModManager).GetField(nameof(ModManager.MSC))), // Загрузка поля ModManager.MSC
                x => x.MatchBrfalse(out _), // Условие перехода, если false
                x => x.MatchLdarg(1), // Загрузка nextAction
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorAction).GetField("Rubicon")), // Загрузка Rubicon
                x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.Action>).GetMethod("op_Equality")), // Вызов op_Equality для сравнения
                x => x.MatchBrfalse(out _), // Условие перехода, если false
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("Rubicon")),
                x => x.MatchStloc(0),
                x => x.MatchBr(out _)
                );
            c.MoveAfterLabels();

            Logger.LogInfo($"Set up cursor for first hook.");
            Logger.LogInfo($"Cursor at: {c.Index}");

            var label1SecondIf = c.DefineLabel();

            c.Emit(OpCodes.Ldarg, 1); //Помещаем в стек значение аргумента 1 (nextAction)
            bool CheckDroneMeetPlayerSubID(SSOracleBehavior.Action nextAction)
            {
                Logger.LogMessage($"CheckDroneMeetPlayerSubID got nextAction: {nextAction}");
                if (ModManager.MSC && (nextAction == DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init || nextAction == DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Talking))
                {
                    Logger.LogMessage($"CheckDroneMeetPlayerSubID returns true");
                    return true;
                }
                Logger.LogMessage($"CheckDroneMeetPlayerSubID returns false");
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior.Action, bool>>(CheckDroneMeetPlayerSubID); //Вызываем делегат проверки условий
            c.Emit(OpCodes.Brfalse, label1SecondIf); //Если условия не выполняются, то переходим ко второму if'у
            c.Emit(OpCodes.Ldsfld, typeof(DroneForAllEnums.SSOracleBehaviorSubBehavID).GetField("Drone_MeetPlayer")); //Загружаем в стек значение поля
            c.Emit(OpCodes.Stloc_0); //Записываем значение с верхушки стека в subBehavID
            c.Emit(OpCodes.Br, il_0350); //Завершаем if-else блок
            c.MarkLabel(label1SecondIf);
            c.Emit(OpCodes.Ldarg, 1); //Помещаем в стек значение аргумента 1 (nextAction)
            bool CheckDroneSlumberPartySubID(SSOracleBehavior.Action nextAction)
            {
                Logger.LogMessage($"CheckDroneSlumberPartySubID got nextAction: {nextAction}");
                if (ModManager.MSC && nextAction == DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty)
                {
                    Logger.LogMessage($"CheckDroneSlumberPartySubID returns true");
                    return true;
                }
                Logger.LogMessage($"CheckDroneSlumberPartySubID returns false");
                return false;
            }
            c.EmitDelegate<Func<SSOracleBehavior.Action, bool>>(CheckDroneSlumberPartySubID); //Вызываем делегат проверки условий
            c.Emit(OpCodes.Brfalse, il_0350);

            void ConsoleLogSlumberPartySubID()
            {
                Logger.LogMessage($"Is DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty field NULL? {DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty == null}");
            }
            c.EmitDelegate<Action>(ConsoleLogSlumberPartySubID);
            c.Emit(OpCodes.Ldsfld, typeof(DroneForAllEnums.SSOracleBehaviorSubBehavID).GetField("Drone_SlumberParty"));
            c.Emit(OpCodes.Stloc_0); //Записываем значение с верхушки стека в subBehavID
            c.Emit(OpCodes.Ldloc_0); //DEBUG
            void ConsoleLogSubBehIDAfterSlumberParty(SSOracleBehavior.SubBehavior.SubBehavID subBehavID)
            {
                Logger.LogMessage($"What local variable SubBehavID (Stloc_0) is? {subBehavID}");
            }
            c.EmitDelegate<Action<SSOracleBehavior.SubBehavior.SubBehavID>>(ConsoleLogSubBehIDAfterSlumberParty);
            c.Emit(OpCodes.Br, il_0350); //Завершаем if-else блок

            Logger.LogInfo("Attempting second SSOracleBehavior hook");

            ILLabel il_0500 = null;
            var myCodeEntry = f.DefineLabel();


            g.GotoNext(
                    MoveType.After,
                    x => x.MatchLdsfld(typeof(ModManager).GetField(nameof(ModManager.MSC))), // Загрузка поля ModManager.MSC
                    x => x.MatchBrfalse(out _), // Условие перехода, если false
                    x => x.MatchLdloc(0), // Загрузка nextAction
                    x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("Rubicon")),
                    x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.SubBehavior.SubBehavID>).GetMethod("op_Equality")),
                    x => x.MatchBrfalse(out _),
                    x => x.MatchLdarg(0),
                    x => x.MatchNewobj(typeof(SSOracleBehavior.SSOracleRubicon).GetConstructor(new[] { typeof(SSOracleBehavior) })),
                    x => x.MatchStloc(1)
                    );
            il_0500 = g.DefineLabel();
            g.MarkLabel(il_0500);

            h.GotoNext(
                    MoveType.After,
                    x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("MeetGourmand")),
                    x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.SubBehavior.SubBehavID>).GetMethod("op_Equality")),
                    x => x.MatchBrfalse(out _),
                    x => x.MatchLdarg(0),
                    x => x.MatchNewobj(typeof(SSOracleBehavior.SSOracleMeetGourmand).GetConstructor(new[] { typeof(SSOracleBehavior) })),
                    x => x.MatchStloc(1),
                    x => x.MatchBr(out _),
                    x => x.MatchLdsfld(typeof(ModManager).GetField(nameof(ModManager.MSC))) // Загрузка поля ModManager.MSC
                    );
            h.Remove();
            h.Emit(OpCodes.Brfalse, myCodeEntry);

            h.GotoNext(
                    MoveType.After,
                    //x => x.MatchBrfalse(out _), // Условие перехода, если false ПОЛЕ ПРОВЕРКИ MODMANAGER.MSC
                    x => x.MatchLdloc(0), // Загрузка nextAction
                    x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("Rubicon")),
                    x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.SubBehavior.SubBehavID>).GetMethod("op_Equality"))
                    //x => x.MatchBrfalse(out _) ПОЛЕ ПРОВЕРКИ NEXTACTION == RUBICON
                    );
            h.Remove();
            h.Emit(OpCodes.Brfalse, myCodeEntry);

            f.GotoNext(
                    MoveType.After,
                    x => x.MatchLdsfld(typeof(ModManager).GetField(nameof(ModManager.MSC))), // Загрузка поля ModManager.MSC
                    x => x.MatchBrfalse(out _), // Условие перехода, если false
                    x => x.MatchLdloc(0), // Загрузка nextAction
                    x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SSOracleBehaviorSubBehavID).GetField("Rubicon")),
                    x => x.MatchCall(typeof(ExtEnum<SSOracleBehavior.SubBehavior.SubBehavID>).GetMethod("op_Equality")),
                    x => x.MatchBrfalse(out _),
                    x => x.MatchLdarg(0),
                    x => x.MatchNewobj(typeof(SSOracleBehavior.SSOracleRubicon).GetConstructor(new[] { typeof(SSOracleBehavior) })),
                    x => x.MatchStloc(1)
                    );

            Logger.LogInfo($"Set up cursor for second hook.");

            ILLabel label2SecondIf = f.DefineLabel();

            f.Emit(OpCodes.Br, il_0500);
            f.MarkLabel(myCodeEntry);
            f.Emit(OpCodes.Ldloc_0);
            bool CheckDroneMeetPlayerSubBeh(SSOracleBehavior.SubBehavior.SubBehavID subBehavID)
            {
                Logger.LogMessage($"CheckDroneMeetPlayerSubBeh got subBehavID: {subBehavID}");
                if (ModManager.MSC && subBehavID == DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_MeetPlayer)
                {
                    Logger.LogMessage("CheckDroneMeetPlayerSubBeh returned true");
                    return true;
                }
                Logger.LogMessage("CheckDroneMeetPlayerSubBeh returned false");
                return false;
            }
            f.EmitDelegate<Func<SSOracleBehavior.SubBehavior.SubBehavID, bool>>(CheckDroneMeetPlayerSubBeh);
            f.Emit(OpCodes.Brfalse, label2SecondIf);
            f.Emit(OpCodes.Ldarg_0);
            f.Emit(OpCodes.Newobj, typeof(SSOracleMeetPlayer_Drone).GetConstructor(new[] { typeof(SSOracleBehavior) }));
            f.Emit(OpCodes.Stloc_1);
            f.Emit(OpCodes.Br, il_0500);

            f.MarkLabel(label2SecondIf);
            f.Emit(OpCodes.Ldloc_0);
            bool CheckDroneSlumberPartySubBeh(SSOracleBehavior.SubBehavior.SubBehavID subBehavID)
            {
                Logger.LogMessage($"CheckDroneSlumberPartySubBeh got subBehavID: {subBehavID}");
                if (ModManager.MSC && subBehavID == DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty)
                {
                    
                    Logger.LogMessage("CheckDroneSlumberPartySubBeh returned true");
                    return true;
                }
                Logger.LogMessage("CheckDroneSlumberPartySubBeh returned false");
                return false;
            }
            f.EmitDelegate<Func<SSOracleBehavior.SubBehavior.SubBehavID, bool>>(CheckDroneSlumberPartySubBeh);
            f.Emit(OpCodes.Brfalse, il_0500);
            f.Emit(OpCodes.Ldarg_0);
            f.Emit(OpCodes.Newobj, typeof(SSSleepoverBehavior_Drone).GetConstructor(new[] { typeof(SSOracleBehavior) }));
            f.Emit(OpCodes.Stloc_1);
            f.Emit(OpCodes.Br, il_0500);
        }

    }
}