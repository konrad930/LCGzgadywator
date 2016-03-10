using System;
using System.Collections.Generic;
using System.Linq;

namespace LCGzgadywator
{
    internal class Program
    {
        public static void Main()
        {
            var ms = new Lcg(910955363);

            ms.Seq().Take(4).ToList().ForEach(Console.WriteLine);

            var dets = new List<long>();
            var input = new List<long> {910955363, 378365196, 368026102, 170059367, 516212983};
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

                var m = Gcd(dets.ToArray());
                var f = Factors((int) max, (int) m);

                m = f.FirstOrDefault(x => x >= max);

                int a = 0, c = 0;

                for (var i = 0; i < m; i++)
                {
                    if ((input[2] - input[1] == (input[1] - input[0])*i%m) ||
                        (input[3] - input[2] == (input[2] - input[1])*i%m))
                    {
                        a = i;
                        break;
                    }
                }

                for (var i = 0; i < m; i++)
                {
                    if (input[1]%m == (a*input[0] + i)%m)
                    {
                        c = i;
                        break;
                    }
                }
                var result = (a*input.Last() + c)%m;

                Console.WriteLine("Next : " + ms.Next() + " == " + result);
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
                b = a1%b;
            }
        }

        private static IEnumerable<int> Factors(int max, int n)
        {
            return Enumerable.Range(max, n).Where(x => n%x == 0).ToList();
        }

        #endregion
    }
}