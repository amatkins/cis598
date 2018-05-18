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

        #region Methods - Communication

        /// <summary>
        /// Communicate with a junction station.
        /// </summary>
        /// <param name="m">Incoming message from junction.</param>
        /// <returns>Outgoing response from agent.</returns>
        public abstract Message CommJunc(Message m);
        /// <summary>
        /// Communicate with a port station upon entering the graph.
        /// </summary>
        /// <param name="m">Incoming message from port.</param>
        /// <returns>Outgoing responce from agent.</returns>
        public abstract Message CommPort(Message m);
        /// <summary>
        /// Communicate with a terminal station.
        /// </summary>
        /// <param name="m">Incoming message from terminal.</param>
        /// <returns>Outgoing responce from agent.</returns>
        public abstract Message CommTerm(Message m);
        /// <summary>
        /// Communicate with a port station upon leaving the graph.
        /// </summary>
        /// <param name="m">Incoming message from port.</param>
        /// <returns>Outgoing response from agent.</returns>
        public abstract Message Dismissal(Message m);

        #endregion

        #region Methods - Graph Interactions

        /// <summary>
        /// The means of acquiring an exit path from the node.
        /// </summary>
        /// <param name="paths">The number of exits.</param>
        /// <returns>The path the agent will leave through.</returns>
        public abstract int GetExit(int paths);

        #endregion

        #region Methods - Logistics

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
