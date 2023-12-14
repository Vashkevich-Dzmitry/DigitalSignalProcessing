using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DSP.FourierTransformations
{
    public static class FastFourierTransform
    {

        public static Complex[] RFFT(IList<Complex> input)
        {
            for (int i = 0; i < input.Count; ++i)
            {
                input[i] = Complex.Conjugate(input[i]);
            }

            return FFT(input).Select(Complex.Conjugate).Select(i => i / input.Count).ToArray();
        }

        public static Complex[] FFT(IList<double> input) => FFT(input.Select(i => new Complex(i, 0)).ToList());

        public static Complex[] FFT(IList<Complex> input)
        {
            var reversedBits = GetReversedBitsArray(input.Count);

            var data = new Complex[input.Count];

            for (int i = 0; i < input.Count; ++i)
            {
                var reversedIndex = reversedBits[i];
                data[reversedIndex] = input[i];
            }

            for (int length = 2; length <= input.Count; length <<= 1)
            {
                var angle = 2 * Math.PI / length;
                var Wn = new Complex(Math.Cos(angle), Math.Sin(angle));
                for (int i = 0; i < input.Count - 1; i += length)
                {
                    var w = new Complex(1, 0);
                    for (int j = 0; j < length / 2; ++j)
                    {
                        int vI = i + j;
                        int uI = i + j + length / 2;
                        var u = data[vI];
                        var v = data[uI] * w;

                        data[vI] = u + v;
                        data[uI] = u - v;

                        w *= Wn;
                    }
                }
            }

            return data;
        }

        private static int[] GetReversedBitsArray(int length)
        {
            var bitsCount = GetBitsCount(length);

            var result = new int[length];
            for (int i = 0; i < length; ++i)
            {
                result[i] = GetReversedBits(i, bitsCount);
            }

            return result;
        }

        private static int GetReversedBits(int number, int bitsCount)
        {
            int result = 0;
            int bitCheckMask = 1;
            int bitSetMask = 1 << (bitsCount - 1);
            for (int i = 0; i < bitsCount; ++i)
            {
                if ((number & bitCheckMask) != 0)
                {
                    result |= bitSetMask;
                }
                bitCheckMask <<= 1;
                bitSetMask >>= 1;
            }
            return result;
        }

        private static int GetBitsCount(int number)
        {
            var result = 0;
            var temp = 1;
            while (temp < number)
            {
                result++;
                temp <<= 1;
            }

            return result;
        }
    }
}
