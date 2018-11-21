using System;
using System.Collections.Generic;
using System.Text;

namespace StratumFive.Core
{
    public class Input
    {
        public Grid Grid { get; set; }

        // Position
        public Position Position { get; set; }

        // Heading
        public Heading Heading { get; set; }

        // Instructions
        public List<Instruction> Instructions { get; set; }
    }
}
