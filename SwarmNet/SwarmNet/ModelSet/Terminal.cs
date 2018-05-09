using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Terminal", Namespace = "SwarmNet")]
    public abstract class Terminal : SetPiece
    {
        #region Properties
        
        /// <summary>
        /// The current input into this terminal.
        /// </summary>
        [DataMember]
        public object Input { protected get; set; }
        /// <summary>
        /// The current output from this terminal.
        /// </summary>
        [DataMember]
        public object Output { get; protected set; }

        #endregion
    }
}
