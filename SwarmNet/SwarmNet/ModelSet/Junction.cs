using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Junction", Namespace = "SwarmNet")]
    public abstract class Junction<JI, JO, TI, TO> : SetPiece<JI, JO, TI, TO>
    {
        #region Fields

        /// <summary>
        /// The max value of the 0-based state;
        /// </summary>
        [DataMember(Name = "Max")]
        protected int _max;
        /// <summary>
        /// The current numerical state of the junction, used for determining the exit path
        /// </summary>
        [DataMember(Name = "State")]
        protected int _state;

        #endregion

        #region Properties

        /// <summary>
        /// The max state that this junction can be.
        /// </summary>
        public int Max
        {
            get
            {
                return _max;
            }
        }
        /// <summary>
        /// The current state of this junction.
        /// </summary>
        public int State
        {
            get
            {
                return _state;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Communicate with an agent.
        /// </summary>
        /// <param name="m">The message from the agent.</param>
        /// <returns>The response from the junction.</returns>
        public abstract Message<JO> Communicate(Message<JI> m);
        /// <summary>
        /// Means of getting an exit path from the current state of the junction.
        /// </summary>
        /// <param name="paths">The number of exits.</param>
        /// <returns>The path the agent will leave by.</returns>
        public abstract int GetExit(int paths);
        /// <summary>
        /// Initiate communications with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public abstract Message<JO> InitComm();

        #endregion
    }
}