using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Portal", Namespace = "SwarmNet")]
    public abstract class Portal<JI, JO, TI, TO> : SetPiece<JI, JO, TI, TO>
    {
        #region Methods

        /// <summary>
        /// Spawns a new agent on the node.
        /// </summary>
        /// <returns>The agent that will be placed on the node.</returns>
        public abstract Agent<JI, JO, TI, TO> Enter();
        /// <summary>
        /// Despawns an agent from the node.
        /// </summary>
        /// <param name="a">The agent to remove from the node.</param>
        public abstract void Leave(Agent<JI, JO, TI, TO> a);

        #endregion
    }
}
