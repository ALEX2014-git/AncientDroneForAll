using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWCustom;
using UnityEngine;

    namespace MoreSlugcats
    {
        public class CustomCutsceneArtificerRobo : UpdatableAndDeletable
        {
            public CustomCutsceneArtificerRobo(Room room, Vector2 droneCoords, Color droneColor, bool useCoordY)
            {
                Custom.Log(new string[]
                {
                "CUSTOM ARTIFICER BOT CUTSCENE START (CTOR)"
                });
                this.room = room;
                this.useCoordY = useCoordY;
                this.SetTriggerCoords(player);
                this.phase = CustomCutsceneArtificerRobo.Phase.Init;
                this.bot = new AncientBot(droneCoords, droneColor, null, false);
                room.AddObject(this.bot);
            }


            public override void Update(bool eu)
            {
                base.Update(eu);
            if (this.player == null || this.player?.room == null) return;
            if (this.player.room != this.room) return;
            if (!this.initController && this.useCoordY && (this.player.mainBodyChunk.pos.x < triggerX1 || this.player.mainBodyChunk.pos.x > triggerX2 && this.player.mainBodyChunk.pos.y < triggerY1 || this.player.mainBodyChunk.pos.y > triggerY2)) return;
            if (!this.initController && (this.player.mainBodyChunk.pos.x < triggerX1 || this.player.mainBodyChunk.pos.x > triggerX2)) return;

            if (this.phase == CustomCutsceneArtificerRobo.Phase.Init)
                {
                    if (this.player != null && !this.initController && this.player.controller == null)
                    {
                        this.startController = new CustomCutsceneArtificerRobo.StartController(this);
                        this.player.controller = this.startController;
                        this.bot.tiedToObject = this.player;
                        this.initController = true;
                    }
                    if (this.initController)
                    {
                        this.phase = CustomCutsceneArtificerRobo.Phase.ActivateRobo;
                        return;
                    }
                }
                else
                {
                    if (this.phase == CustomCutsceneArtificerRobo.Phase.ActivateRobo)
                    {
                     this.cutsceneTimer++;
                    if (this.player != null)
                    {
                        (this.player.graphicsModule as PlayerGraphics).LookAtPoint(bot.pos, 1f);
                    }
                    return;
                    }
                    if (this.phase == CustomCutsceneArtificerRobo.Phase.End)
                    {
                        Custom.Log(new string[]
                        {
                        "CUSTOM ARTIFICER BOT CUTSCENE END"
                        });
                        if (this.player != null)
                        {
                        (this.player.graphicsModule as PlayerGraphics).LookAtNothing();
                        this.player.controller = null;
                        }
                        //this.room.game.cameras[0].hud.textPrompt.AddMessage(this.room.game.rainWorld.inGameTranslator.Translate("This ancient device may give you access to new areas."), 20, 500, true, true);
                        this.Destroy();
                    }
                }
            }
            public Player.InputPackage GetInput()
            {
                if (this.player == null)
                {
                    return new Player.InputPackage(false, Options.ControlSetup.Preset.None, 0, 0, false, false, false, false, false);
                }
                int x = 0;
                int y = 0;
                bool jmp = false;
                bool pckp = false;
                bool thrw = false;
                if (this.phase == CustomCutsceneArtificerRobo.Phase.ActivateRobo)
                {
                    if (this.bot.myAnimation == AncientBot.Animation.IdleOffline && this.cutsceneTimer >= 45)
                    {
                        this.bot.myAnimation = AncientBot.Animation.TurnOn;
                    }
                    if (this.bot.myMovement != AncientBot.FollowMode.Offline)
                    {
                        this.phase = CustomCutsceneArtificerRobo.Phase.End;
                        if (this.room.game.IsStorySession)
                        {
                            (this.room.game.session as StoryGameSession).saveState.hasRobo = true;
                        }
                        this.player.myRobot = this.bot;
                    }
                }
                return new Player.InputPackage(false, Options.ControlSetup.Preset.None, x, y, jmp, thrw, pckp, false, false);
            }
            public Player player
            {
                get
                {
                    AbstractCreature firstAlivePlayer = this.room.game.FirstAlivePlayer;
                    if (this.room.game.Players.Count > 0 && firstAlivePlayer != null && firstAlivePlayer.realizedCreature != null)
                    {
                        return firstAlivePlayer.realizedCreature as Player;
                    }
                    return null;
                }
            }

        public void SetTriggerCoords(Player player)
        {
            triggerX1 = 590;
            triggerX2 = 620;
            triggerY1 = 0;
            triggerY2 = 0;
            if (player.slugcatStats.name == SlugcatStats.Name.White)
            {
                triggerX1 = 590;
                triggerX2 = 620;
                triggerY1 = 0;
                triggerY2 = 0;
            }
            if (player.slugcatStats.name == SlugcatStats.Name.Yellow)
            {
                triggerX1 = 590;
                triggerX2 = 620;
                triggerY1 = 0;
                triggerY2 = 0;
            }
            if (player.slugcatStats.name == SlugcatStats.Name.Red)
            {
                triggerX1 = 580;
                triggerX2 = 680;
                triggerY1 = 420;
                triggerY2 = 530;
            }
            if (player.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
            {
                triggerX1 = 740;
                triggerX2 = 830;
                triggerY1 = 360;
                triggerY2 = 410;
            }
        }


            public AncientBot bot;
            public bool initController;
            public CustomCutsceneArtificerRobo.Phase phase;
            public CustomCutsceneArtificerRobo.StartController startController;
            public int cutsceneTimer;
            public float triggerX1, triggerX2, triggerY1, triggerY2; //X1 - left, X2 - right, Y1 - down, Y2 - up
            public bool useCoordY;
            public class Phase : ExtEnum<CustomCutsceneArtificerRobo.Phase>
            {
                public Phase(string value, bool register = false) : base(value, register)
                {
                }

                public static readonly CustomCutsceneArtificerRobo.Phase Init = new CustomCutsceneArtificerRobo.Phase("Init", true);

 
                public static readonly CustomCutsceneArtificerRobo.Phase ActivateRobo = new CustomCutsceneArtificerRobo.Phase("ActivateRobo", true);

                public static readonly CustomCutsceneArtificerRobo.Phase End = new CustomCutsceneArtificerRobo.Phase("End", true);
            }

            public class StartController : Player.PlayerController
            {

                public StartController(CustomCutsceneArtificerRobo owner)
                {
                    this.owner = owner;
                }

                public override Player.InputPackage GetInput()
                {
                    return this.owner.GetInput();
                }

                private CustomCutsceneArtificerRobo owner;
            }
        }
    }
