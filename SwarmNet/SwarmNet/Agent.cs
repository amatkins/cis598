using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public abstract class Agent
    {
        #region Fields
        
        /// <summary>
        /// Priority when multiple agents are on the same node.
        /// </summary>
        private int _priority;
        /// <summary>
        /// The identity tag unique to this agent.
        /// </summary>
        private string _tag;

        #endregion
    }
}
