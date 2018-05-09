using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class ProblemColumn : Junction
    {
        #region Fields

        private int[] _coefs;
        private bool _fail;
        private decimal _mag;
        private int _maxAttempts;
        private bool _pass;
        private string _tag;

        #endregion

        #region Constructors

        public ProblemColumn(int id, int maxAttempts, int leaves, params int[] coefs)
        {
            _coefs = coefs;
            _maxAttempts = maxAttempts;
            Max = leaves;
            Min = 1;
            State = 0;
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
                        _fail = true;
                    }
                    else
                    {
                        _mag = Math.Round((_tag.Equals(mssg.Item1) ? 1 : 0) - _coefs.Select((c, i) => c * mssg.Item3[i]).Aggregate((a, b) => a + b), 1);
                        _pass = _mag == 0;
                        _fail = false;
                    }
                    return new Message(new Tuple<int, bool, bool, decimal>(1, _pass, _fail, _mag), CommType.RESP);
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
            int exit;

            if (_pass)
                return exit = Max + 1;
            else
            {
                exit = State;
                State = Min + (State + 1) % Max;
            }
                
            return exit;
        }

        #endregion
    }
}
