using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Root", Namespace = "SwarmNet")]
    public class RootNode : GraphNode
    {
        #region Properties
        
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
                return (Port)Station;
            }
            set
            {
                Station = value;
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
            Station = null;
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
            Station = portal;
            portal.Location = this;
        }

        #endregion

        #region Methods - Station Operations
        
        /// <summary>
        /// Use the portal to spawn a new agent into the out flow.
        /// </summary>
        /// <returns>The agent that was added.</returns>
        public Agent Enter()
        {
            Agent a = null;
            
            if (Station != null)
            {
                // Spawn a new agent
                a = ((Port)Station).Enter();
                // Add it to the node and sort
                ExternalIn.Add(a);
                ExternalIn.Sort();
            }
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
        /// Leave the root onto it's only neighbor.
        /// </summary>
        /// <param name="a">The agent that is traveling.</param>
        /// <returns>The next node.</returns>
        public override GraphNode GetExit(Agent a)
        {
            return Neighbors[0];
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
            if (Station != null)
                ((Port)Station).Leave(a);
            // Return it for use by the graph
            return a;
        }

        #endregion
    }
}
