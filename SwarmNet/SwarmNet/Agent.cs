using System;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Agent", Namespace = "SwarmNet")]
    public abstract class Agent : IComparable<Agent>
    {
        #region Properties

        /// <summary>
        /// The priority of this agent in relation to other agents.
        /// </summary>
        [DataMember]
        public int Priority { get; set; }
        /// <summary>
        /// The unique identity tag of the agent.
        /// </summary>
        [DataMember]
        public string Tag { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Communicate with a port set piece.
        /// </summary>
        /// <param name="m">Incoming message from port.</param>
        /// <returns>Outgoing responce from agent.</returns>
        public abstract Message CommPort(Message m);
        /// <summary>
        /// Communicate with a junction set piece.
        /// </summary>
        /// <param name="m">Incoming message from junction.</param>
        /// <returns>Outgoing response from agent.</returns>
        public abstract Message CommJunc(Message m);
        /// <summary>
        /// Communicate with a terminal set piece.
        /// </summary>
        /// <param name="m">Incoming message from terminal.</param>
        /// <returns>Outgoing responce from agent.</returns>
        public abstract Message CommTerm(Message m);
        /// <summary>
        /// Allows Two agents to be compared via their priority.
        /// </summary>
        /// <param name="a">The agent to compare to this one.</param>
        /// <returns>less 0 = this is higher priority, 0 = same priority, greater 0 = this is lower priority</returns>
        public int CompareTo(Agent a)
        {
            return Priority.CompareTo(a.Priority);
        }

        #endregion
    }
}
