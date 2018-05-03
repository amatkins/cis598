using System;
using System.Runtime.Serialization;
using SwarmNet;

namespace PolyE
{
    [DataContract(Name = "Modification", Namespace = "PolyE")]
    public class Modification : Terminal
    {
        #region Fields

        /// <summary>
        /// The operation to perform on the coefficient.
        /// </summary>
        [DataMember(Name = "Operation")]
        private AlgOp _oper;
        /// <summary>
        /// The term of the coefficient.
        /// </summary>
        [DataMember(Name = "Term")]
        private int _term;
        /// <summary>
        /// The value to modifiy the cofficient.
        /// </summary>
        [DataMember(Name = "Value")]
        private int _val;

        #endregion

        #region Properties

        /// <summary>
        /// The operation to perform on the coefficient.
        /// </summary>
        public AlgOp Operation
        {
            get
            {
                return _oper;
            }
        }
        /// <summary>
        /// The term of the coefficient.
        /// </summary>
        public int Term
        {
            get
            {
                return _term;
            }
        }
        /// <summary>
        /// The value to modifiy the cofficient.
        /// </summary>
        public int Value
        {
            get
            {
                return _val;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new terminal.
        /// </summary>
        /// <param name="term">The term of the coefficient.</param>
        /// <param name="oper">The operation to perform on the coefficient.</param>
        /// <param name="val">The value to modifiy the cofficient.</param>
        public Modification(int term, AlgOp oper, int val)
        {
            _oper = oper;
            _term = term;
            _val = val;
        }

        #endregion

        #region Methods - Logistics

        /// <summary>
        /// Converts this modification to a string.
        /// </summary>
        /// <returns>String representation of this modification.</returns>
        public override string ToString()
        {
            return string.Format("{0} {1} {2}", _term, _oper, _val);
        }

        #endregion

        #region Methods - Terminal Operations

        /// <summary>
        /// Terminates the transaction.
        /// </summary>
        /// <param name="m">Message containing the updated term.</param>
        /// <returns>Termination message containing the term modification.</returns>
        public override Message Communicate(Message m)
        {
            return new Message(null, CommType.TERM);
        }
        /// <summary>
        /// Initiates the transaction.
        /// </summary>
        /// <returns>Initial message containing the term modification.</returns>
        public override Message InitComm()
        {
            return new Message(new Tuple<int, int, AlgOp>(_term, _val, _oper), CommType.INIT);
        }

        #endregion
    }
}
