using Microsoft.Xna.Framework.Audio;
using NVorbis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using tainicom.Aether.Physics2D;

namespace MonoGameJam3Entry
{
    public class StreamedSound : IDisposable
    {
        VorbisReader reader;
        public DynamicSoundEffectInstance sound;
        Thread thread;

        int samplesReady = 0;
        int buffersToRead = 3;

        const int BufferSize = 8192;
        float[] _readBuffer = new float[BufferSize];
        byte[] _castBuffer = new byte[BufferSize * sizeof(short)];

        public StreamedSound(string s)
        {
            reader = new VorbisReader(s);
            reader.ClipSamples = false;
            sound = new DynamicSoundEffectInstance(44100, AudioChannels.Stereo);
            sound.BufferNeeded += Sound_BufferNeeded;
        }

        public void Play()
        {
            sound.Play();
            thread = new Thread(new ThreadStart(AudioThread));
            thread.Start();
        }

        private void Sound_BufferNeeded(object sender, EventArgs e)
        {
            Interlocked.Increment(ref buffersToRead);
        }

        public void Update()
        {
            lock (uploadMutex)
            {
                if (samplesReady > 0)
                {
                    sound.SubmitBuffer(_castBuffer, 0, samplesReady * sizeof(short));
                    samplesReady = 0;
                }
            }
        }

        public void Pause()
        {
            sound.Pause();
        }
        readonly object uploadMutex = new();
        bool stopThread;
        void AudioThread()
        {
            while (!stopThread)
            {
                lock (uploadMutex)
                {
                    if (samplesReady == 0 && buffersToRead > 0)
                    {
                        Interlocked.Decrement(ref buffersToRead);
                        decode:;
                        int readSamples = reader.ReadSamples(_readBuffer, 0, BufferSize);
                        if (readSamples > 0)
                        {
                            Span<float> dataSpan = _readBuffer.AsSpan(0, readSamples);
                            Span<short> castSpan = MemoryMarshal.Cast<byte, short>(_castBuffer).Slice(0, readSamples);
                            FunnyAudioUtils.ConvertSingleToInt16(dataSpan, castSpan);
                            Interlocked.Add(ref samplesReady, readSamples);
                        }
                        else
                        {
                            reader.DecodedTime = TimeSpan.Zero;
                            goto decode;
                        }

                    }
                }
                Thread.Sleep(10);
            }
        }

        public void Dispose()
        {
            reader.Dispose();
            stopThread = true;
            sound.Dispose();

        }
    }
}

