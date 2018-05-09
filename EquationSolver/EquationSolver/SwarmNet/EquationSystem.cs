using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SwarmNet;

namespace EquationSolver.SwarmNet
{
    class EquationSystem
    {
        public ModelGraph System { get; private set; }

        public EquationSystem(int minRange, int maxRange, int leaves, int cycles,int adjustments, params int[][] eqs)
        {
            GraphNode cur, gate;

            System = new ModelGraph();

            RootNode root = new RootNode(new RowMaker(eqs[0].Length, minRange, maxRange, 20, cycles));

            root.AddNeighbor(new BranchNode(new Gate(cycles, eqs.Length), 3));
            System.Roots.Add(root);

            gate = root.Neighbors[0];
            System.Branches.Add((BranchNode)root.Neighbors[0]);
            cur = gate;
            cur.AddNeighbor(new BranchNode(new ProblemColumn(0, adjustments, leaves, eqs[0]), leaves + 2));
            System.Branches.Add((BranchNode)cur.Neighbors[1]);
            cur = cur.Neighbors[1];

            for (int i = 1; i < eqs.Length; i++)
            {
                for (int j = 0; j < leaves; j++)
                {
                    cur.AddNeighbor(new LeafNode(new RowAdjuster(eqs[i - 1])));
                    System.Leaves.Add((LeafNode)cur.Neighbors[1 + j]);
                }
                cur.AddNeighbor(new BranchNode(new ProblemColumn(i, adjustments, leaves, eqs[i]), leaves + 2));
                System.Branches.Add((BranchNode)cur.Neighbors[leaves + 1]);
                cur = cur.Neighbors[leaves + 1];
            }

            for (int j = 0; j < leaves; j++)
            {
                cur.AddNeighbor(new LeafNode(new RowAdjuster(eqs[eqs.Length - 1])));
                System.Leaves.Add((LeafNode)cur.Neighbors[1 + j]);
            }
            cur.AddNeighbor(gate);
        }
    }
}
