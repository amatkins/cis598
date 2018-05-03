using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Leaf", Namespace = "SwarmNet")]
    public class LeafNode : GraphNode
    {
        #region Properties

        /// <summary>
        /// The exit from this node.
        /// </summary>
        public override GraphNode Exit
        {
            get
            {
                return Neighbors[0];
            }
        }
        /// <summary>
        /// The terminal on this node.
        /// </summary>
        public Terminal Terminal
        {
            get
            {
                return (Terminal)Piece;
            }
            set
            {
                Piece = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new leaf node.
        /// </summary>
        public LeafNode()
        {
            In = new List<Agent>();
            Neighbors = new GraphNode[1];
            _nextNeighbor = 0;
            Out = new List<Agent>();
            Piece = null;
        }
        /// <summary>
        /// Constructs a new leaf node that contains the provided terminal.
        /// </summary>
        /// <param name="terminal">The terminal this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public LeafNode(Terminal terminal)
        {
            In = new List<Agent>();
            Neighbors = new GraphNode[1];
            _nextNeighbor = 0;
            Out = new List<Agent>();
            Piece = terminal;
            terminal.Location = this;
        }

        #endregion

        #region Methods - Set Piece Operations
        
        /// <summary>
        /// Acts as a middle man between set piece and agent.
        /// </summary>
        /// <param name="m">The message to pass to the set piece.</param>
        /// <returns>The response from the set piece.</returns>
        public Message Communicate(Message m)
        {
            return ((Terminal)Piece).Communicate(m);
        }
        /// <summary>
        /// Starts communication with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public Message InitComm()
        {
            return ((Terminal)Piece).InitComm();
        }

        #endregion
    }
}
