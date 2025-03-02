using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RWCustom;
using UnityEngine;
using CoralBrain;
using Noise;

namespace MoreSlugcats
    {
        public class CustomCutsceneArtificerRobo : UpdatableAndDeletable
        {
            public CustomCutsceneArtificerRobo(Room room, Vector2 coords, Color color, bool useCoordY)
            {
                Custom.Log(new string[]
                {
                "CUSTOM ARTIFICER BOT CUTSCENE START (CTOR)"
                });
                this.room = room;
                this.useCoordY = useCoordY;
                this.phase = CustomCutsceneArtificerRobo.Phase.Init;
                this.bot = new AncientBot(coords, color, null, false);
                room.AddObject(this.bot);
            }
            public override void Update(bool eu)
            {
                base.Update(eu);
            if (this.player == null || this.player?.room == null) return;
            if (this.player.room != this.room) return;
            if (!this.initController && this.useCoordY && ((this.player.mainBodyChunk.pos.x < triggerX1 || this.player.mainBodyChunk.pos.x > triggerX2) || (this.player.mainBodyChunk.pos.y < triggerY1 || this.player.mainBodyChunk.pos.y > triggerY2))) return;
            if (!this.initController && (this.player.mainBodyChunk.pos.x < triggerX1 || this.player.mainBodyChunk.pos.x > triggerX2)) return;

                if (this.phase == CustomCutsceneArtificerRobo.Phase.Init)
                {
                    if (this.player != null && !this.initController && this.player.controller == null)
                    {
                    if (this.player.grabbedBy.Count > 0)
                    {
                        Creature grabber = this.player.grabbedBy[0].grabber;
                        this.player.AllGraspsLetGoOfThisObject(true);
                        grabber.Stun(5);
                    }
                    this.CreateFear();
                    this.scareObj.lifeTime = -800;
                    this.scareObj.fearRange = 24000f;
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
                foreach (AbstractCreature creature in this.player.room.abstractRoom.creatures)
                {
                    if (creature.realizedCreature is not Player)
                    {
                        creature.realizedCreature.Blind(30);
                        creature.realizedCreature.Deafen(30);
                    }
                }
                if (this.phase == CustomCutsceneArtificerRobo.Phase.ActivateRobo)
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
                    if (this.scareObj != null)
                    {
                        this.scareObj.Destroy();
                    }
                        if (this.player != null)
                        {
                        (this.player.graphicsModule as PlayerGraphics).LookAtNothing();

                        if (this.player.slugcatStats.name == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel)
                        {
                            this.bot.lightOn = false;
                            this.player.myRobot = null;
                            this.bot.Destroy();
                            this.Explode();                          
                        }
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
                    if (this.player.room.game.StoryCharacter != MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel)
                    {
                        if (this.room.game.IsStorySession)
                        {
                            AncientDroneForAll.AncientDroneForAll.hasDrone = true;
                        }
                        this.player.myRobot = this.bot;
                    }
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

        private void SetTriggerCoords(Player player)
        {
            triggerX1 = 590;
            triggerX2 = 620;
            triggerY1 = 0;
            triggerY2 = 0;
            if (player.room.game.StoryCharacter == SlugcatStats.Name.White)
            {
                triggerX1 = 590;
                triggerX2 = 620;
                triggerY1 = 0;
                triggerY2 = 0;
            }
            if (player.room.game.StoryCharacter == SlugcatStats.Name.Yellow)
            {
                triggerX1 = 460;
                triggerX2 = 580;
                triggerY1 = 0;
                triggerY2 = 0;
            }
            if (player.room.game.StoryCharacter == SlugcatStats.Name.Red)
            {
                triggerX1 = 580;
                triggerX2 = 680;
                triggerY1 = 420;
                triggerY2 = 530;
            }
            if (player.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Gourmand)
            {
                triggerX1 = 740;
                triggerX2 = 830;
                triggerY1 = 360;
                triggerY2 = 410;
            }
            if (player.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Rivulet)
            {
                triggerX1 = 1560;
                triggerX2 = 1650;
                triggerY1 = 305;
                triggerY2 = 360;
            }
            if (player.room.game.StoryCharacter == MoreSlugcatsEnums.SlugcatStatsName.Sofanthiel)
            {
                triggerX1 = 510;
                triggerX2 = 610;
                triggerY1 = 40;
                triggerY2 = 100;
            }
        }

        public void CreateFear()
        {
            if (this.scareObj == null)
            {
                this.scareObj = new FirecrackerPlant.ScareObject(this.bot.pos);
                this.scareObj.fearRange = 8000f;
                this.scareObj.fearScavs = true;
                this.room.AddObject(this.scareObj);
                //this.room.InGameNoise(new InGameNoise(base.firstChunk.pos, 8000f, this, 1f));
            }
        }


        public void Explode()
        {
            Custom.Log(new string[]
                {
                "SINGULARITY EXPLODE"
                });
            Vector2 vector = this.bot.pos;
            this.room.AddObject(new SingularityBomb.SparkFlash(this.bot.pos, 300f, new Color(0f, 0f, 1f)));
            this.room.AddObject(new Explosion(this.room, null, vector, 7, 450f, 6.2f, 10000f, 280f, 0.25f, null, 1f, 160f, 1f));
            this.room.AddObject(new Explosion(this.room, null, vector, 7, 2000f, 4f, 1000f, 400f, 0.25f, null, 1f, 200f, 1f));
            this.room.AddObject(new Explosion.ExplosionLight(vector, 280f, 1f, 7, new Color(1f, 0.2f, 0.2f)));
            this.room.AddObject(new Explosion.ExplosionLight(vector, 230f, 1f, 3, new Color(1f, 1f, 1f)));
            this.room.AddObject(new Explosion.ExplosionLight(vector, 2000f, 2f, 60, new Color(1f, 0.2f, 0.2f)));
            this.room.AddObject(new ShockWave(vector, 350f, 0.485f, 300, true));
            this.room.AddObject(new ShockWave(vector, 2000f, 0.185f, 180, false));
            for (int i = 0; i < 25; i++)
            {
                Vector2 a = Custom.RNV();
                if (this.room.GetTile(vector + a * 20f).Solid)
                {
                    if (!this.room.GetTile(vector - a * 20f).Solid)
                    {
                        a *= -1f;
                    }
                    else
                    {
                        a = Custom.RNV();
                    }
                }
                for (int j = 0; j < 3; j++)
                {
                    this.room.AddObject(new Spark(vector + a * Mathf.Lerp(30f, 60f, UnityEngine.Random.value), a * Mathf.Lerp(7f, 38f, UnityEngine.Random.value) + Custom.RNV() * 20f * UnityEngine.Random.value, Color.Lerp(new Color(1f, 0.2f, 0.2f), new Color(1f, 1f, 1f), UnityEngine.Random.value), null, 11, 28));
                }
                this.room.AddObject(new Explosion.FlashingSmoke(vector + a * 40f * UnityEngine.Random.value, a * Mathf.Lerp(4f, 20f, Mathf.Pow(UnityEngine.Random.value, 2f)), 1f + 0.05f * UnityEngine.Random.value, new Color(1f, 1f, 1f), new Color(1f, 0.2f, 0.2f), UnityEngine.Random.Range(3, 11)));
            }
            for (int k = 0; k < 6; k++)
            {
                this.room.AddObject(new SingularityBomb.BombFragment(vector, Custom.DegToVec(((float)k + UnityEngine.Random.value) / 6f * 360f) * Mathf.Lerp(18f, 38f, UnityEngine.Random.value)));
            }
            this.room.ScreenMovement(new Vector2?(vector), default(Vector2), 0.9f);
            this.room.PlaySound(SoundID.Bomb_Explode, vector);
            this.room.InGameNoise(new InGameNoise(vector, 9000f, null, 1f));
            for (int m = 0; m < this.room.physicalObjects.Length; m++)
            {
                for (int n = 0; n < this.room.physicalObjects[m].Count; n++)
                {
                    if (this.room.physicalObjects[m][n] is Creature && Custom.Dist(this.room.physicalObjects[m][n].firstChunk.pos, this.bot.pos) < 350f)
                    {
                        (this.room.physicalObjects[m][n] as Creature).Die();
                    }
                    if (this.room.physicalObjects[m][n] is ElectricSpear)
                    {
                        if ((this.room.physicalObjects[m][n] as ElectricSpear).abstractSpear.electricCharge == 0)
                        {
                            (this.room.physicalObjects[m][n] as ElectricSpear).Recharge();
                        }
                        else
                        {
                            (this.room.physicalObjects[m][n] as ElectricSpear).ExplosiveShortCircuit();
                        }
                    }
                }
            }
            this.room.InGameNoise(new InGameNoise(vector, 12000f, null, 1f));
        }


            public AncientBot bot;
            public bool initController;
            public CustomCutsceneArtificerRobo.Phase phase;
            public CustomCutsceneArtificerRobo.StartController startController;
            public int cutsceneTimer;
            public float triggerX1, triggerX2, triggerY1, triggerY2; //X1 - left, X2 - right, Y1 - down, Y2 - up
            public bool useCoordY;
            internal FirecrackerPlant.ScareObject scareObj;
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
