using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "SetPiece", Namespace = "SwarmNet")]
    [KnownType("GetKnownTypes")]
    public abstract class SetPiece<JI, JO, TI, TO>
    {
        #region Fields

        /// <summary>
        /// The node that this set piece belongs to.
        /// </summary>
        [DataMember(Name = "Location")]
        protected GraphNode<JI, JO, TI, TO> _loc;

        #endregion

        #region Properties

        /// <summary>
        /// The node that this set piece is on.
        /// </summary>
        public GraphNode<JI, JO, TI, TO> Location
        {
            get
            {
                return _loc;
            }
            set
            {
                _loc = value;
            }
        }

        #endregion

        #region Methods - DataContract

        /// <summary>
        /// Gets the collection of known types to be used for the DataContract.
        /// </summary>
        /// <returns>Collection of known types.</returns>
        private static Type[] GetKnownTypes()
        {
            List<Type> types = new List<Type>();

            types.Add(typeof(Portal<JI, JO, TI, TO>));
            types.Add(typeof(Junction<JI, JO, TI, TO>));
            types.Add(typeof(Terminal<JI, JO, TI, TO>));

            return types.ToArray();
        }

        #endregion
    }
}
