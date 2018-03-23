using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SwarmNet
{
    [DataContract(IsReference = true, Name = "Leaf", Namespace = "SwarmNet")]
    public class LeafNode<JI, JO, TI, TO> : GraphNode<JI, JO, TI, TO>
    {
        #region Properties

        /// <summary>
        /// The exit from this node.
        /// </summary>
        public override GraphNode<JI, JO, TI, TO> Exit
        {
            get
            {
                return _neighbors[0];
            }
        }
        /// <summary>
        /// The terminal on this node.
        /// </summary>
        public Terminal<JI, JO, TI, TO> Terminal
        {
            get
            {
                return (Terminal<JI, JO, TI, TO>)_piece;
            }
            set
            {
                _piece = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new leaf node.
        /// </summary>
        public LeafNode()
        {
            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[1];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = null;
        }
        /// <summary>
        /// Constructs a new leaf node that contains the provided terminal.
        /// </summary>
        /// <param name="terminal">The terminal this node will have.</param>
        /// <param name="neighbors">The max number of neighbors this node can have.</param>
        public LeafNode(Terminal<JI, JO, TI, TO> terminal)
        {
            _inFlow = new List<Agent<JI, JO, TI, TO>>();
            _neighbors = new GraphNode<JI, JO, TI, TO>[1];
            _nextNeighbor = 0;
            _outFlow = new List<Agent<JI, JO, TI, TO>>();
            _piece = terminal;
            terminal.Location = this;
        }

        #endregion

        #region Methods - Set Piece Operations
        
        /// <summary>
        /// Acts as a middle man between set piece and agent.
        /// </summary>
        /// <param name="m">The message to pass to the set piece.</param>
        /// <returns>The response from the set piece.</returns>
        public Message<TO> Communicate(Message<TI> m)
        {
            return ((Terminal<JI, JO, TI, TO>)_piece).Communicate(m);
        }
        /// <summary>
        /// Starts communication with an agent.
        /// </summary>
        /// <returns>The first message.</returns>
        public Message<TO> InitComm()
        {
            return ((Terminal<JI, JO, TI, TO>)_piece).InitComm();
        }

        #endregion
    }
}
