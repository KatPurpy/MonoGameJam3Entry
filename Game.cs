using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NVorbis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;
using System.Threading.Tasks.Dataflow;
using Dcrew.Camera;

namespace MonoGameJam3Entry
{
    class Game : Microsoft.Xna.Framework.Game
    {
        public const float PixelsPerMeter = 10;
        public static Game _;
        public GraphicsDeviceManager gdm;
        public SpriteBatch spriteBatch;
        public Game()
        {
            gdm = new GraphicsDeviceManager(this);
            Content.RootDirectory = "";
        }
        SoundEffect dd;
        Texture2D bonk;
        StreamedSound ss;
        Texture2D testpattern;

        World world = new World(Vector2.Zero);

        Bathtub bathtub;

        DebugView dv;

        Camera cam;

        protected override void Initialize()
        {
            _ = this;
            base.Initialize();
            spriteBatch = new SpriteBatch(gdm.GraphicsDevice);
            ss =  LoadMusic("test.ogg");
            bonk = LoadTexture("bonk.bmp");
            dd = LoadSound("popup.ogg");
            testpattern = LoadTexture("testpattern.bmp");
            var a = dd.CreateInstance();
            a.IsLooped = true;

            
            cam = new Camera(new Vector2(0, 0));
            cam.Init();
            cam.Scale = Vector2.One * 0.5f;
      ///      a.Play();
       //     ss.Play();

            bathtub = new Bathtub(world);
            bathtub.Texture = LoadTexture("basecart.bmp");
            //bathtub.Position = new Vector2(gdm.PreferredBackBufferWidth/2, gdm.PreferredBackBufferHeight / 2);
            world.CreateEdge(new(-100, -100), new(100, -100));
            dv = new DebugView(world);
            
            dv.Enabled = true;
            dv.AppendFlags(DebugViewFlags.Shape);
            dv.LoadContent(gdm.GraphicsDevice, Content);
        }

        

        protected override void Update(GameTime gameTime)
        {
            ss.Update();
            
            bathtub.Update(gameTime);
            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            cam.XY = Vector2.Lerp(cam.XY,bathtub.VisualPosition - bathtub.Velocity * PixelsPerMeter,0.2f);
            

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                ss.Pause();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                ss.Play();
            }

            base.Update(gameTime);
        }

        



        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(transformMatrix: cam.View(), samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(testpattern, Vector2.Zero,Color.Red);
            spriteBatch.Draw(bonk,Vector2.Zero,Color.Red);
            bathtub.Draw(gameTime);
            spriteBatch.End();

            //cam.Scale = Vector2.One * PhysicsScale; 
            var a =  cam.Projection;
            var b =  cam.View() ;
            var c = Matrix.CreateScale(PixelsPerMeter);
            //dv.AdaptiveLimits = true;
            dv.RenderDebugData(ref a,ref b,ref c);
  
            base.Draw(gameTime);
        }

        Vector2 ViewCenter;

        protected override void OnExiting(object sender, EventArgs args)
        {
            ss.Dispose();
        }


        Texture2D LoadTexture(string s)
        {
            using var stream = File.OpenRead(s);
            Texture2D t2d = Texture2D.FromStream(gdm.GraphicsDevice, stream);
            byte[] data = new byte[t2d.Width * t2d.Height * 4];
            t2d.GetData(data);
            Span<Color> c = MemoryMarshal.Cast<byte, Color>(data.AsSpan());
            for (int i = 0; i < c.Length; i++) if (c[i] == Color.Magenta || c[i] == new Color(206,0,206,255)) c[i] = Color.Transparent;
            t2d.SetData(data);
            return t2d;
        }

        SoundEffect LoadSound(string s)
        {
            using (var reader = new VorbisReader(s))
            {
                reader.ClipSamples = false;
                const int chunkLength = 8192;
                float[] buffer = new float[chunkLength];
                int sampleRate = reader.SampleRate;
                byte[] pcmBytes = new byte[sampleRate * 8];
                int samplesWritten = 0;

                int samplesRead = 0;
                while ((samplesRead = reader.ReadSamples(buffer, 0, chunkLength)) > 0)
                {
                    if (pcmBytes.Length < (samplesWritten + samplesRead) * sizeof(short))
                    {
                        Array.Resize(ref pcmBytes, pcmBytes.Length + sampleRate * 8);
                    }

                    Span<short> dst = MemoryMarshal.Cast<byte, short>(pcmBytes).Slice(samplesWritten, samplesRead);
                    Span<float> src = buffer.AsSpan(0, samplesRead);
                    FunnyAudioUtils.ConvertSingleToInt16(src, dst);
                    samplesWritten += samplesRead;
                }

                // TODO: fix getting correct channels from reader
                return new SoundEffect(pcmBytes, 0, samplesWritten * sizeof(short), sampleRate, reader.Channels == 1 ? AudioChannels.Mono : AudioChannels.Stereo, 0, samplesWritten);
            }
        }

        StreamedSound LoadMusic(string s) => new(s);
    }
}
