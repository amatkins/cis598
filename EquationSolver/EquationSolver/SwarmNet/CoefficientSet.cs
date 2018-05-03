using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class CoefficientSet : Junction
    {
        #region Fields

        private int[] _coefs;
        private int _maxAttempts;
        private bool[] _nextDirs;
        private bool _dir, _pass;
        private string _tag;

        #endregion

        #region Constructors

        public CoefficientSet(int id, int max, params int[] coefs)
        {
            _coefs = coefs;
            _maxAttempts = max;
            _pass = false;
            _tag = id.ToString();
        }

        #endregion

        #region Methods - Communication

        public override Message Communicate(Message m)
        {
            switch (m.Type)
            {
                case CommType.RESP:
                    Tuple<string, int, decimal[]> mssg = (Tuple<string, int, decimal[]>)m.Contents;
                    if (mssg.Item2 > _maxAttempts)
                    {
                        _pass = true;
                        _dir = false;
                    }
                    else
                    {
                        decimal result = Math.Round((_tag.Equals(mssg.Item1)? 1 : 0) - _coefs.Select((c, i) => c * mssg.Item3[i]).Aggregate((a, b) => a + b), 2);
                        _pass = result == 0;
                        _dir = result > -1;
                        _nextDirs = _coefs.Select(c => c < 0 ? !_dir : _dir).ToArray();
                    }
                    return new Message(new JuncMessage(1, _pass, _dir, _nextDirs), CommType.RESP);
                default:
                    return new Message(null, CommType.TERM);
            }

        }
        public override Message InitComm()
        {
            return new Message(1, CommType.INIT);
        }

        #endregion

        #region Methods - Control Flow

        public override int GetExit(int paths)
        {
            return _pass ? 2 : 1;
        }

        #endregion
    }
}
