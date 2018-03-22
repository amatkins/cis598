using System;
using System.Text;
using System.Linq;
using SwarmNet;

namespace PolynomialSwarm
{
    class Program
    {
        private static Random _r = new Random();

        static void Main(string[] args)
        {
            ModelGraph<int,int,object,Tuple<int,int>> model = new ModelGraph<int,int,object,Tuple<int,int>>(new Random(), 20, 4, 1, 35, SpawnMaker, JuncMaker, TermMaker);

            while (true)
            {
                if (model.Population.Length < 10)
                    model.Spawn();
                model.Tick();
                Console.Clear();
                Console.WriteLine(GraphToString(model));
                Console.ReadKey();
            }
        }

        static FuncGen SpawnMaker()
        {
            return new FuncGen(4, 16);
        }

        static TurnStyle JuncMaker(int paths)
        {
            return new TurnStyle(_r.Next(16) + 8, paths);
        }

        static Transformer TermMaker()
        {
            return new Transformer(_r.Next(4), _r.Next(47) - 23);
        }

        static string GraphToString(ModelGraph<int,int,object,Tuple<int, int>> model)
        {
            return string.Format("Branches:\n{0}\nLeaves:\n{1}\nAgents:\n{2}",
                string.Join("\n", model.Branches.Select(branch => string.Format("State: {0}, Max: {1}, Agents: [{2}]",
                    ((TurnStyle)branch.Junction).State,
                    ((TurnStyle)branch.Junction).Max,
                    string.Join(", ", branch.Out.Select(a => a.Tag).ToArray()))).ToArray()),
                string.Join("\n", model.Leaves.Select(leaf => string.Format("Degree: {0}, Change: {1}, Agents: [{2}]",
                    ((Transformer)leaf.Terminal).Degree,
                    ((Transformer)leaf.Terminal).Change,
                    string.Join(", ", leaf.Out.Select(a => a.Tag).ToArray()))).ToArray()),
                string.Join("\n", model.Population.Select(a => string.Format("Tag: {0}, Expression: {1}",
                    a.Tag,
                    string.Join("+", ((PolyNum)a).Coefficients.Select(c => string.Format("{0}*x", c)).ToArray()))).ToArray()));
        }
    }
}
