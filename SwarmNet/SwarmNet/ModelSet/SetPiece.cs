using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet
{
    public abstract class SetPiece<JI, JO, TI, TO>
    {
        #region Fields

        /// <summary>
        /// The node that this set piece belongs to.
        /// </summary>
        protected GraphNode<JI, JO, TI, TO> _loc;

        #endregion

        #region Properties

        /// <summary>
        /// The node that this set piece is on.
        /// </summary>
        public GraphNode<JI, JO, TI, TO> Location
        {
            get
            {
                return _loc;
            }
            set
            {
                _loc = value;
            }
        }

        #endregion
    }
}
