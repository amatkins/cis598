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

        #endregion

        #region Constructors

        public Gate(int maxAttempts, int colNum)
        {
            _tot = colNum;
            _maxAttempts = maxAttempts;
            Max = 3;
            Min = 0;
            State = 1;
        }

        #endregion

        #region Methods- Communication

        public override Message Communicate(Message m)
        {
            switch (m.Type)
            {
                case CommType.RESP:
                    Tuple<int, int> mssg = (Tuple<int, int>)m.Contents;
                    State = mssg.Item1 > _maxAttempts || mssg.Item2 >= _tot ? 0 : 1;
                    return new Message(new Tuple<int, bool, bool, decimal>(0, State == 0, false, 0), CommType.RESP);
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
            return State;
        }

        #endregion
    }
}
