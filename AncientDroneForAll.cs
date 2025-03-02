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
using MonoMod.RuntimeDetour.HookGen;
using System.Reflection.Emit;
using MonoMod;
using Unity.Collections.LowLevel.Unsafe;
using On;
using HarmonyLib.Tools;

#pragma warning disable CS0618
[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618

namespace AncientDroneForAll;

[BepInPlugin("ALEX2014.ArtyDroneForAll", "Arty's Drone for Everyone", "1.0.0")]
public partial class AncientDroneForAll : BaseUnityPlugin
{
    internal PluginOptions options;
    public static bool hasDrone, isDroneResynced, rivDroneTalk;
    
    public static ManualLogSource Logger { get; private set; }

    private static AncientDroneForAll _instance;
    public static AncientDroneForAll Instance
    {
        get
        {
            if (_instance == null)
            {
                throw new Exception("Instance of AncientDroneForAll is not created yet!");
            }
            return _instance;
        }
    }

    public AncientDroneForAll()
    {
        try
        {
            _instance = this;
            options = new PluginOptions(this, Logger);
            Logger.LogInfo($"Set up options from CTOR. Is NULL? {options == null}");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    private void OnEnable()
    {
        AncientDroneForAll.Logger = base.Logger;
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit;
    }

    BindingFlags propFlags = BindingFlags.Instance | BindingFlags.Public;
    BindingFlags myMethodFlags = BindingFlags.Static | BindingFlags.Public;
    private bool IsInit;
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);
        try
        {
            if (IsInit) return;
            On.RainWorldGame.ShutDownProcess += RainWorldGameOnShutDownProcess;
            On.GameSession.ctor += GameSessionOnctor;
            On.Player.UpdateMSC += Player_UpdateMSC;
            On.SaveState.SaveToString += SaveState_SaveToString;
            On.SaveState.LoadGame += SaveState_LoadGame;
            On.Room.Loaded += Room_Loaded;
            On.SSOracleBehavior.PebblesConversation.AddEvents += PebblesConversation_AddEvents;         
            On.SSOracleBehavior.SpecialEvent += SSOracleBehavior_SpecialEvent;
            On.SSOracleBehavior.SeePlayer += SSOracleBehavior_SeePlayer;
            On.RainWorld.OnModsDisabled += RainWorld_OnModsDisabled;
            On.SSOracleBehavior.NewAction += SSOracleBehavior_NewAction1;

            DroneForAllEnums.ConversationID.RegisterValues();
            DroneForAllEnums.SSOracleBehaviorAction.RegisterValues();
            DroneForAllEnums.SSOracleBehaviorSubBehavID.RegisterValues();

            IL.GateKarmaGlyph.Update += GateKarmaGlyph_Update;
            IL.HUD.Map.ctor += Map_ctor;
            IL.SSOracleBehavior.NewAction += SSOracleBehavior_NewAction;
            IL.SSOracleBehavior.SeePlayer += SSOracleBehavior_SeePlayer1;
            IL.Oracle.HitByWeapon += Oracle_HitByWeapon;
            IL.SSOracleBehavior.ThrowOutBehavior.Update += ThrowOutBehavior_Update;
            IL.MoreSlugcats.SSOracleRotBehavior.RMConversation.AddEvents += RMConversation_AddEvents;

            Hook RegionGateHook = new Hook(typeof(RegionGate).GetProperty("MeetRequirement", propFlags).GetGetMethod(), typeof(AncientDroneForAll).GetMethod("RegionGate_MeetRequirement_get", myMethodFlags));
            
            MachineConnector.SetRegisteredOI("ALEX2014.ArtyDroneForAll", options);

            Logger.LogMessage($"Is enums registered? {(DroneForAllEnums.ConversationID.Drone_MeetPlayer != null && DroneForAllEnums.ConversationID.Drone_ResyncScene != null && DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init != null && DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Talking != null && DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty != null && DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_MeetPlayer != null && DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty != null)}");
            IsInit = true;  
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    private void RainWorld_OnModsDisabled(On.RainWorld.orig_OnModsDisabled orig, RainWorld self, ModManager.Mod[] newlyDisabledMods)
    {
        orig(self, newlyDisabledMods);
        try
        {
            foreach (ModManager.Mod mod in newlyDisabledMods)
            {
                if (mod.id == "ALEX2014.ArtyDroneForAll")
                {
                    DroneForAllEnums.ConversationID.UnregisterValues();
                    DroneForAllEnums.SSOracleBehaviorAction.UnregisterValues();
                    DroneForAllEnums.SSOracleBehaviorSubBehavID.UnregisterValues();
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            throw;
        }
    }

    private void RainWorldGameOnShutDownProcess(On.RainWorldGame.orig_ShutDownProcess orig, RainWorldGame self)
    {
        orig(self);
        ClearMemory();
    }
    private void GameSessionOnctor(On.GameSession.orig_ctor orig, GameSession self, RainWorldGame game)
    {
        orig(self, game);
        ClearMemory();
    }

    #region Helper Methods

    private void ClearMemory()
    {
        //If you have any collections (lists, dictionaries, etc.)
        //Clear them here to prevent a memory leak
        //YourList.Clear();
    }

    #endregion
}
