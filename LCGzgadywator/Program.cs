using System;
using System.Collections.Generic;
using System.Linq;

namespace LCGzgadywator
{
    internal class Program
    {
        public static void Main()
        {
            var ms = new Lcg(54294923);

            ms.Seq().Take(5).ToList().ForEach(Console.WriteLine);

            var dets = new List<long>();
            var input = ms.Seq().Take(10).ToList();
            var cycle = new List<long>();
            var max = input.Max();
            var agr = input.GroupBy(x => x);

            var enumerable = agr as IList<IGrouping<long, long>> ?? agr.ToList();
            if (enumerable.Count != input.Count)
                cycle.AddRange(enumerable.Select(g => g.Key));

            if (cycle.Any())
            {
                var i = cycle.Where(x => x.Equals(input.Last())).GetEnumerator();
                i.MoveNext();
                var next = i.MoveNext() ? i.Current : cycle[0];
                Console.WriteLine("Next : " + ms.Next() + " == " + next);
            }
            else
            {
                for (var i = 1; i < input.Count - 2; i++)
                    dets.Add(CalcDet(i, i + 1, input));

                var m = (int) Gcd(dets.ToArray());
                var f = Factors((int) max, m);

                m = f.FirstOrDefault(x => x >= max);

                var a = CalcA(input[0],input[1],input[2], m);
                var c = Mod(input[2] - (a*input[1]),m);

                var result = (a*input.Last() + c)%m;

                Console.WriteLine("Next  : " + ms.Next() + " == " + result);
                Console.WriteLine("M:" + m + " a: " + a + " c:" + c);
            }
            Console.ReadKey();
        }

        private static long CalcDet(int i, int j, IReadOnlyList<long> x)
        {
            var a1 = x[i] - x[0];
            var b1 = x[i + 1] - x[1];
            var a2 = x[j] - x[0];
            var b2 = x[j + 1] - x[1];

            return Math.Abs(a1*b2 - a2*b1);
        }

        #region Private Methods

        private static long Gcd(long[] numbers)
        {
            return numbers.Aggregate(Gcd);
        }

        private static long Gcd(long a, long b)
        {
            while (true)
            {
                if (b == 0) return a;
                var a1 = a;
                a = b;
                b = Mod(a1,b);
            }
        }

        private static long Mod(long x, long m)
        {
            return (x % m + m) % m;
        }

        private static IEnumerable<int> Factors(int max, int n)
        {
            return Enumerable.Range(max, n).Where(x => n%x == 0).ToList();
        }

        static void ExtendedEuclid(long a, long b,out long d, out long x, out long y)
        {
            long q, r, x1, x2, y1, y2;
            d = a;
            x = 1;
            y =0;

            if (b == 0)
            {
                d = a; x = 1; y = 0;
                return;
            }
            x2 = 1; x1 = 0; y2 = 0; y1 = 1;
            while (b > 0)
            {
                q = a / b; r = a - q * b;
                x = x2 - q * x1; y = y2 - q * y1;
                a = b; b = r;
                x2 = x1; x1 = x; y2 = y1; y1 = y;
            }
            Console.WriteLine(a + " " + x2 + " " + y2);

            d = a; x = x2; y = y2;
        }


        public static  long CalcA(long x0,long x1,long x2,long m)
        {
            List<long> result = new List<long>();

            long d = 0,x=0,y=0;

            var b = Mod(x2-x1, m);
            var a = Mod(x1 - x0, m);

            ExtendedEuclid(a, m,out d,out x,out y);

            var t = x * Mod(b / d, m);

            for(int i=0;i<= d; i++)
            {
                result.Add(Mod(t + (i * (m / d)), m));
            }
            return result.FirstOrDefault();
        }

        #endregion
    }
}