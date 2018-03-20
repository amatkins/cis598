using System;
using SwarmNet.RigSet;

namespace PolynomialSwarm
{
    class Program
    {
        static void Main(string[] args)
        {
            RigGraph graph = new RigGraph();

            while (true)
            {
                Console.Clear();
                graph.GenTree(new Random(), 100000, 4, 1, 35);
                Console.Write(graph.ToString());
                Console.ReadKey();
            }
        }
    }
}
