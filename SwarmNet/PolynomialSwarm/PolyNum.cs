using System;
using SwarmNet;

namespace PolynomialSwarm
{
    class PolyNum : Agent<int, int, object, Tuple<int, int>>
    {
        private int[] _coeffs;

        public int[] Coefficients
        {
            get
            {
                return _coeffs;
            }
        }

        public PolyNum(string tag, int priority, int degree)
        {
            if (degree < 1)
                throw new ArgumentException("The degree of the PolyNum cannot be less than 1.");

            _coeffs = new int[degree];
            _priority = priority;
            _tag = tag;
        }

        public PolyNum(string tag, int priority, params int[] coeffs)
        {
            _coeffs = coeffs;
            _priority = priority;
            _tag = tag;
        }

        public override Message<int> CommJunc(Message<int> m)
        {
            int output = 0;

            switch (m.Type)
            {
                case CommType.START:
                    for (int i = 0; i < _coeffs.Length; i++)
                    {
                        output += (int)Math.Pow(m.Contents, i) * _coeffs[i];
                    }
                    break;
                default:
                    break;
            }

            return new Message<int>(CommType.FINISH, output);
        }

        public override Message<object> CommTerm(Message<Tuple<int, int>> m)
        {
            switch (m.Type)
            {
                case CommType.START:
                    // Get transformation from contents of message
                    Tuple<int, int> trans = m.Contents;
                    // Apply transformation to coefficients
                    if (trans.Item1 > -1 && trans.Item1 < _coeffs.Length)
                        _coeffs[trans.Item1] += trans.Item2;
                    break;
                default:
                    break;
            }

            return new Message<object>(CommType.FINISH, null);
        }
    }
}
