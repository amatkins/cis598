using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Junction", Namespace = "SwarmNet")]
    public abstract class Junction : Station
    {
        #region Properties

        /// <summary>
        /// The maximum value of the state.
        /// </summary>
        [DataMember]
        public int Max { get; protected set; }
        /// <summary>
        /// The minimum value of the state.
        /// </summary>
        [DataMember]
        public int Min { get; protected set; }
        /// <summary>
        /// The current state of this junction.
        /// </summary>
        [DataMember]
        public int State { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// The means of aquiring an exit from the node.
        /// </summary>
        /// <param name="paths">The number of exits.</param>
        /// <returns>The path the agent will leave through.</returns>
        public abstract int GetExit(int paths);

        #endregion
    }
}