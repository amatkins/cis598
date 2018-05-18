using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Port", Namespace = "SwarmNet")]
    public abstract class Port : Station
    {
        #region Methods - Communication

        /// <summary>
        /// Dismiss an agent.
        /// </summary>
        /// <param name="m">The message from the agent.</param>
        /// <returns>The response from the port.</returns>
        public abstract Message Dismissal(Message m);
        /// <summary>
        /// Initiate dismissal of an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public abstract Message InitDism();

        #endregion

        #region Methods - Agent Handling

        /// <summary>
        /// Spawns a new agent on the node.
        /// </summary>
        /// <returns>The agent that will be placed on the node.</returns>
        public abstract Agent Enter();
        /// <summary>
        /// Despawns an agent from the node.
        /// </summary>
        /// <param name="a">The agent to remove from the node.</param>
        public abstract void Leave(Agent a);

        #endregion
    }
}
