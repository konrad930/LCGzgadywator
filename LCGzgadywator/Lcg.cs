using System.Collections.Generic;

namespace LCGzgadywator
{
    public class Lcg
    {
        private long _state;

        public Lcg(int state)
        {
            _state = state;
        }

        public long Next()
        {
            return _state = Mod(2*_state + 5, 295075153);
        }

        public IEnumerable<long> Seq()
        {
            while (true)
                yield return Next();
        }

        private static long Mod(long x, long m)
        {
            return (x % m + m) % m;
        }
    }
}
