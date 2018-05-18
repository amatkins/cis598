using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace SwarmNet
{
    [DataContract(Name = "Graph", Namespace = "SwarmNet")]
    public class ModelGraph
    {
        #region Properties

        /// <summary>
        /// The agents of the graph.
        /// </summary>
        [DataMember]
        public List<Agent> Agents { get; protected set; }
        /// <summary>
        /// The branches of the graph.
        /// </summary>
        [DataMember]
        public List<BranchNode> Branches { get; protected set; }
        /// <summary>
        /// The leaves of the graph.
        /// </summary>
        [DataMember]
        public List<LeafNode> Leaves { get; protected set; }
        /// <summary>
        /// The roots of the graph.
        /// </summary>
        [DataMember]
        public List<RootNode> Roots { get; protected set; }
        /// <summary>
        /// The number of nodes in each tier of a tree layout for this graph.
        /// </summary>
        [DataMember]
        public List<int> Counts { get; protected set; }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Base graph constructor.
        /// </summary>
        public ModelGraph()
        {
            Agents = new List<Agent>();
            Branches = new List<BranchNode>();
            Leaves = new List<LeafNode>();
            Roots = new List<RootNode>();
            Counts = new List<int>();
        }
        /// <summary>
        /// Constructs a new tree.
        /// </summary>
        /// <param name="r">The randomizer object.</param>
        /// <param name="n">The number of nodes in the tree.</param>
        /// <param name="m">The max number of children each node can have.</param>
        /// <param name="o">The offset from the average number of children for a tier.</param>
        /// <param name="l">The chance a node will be a leaf.</param>
        public ModelGraph(Random r, int n, int m, int o, int l)
        {
            Agents = new List<Agent>();
            GenTree(r, n, m, o, l);
        }
        /// <summary>
        /// Constructs a new tree and builds set pieces on it using the methods provided.
        /// </summary>
        /// <param name="r">The randomizer object.</param>
        /// <param name="n">The number of nodes in the tree.</param>
        /// <param name="m">The max children each node can have.</param>
        /// <param name="o">The offset from the average.</param>
        /// <param name="l">The chance a node will be a leaf.</param>
        /// <param name="p">The method of creating portals.</param>
        /// <param name="j">The method of creating junctions.</param>
        /// <param name="t">The method of creating terminals.</param>
        public ModelGraph(Random r, int n, int m, int o, int l, Func<Port> p, Func<int, Junction> j, Func<Terminal> t)
        {
            Agents = new List<Agent>();
            GenTree(r, n, m, o, l);
            BuildSet(p, j, t);
        }

        #endregion

        #region Methods - Graph Operations

        /// <summary>
        /// Converts a tree styled rigging graph to a model graph.
        /// </summary>
        /// <param name="rig">The rigging graph to conver.</param>
        /// <param name="port">Method for generating portals.</param>
        /// <param name="junc">Method for generating junctions.</param>
        /// <param name="term">Method for generating terminals.</param>
        public void BuildSet(Func<Port> port, Func<int, Junction> junc, Func<Terminal> term)
        {
            List<GraphNode> current_model, next_model = Roots.ToList<GraphNode>();
            GraphNode model_node;
            bool isHead = true;

            do
            {
                // Switch next to current, and get new next
                current_model = next_model;
                next_model = new List<GraphNode>();

                // Got through each node of current
                for (int i = 0; i < current_model.Count; i++)
                {
                    // Get current node
                    model_node = current_model[i];

                    // Build set piece on node and add un-built children to next tier
                    if (isHead)
                    {
                        ((RootNode)model_node).Port = port();
                        if (model_node.Neighbors[0].Station == null)
                            next_model.Add(model_node.Neighbors[0]);
                    }
                    else
                    {
                        if (model_node.Neighbors.Length > 1)
                        {
                            ((BranchNode)model_node).Junction = junc(model_node.Neighbors.Length);
                            for (int j = 0; j < model_node.Neighbors.Length; j++)
                            {
                                if (model_node.Neighbors[j].Station == null)
                                    next_model.Add(model_node.Neighbors[j]);
                            }
                        }
                        else
                            ((LeafNode)model_node).Terminal = term();
                    }
                }

                // Flip bool for speach head case
                if (isHead)
                    isHead = false;
            } while (next_model.Count > 0);
        }
        /// <summary>
        /// Generate a new graph for this object.
        /// </summary>
        /// <param name="rand">The random object to be used for generation.</param>
        /// <param name="total_remain">The total number of nodes in this graph.</param>
        /// <param name="max_children">The max number of children per node.</param>
        /// <param name="offset">The offset from the average.</param>
        public void GenTree(Random rand, int total_remain, int max_children, int offset, int leaf_chance)
        {
            int n_kids_tot, c_kids_tot, n_kids_max, n_kids_min, kids_remain, avg_kpn, real_kpn, tier, tier_index, min;
            GraphNode[] cur_tier;
            List<GraphNode> next_tier;
            RootNode root;
            List<BranchNode> branches;
            List<LeafNode> leaves;
            GraphNode current;
            List<int> tiers;
            bool isRoot;

            // Initialize branches and leaves;
            branches = new List<BranchNode>();
            leaves = new List<LeafNode>();
            tiers = new List<int>();
            // Initialize the positioning variables
            tier = 0;
            tier_index = 0;
            // The head will have one neighbor
            n_kids_tot = 1;
            // Subtract the head and it's neighbor from the remaining total
            total_remain -= n_kids_tot + 1;
            // Initialize the head
            root = new RootNode();
            root.Layout = new int[] { tier, tier_index };
            tier_index++;
            // Insert head as only node of next tier of nodes to work on
            next_tier = new List<GraphNode>() { root };
            // set first tier bool
            isRoot = true;

            while (n_kids_tot > 0)
            {
                // Set next tier as current tier and get new next tier
                cur_tier = next_tier.ToArray();
                next_tier = new List<GraphNode>();
                tier++;
                tiers.Add(tier_index);
                tier_index = 0;
                // Set next total to current tier and reset next total
                c_kids_tot = n_kids_tot;
                n_kids_tot = 0;
                // Get total possible nodes for next tier as multiple of nodes to generate times
                // the max number of children each node can have
                n_kids_max = c_kids_tot * max_children;

                // If possible total is larger than total remaining, clamp to later
                if (total_remain < n_kids_max)
                    kids_remain = total_remain;
                // Otherwise set projected total for next tier as a random number between half
                // this tier and the possible total
                else
                {
                    n_kids_min = c_kids_tot / 2 < 1 ? 1 : c_kids_tot / 2;
                    kids_remain = rand.Next(n_kids_min, n_kids_max + 1);
                }

                // Subtract this tier's total from remaining total
                total_remain -= kids_remain;
                // Get the average number of children per node of this tier
                avg_kpn = kids_remain / c_kids_tot < 1 ? 1 : kids_remain / c_kids_tot;

                // For each node in this tier
                for (int i = 0; i < cur_tier.Length; i++)
                {
                    // For each child of this node
                    for (int j = isRoot ? 0 : 1; j < cur_tier[i].Neighbors.Length; j++)
                    {
                        // If the average is greater than what remains to be generated, then clamp to later
                        if (kids_remain < avg_kpn)
                            real_kpn = kids_remain;
                        // Otherwise, random chance for leaf
                        else if (n_kids_tot > 0 && rand.Next(100) < leaf_chance)
                            real_kpn = 0;
                        // Otherwise set real number of children to some random offset from the average
                        // and clamp the value to max or the remaining children if necessary
                        else
                        {
                            min = Math.Min(offset, avg_kpn);
                            real_kpn = Math.Min(avg_kpn + rand.Next(offset + min) - min + 1, Math.Min(max_children, kids_remain));

                            if (real_kpn == 0)
                                return;
                        }

                        // Subtract this node's children from remaining children
                        kids_remain -= real_kpn;
                        // Add to total of next tier
                        n_kids_tot += real_kpn;

                        // Generate a node with the calculated number of children
                        if (real_kpn == 0)
                        {
                            current = new LeafNode();
                            leaves.Add((LeafNode)current);
                        }
                        else
                        {
                            current = new BranchNode(real_kpn + 1);
                            branches.Add((BranchNode)current);
                            next_tier.Add(current);
                        }
                        // Position current node
                        current.Layout = new int[] { tier, tier_index };
                        tier_index++;
                        // Add node to graph
                        cur_tier[i].AddNeighbor(current);
                    }
                }
                // Add back on any un-generated children
                total_remain += kids_remain;
                // turn off special head case bool
                if (isRoot)
                    isRoot = false;
            }

            // Set all the finalized values
            Roots = new List<RootNode> { root };
            Branches = branches;
            Leaves = leaves;
            Counts = tiers;
        }

        #endregion

        #region Methods - Events

        /// <summary>
        /// Performs a dismissal interaction between port and agent.
        /// </summary>
        /// <param name="s">Port station dismissing the agent.</param>
        /// <param name="a">Agent being dismissed.</param>
        private void Dismissal(Port s, Agent a)
        {
            if (s != null)
            {
                Message m = s.InitDism();

                do
                {
                    m = s.Dismissal(a.Dismissal(m));
                } while (m.Type != CommType.TERM);
            }
        }
        /// <summary>
        /// Performs interactions between a port and agent.
        /// </summary>
        /// <param name="s">The port the agent is on.</param>
        /// <param name="a">The agent on the port.</param>
        private void InteractPort(Port s, Agent a)
        {
            if (s != null)
            {
                Message m = s.InitComm();

                do
                {
                    m = s.Communicate(a.CommPort(m));
                } while (m.Type != CommType.TERM);
            }
        }
        /// <summary>
        /// Performs interactions between a junction and agent.
        /// </summary>
        /// <param name="s">The junction the agent is on.</param>
        /// <param name="a">The agent on the junction.</param>
        private void InteractJunc(Junction s, Agent a)
        {
            if (s != null)
            {
                Message m = s.InitComm();

                do
                {
                    m = s.Communicate(a.CommJunc(m));
                } while (m.Type != CommType.TERM);
            }
        }
        /// <summary>
        /// Performs interactions between a terminal and agent.
        /// </summary>
        /// <param name="s">The terminal the agent is on.</param>
        /// <param name="a">The agent on the terminal.</param>
        private void InteractTerm(Terminal s, Agent a)
        {
            if (s != null)
            {
                Message m = s.InitComm();

                do
                {
                    m = s.Communicate(a.CommTerm(m));
                } while (m.Type != CommType.TERM);
            }
        }
        /// <summary>
        /// Add a new agent to the graph.
        /// </summary>
        public bool Enter(int index)
        {
            if (index < 0 || index >= Roots.Count)
                return false;

            Agent a = Roots[index].Enter();
            if (a != null)
            {
                Agents.Add(a);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Moves the simulation of the model along by one generic time unit.
        /// </summary>
        public void Tick()
        {
            // Perform interactions for all the root nodes
            Parallel.ForEach(Roots, ProcessRootAll);

            // Perform interactions for all the branch nodes
            Parallel.ForEach(Branches, ProcessBranchAll);

            // Perform interactions for all the leaf nodes
            Parallel.ForEach(Leaves, ProcessLeafAll);

            // Move all agents into their respective nodes
            Parallel.ForEach(Roots, root => root.Flush());
            Parallel.ForEach(Branches, branch => branch.Flush());
            Parallel.ForEach(Leaves, leaf => leaf.Flush());
        }
        /// <summary>
        /// Processes all agents in a root node.
        /// </summary>
        /// <param name="r">Index of root node to processes.</param>
        private void ProcessRootAll(RootNode root)
        {
            // Exit graph through portal
            while (root.ExternalOut.Count > 0)
            {
                // Get agent from node
                Agent a = root.Leave();

                // Perform transaction between station and agent
                Dismissal(root.Port, a);

                // Remove agent from graph
                Agents.Remove(a);
            }
            // Enter the graph
            while (root.Out.Count > 0)
            {
                // Get agent from node
                Agent a = root.Dequeue();

                // Perform transaction between station and agent
                InteractPort(root.Port, a);

                // Get node that agent will travel to next
                GraphNode exit = root.GetExit(a);

                // Move agent to node if available, otherwise put back onto root
                if (exit != null)
                    exit.Enqueue(a);
                else
                    root.Enqueue(a);
            }
        }
        /// <summary>
        /// Processes all agents in a branch node.
        /// </summary>
        /// <param name="branch">Branch node to processes.</param>
        private void ProcessBranchAll(BranchNode branch)
        {
            // For every agent on the node...
            while (branch.Out.Count > 0)
            {
                // Get next agent
                Agent a = branch.Dequeue();

                // Perfrom transaction between station and agent
                InteractJunc(branch.Junction, a);

                // Get node that agent will travel to next
                GraphNode exit = branch.GetExit(a);

                // Move agent to node if available, otherwise put back onto branch
                if (exit != null)
                    exit.Enqueue(a);
                else
                    branch.Enqueue(a);
            }
        }
        /// <summary>
        /// Process all agents in a leaf node.
        /// </summary>
        /// <param name="leaf">Leaf node to processes.</param>
        private void ProcessLeafAll(LeafNode leaf)
        {
            // For every agent on the node...
            while (leaf.Out.Count > 0)
            {
                // Get next agent
                Agent a = leaf.Dequeue();

                // Perform transaction between station and agent
                InteractTerm(leaf.Terminal, a);

                // Get node that agent will travel to next
                GraphNode exit = leaf.GetExit(a);

                // Move agent to node if available, otherwise put back onto leaf
                if (exit != null)
                    exit.Enqueue(a);
                else
                    leaf.Enqueue(a);
            }
        }

        /// <summary>
        /// Moves the simulation of the model along by a part of one generic time unit.
        /// </summary>
        /// <returns>Whether or not a tick has passed.</returns>
        public bool SemiTick()
        {
            long toProcess = 0;

            // Perform interaction for all the root nodes
            Parallel.For<long>(0, Roots.Count, () => 0, ProcessRootSingle, (d) => Interlocked.Add(ref toProcess, d));
            
            // Perform interactions for all the branch nodes
            Parallel.For<long>(0, Branches.Count, () => 0, ProcessBranchSingle, (d) => Interlocked.Add(ref toProcess, d));

            // Perform interactions for all the branch nodes
            Parallel.For<long>(0, Branches.Count, () => 0, ProcessLeafSingle, (d) => Interlocked.Add(ref toProcess, d));

            // If this is the end of the tick
            if (toProcess == 0)
            {
                // Move all agents into their respective nodes
                Parallel.ForEach(Roots, root => root.Flush());
                Parallel.ForEach(Branches, branch => branch.Flush());
                Parallel.ForEach(Leaves, leaf => leaf.Flush());
            }

            return toProcess == 0;
        }
        /// <summary>
        /// Processes the first available agent in a root node.
        /// </summary>
        /// <param name="r">Index of the root node to process.</param>
        /// <param name="state">State of the parallel loop.</param>
        /// <param name="doneTick">Whether or not the process cycle has been completed.</param>
        /// <returns>The doneTick variable.</returns>
        private long ProcessRootSingle(int r, ParallelLoopState state, long toProcess)
        {
            // Get root
            RootNode root = Roots[r];

            // If there is an agent leaving
            if (root.ExternalOut.Count > 0)
            {
                // Get agent from node
                Agent a = root.Leave();

                // Perform transaction between station and agent
                Dismissal(root.Port, a);

                // Remove agent from graph
                Agents.Remove(a);

                // If there are more agents, then the tick isn't done
                if (root.ExternalOut.Count > 0)
                    toProcess++;
            }

            // If there is an agent entering
            if (root.Out.Count > 0)
            {
                // Get agent from the node
                Agent a = root.Dequeue();

                // Perform transaction between station and agent
                InteractPort(root.Port, a);

                // Get node that agent will travel to next
                GraphNode exit = root.GetExit(a);

                // Move agent to node if avaialable, otherwise put back onto root
                if (exit != null)
                    exit.Enqueue(a);
                else
                    root.Enqueue(a);

                // If there are more agents, then the tick isn't done
                if (root.Out.Count > 0)
                    toProcess++;
            }

            return toProcess;
        }
        /// <summary>
        /// Processes the first available agent in a branch node.
        /// </summary>
        /// <param name="b">Index of the branch node to process.</param>
        /// <param name="state">State of the parallel loop.</param>
        /// <param name="doneTick">Whether or not the process cylce has been completed.</param>
        /// <returns>The doneTick variable.</returns>
        private long ProcessBranchSingle(int b, ParallelLoopState state, long toProcess)
        {
            // Get branch
            BranchNode branch = Branches[b];

            // If there is an agent to process
            if (branch.Out.Count > 0)
            {
                // Get agent from node
                Agent a = branch.Dequeue();

                // Perform transaction between station and agent
                InteractJunc(branch.Junction, a);

                // Get node that agent will travel to next
                GraphNode exit = branch.GetExit(a);

                // Move agent to node if available, otherwise put back onto branch
                if (exit != null)
                    exit.Enqueue(a);
                else
                    branch.Enqueue(a);

                // If there are more agents, then the tick isn't done
                if (branch.Out.Count > 0)
                    toProcess++;
            }

            return toProcess++;
        }
        /// <summary>
        /// Processes the first available agent in a leaf node.
        /// </summary>
        /// <param name="l">Index of the leaf node to process.</param>
        /// <param name="state">State of the parallel loop.</param>
        /// <param name="doneTick">Whether or not the process cycle has been completed.</param>
        /// <returns>The doneTick variable.</returns>
        private long ProcessLeafSingle(int l, ParallelLoopState state, long toProcess)
        {
            // Get leaf
            LeafNode leaf = Leaves[l];

            // If there is an agent to process
            if (leaf.Out.Count > 0)
            {
                // Get agent from node
                Agent a = leaf.Dequeue();

                // Perform transaction between station and agent
                InteractTerm(leaf.Terminal, a);

                // Get node that agent will travel to next
                GraphNode exit = leaf.GetExit(a);

                // Move agent to node if available, otherwise put back onto leaf
                if (exit != null)
                    exit.Enqueue(a);
                else
                    leaf.Enqueue(a);

                // If there are more agents, then the tick isn't done
                if (leaf.Out.Count > 0)
                    toProcess++;
            }

            return toProcess;
        }

        #endregion
    }
}
