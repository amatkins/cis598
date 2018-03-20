using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet.RigSet
{
    public class RigGraph
    {
        #region Fields

        /// <summary>
        /// The head of the rigging graph.
        /// </summary>
        private RigNode _head;
        /// <summary>
        /// All of the nodes with no children.
        /// </summary>
        private RigNode[] _leaves;
        /// <summary>
        /// All of the nodes with children.
        /// </summary>
        private RigNode[] _branches;

        #endregion

        #region Properties

        /// <summary>
        /// The head of the graph.
        /// </summary>
        public RigNode Head
        {
            get
            {
                return _head;
            }
        }

        /// <summary>
        /// The branches of the graph.
        /// </summary>
        public RigNode[] Branches
        {
            get
            {
                return _branches;
            }
        }

        /// <summary>
        /// The leaves of the graph.
        /// </summary>
        public RigNode[] Leaves
        {
            get
            {
                return _leaves;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs an empty graph.
        /// </summary>
        public RigGraph()
        {
            _head = null;
            _leaves = null;
            _branches = null;
        }

        #endregion

        #region Methods - Manip

        /// <summary>
        /// Generate a new graph for this object.
        /// </summary>
        /// <param name="rand">The random object to be used for generation.</param>
        /// <param name="total_remain">The total number of nodes in this graph.</param>
        /// <param name="max_children">The max number of children per node.</param>
        /// <param name="offset">The offset from the average.</param>
        public void GenTree(Random rand, int total_remain, int max_children, int offset, int leaf_chance)
        {
            int n_kids_tot, c_kids_tot, n_kids_max, n_kids_min, kids_remain, avg_kpn, real_kpn;
            RigNode[] cur_tier;
            List<RigNode> next_tier, branches, leaves;
            RigNode head, current;
            bool first;

            // Initialize branches and leaves;
            branches = new List<RigNode>();
            leaves = new List<RigNode>();
            // Get the total number of children of the head
            n_kids_tot = Math.Min(rand.Next(max_children) + 1, total_remain);
            // Subtract this tiers total from remaining total
            total_remain -= n_kids_tot + 1;
            // Initialize the head with calculated number of children
            head = new RigNode(n_kids_tot);
            // Insert head as only node of next tier of nodes to work on
            next_tier = new List<RigNode>() { head };
            // set first tier bool
            first = true;

            while (n_kids_tot > 0)
            {
                // Set next tier as current tier and get new next tier
                cur_tier = next_tier.ToArray();
                next_tier = new List<RigNode>();
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
                    for (int j = first ? 0 : 1; j < cur_tier[i].Length; j++)
                    {
                        // If the average is greater than what remains to be generated, then clamp to later
                        if (kids_remain < avg_kpn)
                            real_kpn = kids_remain;
                        // Otherwise, make leaf if randomly slected
                        else if (n_kids_tot > 0 && rand.Next(100) < leaf_chance)
                            real_kpn = 0;
                        // Otherwise set real number of children to some random offset from the average
                        // and clamp the value to max or the remaining children if necessary
                        else
                        {
                            real_kpn = Math.Min(avg_kpn + rand.Next(offset * 2 + 1) - Math.Min(offset, avg_kpn) + 1, Math.Min(max_children, kids_remain));

                            if (real_kpn == 0)
                                return;
                        }

                        // Subtract this node's children from remaining children
                        kids_remain -= real_kpn;
                        // Add to total of next tier
                        n_kids_tot += real_kpn;

                        // Generate a node with the calculated number of children
                        current = new RigNode(real_kpn + 1);
                        // Add node to graph
                        cur_tier[i].AddNeighbor(current);
                        // Add to next tier and branches if it has children
                        if (real_kpn > 0)
                        {
                            next_tier.Add(current);
                            branches.Add(current);
                        }
                        // Othersie add to the leaves
                        else
                            leaves.Add(current);
                    }
                }
                // Add back on any un-generated children
                total_remain += kids_remain;
                // turn off first tier bool
                if (first)
                    first = !first;
            }

            // Set all the finalized values
            _head = head;
            _branches = branches.ToArray();
            _leaves = leaves.ToArray();
        }

        #endregion

        #region Methods - Logistic

        public override string ToString()
        {
            return string.Format("Head:\n{0}\n\nBranches:\n{1}\nLeaves:\n{2}\n",
                _head.ToString(),
                string.Concat(_branches.Select(n => string.Format("{0}\n", n.ToString())).ToArray()),
                string.Concat(_leaves.Select(n => string.Format("{0}\n", n.ToString())).ToArray()));
        }

        #endregion
    }
}
