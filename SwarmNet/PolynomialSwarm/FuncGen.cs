using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace PolynomialSwarm
{
    class FuncGen : Spawner<int, int, object, Tuple<int, int>>
    {
        private int _deg;
        private int _max;
        private Random _rand;
        static int _nextTag;

        public FuncGen(int degree, int max)
        {
            _deg = degree;
            _max = max;
            _rand = new Random();
            _nextTag = 0;
        }

        public override void Despawn(Agent<int, int, object, Tuple<int, int>> a)
        {
            // Do nothing
            return;
        }

        public override Agent<int, int, object, Tuple<int, int>> Spawn()
        {
            int[] coeffs = new int[_deg];
            int tag = _nextTag;

            for (int i = 0; i < coeffs.Length; i++)
            {
                coeffs[i] = _rand.Next(_max);
            }

            _nextTag++;

            return new PolyNum(string.Format("{0}", tag), 0, coeffs);
        }
    }
}
