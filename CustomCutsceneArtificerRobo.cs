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
            public CustomCutsceneArtificerRobo(Room room)
            {
                Custom.Log(new string[]
                {
                "CUSTOM ARTIFICER BOT CUTSCENE START (CTOR)"
                });
                this.room = room;
                this.phase = CustomCutsceneArtificerRobo.Phase.Init;
                this.bot = new AncientBot(new Vector2(650f, 192f), new Color(228f, 205f, 0f), null, false);
                room.AddObject(this.bot);
            }
            public override void Update(bool eu)
            {
                base.Update(eu);
            if (this.player == null || this.player?.room == null) return;
            if (this.player.room.abstractRoom.name != "UW_H01") return;
            if (this.player.mainBodyChunk.pos.x > 840 && this.player.mainBodyChunk.pos.x < 270) return;

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
                        this.phase = CustomCutsceneArtificerRobo.Phase.PlayerRun;
                        return;
                    }
                }
                else
                {
                    if (this.phase == CustomCutsceneArtificerRobo.Phase.PlayerRun || this.phase == CustomCutsceneArtificerRobo.Phase.ActivateRobo)
                    {
                        this.cutsceneTimer++;
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
                            this.player.controller = null;
                        }
                        this.room.game.cameras[0].hud.textPrompt.AddMessage(this.room.game.rainWorld.inGameTranslator.Translate("Default text lmao."), 20, 500, true, true);
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
                if (this.phase == CustomCutsceneArtificerRobo.Phase.PlayerRun)
                {
                if (this.player.mainBodyChunk.pos.x <= 600f)
                {
                    x = 1;
                }
                else x = -1;

                    if ((!this.player.standing && this.cutsceneTimer % 2 == 0) || (this.player.mainBodyChunk.pos.y < 155))
                    {
                        y = 1;
                    }
                    if (this.player.mainBodyChunk.pos.x >= 570f && this.player.mainBodyChunk.pos.x <= 720f && this.player.mainBodyChunk.pos.y > 180f)
                    {
                        this.phase = CustomCutsceneArtificerRobo.Phase.ActivateRobo;
                        this.cutsceneTimer = 0;
                    }
                }
                else if (this.phase == CustomCutsceneArtificerRobo.Phase.ActivateRobo)
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
            public AncientBot bot;
            public bool initController;
            public CustomCutsceneArtificerRobo.Phase phase;
            public CustomCutsceneArtificerRobo.StartController startController;
            public int cutsceneTimer;
            public class Phase : ExtEnum<CustomCutsceneArtificerRobo.Phase>
            {
                public Phase(string value, bool register = false) : base(value, register)
                {
                }

                public static readonly CustomCutsceneArtificerRobo.Phase Init = new CustomCutsceneArtificerRobo.Phase("Init", true);

                public static readonly CustomCutsceneArtificerRobo.Phase PlayerRun = new CustomCutsceneArtificerRobo.Phase("PlayerRun", true);

 
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
