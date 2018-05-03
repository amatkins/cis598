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

        public EquationSystem(int min, int max, int attempts, params int[][] eqs)
        {
            GraphNode cur, gate;

            System = new ModelGraph();

            RootNode root = new RootNode(new RowMaker(eqs[0].Length, min, max, 20, attempts));

            root.AddNeighbor(new BranchNode(new Gate(attempts, eqs.Length), 3));
            System.Roots.Add(root);

            gate = root.Neighbors[0];
            System.Branches.Add((BranchNode)root.Neighbors[0]);
            cur = gate;
            cur.AddNeighbor(new BranchNode(new CoefficientSet(0, attempts, eqs[0]), 3));
            System.Branches.Add((BranchNode)cur.Neighbors[1]);
            cur = cur.Neighbors[1];

            for (int i = 1; i < eqs.Length; i++)
            {
                cur.AddNeighbor(new LeafNode(new VariableAdjuster()));
                System.Leaves.Add((LeafNode)cur.Neighbors[1]);
                cur.AddNeighbor(new BranchNode(new CoefficientSet(i, attempts, eqs[i]), 3));
                System.Branches.Add((BranchNode)cur.Neighbors[2]);
                cur = cur.Neighbors[2];
            }

            cur.AddNeighbor(new LeafNode(new VariableAdjuster()));
            System.Leaves.Add((LeafNode)cur.Neighbors[1]);
            cur.AddNeighbor(gate);
        }
    }
}
