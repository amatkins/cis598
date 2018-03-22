using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public abstract class GraphNode<JI, JO, TI, TO>
    {
        #region Fields

        /// <summary>
        /// The agents queued to enter this node.
        /// </summary>
        protected List<Agent<JI, JO, TI, TO>> _inFlow;
        /// <summary>
        /// The other nodes connected to this one.
        /// </summary>
        protected GraphNode<JI, JO, TI, TO>[] _neighbors;
        /// <summary>
        /// The next available space for a neighbor node.
        /// </summary>
        protected int _nextNeighbor;
        /// <summary>
        /// The agents queued to leave this node.
        /// </summary>
        protected List<Agent<JI, JO, TI, TO>> _outFlow;
        /// <summary>
        /// The set piece that is on this node.
        /// </summary>
        protected SetPiece<JI, JO, TI, TO> _piece;

        #endregion

        #region Properties

        /// <summary>
        /// The neighbor that leaving agents will go to.
        /// </summary>
        public abstract GraphNode<JI, JO, TI, TO> Exit { get; }
        /// <summary>
        /// The in flow into this node.
        /// </summary>
        public List<Agent<JI, JO, TI, TO>> In
        {
            get
            {
                return _inFlow;
            }
        }
        /// <summary>
        /// The numner of agents in the in flow.
        /// </summary>
        public int InCount
        {
            get
            {
                return _inFlow.Count;
            }
        }
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
        /// The number of neighbors this node can have.
        /// </summary>
        public int Length
        {
            get
            {
                return _neighbors.Length;
            }
        }
        /// <summary>
        /// The out flow from this node.
        /// </summary>
        public List<Agent<JI, JO, TI, TO>> Out
        {
            get
            {
                return _outFlow;
            }
        }
        /// <summary>
        /// The number of agents in the out flow.
        /// </summary>
        public int OutCount
        {
            get
            {
                return _outFlow.Count;
            }
        }
        /// <summary>
        /// The set piece on this node.
        /// </summary>
        public SetPiece<JI, JO, TI, TO> Piece
        {
            get
            {
                return _piece;
            }
        }
        /// <summary>
        /// Access the neighbors of this node.
        /// </summary>
        /// <param name="index">Index into the array of neighbors.</param>
        /// <returns>A node if it exists, otherwise null.</returns>
        public GraphNode<JI, JO, TI, TO> this[int index]
        {
            get
            {
                if (index > -1 && index < _neighbors.Length)
                    return _neighbors[index];
                else
                    return null;
            }
        }

        #endregion

        #region Methods - Generic Node Operations

        /// <summary>
        /// Add a new neighbor to this node if possible.
        /// </summary>
        /// <param name="neighbor">The neighbor to add.</param>
        public void AddNeighbor(GraphNode<JI, JO, TI, TO> neighbor)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (_nextNeighbor < 0)
                throw new InvalidOperationException("This noce can no longer accept neighbors.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            // Add the neighbor to the first available space in this node
            _neighbors[_nextNeighbor] = neighbor;
            // Move to next available space
            _nextNeighbor = Array.IndexOf(_neighbors, null);

            // Add this node to the first available space in the neighbor
            neighbor._neighbors[neighbor._nextNeighbor] = this;
            // Move to next available space
            neighbor._nextNeighbor = Array.IndexOf(neighbor._neighbors, null);
        }
        /// <summary>
        /// Replace a position in the neighbor array with a new neighbor.
        /// </summary>
        /// <param name="neighbor">The node to add.</param>
        /// <param name="index">The place to add the new node.</param>
        /// <returns>What used to be at that position in the neighbor array.</returns>
        public GraphNode<JI, JO, TI, TO> SwapNeighbor(GraphNode<JI, JO, TI, TO> neighbor, int index)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            if (index < 0 || index >= _neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            GraphNode<JI, JO, TI, TO> old_Neighbor;
            int thindex;

            // Get neighbore and this index into neighbor's neighbor array
            old_Neighbor = _neighbors[index];

            if (old_Neighbor != null)
            {
                // Get this node's position in old neighbor's neighbor array
                thindex = Array.IndexOf(old_Neighbor._neighbors, this);

                // Remove this node from old neighbor
                old_Neighbor._neighbors[thindex] = null;
                old_Neighbor._nextNeighbor = thindex;
            }

            // Put the new neighbor in the provided position
            _neighbors[index] = neighbor;

            // Add this node to the new neighbor
            neighbor._neighbors[neighbor._nextNeighbor] = this;
            neighbor._nextNeighbor = Array.IndexOf(neighbor._neighbors, null);

            // Return what used to be in the position
            return old_Neighbor;
        }
        /// <summary>
        /// Attempts to remove the neighbor at the given index.
        /// </summary>
        /// <param name="index">The neighbor to remove.</param>
        /// <returns>What was present in that position.</returns>
        public GraphNode<JI, JO, TI, TO> RemoveNeighbor(int index)
        {
            if (index < 0 || index >= _neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            // Return if there is nothing to remove
            if (_neighbors[index] == null)
                return null;

            GraphNode<JI, JO, TI, TO> neighbor;
            int thindex;

            // Get neighbore and this index into neighbor's neighbor array
            neighbor = _neighbors[index];
            thindex = Array.IndexOf(neighbor._neighbors, this);

            // Remove neighbor from this
            _neighbors[index] = null;
            _nextNeighbor = index;

            // Remove this from neighbor
            neighbor._neighbors[thindex] = null;
            neighbor._nextNeighbor = thindex;

            return neighbor;
        }

        #endregion

        #region Methods - Agent Operations

        /// <summary>
        /// Gets the first agent 
        /// </summary>
        /// <returns></returns>
        public Agent<JI, JO, TI, TO> Dequeue()
        {
            if (_outFlow.Count < 1)
                return null;

            // Get the first agent
            Agent<JI, JO, TI, TO> a = _outFlow[0];
            // Remove from the population
            _outFlow.RemoveAt(0);
            // Return the agent
            return a;
        }
        /// <summary>
        /// Adds an agent to the population and sorts based on priority.
        /// </summary>
        /// <param name="a">The agent to add.</param>
        public void Enqueue(Agent<JI, JO, TI, TO> a)
        {
            _inFlow.Add(a);
            _inFlow.Sort();
        }
        /// <summary>
        /// Flushes the inflow to the outflow and sorts the queue.
        /// </summary>
        public void Flush()
        {
            Agent<JI, JO, TI, TO> a;

            while(_inFlow.Count > 0)
            {
                a = _inFlow[0];
                _inFlow.Remove(a);
                _outFlow.Add(a);
            }
            _outFlow.Sort();
        }

        #endregion
    }
}
