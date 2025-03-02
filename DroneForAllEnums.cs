using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AncientDroneForAll;

namespace AncientDroneForAll
{
    public static class DroneForAllEnums
    {

        public class ConversationID
        {

            public static void RegisterValues()
            {
                DroneForAllEnums.ConversationID.Drone_MeetPlayer = new Conversation.ID("Drone_MeetPlayer", true);
                DroneForAllEnums.ConversationID.Drone_ResyncScene = new Conversation.ID("Drone_ResyncScene", true);
            }

            public static void UnregisterValues()
            {
                Conversation.ID drone_MeetPlayer = DroneForAllEnums.ConversationID.Drone_MeetPlayer;
                if (drone_MeetPlayer != null)
                {
                    drone_MeetPlayer.Unregister();
                }
                DroneForAllEnums.ConversationID.Drone_MeetPlayer = null;

                Conversation.ID drone_ResyncScene = DroneForAllEnums.ConversationID.Drone_ResyncScene;
                if (drone_ResyncScene != null)
                {
                    drone_ResyncScene.Unregister();
                }
                DroneForAllEnums.ConversationID.Drone_ResyncScene = null;
            }

            public static Conversation.ID Drone_MeetPlayer;

            public static Conversation.ID Drone_ResyncScene;

        }

        public class SSOracleBehaviorAction
        {
            public static void RegisterValues()
            {
                DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init = new SSOracleBehavior.Action("Drone_MeetPlayer_Init", true);
                DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Talking = new SSOracleBehavior.Action("Drone_MeetPlayer_Talking", true);
                DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty = new SSOracleBehavior.Action("Drone_SlumberParty", true);
            }

            public static void UnregisterValues()
            {
                SSOracleBehavior.Action drone_MeetPlayer_Init = DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init;
                if (drone_MeetPlayer_Init != null)
                {
                    drone_MeetPlayer_Init.Unregister();
                }
                DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Init = null;

                SSOracleBehavior.Action drone_MeetPlayer_Talking = DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Talking;
                if (drone_MeetPlayer_Talking != null)
                {
                    drone_MeetPlayer_Talking.Unregister();
                }
                DroneForAllEnums.SSOracleBehaviorAction.Drone_MeetPlayer_Talking = null;

                SSOracleBehavior.Action drone_SlumberParty = DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty;
                if (drone_SlumberParty != null)
                {
                    drone_SlumberParty.Unregister();
                }
                DroneForAllEnums.SSOracleBehaviorAction.Drone_SlumberParty = null;
            }

            public static SSOracleBehavior.Action Drone_MeetPlayer_Init;

            public static SSOracleBehavior.Action Drone_MeetPlayer_Talking;

            public static SSOracleBehavior.Action Drone_SlumberParty;

        }

        public class SSOracleBehaviorSubBehavID
        {
            public static void RegisterValues()
            {
                DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_MeetPlayer = new SSOracleBehavior.SubBehavior.SubBehavID("Drone_MeetPlayer", true);
                DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty = new SSOracleBehavior.SubBehavior.SubBehavID("Drone_SlumberParty", true);
            }

            public static void UnregisterValues()
            {
                SSOracleBehavior.SubBehavior.SubBehavID drone_MeetPlayer = DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_MeetPlayer;
                if (drone_MeetPlayer != null)
                {
                    drone_MeetPlayer.Unregister();
                }
                DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_MeetPlayer = null;

                SSOracleBehavior.SubBehavior.SubBehavID drone_SlumberParty = DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty;
                if (drone_SlumberParty != null)
                {
                    drone_SlumberParty.Unregister();
                }
                DroneForAllEnums.SSOracleBehaviorSubBehavID.Drone_SlumberParty = null;
            }


            public static SSOracleBehavior.SubBehavior.SubBehavID Drone_MeetPlayer;

            public static SSOracleBehavior.SubBehavior.SubBehavID Drone_SlumberParty;

        }


    }
}