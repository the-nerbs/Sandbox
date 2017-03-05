using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    struct SourceLocation
    {
        public int RawLocation { get; }
        public int Line { get; }
        public int Column { get; }


        public SourceLocation(int index, int line, int column)
        {
            RawLocation = index;
            Line = line;
            Column = column;
        }
    }
}
