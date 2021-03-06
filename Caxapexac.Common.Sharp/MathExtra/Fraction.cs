using System;


namespace Caxapexac.Common.Sharp.MathExtra
{
    // https://rosettacode.org/wiki/Convert_decimal_number_to_rational#C.23
    public class Fraction
    {
        public long Numerator;
        public long Denominator;

        public Fraction(double f, long maximumDenominator = 4096)
        {
            /* Translated from the C version. */
            /*  a: continued fraction coefficients. */
            var h = new long[3] {0, 1, 0};
            var k = new long[3] {1, 0, 0};
            long n = 1;
            int i, neg = 0;

            if (maximumDenominator <= 1)
            {
                Denominator = 1;
                Numerator = (long)f;
                return;
            }

            if (f < 0)
            {
                neg = 1;
                f = -f;
            }

            while (f != Math.Floor(f))
            {
                n <<= 1;
                f *= 2;
            }
            var d = (long)f;

            /* continued fraction and check denominator each step */
            for (i = 0; i < 64; i++)
            {
                var a = (n != 0) ? d / n : 0;
                if ((i != 0) && (a == 0)) break;

                var x = d;
                d = n;
                n = x % n;

                x = a;
                if (k[1] * a + k[0] >= maximumDenominator)
                {
                    x = (maximumDenominator - k[0]) / k[1];
                    if (x * 2 >= a || k[1] >= maximumDenominator)
                        i = 65;
                    else
                        break;
                }

                h[2] = x * h[1] + h[0];
                h[0] = h[1];
                h[1] = h[2];
                k[2] = x * k[1] + k[0];
                k[0] = k[1];
                k[1] = k[2];
            }
            Denominator = k[1];
            Numerator = neg != 0 ? -h[1] : h[1];
        }

        public override string ToString()
        {
            return $"{Numerator} / {Denominator}";
        }
    }
}