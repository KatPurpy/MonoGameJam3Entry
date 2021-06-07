using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameJam3Entry
{
    //great thanks to sir techpizza
    class FunnyAudioUtils
    {
        /// <summary>
        /// Convert buffer containing IEEE 32-bit float data to a 16-bit signed PCM buffer.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public static void ConvertSingleToInt16(ReadOnlySpan<float> src, Span<short> dst)
        {
            if (dst.Length < src.Length)
                throw new ArgumentException("The destination span is too small.");

            if (Vector.IsHardwareAccelerated)
            {
                var vMax = new Vector<float>(short.MaxValue);
                var vMin = new Vector<float>(short.MinValue);
                var vHalf = new Vector<float>(0.5f);

                while (src.Length >= Vector<float>.Count * 2)
                {
                    var vSrc1 = new Vector<float>(src);
                    src = src[Vector<float>.Count..];
                    var vResult1 = Vector.Add(Vector.Multiply(vSrc1, vMax), vHalf);
                    vResult1 = Vector.Max(vMin, Vector.Min(vResult1, vMax));
                    var vIntResult1 = Vector.ConvertToInt32(vResult1);

                    var vSrc2 = new Vector<float>(src);
                    src = src[Vector<float>.Count..];
                    var vResult2 = Vector.Add(Vector.Multiply(vSrc2, vMax), vHalf);
                    vResult2 = Vector.Max(vMin, Vector.Min(vResult2, vMax));
                    var vIntResult2 = Vector.ConvertToInt32(vResult2);

                    var vResult = Vector.Narrow(vIntResult1, vIntResult2);
                    vResult.CopyTo(dst);
                    dst = dst[Vector<short>.Count..];
                }
            }
            else
            {


                for (int i = 0; i < src.Length; i++)
                {
                    int tmp = (int)(src[i] * short.MaxValue);
                    if (tmp > short.MaxValue)
                        dst[i] = short.MaxValue;
                    else if (tmp < short.MinValue)
                        dst[i] = short.MinValue;
                    else
                        dst[i] = (short)tmp;
                }
            }
        }
    }
}
