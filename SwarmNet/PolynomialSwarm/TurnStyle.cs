using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace PolynomialSwarm
{
    class TurnStyle : Junction<int, int, object, Tuple<int, int>>
    {
        private int[] _divs;

        public TurnStyle(int max, int paths)
        {
            _max = max;
            _state = 0;
            _divs = new int[paths];

            // Evenly divide the state space between the paths
            int cur = max / paths,
                dif = cur;
            for(int i = 0; i < _divs.Length; i++)
            {
                _divs[i] = cur;
                cur += dif;
            }
        }

        public override Message<int> Communicate(Message<int> m)
        {
            switch (m.Type)
            {
                case CommType.RESPONSE:
                case CommType.FINISH:
                    _state = m.Contents % _max;
                    return new Message<int>(CommType.FINISH, _state);
                default:
                    return new Message<int>(CommType.RESPONSE, _state);
            }
        }

        public override int GetExit(int paths)
        {
            for (int i = 0; i < _divs.Length; i++)
            {
                if (_state < _divs[i])
                    return i;
            }

            return 0;
        }

        public override Message<int> InitComm()
        {
            return new Message<int>(CommType.START, _state);
        }
    }
}
