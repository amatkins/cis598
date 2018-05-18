using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Leaf", Namespace = "SwarmNet")]
    public class LeafNode : GraphNode
    {
        #region Properties
        
        /// <summary>
        /// The terminal on this node.
        /// </summary>
        public Terminal Terminal
        {
            get
            {
                return (Terminal)Station;
            }
            set
            {
                Station = value;
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
            Station = null;
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
            Station = terminal;
            terminal.Location = this;
        }

        #endregion

        #region Methods - Station Operations
        
        /// <summary>
        /// Leave the leaf onto it's only neighbor.
        /// </summary>
        /// <param name="a">The agent that is traveling.</param>
        /// <returns>The next node.</returns>
        public override GraphNode GetExit(Agent a)
        {
            return Neighbors[0];
        }

        #endregion
    }
}
