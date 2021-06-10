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

        public void Win()
        {

        }

        static Track_Waypoints wayPoints;

        public override void Enter()
        {
            em = new EntityManager(Game._);
            track = new EntityManager(Game._);
            camera = new Camera(new Vector2(0, 0));
            camera.Init();
            camera.Scale = Vector2.One * 0.5f;
            camera.VirtualRes = (800, 480);


            //            bathtub = new Bathtub(physicsWorld);
            //       bathtub.CarTexture = Assets.Sprites.basecart;
            //           bathtub.CharacterTexture = Assets.Sprites.chr_monkey;
            //         bathtub.AI_Waypoints = wayPoints;
            //em.AddEntity(bathtub);
  

            track.AddEntity(wayPoints = new Track_Waypoints());
            

            var testtt = new Track_PalmWall()
            {
                camera = GameScene.camera,
                world = physicsWorld,
                EntityManager = track
            };
            testtt.Positions.Add(new Vector2(0));
            testtt.Positions.Add(new Vector2(30, 20));
            testtt.Positions.Add(new Vector2(50, 10));
            
            track.AddEntity(testtt);

            //bathtub.Position = new Vector2(gdm.PreferredBackBufferWidth/2, gdm.PreferredBackBufferHeight / 2);
            physicsWorld.CreateEdge(new(-100, -100), new(100, -100));

            InitEditor();
        }


        


        enum EditMode
        {
            Entity,
            Track
        };

        EditMode editMode = EditMode.Track;

        

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
        };

        public override void Update(GameTime gameTime)
        {
            //em.Update(gameTime);
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


            EditorFunctions(time);
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
            Console.WriteLine(width + " " + height);
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
