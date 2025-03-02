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
        private void RMConversation_AddEvents(ILContext il)
        {
            var c = new ILCursor(il);
            var d = new ILCursor(il);

            var c2 = new ILCursor(il);
            var d2 = new ILCursor(il);

            var c3 = new ILCursor(il);
            var d3 = new ILCursor(il);

            bool CheckDroneTalk()
            {
                if (AncientDroneForAll.hasDrone && !AncientDroneForAll.rivDroneTalk)
                {
                    return true;
                }
                return false;
            }

            c2.GotoNext(
                MoveType.After,
                x => x.MatchLdarg(0),
                x => x.MatchLdcI4(152),
                x => x.MatchCall(typeof(Conversation).GetMethod("LoadEventsFromFile", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null))
                );
            //c2.MoveAfterLabels();

            d2.GotoNext(
                    MoveType.After,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(152),
                    x => x.MatchCall(typeof(Conversation).GetMethod("LoadEventsFromFile", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null))
                    );
            var origCode2 = d2.DefineLabel();
            d2.MarkLabel(origCode2);

            c2.EmitDelegate<Func<bool>>(CheckDroneTalk);
            c2.Emit(OpCodes.Brfalse, origCode2);
            c2.Emit(OpCodes.Ldarg_0);
            void InitiateDroneTalkRMPostGame1(MoreSlugcats.SSOracleRotBehavior.RMConversation this_arg)
            {
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, ".  .  .", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "I can see that you have acquired possession of the old Citizen ID drone.", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "Unfortunately, at this time I am unable to provide you the means needed to synchronize this machine to my structure.", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "There is nothing I can do for you.", 60));
                AncientDroneForAll.rivDroneTalk = true;
            }
            c2.EmitDelegate<Action<MoreSlugcats.SSOracleRotBehavior.RMConversation>>(InitiateDroneTalkRMPostGame1);

            c3.GotoNext(
                    MoveType.After,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(153),
                    x => x.MatchCall(typeof(Conversation).GetMethod("LoadEventsFromFile", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null))
                    );
            //c3.MoveAfterLabels();

            d3.GotoNext(
                    MoveType.After,
                    x => x.MatchLdarg(0),
                    x => x.MatchLdcI4(153),
                    x => x.MatchCall(typeof(Conversation).GetMethod("LoadEventsFromFile", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null))
                    );
            var origCode3 = d3.DefineLabel();
            d3.MarkLabel(origCode3);

            c3.EmitDelegate<Func<bool>>(CheckDroneTalk);
            c3.Emit(OpCodes.Brfalse, origCode3);
            c3.Emit(OpCodes.Ldarg_0);
            void InitiateDroneTalkRMPostGame2(MoreSlugcats.SSOracleRotBehavior.RMConversation this_arg)
            {
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, ".  .  .", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "I can also see that you have acquired possession of the old Citizen ID drone.", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "In older times, devices like this were used by the inhabitants of my city. These devices allowed them to connect to the infrastructure and provided many other benefits, such as notifications and access to different areas, depending on the user's privileges.", 150));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "Unfortunately, at this time I am unable to provide you the means needed to synchronize this machine to my structure, nor do you have the needs to remain here.", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "Now leave, little creature. It's not safe to stay here.", 60));
                AncientDroneForAll.rivDroneTalk = true;
            }
            c3.EmitDelegate<Action<MoreSlugcats.SSOracleRotBehavior.RMConversation>>(InitiateDroneTalkRMPostGame2);

            c.GotoNext(
                MoveType.After,
                x => x.MatchLdarg(0),
                x => x.MatchLdcI4(151),
                x => x.MatchCall(typeof(Conversation).GetMethod("LoadEventsFromFile", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null)),
                x => x.MatchLdarg(0),
                x => x.MatchLdfld(typeof(MoreSlugcats.SSOracleRotBehavior.RMConversation).GetField("owner")),
                x => x.MatchLdfld(typeof(OracleBehavior).GetField("oracle")),
                x => x.MatchLdfld(typeof(UpdatableAndDeletable).GetField("room")),
                x => x.MatchLdfld(typeof(Room).GetField("game")),
                x => x.MatchCallvirt(typeof(RainWorldGame).GetMethod("get_GetStorySession")),
                x => x.MatchLdfld(typeof(StoryGameSession).GetField("saveState")),
                x => x.MatchLdfld(typeof(SaveState).GetField("miscWorldSaveData")),
                x => x.MatchLdcI4(3),
                x => x.MatchStfld(typeof(MiscWorldSaveData).GetField("energySeenState")),
                x => x.MatchRet()
                );
            c.MoveAfterLabels();

            d.GotoNext(
                MoveType.After,
                x => x.MatchLdarg(0),
                x => x.MatchLdcI4(151),
                x => x.MatchCall(typeof(Conversation).GetMethod("LoadEventsFromFile", BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { typeof(int) }, null)),
                x => x.MatchLdarg(0),
                x => x.MatchLdfld(typeof(MoreSlugcats.SSOracleRotBehavior.RMConversation).GetField("owner")),
                x => x.MatchLdfld(typeof(OracleBehavior).GetField("oracle")),
                x => x.MatchLdfld(typeof(UpdatableAndDeletable).GetField("room")),
                x => x.MatchLdfld(typeof(Room).GetField("game")),
                x => x.MatchCallvirt(typeof(RainWorldGame).GetMethod("get_GetStorySession")),
                x => x.MatchLdfld(typeof(StoryGameSession).GetField("saveState")),
                x => x.MatchLdfld(typeof(SaveState).GetField("miscWorldSaveData")),
                x => x.MatchLdcI4(3),
                x => x.MatchStfld(typeof(MiscWorldSaveData).GetField("energySeenState")),
                x => x.MatchRet()
                );
            var origCode1 = d.DefineLabel();
            d.MarkLabel(origCode1);


            c.EmitDelegate<Func<bool>>(CheckDroneTalk);
            c.Emit(OpCodes.Brfalse, origCode1);
            c.Emit(OpCodes.Ldarg_0);
            void InitiateDroneTalkRMPrePostGame(MoreSlugcats.SSOracleRotBehavior.RMConversation this_arg)
            {
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, ".  .  .", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "I can see that you have acquired possession of the old Citizen ID drone.", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "In older times, devices like this were used by the inhabitants of my city. These devices allowed them to connect to the infrastructure and provided many other benefits, such as notifications and access to different areas, depending on the user's privileges.", 150));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "Unfortunately, at this time I am unable to provide you the means needed to synchronize this machine to my structure, nor do you have the needs to remain here.", 30));
                this_arg.events.Add(new Conversation.TextEvent(this_arg, 0, "Now leave, little creature. It's not safe to stay here.", 60));
                AncientDroneForAll.rivDroneTalk = true;
            }
            c.EmitDelegate<Action<MoreSlugcats.SSOracleRotBehavior.RMConversation>>(InitiateDroneTalkRMPrePostGame);
            c.Emit(OpCodes.Ret);
        }
    }
}
