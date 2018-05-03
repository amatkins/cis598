using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Node", Namespace = "SwarmNet")]
    [KnownType("GetKnownTypes")]
    public abstract class GraphNode
    {
        #region Fields
        
        /// <summary>
        /// The next available space for a neighbor node.
        /// </summary>
        [DataMember]
        protected int _nextNeighbor;

        #endregion

        #region Properties

        /// <summary>
        /// The angular index of a given radius in a circular layout.
        /// </summary>
        [DataMember]
        public int? AngularIndex { get; set; }
        /// <summary>
        /// The neighbor that leaving agents will go to.
        /// </summary>
        public abstract GraphNode Exit { get; }
        /// <summary>
        /// The in flow into this node.
        /// </summary>
        [DataMember]
        public List<Agent> In { get; protected set; }
        /// <summary>
        /// Whether or not there all neighbor spaces are occupied.
        /// </summary>
        public bool IsSurrounded
        {
            get
            {
                return _nextNeighbor < 0;
            }
        }
        /// <summary>
        /// Access the neighbors of this node.
        /// </summary>
        /// <param name="index">Index into the array of neighbors.</param>
        /// <returns>A node if it exists, otherwise null.</returns>
        [DataMember]
        public GraphNode[] Neighbors { get; protected set; }
        /// <summary>
        /// The out flow from this node.
        /// </summary>
        [DataMember]
        public List<Agent> Out { get; protected set; }
        /// <summary>
        /// The set piece on this node.
        /// </summary>
        [DataMember]
        public SetPiece Piece { get; set; }
        /// <summary>
        /// The radius from the the center of a circular layout.
        /// </summary>
        [DataMember]
        public int? RadiusIndex { get; set; }
        /// <summary>
        /// The x index of a grid layout.
        /// </summary>
        [DataMember]
        public int? X { get; set; }
        /// <summary>
        /// The y index of a grid layout.
        /// </summary>
        [DataMember]
        public int? Y { get; set; }

        #endregion

        #region Methods - Generic Node Operations

        /// <summary>
        /// Add a new neighbor to this node if possible.
        /// </summary>
        /// <param name="neighbor">The neighbor to add.</param>
        public void AddNeighbor(GraphNode neighbor)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (_nextNeighbor < 0)
                throw new InvalidOperationException("This node can no longer accept neighbors.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            // Add the neighbor to the first available space in this node
            Neighbors[_nextNeighbor] = neighbor;
            // Move to next available space
            _nextNeighbor = Array.IndexOf(Neighbors, null);

            // Add this node to the first available space in the neighbor
            neighbor.Neighbors[neighbor._nextNeighbor] = this;
            // Move to next available space
            neighbor._nextNeighbor = Array.IndexOf(neighbor.Neighbors, null);
        }
        /// <summary>
        /// Attempts to remove the neighbor at the given index.
        /// </summary>
        /// <param name="index">The neighbor to remove.</param>
        /// <returns>What was present in that position.</returns>
        public GraphNode RemoveNeighbor(int index)
        {
            if (index < 0 || index >= Neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            // Return if there is nothing to remove
            if (Neighbors[index] == null)
                return null;

            GraphNode neighbor;
            int thindex;

            // Get neighbore and this index into neighbor's neighbor array
            neighbor = Neighbors[index];
            thindex = Array.IndexOf(neighbor.Neighbors, this);

            // Remove neighbor from this
            Neighbors[index] = null;
            _nextNeighbor = index;

            // Remove this from neighbor
            neighbor.Neighbors[thindex] = null;
            neighbor._nextNeighbor = thindex;

            return neighbor;
        }
        /// <summary>
        /// Replace a position in the neighbor array with a new neighbor.
        /// </summary>
        /// <param name="neighbor">The node to add.</param>
        /// <param name="index">The place to add the new node.</param>
        /// <returns>What used to be at that position in the neighbor array.</returns>
        public GraphNode SwapNeighbor(GraphNode neighbor, int index)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            if (index < 0 || index >= Neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            GraphNode old_Neighbor;
            int thindex;

            // Get neighbore and this index into neighbor's neighbor array
            old_Neighbor = Neighbors[index];

            if (old_Neighbor != null)
            {
                // Get this node's position in old neighbor's neighbor array
                thindex = Array.IndexOf(old_Neighbor.Neighbors, this);

                // Remove this node from old neighbor
                old_Neighbor.Neighbors[thindex] = null;
                old_Neighbor._nextNeighbor = thindex;
            }

            // Put the new neighbor in the provided position
            Neighbors[index] = neighbor;

            // Add this node to the new neighbor
            neighbor.Neighbors[neighbor._nextNeighbor] = this;
            neighbor._nextNeighbor = Array.IndexOf(neighbor.Neighbors, null);

            // Return what used to be in the position
            return old_Neighbor;
        }

        #endregion

        #region Methods - Agent Operations

        /// <summary>
        /// Gets the first agent 
        /// </summary>
        /// <returns></returns>
        public Agent Dequeue()
        {
            if (Out.Count < 1)
                return null;

            // Get the first agent
            Agent a = Out[0];
            // Remove from the population
            Out.RemoveAt(0);
            // Return the agent
            return a;
        }
        /// <summary>
        /// Adds an agent to the population and sorts based on priority.
        /// </summary>
        /// <param name="a">The agent to add.</param>
        public void Enqueue(Agent a)
        {
            In.Add(a);
            In.Sort();
        }
        /// <summary>
        /// Flushes the inflow to the outflow and sorts the queue.
        /// </summary>
        public void Flush()
        {
            Agent a;

            while(In.Count > 0)
            {
                a = In[0];
                In.Remove(a);
                Out.Add(a);
            }
            Out.Sort();
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

            types.Add(typeof(RootNode));
            types.Add(typeof(BranchNode));
            types.Add(typeof(LeafNode));

            return types.ToArray();
        }

        #endregion
    }
}
