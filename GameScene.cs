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

        public static Camera camera;
        Entity focusedEntity;

        public static bool EditorMode = true;

        public static bool WinFlag;
        public static bool LoseFlag;
        
        public static int PlayerID;
        public static bool WrongWay;
        public static int GoalLaps = 1;
        public static int PlayerLaps;
        public static List<TimeSpan> LapTimers = new List<TimeSpan>() { new TimeSpan() };

        TimeSpan timeSinceLoad = new();

        public static void Win()
        {
            WinFlag = true;
        }
        bool started;


        static Track_Waypoints wayPoints;

        public override void Enter()
        {
            WinFlag = LoseFlag = false;
            PlayerID = 0;
            LapTimers = new () { new() };

            em = new EntityManager(Game._);
            track = new EntityManager(Game._);
            camera = new Camera(new Vector2(0, 0));
            camera.Init();
            camera.Scale = Vector2.One * 0.5f;
            camera.VirtualRes = (800, 480);
            track.AddEntity(wayPoints = new Track_Waypoints());

            started = EditorMode == true;
            if (EditorMode) InitEditor();
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
                (game, world, _) =>
                {
                    return new Track_Palm(world)
                    {
                        texture = Assets.Sprites.palm
                    };
                }
            },
            {
                typeof(Track_PalmWall),
                (game, world, entMan) =>
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
                (game, world, entMan) => new Track_FinishBackground()
            },
            {
                typeof(Track_Decoration),
                (_, _, _) => new Track_Decoration()
            },
            {
                typeof(Track_FlowerField),
                (_,_,_)=>new Track_FlowerField()
            }
        };

        public override void Update(GameTime gameTime)
        {
            if (Run && !fileIOActive && started)
            {
                for (int i = 0; i < em.SerializableEntities.Count; i++) {
                    if(em.SerializableEntities[i] is Bathtub b && b.PlayerControlled) focusedEntity = b;
                }
                if(!WinFlag && !LoseFlag) em.Update(gameTime);
            }
            track.Update(gameTime);
            physicsWorld.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
           
            if(focusedEntity != null) camera.XY = Vector2.Lerp(camera.XY, focusedEntity.VisualPosition, 0.2f);
        }
        int frame = 0;
        public override void Draw(GameTime time)
        {
            DrawBackground(0);
            DrawBackground(0.5f);

            Game._.spriteBatch.Begin(SpriteSortMode.FrontToBack, transformMatrix: camera.View(), samplerState: SamplerState.PointClamp);
            em.Draw(time, camera);
            track.Draw(time, camera);
            Game._.spriteBatch.End();

            ResultScreen();

            if (EditorMode)
            {
                EditorFunctions(time);
            }
            else if(!started)
            {
                //i don't flopping what magic dust does this work on
                //but i don't have time to figure it out
                timeSinceLoad += time.ElapsedGameTime;
                var _time = MathF.Abs((int)timeSinceLoad.TotalSeconds - 1 - 3);
                switch(_time)
                {
                    case 0:
                        ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), ImGui.GetFontSize() * 5, new(640 - 40, 360 - 40), frame % 2 == 0 ? Color.Red.PackedValue : Color.Yellow.PackedValue,
                    "GO!");
                        break;
                    case 1:
                    case 2:
                    case 3:
                    ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(),ImGui.GetFontSize()*5,new (640-40,360-40), frame % 2 == 0 ? Color.Red.PackedValue : Color.Yellow.PackedValue,
                        _time.ToString());
                        break;
                        
                }
                if(timeSinceLoad.TotalSeconds > 5)
                {
                    started = true;
                }
            }
           

            ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), ImGui.GetFontSize() * 3, System.Numerics.Vector2.UnitX * (1280 - 200), 0xFF00FF00,
                string.Format("LAPS: {0}/{1}", PlayerLaps, GoalLaps));

            if(frame++%2==0 && WrongWay)
            ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), ImGui.GetFontSize() * 3, System.Numerics.Vector2.UnitX * (1280/2-200), Color.Red.PackedValue,
                string.Format("WRONG WAY", PlayerLaps, GoalLaps));
            
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

                ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(), ImGui.GetFontSize() * 2, (LapTimers.Count - i + 0.5f) * System.Numerics.Vector2.UnitY * ImGui.GetFontSize() * 2 + System.Numerics.Vector2.UnitX * (1280 - 200),
                    color.PackedValue,
                    LapTimers[i].ToString(@"mm\:ss\.ff"));
            }
            if (EditorMode)
            {
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
            }


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
                        ImGui.End();
                        var scene = new GameScene();
                        EditorMode = false;
                       
                        scene.Run = true;
                        Game._.SceneManager.SwitchScene(scene);
                        scene.Load(levelName);
                        
                        return;
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
            physicsDebugDraw?.Dispose();
        }
    }
}
