using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public class BranchNode<JI, JO, TI, TO> : GraphNode<JI, JO, TI, TO>
    {
        #region Properties

        /// <summary>
        /// The path that an agent can leave through.
        /// </summary>
        public override GraphNode<JI, JO, TI, TO> Exit
        {
            get
            {
                return _neighbors[((Junction<JI, JO, TI, TO>)_piece).GetExit(_neighbors.Length)];
            }
        }
        /// <summary>
        /// The junction on this node.
        /// </summary>
        public Junction<JI, JO, TI, TO> Junction
        {
            get
            {
                return (Junction<JI, JO, TI, TO>)_piece;
            }
            set
            {
                _piece = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new branch node with a set number of neighbors.
        /// </summary>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public BranchNode(int neighbors)
        {
            if (neighbors < 1)
                throw new ArgumentException("Node cannot have less than 1 potential neighbors.");

            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[neighbors];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = null;
        }
        /// <summary>
        /// Constructs a new branch node with a set number of neighbors and containing the provided junction.
        /// </summary>
        /// <param name="junction">The junction this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public BranchNode(Junction<JI, JO, TI, TO> junction, int neighbors)
        {
            if (neighbors < 1)
                throw new ArgumentException("Node cannot have less than 1 potential neighbors.");

            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[neighbors];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = junction;
            junction.Location = this;
        }

        #endregion

        #region Methods - Set Piece Operations

        /// <summary>
        /// Acts as middle man between agent and set piece.
        /// </summary>
        /// <param name="m">The message to pass to the set piece.</param>
        /// <returns>The set piece's reponse.</returns>
        public Message<JO> Communicate(Message<JI> m)
        {
            return ((Junction<JI, JO, TI, TO>)_piece).Communicate(m);
        }
        /// <summary>
        /// Start communications with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public Message<JO> InitComm()
        {
            return ((Junction<JI, JO, TI, TO>)_piece).InitComm();
        }

        #endregion
    }
}
