using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class SolutionRow : Agent
    {
        #region Static Variables

        private static Random _rand = new Random();

        #endregion

        #region Fields

        private int _adjustments, _attempts;

        #endregion

        public int Passed { get; private set; }
        public decimal AdjustMag { get; private set; }
        public decimal[] Columns { get; private set; }

        #region Constructors

        public SolutionRow(int id, int size, int min, int max)
        {
            AdjustMag = 0;
            _adjustments = 0;
            _attempts = 0;
            Columns = new decimal[size];
            Passed = 0;
            Tag = id.ToString();

            for (int i = 0; i < size; i++)
            {
                Columns[i] = _rand.Next(min, max) * 0.1M;
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
                        return new Message(new Tuple<string, int, decimal[]>(Tag, _adjustments, Columns), CommType.RESP);
                default:
                    if (m.Contents != null)
                    {
                        Tuple<int, bool, bool, decimal> mssg = (Tuple<int, bool, bool, decimal>)m.Contents;
                        switch (mssg.Item1)
                        {
                            case 0:
                                if (!mssg.Item2)
                                {
                                    Passed = 0;
                                    _attempts++;
                                }
                                break;
                            default:
                                if (mssg.Item2)
                                {
                                    if (!mssg.Item3)
                                        Passed++;
                                    _adjustments = 0;
                                }
                                else
                                {
                                    _adjustments++;
                                    AdjustMag = mssg.Item4;
                                }
                                break;
                        }
                    }
                    return new Message(null, CommType.TERM);
            }
        }
        public override Message CommPort(Message m)
        {
            return new Message(new Tuple<string, int, decimal[]>(Tag, _attempts, Columns), CommType.RESP);
        }
        public override Message CommTerm(Message m)
        {
            Passed = 0;

            switch (m.Type)
            {
                case CommType.INIT:
                    return new Message(AdjustMag, CommType.RESP);
                default:
                    if (m.Contents != null)
                    {
                        Tuple<int, bool> cont = (Tuple<int, bool>)m.Contents;

                        Columns[cont.Item1] += (cont.Item2 ? 0.1M : -0.1M);
                    }
                    return new Message(null, CommType.TERM);
            }
        }

        public override int GetExit(int paths)
        {
            return 0;
        }

        #endregion
    }
}
