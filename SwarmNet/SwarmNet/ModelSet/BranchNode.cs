using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Branch", Namespace = "SwarmNet")]
    public class BranchNode : GraphNode
    {
        #region Properties

        /// <summary>
        /// The path that an agent can leave through.
        /// </summary>
        public override GraphNode Exit
        {
            get
            {
                return Neighbors[((Junction)Piece).GetExit(Neighbors.Length)];
            }
        }
        /// <summary>
        /// The junction on this node.
        /// </summary>
        public Junction Junction
        {
            get
            {
                return (Junction)Piece;
            }
            set
            {
                Piece = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new branch node with a set number of neighbors.
        /// </summary>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public BranchNode(int neighbors)
        {
            if (neighbors < 1)
                throw new ArgumentException("Node cannot have less than 1 potential neighbors.");

            In = new List<Agent>();
            Neighbors = new GraphNode[neighbors];
            _nextNeighbor = 0;
            Out = new List<Agent>();
            Piece = null;
        }
        /// <summary>
        /// Constructs a new branch node with a set number of neighbors and containing the provided junction.
        /// </summary>
        /// <param name="junction">The junction this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public BranchNode(Junction junction, int neighbors)
        {
            if (neighbors < 1)
                throw new ArgumentException("Node cannot have less than 1 potential neighbors.");

            In = new List<Agent>();
            Neighbors = new GraphNode[neighbors];
            _nextNeighbor = 0;
            Out = new List<Agent>();
            Piece = junction;
            junction.Location = this;
        }

        #endregion
        
        #region Methods - Setpiece Operations

        /// <summary>
        /// Acts as middle man between agent and setpiece.
        /// </summary>
        /// <param name="m">The message to pass to the setpiece.</param>
        /// <returns>The setpiece's reponse.</returns>
        public Message Communicate(Message m)
        {
            return ((Junction)Piece).Communicate(m);
        }
        /// <summary>
        /// Start communications with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public Message InitComm()
        {
            return ((Junction)Piece).InitComm();
        }

        #endregion
    }
}
