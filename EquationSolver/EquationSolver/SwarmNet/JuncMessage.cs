using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquationSolver.SwarmNet
{
    class JuncMessage
    {
        public int Tag { get; set; }
        public bool Pass { get; set; }
        public bool Possitive { get; set; }
        public bool[] NextDir { get; set; }

        public JuncMessage(int tag, bool pass, bool poss, params bool[] dirs)
        {
            Tag = tag;
            Pass = pass;
            Possitive = poss;
            NextDir = dirs;
        }
    }
}
