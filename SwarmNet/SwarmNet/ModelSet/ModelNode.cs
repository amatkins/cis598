using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet.ModelSet
{
    public abstract class ModelNode<P> where P : SetPiece 
    {
        #region Fields

        /// <summary>
        /// The set piece that is on this node.
        /// </summary>
        private P _body;
        /// <summary>
        /// The agents inhabiting this node.
        /// </summary>
        private SortedList<int, Agent> _inhabitants;
        /// <summary>
        /// The other nodes connected to this one.
        /// </summary>
        private ModelNode<P>[] _neighbors;
        /// <summary>
        /// The next available space for a neighbor node.
        /// </summary>
        private int _nextNeighbor;

        #endregion

        #region Methods - Generic Node Operations

        /// <summary>
        /// Add a new neighbor to this node if possible.
        /// </summary>
        /// <param name="neighbor">The neighbor to add.</param>
        public void AddNeighbor(ModelNode<P> neighbor)
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
        public ModelNode<P> SwapNeighbor(ModelNode<P> neighbor, int index)
        {
            if (neighbor == null)
                throw new ArgumentException("Neighbor cannot be null, try RemoveNeighbor instead.");

            if (neighbor._nextNeighbor < 0)
                throw new ArgumentException("The neighbor node can no longer accept neighbors.");

            if (index < 0 || index >= _neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            ModelNode<P> old_Neighbor;
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
        public ModelNode<P> RemoveNeighbor(int index)
        {
            if (index < 0 || index >= _neighbors.Length)
                throw new ArgumentException("Inavlid index into neighbor array.");

            // Return if there is nothing to remove
            if (_neighbors[index] == null)
                return null;

            ModelNode<P> neighbor;
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
    }
}
