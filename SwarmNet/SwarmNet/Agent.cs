using System;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Agent", Namespace = "SwarmNet")]
    public abstract class Agent<JI, JO, TI, TO> : IComparable<Agent<JI, JO, TI, TO>>
    {
        #region Fields

        /// <summary>
        /// Priority when multiple agents are on the same node.
        /// </summary>
        [DataMember(Name = "Priority")]
        protected int _priority;
        /// <summary>
        /// The identity tag unique to this agent.
        /// </summary>
        [DataMember(Name = "IDTag")]
        protected string _tag;

        #endregion

        #region Properties

        /// <summary>
        /// The priority of this agent in relation to other agents.
        /// </summary>
        public int Priority
        {
            get
            {
                return _priority;
            }
        }
        /// <summary>
        /// The unique identity tag of the agent.
        /// </summary>
        public string Tag
        {
            get
            {
                return _tag;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Communicate with a junction set piece.
        /// </summary>
        /// <param name="m">Incoming message from junction.</param>
        /// <returns>Outgoing response from agent.</returns>
        public abstract Message<JI> CommJunc(Message<JO> m);
        /// <summary>
        /// Communicate with a terminal set piece.
        /// </summary>
        /// <param name="m">Incoming message from terminal.</param>
        /// <returns>Outgoing responce from agent.</returns>
        public abstract Message<TI> CommTerm(Message<TO> m);
        /// <summary>
        /// Allows Two agents to be compared via their priority.
        /// </summary>
        /// <param name="a">The agent to compare to this one.</param>
        /// <returns>less 0 = this is higher priority, 0 = same priority, greater 0 = this is lower priority</returns>
        public int CompareTo(Agent<JI, JO, TI, TO> a)
        {
            return _priority.CompareTo(a._priority);
        }

        #endregion
    }
}
