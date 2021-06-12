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
using ImGuiNET;
using DSastR.Core;
using System.Timers;

namespace MonoGameJam3Entry
{
    public sealed class Game : Microsoft.Xna.Framework.Game
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

        StreamedSound ss;

        public static ImGuiRenderer ImGuiRenderer;

        public SceneManager SceneManager;

        static ImFontPtr fontPTR;


        public static Action DoAfterWin;

        public static void Win()
        {
            DoAfterWin?.Invoke();
            PlayerProfile.Save();
        }

        protected override void Initialize()
        {
            
            _ = this;
            SceneManager = new(this);
            IsMouseVisible = true;
            base.Initialize();
            spriteBatch = new SpriteBatch(gdm.GraphicsDevice);

            Assets.Load(this);
            ImGuiRenderer = new ImGuiRenderer(this);
            ImGuiRenderer.RebuildFontAtlas();


            SceneManager.SwitchScene(new GameScene());

            //PlayerProfile.Load();
            //SceneManager.SwitchScene(new MainMenuScene());
        }

        protected override void LoadContent()
        {
            gdm.PreferredBackBufferWidth = 1280;
            gdm.PreferredBackBufferHeight = 720;
            gdm.ApplyChanges();
        }



        protected override void Update(GameTime gameTime)
        {
            SceneManager.Update(gameTime);
            base.Update(gameTime);
        }

 
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Game.ImGuiRenderer.BeforeLayout(gameTime);
            unsafe
            {
                if ((ImFont*)0 == fontPTR.NativePtr)
                {

                    fontPTR = ImGui.GetIO().Fonts.AddFontFromFileTTF("FONT/ComicNeue-Bold.ttf", 18, null, ImGui.GetIO().Fonts.GetGlyphRangesDefault());

                    Game.ImGuiRenderer.RebuildFontAtlas();

                }
            }
            ImGui.PushFont(fontPTR);
            ImGuiUtils.SetStyle();
            SceneManager.Draw(gameTime);
            
            Game.ImGuiRenderer.AfterLayout();
            //cam.Scale = Vector2.One * PhysicsScale; 


            base.Draw(gameTime);
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            ss?.Dispose();
        }


        public static Texture2D LoadTexture(string s)
        {
            using var stream = File.OpenRead(s);
            Texture2D t2d = Texture2D.FromStream(Game._.gdm.GraphicsDevice, stream);
            byte[] data = new byte[t2d.Width * t2d.Height * 4];
            t2d.GetData(data);
            Span<Color> c = MemoryMarshal.Cast<byte, Color>(data.AsSpan());
            for (int i = 0; i < c.Length; i++) if (c[i] == Color.Magenta) c[i] = Color.Transparent;
            t2d.SetData(data);
            return t2d;
        }

        public static SoundEffect LoadSound(string s)
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

        public static StreamedSound LoadMusic(string s) => new(s);
    }
}
