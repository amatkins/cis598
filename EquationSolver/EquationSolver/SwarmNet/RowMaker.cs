using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class RowMaker : Port
    {
        #region Fields

        private int _maxAttempts, _maxRows, _maxRange, _minRange, _colNum;
        private int[] _rowNum;

        #endregion

        public bool Solved { get; private set; }
        public bool ShouldSpawn { get; private set; }
        public decimal[][] _solution { get; private set; }

        #region Constructors

        public RowMaker(int columns, int minValue, int maxValue, int maxRows, int maxAttempts)
        {
            _maxAttempts = maxAttempts;
            _maxRange = maxValue;
            _maxRows = maxRows;
            _minRange = minValue;
            _rowNum = new int[columns];
            ShouldSpawn = true;
            _solution = new decimal[columns][];
            _colNum = columns;
        }

        #endregion

        #region Methods - Communication

        public override Message Communicate(Message m)
        {
            switch (m.Type)
            {
                case CommType.RESP:
                    Tuple<string, int, decimal[]> mssg = (Tuple<string, int, decimal[]>)m.Contents;
                    int index = int.Parse(mssg.Item1);
                    if (mssg.Item2 > _maxAttempts)
                    {
                        if (_solution[index] == null)
                        {
                            _rowNum[index]--;
                            ShouldSpawn = true;
                        }
                    }
                    else
                    {
                        _solution[index] = mssg.Item3;
                        Solved = _solution.All(da => da != null);
                    }
                    break;
                default:
                    break;
            }
            return new Message(null, CommType.TERM);
        }
        public override Message InitComm()
        {
            return new Message(null, CommType.INIT);
        }

        #endregion

        #region Methods - Control Flow
        
        public override Agent Enter()
        {
            int newID = 0;

            for (int i = 0; i < _rowNum.Length; i++)
            {
                if (_rowNum[i] < _maxRows)
                {
                    _rowNum[i]++;
                    newID = i;
                    break;
                }
            }

            ShouldSpawn = _rowNum.Any(i => i < _maxRows);

            return new VariableSet(newID, _colNum, _minRange, _maxRange);
        }
        public override void Leave(Agent a)
        {
            return;
        }

        #endregion
    }
}
