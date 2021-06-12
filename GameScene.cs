using Dcrew.Camera;
using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    public sealed partial class GameScene : Scene
    {
        World physicsWorld = new World(Vector2.Zero);

        ImGuiRenderer ImGuiRenderer;

        public static Camera camera;
        Entity focusedEntity;

        Bathtub bathtub;

        public static bool EditorMode = true;

        public static bool WinFlag;
        public static bool LoseFlag;

        public static int PlayerID;
        public static int GoalLaps = 1;
        public static int PlayerLaps;
        public static List<TimeSpan> LapTimers = new List<TimeSpan>() { new TimeSpan() };

        public static void Win()
        {
            WinFlag = true;
        }

        static bool loadedfont = false;
        static ImFontPtr fontPTR;

        static Track_Waypoints wayPoints;

        public override void Enter()
        {
            WinFlag = LoseFlag = false;
            PlayerID = 0;
            GoalLaps = 3;
            LapTimers = new () { new() };

            em = new EntityManager(Game._);
            track = new EntityManager(Game._);
            camera = new Camera(new Vector2(0, 0));
            camera.Init();
            camera.Scale = Vector2.One * 0.5f;
            camera.VirtualRes = (800, 480);
            track.AddEntity(wayPoints = new Track_Waypoints());



            InitEditor();
        }



        EntityManager em;
        int currentEntityID;
        
        EntityManager track;
        int track_ThingID;

        Dictionary<Type, Func<Game, World, EntityManager, Entity>> spawn_entity = new (){
            {
                typeof(Bathtub),
                (game,world,_) =>
                {
                    
                    return new Bathtub(world)
                    {
                        
                        CarTexture = Assets.Sprites.basecart,
                        PlayerControlled = true,
                        CharacterTexture = Assets.Sprites.chr_monkey,
                        AI_Waypoints = GameScene.wayPoints,
                        camera = camera
                    };
                }
            },
            
            
        };

        Dictionary<Type, Func<Game, World, EntityManager, Entity>> spawn_track_entity = new()
        {
            {
                typeof(Track_Palm),
                (game, world,_) =>
                {
                    return new Track_Palm(world)
                    {
                        texture = Assets.Sprites.palm
                    };
                }
            },
            {
                typeof(Track_PalmWall),
                (game, world,entMan) =>
                {
                    return new Track_PalmWall()
                    {
                        camera = camera,
                        world = world,
                        EntityManager = entMan
                    };
                }
            },
            {
                typeof(Track_FinishBackground),
                (game, world, entMan) =>
                {
                    return new Track_FinishBackground()
                    {

                    };
                }
            }
        };

        public override void Update(GameTime gameTime)
        {
            if (Run && !fileIOActive)
            {
                for (int i = 0; i < em.SerializableEntities.Count; i++) {
                    if(em.SerializableEntities[i] is Bathtub b && b.PlayerControlled) focusedEntity = b;
                }
                if(!WinFlag && !LoseFlag)
                em.Update(gameTime);
            }
            track.Update(gameTime);
            physicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
           
            if(focusedEntity != null) camera.XY = Vector2.Lerp(camera.XY, focusedEntity.VisualPosition, 0.2f);
     
                //camera.XY = Vector2.Lerp(camera.XY, bathtub.VisualPosition - bathtub.Velocity * Game.PixelsPerMeter, 0.2f);
        }
       
        public override void Draw(GameTime time)
        {


            DrawBackground(0);
            DrawBackground(0.5f);

            Game._.spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: camera.View(), samplerState: SamplerState.PointClamp);
            em.Draw(time, camera);
            track.Draw(time, camera);
            Game._.spriteBatch.End();


            if (!loadedfont)
            {

                fontPTR = ImGui.GetIO().Fonts.AddFontFromFileTTF("FONT/ComicNeue-Bold.ttf", 18, null, ImGui.GetIO().Fonts.GetGlyphRangesDefault());

                ImGuiRenderer.RebuildFontAtlas();

                loadedfont = true;
            }
            ImGuiRenderer.BeforeLayout(time);
            ImGui.PushFont(fontPTR);
            ImGuiUtils.SetStyle();

            ResultScreen();



            EditorFunctions(time);

            ImGui.GetBackgroundDrawList().AddText(fontPTR, ImGui.GetFontSize() * 3, System.Numerics.Vector2.UnitX * (1280 - 200), 0xFF00FF00,
                string.Format("LAPS: {0}/{1}", PlayerLaps, GoalLaps));

            for (int i = LapTimers.Count; i-- > 0;)
            {
                Color color;
                if (i == LapTimers.Count - 1)
                {
                    if (PlayerLaps < LapTimers.Count && LapTimers.Count > 1)
                    {
                        color = LapTimers[PlayerLaps].Ticks < LapTimers[PlayerLaps - 1].Ticks ? Color.Yellow : Color.Red;
                    }
                    else
                    {
                        color = Color.White;
                    }
                }
                else
                {
                    color = Color.White;
                }

                //var color = LapTimers.Count > 1 && i != LapTimers.Count ? (LapTimers[i].Ticks > LapTimers[i - 1].Ticks ?
                //    Color.Yellow.PackedValue :
                //   Color.Red.PackedValue) : Color.White.PackedValue; 
                ImGui.GetBackgroundDrawList().AddText(fontPTR, ImGui.GetFontSize() * 2, (LapTimers.Count - i + 0.5f) * System.Numerics.Vector2.UnitY * ImGui.GetFontSize() * 2 + System.Numerics.Vector2.UnitX * (1280 - 200),
                    color.PackedValue,
                    LapTimers[i].ToString(@"mm\:ss\.ff"));
            }
            ImGui.Begin("Entity Properties");
            try
            {
                focusedEntity?.IMGUI(time);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            ImGui.End();
            ImGuiRenderer.AfterLayout();


        }

        private void ResultScreen()
        {
            if (WinFlag || LoseFlag)
            {
                if (WinFlag)
                {
                    ImGuiUtils.BeginFixedWindow("Well done!", 200, 190);

                    ImGui.LabelText("", "     =====TIME=====");
                    for (int i = 0; i < LapTimers.Count; i++)
                    {
                        ImGui.Text((i + 1).ToString() + ". ");
                        ImGui.SameLine();
                        ImGui.Text(LapTimers[i].ToString(@"mm\:ss\.ff"));
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        var b = em.SerializableEntities[i] as Bathtub;
                        if (b.PlayerControlled)
                        {
                            ImGui.Text("Total time: ");
                            ImGui.SameLine();
                            ImGui.Text(b.time.ToString(@"mm\:ss\.ff"));
                        }
                    }

                    ImGui.Button("Level select");
                    ImGui.SameLine(110);
                    ImGui.Button("Continue");

                    ImGui.End();
                }
                else if (LoseFlag)
                {
                    ImGuiUtils.BeginFixedWindow("You are a loser!", 180, 190 - 100);
                    ImGui.Text("Try again?");
                    if (ImGui.Button("YES"))
                    {
                        var scene = new GameScene();
                        scene.Load(levelName);
                        scene.Run = true;
                        Game._.SceneManager.SwitchScene(scene);
                    }
                    ImGui.SameLine(90 + 53);
                    ImGui.Button("No");
                    ImGui.End();
                }

            }
        }

        private static void DrawBackground(float half)
        {
            Game._.spriteBatch.Begin(transformMatrix: camera.View(), samplerState: SamplerState.PointClamp);
            const int spriteSpacing = 5;
            const int offset = 3;
            int sprWidth = Assets.Sprites.ground.rectangle.Width;
            int sprHeight = Assets.Sprites.ground.rectangle.Height;

            int width = camera.VirtualRes.Width / (int)(sprWidth * camera.Scale.X * spriteSpacing);
            int height = camera.VirtualRes.Height / (int)(sprHeight * camera.Scale.Y * spriteSpacing);
            Point cam = new Point((int)camera.X / sprWidth / spriteSpacing, (int)camera.Y / sprHeight / spriteSpacing);
            for (int x = cam.X - width / 2 - offset; x < width / 2 + cam.X + offset; x++)
            {
                for (int y = cam.Y - height / 2 - offset; y < height / 2 + cam.Y + offset; y++)
                {
                    Assets.Sprites.ground.Draw(new Vector2(x + half, y + half) *
                        new Vector2(sprWidth, sprHeight) * spriteSpacing,
                        scale: Vector2.One * spriteSpacing,
                        color: Color.Gray
                        );
                }
            }
            Game._.spriteBatch.End();
        }

        public override void Leave()
        {
            throw new NotImplementedException();
        }
    }
}
