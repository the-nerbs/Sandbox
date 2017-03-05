using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    interface IParseTreeNode
    {
        bool IsTerminal { get; }

        int ChildCount { get; }

        IReadOnlyList<IParseTreeNode> Children { get; }

        void AppendTree(StringBuilder builder);
    }
}
