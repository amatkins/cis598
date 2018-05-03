using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public enum CommType { INIT, RESP, TERM };

    public class Message
    {
        #region Properties

        /// <summary>
        /// The contents of the message.
        /// </summary>
        public object Contents { get; private set; }
        /// <summary>
        /// The type of message this is.
        /// </summary>
        public CommType Type { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new message of the given type and with the given contents.
        /// </summary>
        /// <param name="cont">Contents of the message.</param>
        /// <param name="type">Type of the message.</param>
        public Message(object cont, CommType type)
        {
            Contents = cont;
            Type = type;
        }

        #endregion
    }
}
