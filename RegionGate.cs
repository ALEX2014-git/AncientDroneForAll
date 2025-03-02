using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using RWCustom;
using BepInEx;
using Debug = UnityEngine.Debug;
using System.Data.SqlClient;
using BepInEx.Logging;
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
using MonoMod.Cil;
using System.Runtime.Remoting.Lifetime;
using System.Xml;
using System.Reflection;
using static MonoMod.InlineRT.MonoModRule;
using HarmonyLib.Tools;

namespace AncientDroneForAll
    {
        public partial class AncientDroneForAll
        {

            public static bool RegionGate_MeetRequirement_get(orig_MeetRequirement orig, RegionGate self)
            {
            bool origData = orig(self);
            if (ModManager.MSC && self.karmaRequirements[(!self.letThroughDir) ? 1 : 0] == MoreSlugcatsEnums.GateRequirement.RoboLock && self.room.game.session is StoryGameSession && ((AncientDroneForAll.hasDrone && Instance.options.PebblesIntro.Value && AncientDroneForAll.isDroneResynced) || (AncientDroneForAll.hasDrone && (self.room.game.StoryCharacter == SlugcatStats.Name.Red || self.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Spear)) || (AncientDroneForAll.hasDrone && !Instance.options.PebblesIntro.Value)) && self.room.world.region.name != "SL" && self.room.world.region.name != "MS" && self.room.world.region.name != "DM")
                {
                    Logger.LogWarning("Overriding orig RegionGate MeetRequirement getter!");
                    return true;
                }
                return origData;
            }
        }

        public delegate bool orig_MeetRequirement(RegionGate self);

    }
