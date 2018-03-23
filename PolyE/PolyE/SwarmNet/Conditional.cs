using System;
using System.Linq;
using SwarmNet;

namespace PolyE
{
    public class Conditional : Junction<int, int, string, Tuple<int, int, AlgOp>>
    {
        #region Fields

        /// <summary>
        /// Divids the state-space to fit the paths.
        /// </summary>
        private int[] _divs;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a new junction.
        /// </summary>
        /// <param name="max">The exclusive max value of the state.</param>
        /// <param name="paths">The number of paths out of the junction.</param>
        public Conditional(int max, int paths)
        {
            _divs = new int[paths - 1];
            _max = max;
            _state = 0;

            int div = max / (paths - 1),
                dif = div;
            for (int i = 0; i < _divs.Length; i++)
            {
                _divs[i] = div;
                div += dif;
            }
        }

        #endregion

        #region Methods - Divider Operations

        /// <summary>
        /// Sets the location of a divider in the state-space.
        /// </summary>
        /// <param name="i">The divider to set.</param>
        /// <param name="d">Where to set the divider.</param>
        public void SetDivider(int i, int d)
        {
            if (i < _divs.Length - 1)
            {
                if (d < _divs[i + 1])
                    _divs[i] = d;
            }
            else
            {
                if (d < _max)
                    _divs[i] = d;
            }
        }
        /// <summary>
        /// Gets the location of a divider in the state-space.
        /// </summary>
        /// <param name="i">The divider to get.</param>
        /// <returns>Where the divider is.</returns>
        public int GetDivider(int i)
        {
            if (i < 0 || i >= _divs.Length)
                throw new ArgumentException("Index out of bounds.");

            return _divs[i];
        }

        #endregion

        #region Methods - Logistics

        /// <summary>
        /// Converts this conditional to a string.
        /// </summary>
        /// <returns>The string representation of this conditional.</returns>
        public override string ToString()
        {
            return string.Format("Conditional: [{0}]", string.Join(", ", _divs));
        }

        #endregion

        #region Methods - Junction Operations

        /// <summary>
        /// Updates state and terminates the transaction.
        /// </summary>
        /// <param name="m">The message containing the new state.</param>
        /// <returns>The termination message.</returns>
        public override Message<int> Communicate(Message<int> m)
        {
            _state = m.Contents % _max;
            return new Message<int>(CommType.FINISH, _state);
        }
        /// <summary>
        /// Determines an exit path based on the current state and the state-space partitions.
        /// </summary>
        /// <param name="paths">The number of paths out of the junction.</param>
        /// <returns>The path to exit by.</returns>
        public override int GetExit(int paths)
        {
            for (int i = 0; i < _divs.Length; i++)
            {
                if (_state < _divs[i])
                    return i;
            }
            
            return 0;
        }
        /// <summary>
        /// Presents state for update.
        /// </summary>
        /// <returns>The initial message with the state of the junction.</returns>
        public override Message<int> InitComm()
        {
            return new Message<int>(CommType.START, _state);
        }

        #endregion
    }
}
