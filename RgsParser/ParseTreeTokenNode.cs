using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    [DebuggerDisplay("({Token})")]
    class ParseTreeTokenNode : IParseTreeNode
    {
        private static readonly IParseTreeNode[] EmptyChildren = { };


        public int ChildCount
        {
            get { return 0; }
        }

        public IReadOnlyList<IParseTreeNode> Children
        {
            get { return EmptyChildren; }
        }

        public bool IsTerminal
        {
            get { return true; }
        }

        public Token Token { get; }


        public ParseTreeTokenNode(Token token)
        {
            Debug.Assert(token != null);
            Token = token;
        }

        public void AppendTree(StringBuilder builder)
        {
            builder.Append('"');
            builder.Append(Token.Value);
            builder.Append('"');
        }
    }
}
