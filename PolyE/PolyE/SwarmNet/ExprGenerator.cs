using System;
using System.Collections.Generic;
using SwarmNet;

namespace PolyE
{
    public class ExprGenerator : Portal<int, int, string, Tuple<int, int, AlgOp>>
    {
        #region Globals

        /// <summary>
        /// The next available tag number.
        /// </summary>
        private static int _nextID = 0;
        /// <summary>
        /// The priority of the expressions.
        /// </summary>
        private static int _priority = 0;
        /// <summary>
        /// The random generator used to make new agents.
        /// </summary>
        private static Random _rand = new Random();

        #endregion

        #region Fields

        /// <summary>
        /// The exclusive maximum value a coefficient can be.
        /// </summary>
        private int _coeffMax;
        /// <summary>
        /// The inclusive minimum value a coefficiant can be.
        /// </summary>
        private int _coeffMin;
        /// <summary>
        /// The degree of expressions.
        /// </summary>
        private int _degree;
        /// <summary>
        /// The queue of agents ready to enter the graph.
        /// </summary>
        private List<Expression> _queue;

        #endregion

        #region Properties

        /// <summary>
        /// The exclusive maximum value a coefficient can be.
        /// </summary>
        public int CoeffMax
        {
            get
            {
                return _coeffMax;
            }
        }
        /// <summary>
        /// The inclusive minimum value a coefficiant can be.
        /// </summary>
        public int CoeffMin
        {
            get
            {
                return _coeffMin;
            }
        }
        /// <summary>
        /// The degree of expressions.
        /// </summary>
        public int Degree
        {
            get
            {
                return _degree;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new spawner.
        /// </summary>
        /// <param name="cRange">The range of coefficients the expressions can have.</param>
        /// <param name="dRange">The range of degrees the expressions can have.</param>
        public ExprGenerator(Tuple<int, int> cRange, int deg)
        {
            _coeffMax = cRange.Item2;
            _coeffMin = cRange.Item1;
            _degree = deg;
            _queue = new List<Expression>();
        }

        #endregion

        #region Methods - Logistics

        /// <summary>
        /// Converts this spawner to a string.
        /// </summary>
        /// <returns>The string representation of this spawner.</returns>
        public override string ToString()
        {
            return string.Format("ExprGenerator: Next: {0} Coeffs: [{1}, {2}) Degrees: {3}", _nextID, _coeffMin, _coeffMax, _degree);
        }

        #endregion

        #region Methods - Spawner Operations

        /// <summary>
        /// Takes an old expression off the queue or creates a new one, and then releases it to the graph.
        /// </summary>
        /// <returns>The agent that will be used for the graph.</returns>
        public override Agent<int, int, string, Tuple<int, int, AlgOp>> Enter()
        {
            Expression a;

            if (_queue.Count > 0)
            {
                a = _queue[0];
                _queue.Remove(a);
                return a;
            }

            int[] coeffs = new int[_degree];

            for (int c = 0; c < coeffs.Length; c++)
            {
                coeffs[c] = _rand.Next(_coeffMin, _coeffMax);
            }

            a = new Expression(string.Format("expr{0}", _nextID), _priority, coeffs);
            _nextID++;

            return a;
        }
        /// <summary>
        /// Stores an old agent for later use on the queue.
        /// </summary>
        /// <param name="a">The agent to store.</param>
        public override void Leave(Agent<int, int, string, Tuple<int, int, AlgOp>> a)
        {
            _queue.Add((Expression)a);
        }

        #endregion
    }
}
