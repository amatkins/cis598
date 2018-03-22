using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace PolynomialSwarm
{
    class Transformer : Terminal<int, int, object, Tuple<int, int>>
    {
        private Tuple<int, int> _tran;

        public int Degree
        {
            get
            {
                return _tran.Item1;
            }
        }

        public int Change
        {
            get
            {
                return _tran.Item2;
            }
        }

        public Transformer(int deg, int mag)
        {
            _tran = new Tuple<int, int>(deg, mag);
        }

        public override Message<Tuple<int, int>> Communicate(Message<object> m)
        {
            return new Message<Tuple<int, int>>(CommType.FINISH, null);
        }

        public override Message<Tuple<int, int>> InitComm()
        {
            return new Message<Tuple<int, int>>(CommType.START, _tran);
        }
    }
}
