using System;
using System.Linq;
using System.Runtime.Serialization;
using SwarmNet;

namespace PolyE
{
    [DataContract(Name = "PolyE", Namespace = "PolyE")]
    class PolyEModel
    {
        #region Fields

        /// <summary>
        /// The graph of the model.
        /// </summary>
        [DataMember(Name = "Graph")]
        private ModelGraph<int, int, string, Tuple<int, int, AlgOp>> _graph;
        /// <summary>
        /// The random number generator for the model.
        /// </summary>
        private Random _rand;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new PolyEModel.
        /// </summary>
        /// <param name="nodes">The number of nodes on the graph.</param>
        /// <param name="kids">The number of kids each node can have at max.</param>
        /// <param name="offset">The offset from the average number of kids during generation.</param>
        /// <param name="leaf">The chance a node will be a leaf.</param>
        public PolyEModel(int nodes, int kids, int offset, int leaf)
        {
            _rand = new Random();
            _graph = new ModelGraph<int, int, string, Tuple<int, int, AlgOp>>(_rand, nodes, kids, offset, leaf);
            _graph.BuildSet(SpawnMaker, JuncMaker, TermMaker);
        }

        #endregion

        #region Methods - Set Piece Builder

        /// <summary>
        /// Method for creating spawners.
        /// </summary>
        /// <returns>A spawner.</returns>
        public ExprGenerator SpawnMaker()
        {
            int cmin, cmax;

            cmin = _rand.Next(6);
            cmax = cmin + _rand.Next(10) + 1;

            return new ExprGenerator(new Tuple<int, int>(cmin, cmax), 5);
        }
        /// <summary>
        /// Method for creating junctions.
        /// </summary>
        /// <param name="paths">The number of avaialable paths.</param>
        /// <returns>A junction.</returns>
        public Conditional JuncMaker(int paths)
        {
            return new Conditional(_rand.Next(paths, paths * 10 + 1), paths);
        }
        /// <summary>
        /// Method for creating terminals.
        /// </summary>
        /// <returns>A terminal.</returns>
        public Modification TermMaker()
        {
            return new Modification(_rand.Next(1, 6), AlgOp.ADD, _rand.Next(32) - 16);
        }

        #endregion

        #region Methods - Logistics

        /// <summary>
        /// Converts this model to a string.
        /// </summary>
        /// <returns>The string representation of this model.</returns>
        public override string ToString()
        {
            return string.Format("{0}" + Environment.NewLine + "[{1}]" + Environment.NewLine + "[{2}]" + Environment.NewLine + "[{3}]",
                string.Join(Environment.NewLine, _graph.Roots.Select(n => string.Format("Head: ({0} In: [{1}] Out: [{2}])",
                    ((ExprGenerator)n.Portal).ToString(),
                    string.Join(", ", n.In.Cast<Expression>()),
                    string.Join(", ", n.Out.Cast<Expression>())))),
                string.Join(Environment.NewLine, _graph.Branches.Select(n => string.Format("Branch: ({0} In: [{1}] Out:[{2}])",
                    ((Conditional)n.Junction).ToString(),
                    string.Join(", ",n.In.Cast<Expression>()),
                    string.Join(", ", n.Out.Cast<Expression>())))),
                string.Join(Environment.NewLine, _graph.Leaves.Select(n => string.Format("Leaf: ({0} In: [{1}] Out:[{2}])",
                    ((Modification)n.Terminal).ToString(),
                    string.Join(", ", n.In.Cast<Expression>()),
                    string.Join(", ", n.Out.Cast<Expression>())))),
                string.Join(Environment.NewLine, _graph.Agents.Cast<Expression>()));
        }
        /// <summary>
        /// Gets a string representation of just the branches of the model.
        /// </summary>
        /// <returns>The string representation of the branches of the model.</returns>
        public string BranchesString()
        {
            return string.Join(Environment.NewLine, _graph.Branches.Select(n => string.Format("Branch: ({0} {1} In: [{2}] Out:[{3}])",
                    n.Junction.State,
                    ((Conditional)n.Junction).ToString(),
                    string.Join(", ", n.In.Cast<Expression>()),
                    string.Join(", ", n.Out.Cast<Expression>()))));
        }
        /// <summary>
        /// Gets a string representation of the head and leaves of the model.
        /// </summary>
        /// <returns>The string representation of the head and leaves of the model.</returns>
        public string LeavesString()
        {
            return string.Format("[{0}" + Environment.NewLine + "{1}]",
                string.Join(Environment.NewLine, _graph.Roots.Select(n => string.Format("Head: ({0} In: [{1}] Out: [{2}])",
                    ((ExprGenerator)n.Portal).ToString(),
                    string.Join(", ", n.In.Cast<Expression>()),
                    string.Join(", ", n.Out.Cast<Expression>())))),
                string.Join(Environment.NewLine, _graph.Leaves.Select(n => string.Format("Leaf: ({0} In: [{1}] Out:[{2}])",
                    ((Modification)n.Terminal).ToString(),
                    string.Join(", ", n.In.Cast<Expression>()),
                    string.Join(", ", n.Out.Cast<Expression>())))));
        }
        /// <summary>
        /// Gets a string representation of just the agents of the model.
        /// </summary>
        /// <returns>The string representation of the agents of the model.</returns>
        public string AgentsString()
        {
            return string.Join(Environment.NewLine, _graph.Agents.Cast<Expression>());
        }

        #endregion

        public void Tick()
        {
            if (_graph.Agents.Length < 10)
                _graph.Enter(0);

            _graph.Tick();
        }
    }
}
