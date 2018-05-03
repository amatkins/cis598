using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class VariableAdjuster : Terminal
    {
        #region Fields

        private decimal _adjust;
        private int _ind;

        #endregion

        #region Methods - Communication

        public override Message Communicate(Message m)
        {
            Tuple<int[], bool[]> sum = (Tuple<int[], bool[]>)m.Contents;

            switch (m.Type)
            {
                case CommType.RESP:
                    _ind = Array.IndexOf(sum.Item1, sum.Item1.Min());
                    _adjust = 1M / (sum.Item2[_ind] ? sum.Item1[_ind] : -sum.Item1[_ind]);
                    return new Message(new Tuple<int, decimal>(_ind, _adjust), CommType.RESP);
                default:
                    return new Message(null, CommType.TERM);
            }
        }
        public override Message InitComm()
        {
            return new Message(null, CommType.INIT);
        }

        #endregion
    }
}
