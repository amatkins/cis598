using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using SwarmNet;

namespace PolyE
{
    [DataContract(Name = "Generator", Namespace = "PolyE")]
    public class ExprGenerator : Port
    {
        #region Globals

        /// <summary>
        /// The next available tag number.
        /// </summary>
        [DataMember(Name = "IDNum")]
        private static int _nextID = 0;
        /// <summary>
        /// The priority of the expressions.
        /// </summary>
        [DataMember(Name = "Priority")]
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
        /// <param name="min">The inclusive minimum value of the coefficients.</param>
        /// <param name="max">The exclusive maximum value of the coefficients.</param>
        /// <param name="deg">The number of terms for the polynomial.</param>
        public ExprGenerator(int min, int max, int deg)
        {
            _coeffMax = max;
            _coeffMin = min;
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
            return string.Format("Next: {0} Coeffs: [{1}, {2}) Degree: {3}", _nextID, _coeffMin, _coeffMax, _degree);
        }

        #endregion

        #region Methods - Communication

        /// <summary>
        /// Communication between an Agent and Port is unnecessary so a null terminator is sent.
        /// </summary>
        /// <param name="m">The incoming message.</param>
        /// <returns>Null terminating message.</returns>
        public override Message Communicate(Message m)
        {
            return new Message(null, CommType.TERM);
        }
        /// <summary>
        /// Dismissal of agents is unnecessary so a null terminator is sent.
        /// </summary>
        /// <param name="m">Dismissal acknowledgment from the agent.</param>
        /// <returns>Null terminating message.</returns>
        public override Message Dismissal(Message m)
        {
            return new Message(null, CommType.TERM);
        }
        /// <summary>
        /// Communication between an agent and Port is unnecessary so a null initializer is sent.
        /// </summary>
        /// <returns>Null initializing message.</returns>
        public override Message InitComm()
        {
            return new Message(null, CommType.INIT);
        }
        /// <summary>
        /// Dismissal of agents is unnecessary so a null initializer is sent.
        /// </summary>
        /// <returns>Null initializing message.</returns>
        public override Message InitDism()
        {
            return new Message(null, CommType.INIT);
        }

        #endregion

        #region Methods - Port Operations

        /// <summary>
        /// Takes an old expression off the queue or creates a new one, and then releases it to the graph.
        /// </summary>
        /// <returns>The agent that will be used for the graph.</returns>
        public override Agent Enter()
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
        public override void Leave(Agent a)
        {
            _queue.Add((Expression)a);
        }

        #endregion
    }
}
