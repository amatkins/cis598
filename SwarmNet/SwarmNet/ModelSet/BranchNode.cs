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
        /// The junction on this node.
        /// </summary>
        public Junction Junction
        {
            get
            {
                return (Junction)Station;
            }
            set
            {
                Station = value;
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
            Station = null;
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
            Station = junction;
            junction.Location = this;
        }

        #endregion
        
        #region Methods - Station Operations

        /// <summary>
        /// Get the node that the agent will travel to next.
        /// </summary>
        /// <param name="a">The agent that will be traveling.</param>
        /// <returns>The next node.</returns>
        public override GraphNode GetExit(Agent a)
        {
            if (Station != null)
            {
                GraphNode exit = Neighbors[((Junction)Station).GetExit(Neighbors.Length)];
                if (exit != null)
                    return exit;
                else
                    return Neighbors[a.GetExit(Neighbors.Length)];
            }
            else
                return Neighbors[a.GetExit(Neighbors.Length)];
        }

        #endregion
    }
}
