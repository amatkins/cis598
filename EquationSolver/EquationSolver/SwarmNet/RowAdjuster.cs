using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class RowAdjuster : Terminal
    {
        #region Fields

        private int[] _rows;
        private int _ind;
        private bool _dir;

        #endregion

        public RowAdjuster(int[] rows)
        {
            _rows = rows;
        }

        #region Methods - Communication

        public override Message Communicate(Message m)
        {
            switch (m.Type)
            {
                case CommType.RESP:
                    decimal cur, mag1, mag2, cont = (decimal)m.Contents;

                    _ind = 0;
                    cur = 0;
                    for (int i = 0; i < _rows.Length; i++)
                    {
                        mag1 = 0.1M * _rows[i];
                        mag2 = -0.1M * _rows[i];

                        if (cont >= 0 ? (mag1 <= cont && mag1 > cur) : (mag1 >= cont && mag1 < cur))
                        {
                            _ind = i;
                            cur = mag1;
                            _dir = true;
                        }
                        if (cont >= 0 ? (mag2 <= cont && mag2 > cur) : (mag2 >= cont && mag2 < cur))
                        {
                            _ind = i;
                            cur = mag2;
                            _dir = false;
                        }
                    }
                    return new Message(new Tuple<int, bool>(_ind, _dir), CommType.RESP);
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
