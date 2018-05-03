using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Port", Namespace = "SwarmNet")]
    public abstract class Port : SetPiece
    {
        #region Methods

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
