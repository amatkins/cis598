using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public class HeadNode<JI, JO, TI, TO> : GraphNode<JI, JO, TI, TO>
    {
        #region Properties

        /// <summary>
        /// The entrance to the graph from the head.
        /// </summary>
        public override GraphNode<JI, JO, TI, TO> Exit
        {
            get
            {
                return _neighbors[0];
            }
        }
        /// <summary>
        /// The spawner on this node.
        /// </summary>
        public Spawner<JI, JO, TI, TO> Spawner
        {
            get
            {
                return (Spawner<JI, JO, TI, TO>)_piece;
            }
            set
            {
                _piece = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new head node.
        /// </summary>
        public HeadNode()
        {
            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[1];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = null;
        }
        /// <summary>
        /// Constructs a new head node that contains the provided spawner.
        /// </summary>
        /// <param name="spawner">The spawner this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public HeadNode(Spawner<JI, JO, TI, TO> spawner)
        {
            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[1];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = spawner;
            spawner.Location = this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Use the spawner to spawn a new agent into the in flow.
        /// </summary>
        /// <returns>The agent that was added.</returns>
        public Agent<JI, JO, TI, TO> Spawn()
        {
            // Spawn a new agent
            Agent<JI, JO, TI, TO> a = ((Spawner<JI, JO, TI, TO>)_piece).Spawn();
            // Add it to the node and sort
            _inFlow.Add(a);
            // Return it for the use by the graph
            return a;
        }
        /// <summary>
        /// Use the spawner to despawn an agent from the out flow as selected by the node.
        /// </summary>
        /// <returns>The agent that the node removed.</returns>
        public Agent<JI, JO, TI, TO> Despawn()
        {
            // Get first agent leaving
            Agent<JI, JO, TI, TO> a = Dequeue();
            // Despawn the agent
            ((Spawner<JI, JO, TI, TO>)_piece).Despawn(a);
            // Return it for use by the graph
            return a;
        }

        #endregion
    }
}
