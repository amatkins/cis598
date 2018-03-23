using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Root", Namespace = "SwarmNet")]
    public class RootNode<JI, JO, TI, TO> : GraphNode<JI, JO, TI, TO>
    {
        #region Fields

        /// <summary>
        /// The agents queued to enter this graph.
        /// </summary>
        [DataMember(Name = "ExternalIn")]
        private List<Agent<JI, JO, TI, TO>> _extInFlow;
        /// <summary>
        /// The agents queued to leave this graph.
        /// </summary>
        [DataMember(Name = "ExternalOut")]
        private List<Agent<JI, JO, TI, TO>> _extOutFlow;

        #endregion

        #region Properties

        /// <summary>
        /// Exit, onto the graph from the head.
        /// </summary>
        public override GraphNode<JI, JO, TI, TO> Exit
        {
            get
            {
                return _neighbors[0];
            }
        }
        /// <summary>
        /// The number of agents waiting to come in.
        /// </summary>
        public int ExInCount
        {
            get
            {
                return _extInFlow.Count;
            }
        }
        /// <summary>
        /// The number of agents waiting to leave.
        /// </summary>
        public int ExOutCount
        {
            get
            {
                return _extOutFlow.Count;
            }
        }
        /// <summary>
        /// The agents queued to enter this graph.
        /// </summary>
        public List<Agent<JI, JO, TI, TO>> ExternalIn
        {
            get
            {
                return _extInFlow;
            }
        }
        /// <summary>
        /// The agents queued to leave this graph.
        /// </summary>
        public List<Agent<JI, JO, TI, TO>> ExternalOut
        {
            get
            {
                return _extOutFlow;
            }
        }
        /// <summary>
        /// The portal on this node.
        /// </summary>
        public Portal<JI, JO, TI, TO> Portal
        {
            get
            {
                return (Portal<JI, JO, TI, TO>)_piece;
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
        public RootNode()
        {
            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _extInFlow = new List<Agent<JI, JO, TI, TO>>();
            _extOutFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[1];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = null;
        }
        /// <summary>
        /// Constructs a new head node that contains the provided spawner.
        /// </summary>
        /// <param name="portal">The spawner this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public RootNode(Portal<JI, JO, TI, TO> portal)
        {
            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _extInFlow = new List<Agent<JI, JO, TI, TO>>();
            _extOutFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[1];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = portal;
            portal.Location = this;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Use the portal to spawn a new agent into the out flow.
        /// </summary>
        /// <returns>The agent that was added.</returns>
        public Agent<JI, JO, TI, TO> Enter()
        {
            // Spawn a new agent
            Agent<JI, JO, TI, TO> a = ((Portal<JI, JO, TI, TO>)_piece).Enter();
            // Add it to the node and sort
            _extInFlow.Add(a);
            _extInFlow.Sort();
            // Return it for the use by the graph
            return a;
        }
        /// <summary>
        /// Use the portal to despawn an agent from the in flow.
        /// </summary>
        /// <returns>The agent that the node removed.</returns>
        public Agent<JI, JO, TI, TO> Leave()
        {
            // Check if any agents are leaving
            if (_extOutFlow.Count < 1)
                return null;
            // Get first agent leaving
            Agent<JI, JO, TI, TO> a = _extOutFlow[0];
            // Remove from queue
            _extOutFlow.RemoveAt(0);
            // Despawn the agent
            ((Portal<JI, JO, TI, TO>)_piece).Leave(a);
            // Return it for use by the graph
            return a;
        }
        /// <summary>
        /// Flushes the external inflow to the outflow and inflow to the external outflow, then sorts the both queues.
        /// </summary>
        public new void Flush()
        {
            Agent<JI, JO, TI, TO> a;

            while (_inFlow.Count > 0)
            {
                a = _inFlow[0];
                _inFlow.Remove(a);
                _extOutFlow.Add(a);
            }
            _extOutFlow.Sort();

            while (_extInFlow.Count > 0)
            {
                a = _extInFlow[0];
                _extInFlow.Remove(a);
                _outFlow.Add(a);
            }
            _outFlow.Sort();
        }

        #endregion
    }
}
