using System;
using System.Linq;
using System.Runtime.Serialization;
using SwarmNet;

namespace PolyE
{
    [DataContract(Name = "AlgOp", Namespace = "PolyE")]
    public enum AlgOp {
        [EnumMember(Value = "Ad")]
        ADD,
        [EnumMember(Value = "Mu")]
        MUL
    };

    [DataContract(Name = "Expression", Namespace = "PolyE")]
    class Expression : Agent
    {
        #region Fields

        /// <summary>
        /// The coefficients of this expression.
        /// </summary>
        [DataMember(Name = "Coefficients")]
        private int[] _coeffs;

        #endregion

        #region Properties

        /// <summary>
        /// The coefficients of this expression.
        /// </summary>
        public int[] Coefficients
        {
            get
            {
                return _coeffs;
            }
        }
        /// <summary>
        /// The degree of this expression.
        /// </summary>
        public int Degree
        {
            get
            {
                return _coeffs.Length;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new expression.
        /// </summary>
        /// <param name="coeffs">The coefficients of the expression.</param>
        public Expression(string tag, int priority, params int[] coeffs)
        {
            _coeffs = coeffs;
            Priority = priority;
            Tag = tag;
        }

        #endregion

        #region Methods - Term Operations

        /// <summary>
        /// Modifies a term of the expression.
        /// </summary>
        /// <param name="index">The term to modify.</param>
        /// <param name="value">The value to use to modifiy the term.</param>
        /// <param name="op">How to modify the term.</param>
        public void ModTerm(int index, int value, AlgOp op)
        {
            if (index < 0 || index >= _coeffs.Length)
                return;

            switch (op)
            {
                case AlgOp.ADD:
                    _coeffs[index] += value;
                    break;
                case AlgOp.MUL:
                    _coeffs[index] *= value;
                    break;
            }
        }
        /// <summary>
        /// Returns a function representing a term of the expression.
        /// </summary>
        /// <param name="index">The term to get.</param>
        /// <returns>A function of the term of the expression.</returns>
        public Func<int, int> GetTerm(int index)
        {
            if (index < 0 || index >= _coeffs.Length)
                return null;

            return x => _coeffs[index] * (int)Math.Pow(x, index);
        }
        /// <summary>
        /// Returns a string representation of a term of the expression.
        /// </summary>
        /// <param name="index">The term to get.</param>
        /// <returns>A string of the term of the expression.</returns>
        public string TermToString(int index)
        {
            if (index < 0 || index >= _coeffs.Length)
                return null;

            return string.Format("{0}*x^{1}", _coeffs[index], index);
        }

        #endregion

        #region Methods - Whole-Expression Operations
        
        /// <summary>
        /// Evaluate the value for x in the expression.
        /// </summary>
        /// <param name="x">The value to use in the expression.</param>
        /// <returns>The result of the evaluation.</returns>
        public int Evaluate(int x)
        {
            return _coeffs.Select((c, i) => c * (int)Math.Pow(x, i)).Aggregate((a, b) => a + b);
        }
        /// <summary>
        /// Converts the expression to a string.
        /// </summary>
        /// <returns>The string representation of this expression.</returns>
        public override string ToString()
        {
            return string.Format("Id: {0} Exprn: {1}", Tag, string.Join("+", _coeffs.Select((c, i) => c == 0 ? "" : string.Format("{0}*x^{1}", c, i)).Where(x => !x.Equals("")).ToArray()));
        }

        #endregion

        #region Methods - Transactions

        /// <summary>
        /// Evaluates the value of the message and returns the output.
        /// </summary>
        /// <param name="m">The message containing the state of the junction.</param>
        /// <returns>The finish message containing the new state.</returns>
        public override Message CommJunc(Message m)
        {
            if (m.Contents != null)
                return new Message(Evaluate((int)m.Contents), CommType.TERM);

            return new Message(null, CommType.TERM);
        }
        /// <summary>
        /// Communication between agents and ports is unnecessary so a null terminator is sent.
        /// </summary>
        /// <param name="m">Incoming message from port.</param>
        /// <returns>Null terminating message.</returns>
        public override Message CommPort(Message m)
        {
            return new Message(null, CommType.TERM);
        }
        /// <summary>
        /// Modifies the expression using the mod in the message.
        /// </summary>
        /// <param name="m">The message containing the mod from the terminal.</param>
        /// <returns>The finish message containing the new term in the expression.</returns>
        public override Message CommTerm(Message m)
        {
            if (m.Contents != null)
            {
                Tuple<int, int, AlgOp> mssg = (Tuple<int, int, AlgOp>)m.Contents;
                ModTerm(mssg.Item1, mssg.Item2, mssg.Item3);
                return new Message(TermToString(mssg.Item1), CommType.TERM);
            }
            return new Message(null, CommType.TERM);
        }

        public override int GetExit(int paths)
        {
            return 0;
        }

        public override Message Dismissal(Message m)
        {
            return new Message(null, CommType.TERM);
        }

        #endregion
    }
}
