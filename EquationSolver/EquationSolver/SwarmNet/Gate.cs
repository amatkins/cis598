using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class Gate : Junction
    {
        #region Fields

        private int _tot, _maxAttempts;
        private bool _pass;

        #endregion

        #region Constructors

        public Gate(int max, int eqNum)
        {
            _tot = eqNum;
            _pass = false;
            _maxAttempts = max;
        }

        #endregion

        #region Methods- Communication

        public override Message Communicate(Message m)
        {
            switch (m.Type)
            {
                case CommType.RESP:
                    Tuple<int, int> mssg = (Tuple<int, int>)m.Contents;
                    _pass = mssg.Item1 > _maxAttempts || mssg.Item2 >= _tot;
                    return new Message(new JuncMessage(0, _pass, false), CommType.RESP);
            }
            return new Message(null, CommType.TERM);
        }
        public override Message InitComm()
        {
            return new Message(0, CommType.INIT);
        }

        #endregion

        #region Methods - Control Flow

        public override int GetExit(int paths)
        {
            return _pass ? 0 : 1;
        }

        #endregion
    }
}
