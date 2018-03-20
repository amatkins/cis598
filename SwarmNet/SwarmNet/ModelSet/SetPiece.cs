using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwarmNet.ModelSet
{
    public abstract class SetPiece
    {
        #region Fields

        /// <summary>
        /// The node that this set piece belongs to.
        /// </summary>
        ModelNode<SetPiece> _loc;

        #endregion
    }
}
