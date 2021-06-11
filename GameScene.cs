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

        public static bool WinFlag;
        public static bool LoseFlag;

        public static int PlayerID;
        public static int Laps = 1;

        public static void Win()
        {
            WinFlag = true;
        }

        static Track_Waypoints wayPoints;

        public override void Enter()
        {
            WinFlag = LoseFlag = false;
            PlayerID = Laps = 0;

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
            if (testMode && !fileIOActive)
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



            ImGuiRenderer.BeforeLayout(time);

            if(WinFlag || LoseFlag)
            {
                int windWidth = 200;
                int windHeight = 150;

                ImGui.SetNextWindowPos(new System.Numerics.Vector2(Game._.gdm.PreferredBackBufferWidth/2-windWidth/2, Game._.gdm.PreferredBackBufferHeight/2-windHeight/2), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowSize(new System.Numerics.Vector2(windWidth, windHeight), ImGuiCond.FirstUseEver);
                ImGui.SetNextWindowFocus();

                ImGui.GetStyle().WindowTitleAlign = System.Numerics.Vector2.One/2;
                ImGui.Begin("Well done!", ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoCollapse);
                
                ///ImGui.LabelText("", "=====TIME=====");

                for (int i = 0; i < 4; i++)
                {
                    var b = em.SerializableEntities[i] as Bathtub;
                    if (b.PlayerControlled)
                    {
                        ImGui.Text("Time: ");
                        ImGui.SameLine();
                        ImGui.Text(b.time.ToString(@"mm\:ss\.ff"));
                    }
                }

                ImGui.End();
            }

            if (drawColliders)
            {
                Matrix proj = camera.Projection, view = camera.View(), world = Matrix.CreateScale(Game.PixelsPerMeter);
                physicsDebugDraw.RenderDebugData(ref proj, ref view, ref world,depthStencilState: DepthStencilState.None,rasterizerState: RasterizerState.CullNone);
            }
            ImGui.GetStyle().WindowRounding = 0.0f;// <- Set this on init or use ImGui::PushStyleVar()
            ImGui.GetStyle().ChildRounding = 0.0f;
            ImGui.GetStyle().FrameRounding = 0.0f;
            ImGui.GetStyle().GrabRounding = 0.0f;
            ImGui.GetStyle().PopupRounding = 0.0f;
            ImGui.GetStyle().ScrollbarRounding = 0.0f;
            EditorFunctions(time);

            ImGui.GetBackgroundDrawList().AddText(ImGui.GetFont(),ImGui.GetFontSize() * 2,System.Numerics.Vector2.Zero ,0xFF00FF00, "AAAAAAAAAAAAAAAAAA");
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
