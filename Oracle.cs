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
        private void Oracle_HitByWeapon(ILContext il)
        {
            var c = new ILCursor(il);
            var d = new ILCursor(il);
            var e = new ILCursor(il);
            var myCodeEntry = c.DefineLabel();

            e.GotoNext(
                MoveType.After,
                        x => x.MatchLdstr("Don't. My systems have already been damaged enough as it is. Go away."), // `ldstr`
                        x => x.MatchCallvirt<OracleBehavior>("Translate"),                  // `callvirt Translate`
                        x => x.MatchLdcI4(0),                                               // `ldc.i4.0`
                        x => x.MatchCallvirt<HUD.DialogBox>("Interrupt"),                   // `callvirt Interrupt`
                        x => x.MatchRet(),
                        x => x.MatchLdsfld<ModManager>("MSC")
                    );
            Logger.LogInfo($"1. Cursor at: {e.Index}");
            e.Remove();
            e.Emit(OpCodes.Brfalse, myCodeEntry);


            e.GotoNext(
                    MoveType.After,
                    //x => x.MatchLdarg(0),                            // ldarg.0
                    x => x.MatchLdfld<UpdatableAndDeletable>("room"), // ldfld Room UpdatableAndDeletable::room
                    x => x.MatchLdfld<Room>("game"),                 // ldfld RainWorldGame Room::game
                    x => x.MatchCallvirt<RainWorldGame>("get_IsStorySession")                                               // `ret`
                    );
            Logger.LogInfo($"2. Cursor at: {e.Index}");
            e.Remove();
            e.Emit(OpCodes.Brfalse, myCodeEntry);

            e.GotoNext(
                MoveType.After,
                //x => x.MatchLdarg(0),                                // ldarg.0
                x => x.MatchLdfld(typeof(UpdatableAndDeletable).GetField("room")),    // ldfld Room UpdatableAndDeletable::room
                x => x.MatchLdfld(typeof(Room).GetField("game")),                    // ldfld RainWorldGame Room::game
                x => x.MatchCallvirt(typeof(RainWorldGame).GetMethod("get_GetStorySession")), // callvirt RainWorldGame::get_GetStorySession()
                x => x.MatchLdfld(typeof(StoryGameSession).GetField("saveStateNumber")),
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName).GetField("Artificer")), // ldsfld SlugcatStats/Name MoreSlugcats.MoreSlugcatsEnums/SlugcatStatsName::Artificer
                x => x.MatchCall(typeof(ExtEnum<SlugcatStats.Name>).GetMethod("op_Equality"))                                             // `ret`
        );
            Logger.LogInfo($"3. Cursor at: {e.Index}");
            e.Remove();
            e.Emit(OpCodes.Brfalse, myCodeEntry);


            d.GotoNext(
                MoveType.After,
                x => x.MatchLdfld(typeof(StoryGameSession).GetField("saveStateNumber")),                // IL_01F1: ldfld saveStateNumber
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName).GetField("Artificer")), // IL_01F6: ldsfld Artificer
                x => x.MatchCall(typeof(ExtEnum<SlugcatStats.Name>).GetMethod("op_Equality")),         // IL_01FB: call op_Equality
                x => x.MatchBrfalse(out _),                                                           // IL_0200: brfalse.s IL_0213
                x => x.MatchLdarg(0),                                                                 // IL_0202: ldarg.0
                x => x.MatchLdfld(typeof(Oracle).GetField("oracleBehavior")),                         // IL_0203: ldfld oracleBehavior
                x => x.MatchIsinst(typeof(SSOracleBehavior)),                                         // IL_0208: isinst SSOracleBehavior
                x => x.MatchCallvirt(typeof(SSOracleBehavior).GetMethod("ReactToHitWeapon")),         // IL_020D: callvirt ReactToHitWeapon
                x => x.MatchRet()                                                                     // IL_0212: ret
            );
            var origCode = d.DefineLabel();
            d.MarkLabel(origCode);


            c.GotoNext(
                MoveType.After,
                x => x.MatchLdfld(typeof(StoryGameSession).GetField("saveStateNumber")),                // IL_01F1: ldfld saveStateNumber
                x => x.MatchLdsfld(typeof(MoreSlugcats.MoreSlugcatsEnums.SlugcatStatsName).GetField("Artificer")), // IL_01F6: ldsfld Artificer
                x => x.MatchCall(typeof(ExtEnum<SlugcatStats.Name>).GetMethod("op_Equality")),         // IL_01FB: call op_Equality
                x => x.MatchBrfalse(out _),                                                           // IL_0200: brfalse.s IL_0213
                x => x.MatchLdarg(0),                                                                 // IL_0202: ldarg.0
                x => x.MatchLdfld(typeof(Oracle).GetField("oracleBehavior")),                         // IL_0203: ldfld oracleBehavior
                x => x.MatchIsinst(typeof(SSOracleBehavior)),                                         // IL_0208: isinst SSOracleBehavior
                x => x.MatchCallvirt(typeof(SSOracleBehavior).GetMethod("ReactToHitWeapon")),         // IL_020D: callvirt ReactToHitWeapon
                x => x.MatchRet()                                                                     // IL_0212: ret
            );
           
            c.MarkLabel(myCodeEntry);
            c.Emit(OpCodes.Ldarg_0);
            bool Oracle_TolerateHitIfPlayerHasDroneCheck(Oracle this_arg)
            {
                if (ModManager.MSC && this_arg.room.game.IsStorySession && AncientDroneForAll.hasDrone && options.PacifyPebbles.Value)
                {
                    Logger.LogWarning($"Oracle tolerated weapon hit.");
                    return true;
                }
                Logger.LogWarning($"Oracle didn't tolerated weapon hit.");
                Logger.LogWarning($"hasDrone? {AncientDroneForAll.hasDrone}");
                Logger.LogWarning($"pacifyPebbles enabled? {options.PacifyPebbles.Value}");
                return false;
            }
            c.EmitDelegate<Func<Oracle, bool>>(Oracle_TolerateHitIfPlayerHasDroneCheck);
            c.Emit(OpCodes.Brfalse, origCode);
            c.Emit(OpCodes.Ldarg_0);
            void Oracle_TolerateHitIfPlayerHasDrone(Oracle this_arg)
            {
                Logger.LogWarning($"Initiated ReactTooHitWeapon and attempting RET from method.");
                (this_arg.oracleBehavior as SSOracleBehavior).ReactToHitWeapon();
                
            }
            c.EmitDelegate<Action<Oracle>>(Oracle_TolerateHitIfPlayerHasDrone);
            c.Emit(OpCodes.Ret);
        }

    }
}
