using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using DSP.FourierTransformations;

namespace DSP.Correlations
{
    class FastCorrelation : ICorrelation
    {
        public CorrelationType Type { get; set; } = CorrelationType.Fast;
        public double[] FindCorrelation(double[] firstSignal, double[] secondSignal)
        {
            int count = GetUpperBoundPowerOf2(firstSignal.Length + secondSignal.Length - 1);

            var firstArray = new double[count];
            firstSignal.CopyTo(firstArray, secondSignal.Length - 1);

            var secondArray = new double[count];
            secondSignal.CopyTo(secondArray, 0);

            var firstFFT = FastFourierTransform.FFT(firstArray.ToList());

            var secondFFT = FastFourierTransform.FFT(secondArray.ToList());

            List<Complex> result = new List<Complex>();

            for (int i = 0; i < firstFFT.Count(); i++)
            {
                Complex conjugate = Conjugate(firstFFT[i]);
                Complex product = conjugate * secondFFT[i];
                Complex normalizedProduct = product / count;
                result.Add(normalizedProduct);
            }

            double[] finalResult = FastFourierTransform.RFFT(result).Select(c => c.Real).ToArray();

            return finalResult;
        }

        public static Complex Conjugate(Complex c)
        {
            return new Complex(c.Real, -c.Imaginary);
        }

        static int GetUpperBoundPowerOf2(int number)
        {
            if ((number & (number - 1)) == 0)
            {
                return number << 1;
            }

            int nextPowerOfTwo = 1;
            while (nextPowerOfTwo < number)
            {
                nextPowerOfTwo <<= 1;
            }

            return nextPowerOfTwo;
        }
    }
}
