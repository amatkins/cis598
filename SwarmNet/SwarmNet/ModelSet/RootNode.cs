using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Root", Namespace = "SwarmNet")]
    public class RootNode : GraphNode
    {
        #region Properties

        /// <summary>
        /// Exit, onto the graph from the head.
        /// </summary>
        public override GraphNode Exit
        {
            get
            {
                return Neighbors[0];
            }
        }
        /// <summary>
        /// The agents queued to enter this graph.
        /// </summary>
        [DataMember]
        public List<Agent> ExternalIn { get; protected set; }
        /// <summary>
        /// The agents queued to leave this graph.
        /// </summary>
        [DataMember]
        public List<Agent> ExternalOut { get; protected set; }
        /// <summary>
        /// The portal on this node.
        /// </summary>
        public Port Port
        {
            get
            {
                return (Port)Piece;
            }
            set
            {
                Piece = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new head node.
        /// </summary>
        public RootNode()
        {
            In = new List<Agent>();
            ExternalIn = new List<Agent>();
            ExternalOut = new List<Agent>();
            Neighbors = new GraphNode[1];
            _nextNeighbor = 0;
            Out = new List<Agent>();
            Piece = null;
        }
        /// <summary>
        /// Constructs a new head node that contains the provided spawner.
        /// </summary>
        /// <param name="portal">The spawner this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public RootNode(Port portal)
        {
            In = new List<Agent>();
            ExternalIn = new List<Agent>();
            ExternalOut = new List<Agent>();
            Neighbors = new GraphNode[1];
            _nextNeighbor = 0;
            Out = new List<Agent>();
            Piece = portal;
            portal.Location = this;
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
            return ((Port)Piece).Communicate(m);
        }
        /// <summary>
        /// Use the portal to spawn a new agent into the out flow.
        /// </summary>
        /// <returns>The agent that was added.</returns>
        public Agent Enter()
        {
            // Spawn a new agent
            Agent a = ((Port)Piece).Enter();
            // Add it to the node and sort
            ExternalIn.Add(a);
            ExternalIn.Sort();
            // Return it for the use by the graph
            return a;
        }
        /// <summary>
        /// Flushes the external inflow to the outflow and inflow to the external outflow, then sorts the both queues.
        /// </summary>
        public new void Flush()
        {
            Agent a;

            while (In.Count > 0)
            {
                a = In[0];
                In.Remove(a);
                ExternalOut.Add(a);
            }
            ExternalOut.Sort();

            while (ExternalIn.Count > 0)
            {
                a = ExternalIn[0];
                ExternalIn.Remove(a);
                Out.Add(a);
            }
            Out.Sort();
        }
        /// <summary>
        /// Start communications with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public Message InitComm()
        {
            return ((Port)Piece).InitComm();
        }
        /// <summary>
        /// Use the portal to despawn an agent from the in flow.
        /// </summary>
        /// <returns>The agent that the node removed.</returns>
        public Agent Leave()
        {
            // Check if any agents are leaving
            if (ExternalOut.Count < 1)
                return null;
            // Get first agent leaving
            Agent a = ExternalOut[0];
            // Remove from queue
            ExternalOut.RemoveAt(0);
            // Despawn the agent
            ((Port)Piece).Leave(a);
            // Return it for use by the graph
            return a;
        }

        #endregion
    }
}
