using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Junction", Namespace = "SwarmNet")]
    public abstract class Junction : SetPiece
    {
        #region Properties

        /// <summary>
        /// The max state that this junction can be.
        /// </summary>
        [DataMember]
        public int Max { get; protected set; }
        /// <summary>
        /// The current state of this junction.
        /// </summary>
        [DataMember]
        public int State { get; protected set; }

        #endregion

        #region Methods

        /// <summary>
        /// Means of getting an exit path from the current state of the junction.
        /// </summary>
        /// <param name="paths">The number of exits.</param>
        /// <returns>The path the agent will leave by.</returns>
        public abstract int GetExit(int paths);

        #endregion
    }
}