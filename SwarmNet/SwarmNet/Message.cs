using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public enum CommType { START, RESPONSE, FINISH };

    public class Message<T>
    {
        #region Fields

        /// <summary>
        /// The contents of this message.
        /// </summary>
        private T _cont;
        /// <summary>
        /// The type of message this is.
        /// </summary>
        private CommType _type;

        #endregion

        #region Properties

        /// <summary>
        /// The contents of the message.
        /// </summary>
        public T Contents
        {
            get
            {
                return _cont;
            }
        }
        /// <summary>
        /// The type of message this is.
        /// </summary>
        public CommType Type
        {
            get
            {
                return _type;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new message of the specific type, with the specifed contents, and from the specified type of sender.
        /// </summary>
        /// <param name="comm">What type of message it is.</param>
        /// <param name="cont">What is in the message.</param>
        public Message(CommType comm, T cont)
        {
            _cont = cont;
            _type = comm;
        }

        #endregion
    }
}
