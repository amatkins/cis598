using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet.RigSet
{
    public class RigNode
    {
        #region Fields

        /// <summary>
        /// The other nodes connected to this node.
        /// </summary>
        RigNode[] _neighbors;
        /// <summary>
        /// The next available space for a neighbor node.
        /// </summary>
        int _nextNeighbor;

        #endregion

        #region Properties

        /// <summary>
        /// Determines if there isn't room for another neighbor.
        /// </summary>
        public bool IsFull
        {
            get
            {
                return _nextNeighbor < 0;
            }
        }

        /// <summary>
        /// The number of neighbors possible for this node.
        /// </summary>
        public int Length
        {
            get
            {
                return _neighbors.Length;
            }
        }

        /// <summary>
        /// Access the neighbors of this node.
        /// </summary>
        /// <param name="index">The index into the array of neighbor.</param>
        /// <returns>A node if it exists, null otherwise.</returns>
        public RigNode this[int index]
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

        #region Constructors

        /// <summary>
        /// Creates a new rigging node with a set number of connections for neighbors.
        /// </summary>
        /// <param name="neighbors">A number greater than 0 determining the potential number of neightbors.</param>
        public RigNode(int neighbors)
        {
            if (neighbors < 1)
                throw new ArgumentException("Node cannot have less than 1 potential neighbors.");
            
            _neighbors = new RigNode[neighbors];
            _nextNeighbor = 0;
        }

        #endregion

        #region Methods - Node Operations

        /// <summary>
        /// Adds a child to this node if possible.
        /// </summary>
        /// <param name="neighbor">The child to attach to this node.</param>
        public void AddNeighbor(RigNode neighbor)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (_nextNeighbor < 0)
                throw new InvalidOperationException("This node can no longer accept neighbors.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            _neighbors[_nextNeighbor] = neighbor;
            _nextNeighbor = Array.IndexOf(_neighbors, null);

            neighbor._neighbors[neighbor._nextNeighbor] = this;
            neighbor._nextNeighbor = Array.IndexOf(neighbor._neighbors, null);
        }

        /// <summary>
        /// Replace a position in the neighbor array with a new neighbor.
        /// </summary>
        /// <param name="neighbor">The node to add.</param>
        /// <param name="index">The place to add the new node.</param>
        /// <returns>What used to be at that position in the neighbor array.</returns>
        public RigNode SwapNeighbor(RigNode neighbor, int index)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            if (index < 0 || index >= _neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            RigNode old_Neighbor;
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
        /// Attempts to remove the child at a given index and return it.
        /// </summary>
        /// <param name="index">The location of the child to remove.</param>
        /// <returns>The child that was removed, or null if no child was present.</returns>
        public RigNode RemoveNeighbor(int index)
        {
            if (index < 0 || index >= _neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            // Return if there is nothing to remove
            if (_neighbors[index] == null)
                return null;

            RigNode neighbor;
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

        #region Methods - Logistic

        public override string ToString()
        {
            return string.Format("Neighbors: [{0}]", _neighbors != null ? new string(_neighbors.Select(c => c != null ? '+' : '-').ToArray()) : "");
        }

        #endregion
    }
}
