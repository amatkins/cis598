using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "SetPiece", Namespace = "SwarmNet")]
    [KnownType("GetKnownTypes")]
    public abstract class SetPiece
    {
        #region Properties

        /// <summary>
        /// The node that this set piece is on.
        /// </summary>
        [DataMember]
        public GraphNode Location { get; set; }

        #endregion

        #region Methods - Communication

        /// <summary>
        /// Communicate with an agent.
        /// </summary>
        /// <param name="m">The message from the agent.</param>
        /// <returns>The response from the setpiece.</returns>
        public abstract Message Communicate(Message m);
        /// <summary>
        /// Initiate communications with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public abstract Message InitComm();

        #endregion

        #region Methods - DataContract

        /// <summary>
        /// Gets the collection of known types to be used for the DataContract.
        /// </summary>
        /// <returns>Collection of known types.</returns>
        private static Type[] GetKnownTypes()
        {
            List<Type> types = new List<Type>();

            types.Add(typeof(Port));
            types.Add(typeof(Junction));
            types.Add(typeof(Terminal));

            return types.ToArray();
        }

        #endregion
    }
}
