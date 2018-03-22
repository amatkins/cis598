using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public abstract class Terminal<JI, JO, TI, TO> : SetPiece<JI, JO, TI, TO>
    {
        #region Methods

        /// <summary>
        /// Communicate with an agent.
        /// </summary>
        /// <param name="m">The message from the agent.</param>
        /// <returns>The response from the terminal.</returns>
        public abstract Message<TO> Communicate(Message<TI> m);
        /// <summary>
        /// Initiate communications with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public abstract Message<TO> InitComm();

        #endregion
    }
}
