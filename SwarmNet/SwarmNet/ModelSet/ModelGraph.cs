using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(Name = "Graph", Namespace = "SwarmNet")]
    public class ModelGraph<JI, JO, TI, TO>
    {
        #region Fields

        /// <summary>
        /// The agents on the graph.
        /// </summary>
        [DataMember(Name = "Agents", Order = 1)]
        private List<Agent<JI, JO, TI, TO>> _agents;
        /// <summary>
        /// The branch nodes of the graph.
        /// </summary>
        [DataMember(Name = "Branches", Order = 3)]
        private BranchNode<JI, JO, TI, TO>[] _branches;
        /// <summary>
        /// The leaf nodes of the graph.
        /// </summary>
        [DataMember(Name = "Leaves", Order = 4)]
        private LeafNode<JI, JO, TI, TO>[] _leaves;
        /// <summary>
        /// The head node of the graph.
        /// </summary>
        [DataMember(Name = "Roots", Order = 2)]
        private RootNode<JI, JO, TI, TO>[] _roots;
        /// <summary>
        /// The number of nodes in each tier.
        /// </summary>
        [DataMember(Name = "TierCount")]
        private int[] _tiers;

        #endregion

        #region Properties

        /// <summary>
        /// The agents of the graph.
        /// </summary>
        public Agent<JI, JO, TI, TO>[] Agents
        {
            get
            {
                return _agents.ToArray();
            }
        }
        /// <summary>
        /// The branches of the graph.
        /// </summary>
        public BranchNode<JI, JO, TI, TO>[] Branches
        {
            get
            {
                return _branches;
            }
        }
        /// <summary>
        /// The leaves of the graph.
        /// </summary>
        public LeafNode<JI, JO, TI, TO>[] Leaves
        {
            get
            {
                return _leaves;
            }
        }
        /// <summary>
        /// The head of the graph.
        /// </summary>
        public RootNode<JI, JO, TI, TO>[] Roots
        {
            get
            {
                return _roots;
            }
        }
        /// <summary>
        /// The number of tiers there are in the graph.
        /// </summary>
        public int TiersCount
        {
            get
            {
                return _tiers.Length;
            }
        }

        #endregion

        #region Constructors
        
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
            _agents = new List<Agent<JI, JO, TI, TO>>();
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
        public ModelGraph(Random r, int n, int m, int o, int l, Func<Portal<JI, JO, TI, TO>> p, Func<int, Junction<JI, JO, TI, TO>> j, Func<Terminal<JI, JO, TI, TO>> t)
        {
            _agents = new List<Agent<JI, JO, TI, TO>>();
            GenTree(r, n, m, o, l);
            BuildSet(p, j, t);
        }

        #endregion

        #region Methods - Graph Operations
        
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
            GraphNode<JI, JO, TI, TO>[] cur_tier;
            List<GraphNode<JI, JO, TI, TO>> next_tier;
            RootNode<JI, JO, TI, TO> root;
            List<BranchNode<JI, JO, TI, TO>> branches;
            List<LeafNode<JI, JO, TI, TO>> leaves;
            GraphNode<JI, JO, TI, TO> current;
            List<int> tiers;
            bool isRoot;

            // Initialize branches and leaves;
            branches = new List<BranchNode<JI, JO, TI, TO>>();
            leaves = new List<LeafNode<JI, JO, TI, TO>>();
            tiers = new List<int>();
            // Initialize the positioning variables
            tier = 0;
            tier_index = 0;
            // The head will have one neighbor
            n_kids_tot = 1;
            // Subtract the head and it's neighbor from the remaining total
            total_remain -= n_kids_tot + 1;
            // Initialize the head
            root = new RootNode<JI, JO, TI, TO>();
            root.Tier = tier;
            root.TierIndex = tier_index;
            tier_index++;
            // Insert head as only node of next tier of nodes to work on
            next_tier = new List<GraphNode<JI, JO, TI, TO>>() { root };
            // set first tier bool
            isRoot = true;

            while (n_kids_tot > 0)
            {
                // Set next tier as current tier and get new next tier
                cur_tier = next_tier.ToArray();
                next_tier = new List<GraphNode<JI, JO, TI, TO>>();
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
                    for (int j = isRoot ? 0 : 1; j < cur_tier[i].Length; j++)
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
                            current = new LeafNode<JI, JO, TI, TO>();
                            leaves.Add((LeafNode<JI, JO, TI, TO>)current);
                        }
                        else
                        {
                            current = new BranchNode<JI, JO, TI, TO>(real_kpn + 1);
                            branches.Add((BranchNode<JI, JO, TI, TO>)current);
                            next_tier.Add(current);
                        }
                        // Position current node
                        current.Tier = tier;
                        current.TierIndex = tier_index;
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
            _roots = new RootNode<JI, JO, TI, TO>[] { root };
            _branches = branches.ToArray();
            _leaves = leaves.ToArray();
            _tiers = tiers.ToArray();
        }
        /// <summary>
        /// Converts a tree styled rigging graph to a model graph.
        /// </summary>
        /// <param name="rig">The rigging graph to conver.</param>
        /// <param name="port">Method for generating portals.</param>
        /// <param name="junc">Method for generating junctions.</param>
        /// <param name="term">Method for generating terminals.</param>
        public void BuildSet(Func<Portal<JI, JO, TI, TO>> port, Func<int, Junction<JI, JO, TI, TO>> junc, Func<Terminal<JI, JO, TI, TO>> term)
        {
            List<GraphNode<JI, JO, TI, TO>> current_model, next_model = _roots.ToList<GraphNode<JI, JO, TI, TO>>();
            GraphNode<JI, JO, TI, TO> model_node;
            bool isHead = true;

            do
            {
                // Switch next to current, and get new next
                current_model = next_model;
                next_model = new List<GraphNode<JI, JO, TI, TO>>();

                // Got through each node of current
                for (int i = 0; i < current_model.Count; i++)
                {
                    // Get current node
                    model_node = current_model[i];

                    // Build set piece on node and add un-built children to next tier
                    if (isHead)
                    {
                        ((RootNode<JI, JO, TI, TO>)model_node).Portal = port();
                        if (model_node[0].Piece == null)
                            next_model.Add(model_node[0]);
                    }
                    else
                    {
                        if (model_node.Length > 1)
                        {
                            ((BranchNode<JI, JO, TI, TO>)model_node).Junction = junc(model_node.Length);
                            for (int j = 0; j < model_node.Length; j++)
                            {
                                if (model_node[j].Piece == null)
                                    next_model.Add(model_node[j]);
                            }
                        }
                        else
                            ((LeafNode<JI, JO, TI, TO>)model_node).Terminal = term();
                    }
                }

                // Flip bool for speach head case
                if (isHead)
                    isHead = false;
            } while (next_model.Count > 0);
        }

        #endregion

        #region Methods - Events

        /// <summary>
        /// Add a new agent to the graph.
        /// </summary>
        public void Enter(int index)
        {
            if (index < 0 || index >= _roots.Length)
                return;

            _agents.Add(_roots[index].Enter());
        }
        /// <summary>
        /// Remove an agent from the graph.
        /// </summary>
        public void Leave(int index)
        {
            if (index < 0 || index >= _roots.Length)
                return;

            _agents.Remove(_roots[index].Leave());
        }
        /// <summary>
        /// Moves the simulation of the model along by one generic time unit.
        /// </summary>
        public void Tick()
        {
            Agent<JI, JO, TI, TO> a;
            Message<JO> jO;
            Message<TO> tO;

            for (int r = 0; r < _roots.Length; r++)
            {
                // If the head is not connected, then throw an exception
                if (_roots[r].Exit == null)
                    throw new InvalidOperationException("Entrance to graph not connected.");

                // Exit graph through portal
                while (_roots[r].ExOutCount > 0)
                {
                    Leave(r);
                }
                // Enter the graph
                while (_roots[r].OutCount > 0)
                {
                    _roots[r].Exit.Enqueue(_roots[r].Dequeue());
                }
            }

            // Perform interactions for all the branch nodes
            foreach (BranchNode<JI, JO, TI, TO> branch in _branches)
            {
                // For every agent on the node...
                while (branch.OutCount > 0)
                {
                    // Get next agent and start communications
                    a = branch.Dequeue();
                    jO = branch.InitComm();

                    // Communicate until the node quits
                    do
                    {
                        jO = branch.Communicate(a.CommJunc(jO));
                    } while (jO.Type != CommType.FINISH);

                    // Throw an exception if the agent can't leave
                    if (branch.Exit == null)
                        throw new InvalidOperationException("Exit path not connected to a node.");

                    // Have the agent leave.
                    branch.Exit.Enqueue(a);
                }
            }

            // Perform interactions for all the leaf nodes
            foreach (LeafNode<JI, JO, TI, TO> leaf in _leaves)
            {
                // For every agent on the node...
                while (leaf.OutCount > 0)
                {
                    // Get next agent and start communications
                    a = leaf.Dequeue();
                    tO = leaf.InitComm();

                    // Communicate until the node quits
                    do
                    {
                        tO = leaf.Communicate(a.CommTerm(tO));
                    } while (tO.Type != CommType.FINISH);

                    // Throw an exception if the agent can't leave
                    if (leaf.Exit == null)
                        throw new InvalidOperationException("Exit path not connected to a node.");

                    // Have the agent leave.
                    leaf.Exit.Enqueue(a);
                }
            }

            // Move all agents into their respective nodes
            foreach (RootNode<JI, JO, TI, TO> root in _roots)
            {
                root.Flush();
            }
            foreach (BranchNode<JI, JO, TI, TO> branch in _branches)
            {
                branch.Flush();
            }
            foreach (LeafNode<JI, JO, TI, TO> leaf in _leaves)
            {
                leaf.Flush();
            }
        }
        /// <summary>
        /// Moves the simulation of the model along by a part of one generic time unit.
        /// </summary>
        /// <returns>Whether or not a tick has passed.</returns>
        public bool SemiTick()
        {
            Agent<JI, JO, TI, TO> a;
            Message<JO> jO;
            Message<TO> tO;
            bool doneTick = true;

            for (int r = 0; r < _roots.Length; r++)
            {
                // If the head is not connected, then throw an exception
                if (_roots[r].Exit == null)
                    throw new InvalidOperationException("Entrance to graph not connected.");

                // Exit graph through portal
                if (_roots[r].ExOutCount > 0)
                {
                    Leave(r);
                    // If this wasn't the last action for this step, then the tick isn't done
                    if (_roots[r].ExOutCount > 0)
                        doneTick = false;
                }
                // Enter the graph
                if (_roots[r].OutCount > 0)
                {
                    _roots[r].Exit.Enqueue(_roots[r].Dequeue());
                    // If this wasn't the last action for this step, then the tick isn't done
                    if (_roots[r].OutCount > 0)
                        doneTick = false;
                }
            }

            // Perform interactions for all the branch nodes
            foreach (BranchNode<JI, JO, TI, TO> branch in _branches)
            {
                // If there are no agents, then continue
                if (branch.OutCount < 1)
                    continue;

                // Get next agent and start communications
                a = branch.Dequeue();
                jO = branch.InitComm();

                // Communicate until the node quits
                do
                {
                    jO = branch.Communicate(a.CommJunc(jO));
                } while (jO.Type != CommType.FINISH);

                // Throw an exception if the agent can't leave
                if (branch.Exit == null)
                    throw new InvalidOperationException("Exit path not connected to a node.");

                // Have the agent leave.
                branch.Exit.Enqueue(a);

                // If this wasn't the last action for this step, then the tick isn't done
                if (branch.OutCount > 0)
                    doneTick = false;
            }

            // Perform interactions for all the branch nodes
            foreach (LeafNode<JI, JO, TI, TO> leaf in _leaves)
            {
                // If there are no agents, then continue
                if (leaf.OutCount < 1)
                    continue;

                // Get next agent and start communications
                a = leaf.Dequeue();
                tO = leaf.InitComm();

                // Communicate until the node quits
                do
                {
                    tO = leaf.Communicate(a.CommTerm(tO));
                } while (tO.Type != CommType.FINISH);

                // Throw an exception if the agent can't leave
                if (leaf.Exit == null)
                    throw new InvalidOperationException("Exit path not connected to a node.");

                // Have the agent leave.
                leaf.Exit.Enqueue(a);

                // If this wasn't the last action for this step, then the tick isn't done
                if (leaf.OutCount > 0)
                    doneTick = false;
            }

            // If this is the end of the tick
            if (doneTick)
            {
                // Move all agents into their respective nodes
                foreach (RootNode<JI, JO, TI, TO> root in _roots)
                {
                    root.Flush();
                }
                foreach (BranchNode<JI, JO, TI, TO> branch in _branches)
                {
                    branch.Flush();
                }
                foreach (LeafNode<JI, JO, TI, TO> leaf in _leaves)
                {
                    leaf.Flush();
                }
            }

            return doneTick;
        }

        #endregion

        #region Methods - Logistics

        /// <summary>
        /// The number of nodes in a tier.
        /// </summary>
        /// <param name="index">The tier to get the number of nodes from.</param>
        /// <returns>The number of nodes in the requested tier.</returns>
        public int TierCount(int index)
        {
            if (index < 0 || index > _tiers.Length)
                throw new ArgumentException("Index out of range.");

            return _tiers[index];
        }

        #endregion
    }
}
