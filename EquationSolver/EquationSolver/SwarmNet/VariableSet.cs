using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class VariableSet : Agent
    {
        #region Static Variables

        private static Random _rand = new Random();

        #endregion

        #region Fields

        private bool _aggrAdjust;
        private int _attempts;
        private int[] _denominators;
        private bool[] _nextAdjust;
        private bool?[] _prevAdjust;

        #endregion

        public int Passed { get; private set; }
        public decimal[] Vars { get; private set; }

        #region Constructors

        public VariableSet(int id, int size, int min, int max)
        {
            _attempts = 0;
            _denominators = new int[size];
            _prevAdjust = new bool?[size];
            Passed = 0;
            Tag = id.ToString();
            Vars = new decimal[size];

            for (int i = 0; i < size; i++)
            {
                Vars[i] = _rand.Next(min, max);
                _denominators[i] = 1;
            }
        }

        #endregion

        #region Methods - Communication

        public override Message CommJunc(Message m)
        {
            switch (m.Type)
            {
                case CommType.INIT:
                    if ((int)m.Contents == 0)
                        return new Message(new Tuple<int, int>(_attempts, Passed), CommType.RESP);
                    else
                        return new Message(new Tuple<string, int, decimal[]>(Tag, _attempts, Vars), CommType.RESP);
                default:
                    if (m.Contents != null)
                    {
                        JuncMessage mssg = (JuncMessage)m.Contents;
                        switch (mssg.Tag)
                        {
                            case 0:
                                if (!mssg.Pass)
                                    Passed = 0;
                                break;
                            default:
                                if (mssg.Pass)
                                {
                                    for (int i = 0; i < _prevAdjust.Length; i++)
                                    {
                                        _denominators[i] = 1;
                                        _prevAdjust[i] = null;
                                    }
                                    Passed++;
                                }
                                else
                                {
                                    _nextAdjust = mssg.NextDir;
                                    _aggrAdjust = mssg.Possitive;
                                }
                                break;
                        }
                    }
                    return new Message(null, CommType.TERM);
            }
        }
        public override Message CommPort(Message m)
        {
            return new Message(new Tuple<string, int, decimal[]>(Tag, _attempts, Vars), CommType.RESP);
        }
        public override Message CommTerm(Message m)
        {
            Passed = 0;

            switch (m.Type)
            {
                case CommType.INIT:
                    return new Message(new Tuple<int[], bool[]> (_denominators.Select((d, i) => ((_prevAdjust[i] != null && _prevAdjust[i] != _nextAdjust[i] && _denominators[i] <= 12) ? d + 1 : d)).ToArray(), _nextAdjust), CommType.RESP);
                default:
                    if (m.Contents != null)
                    {
                        Tuple<int, decimal> cont = (Tuple<int, decimal>)m.Contents;
                        Vars[cont.Item1] = Math.Round(Vars[cont.Item1] + cont.Item2, 2);
                        if (_nextAdjust[cont.Item1] != _prevAdjust[cont.Item1] && _denominators[cont.Item1] <= 100)
                            _denominators[cont.Item1]++;
                        _prevAdjust[cont.Item1] = _nextAdjust[cont.Item1];
                    }
                    _attempts++;
                    return new Message(null, CommType.TERM);
            }
        }

        #endregion
    }
}
