using Dcrew.Camera;
using DSastR.Core;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D.Diagnostics;
using tainicom.Aether.Physics2D.Dynamics;

namespace MonoGameJam3Entry
{
    public sealed partial class GameScene : Scene
    {
        World world = new World(Vector2.Zero);
        DebugView physicsDebugDraw;
        ImGuiRenderer ImGuiRenderer;
        public static Camera camera;


        Bathtub bathtub;

        

        public override void Enter()
        {
            em = new EntityManager(Game._);
            track = new EntityManager(Game._);
            camera = new Camera(new Vector2(0, 0));
            camera.Init();
            camera.Scale = Vector2.One * 0.5f;

            Game._.chr_monkey = Game.LoadTexture("IMAGES/chr_monkey.bmp");
            Game._.basecart = Game.LoadTexture("IMAGES/basecart.bmp");
            bathtub = new Bathtub(world);
            bathtub.CarTexture = Game._.basecart;
            bathtub.CharacterTexture = Game._.chr_monkey;

            var testtt = new Track_PalmWall()
            {
                camera = GameScene.camera,
                world = world,
                EntityManager = track
            };
            testtt.Positions.Add(new Vector2(0));
            testtt.Positions.Add(new Vector2(30,20));
            testtt.Positions.Add(new Vector2(50, 10));

            track.AddEntity(testtt);


            Game._.bonk = Game.LoadTexture("IMAGES/bonk.bmp");
            Game._.testpattern = Game.LoadTexture("IMAGES/testpattern.bmp");

            em.AddEntity(bathtub);


            //bathtub.Position = new Vector2(gdm.PreferredBackBufferWidth/2, gdm.PreferredBackBufferHeight / 2);
            world.CreateEdge(new(-100, -100), new(100, -100));
            physicsDebugDraw = new DebugView(world);

            physicsDebugDraw.Enabled = true;
            physicsDebugDraw.AppendFlags(DebugViewFlags.Shape);
            physicsDebugDraw.LoadContent(Game._.gdm.GraphicsDevice, Game._.Content);

            ImGuiRenderer = new ImGuiRenderer(Game._);
            ImGuiRenderer.RebuildFontAtlas();
        }


        Entity focusedEntity;


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

        Dictionary<Type, Func<Game, World, EntityManager, Entity>> instancableEntities = new (){
            {
                typeof(Bathtub),
                (game,world,_) =>
                {
                    
                    return new Bathtub(world)
                    {
                        
                        CarTexture = game.basecart,
                        PlayerControlled = false,
                        CharacterTexture = game.chr_monkey
                    };
                }
            },
            
        };

        Dictionary<Type, Func<Game, World, EntityManager, Entity>> backgroudnEntities = new()
        {
            {
                typeof(Track_Palm),
                (game, world,_) =>
                {
                    return new Track_Palm(world)
                    {
                        texture = game.track_palm
                    };
                }
            },
            {
                typeof(Track_PalmWall),
                (game, world,entMan) =>
                {
                    return new Track_PalmWall()
                    {
                        camera = GameScene.camera,
                        world = world,
                        EntityManager = entMan
                    };
                }
            }
        };

        public override void Update(GameTime gameTime)
        {
            em.Update(gameTime);
            track.Update(gameTime);
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
           
            if(focusedEntity != null) camera.XY = Vector2.Lerp(camera.XY, focusedEntity.VisualPosition, 0.2f);
     
                //camera.XY = Vector2.Lerp(camera.XY, bathtub.VisualPosition - bathtub.Velocity * Game.PixelsPerMeter, 0.2f);
        }
        bool drawColliders;
        public override void Draw(GameTime time)
        {
            Game._.spriteBatch.Begin(transformMatrix: camera.View(), samplerState: SamplerState.PointClamp);

            for (int i = 0; i < 80; i++)
            {
                Game._.spriteBatch.Draw(Game._.testpattern, new Vector2(i % 10, i / 10) * 512, Color.Yellow);
            }
            Game._.spriteBatch.Draw(Game._.bonk, Vector2.Zero, Color.Red);

            em.Draw(time);
            track.Draw(time);
            Game._.spriteBatch.End();
            if (drawColliders)
            {
                Matrix proj = camera.Projection, view = camera.View(), world = Matrix.CreateScale(Game.PixelsPerMeter);
                physicsDebugDraw.RenderDebugData(ref proj, ref view, ref world);
            }

            EditorFunctions(time);
        }



        public override void Leave()
        {
            throw new NotImplementedException();
        }
    }
}
