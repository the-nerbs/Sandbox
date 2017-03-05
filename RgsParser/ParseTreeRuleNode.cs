using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    [DebuggerDisplay("({Rule} : {ChildCount} Child(ren))")]
    class ParseTreeRuleNode : IParseTreeNode
    {
        private readonly List<IParseTreeNode> _children = new List<IParseTreeNode>();


        public int ChildCount
        {
            get { return _children.Count; }
        }

        public IReadOnlyList<IParseTreeNode> Children
        {
            get { return _children; }
        }

        public bool IsTerminal
        {
            get { return (ChildCount == 0); }
        }

        public Rule Rule { get; }


        public ParseTreeRuleNode(Rule rule, params IParseTreeNode[] children)
            : this(rule, (IEnumerable<IParseTreeNode>)children)
        { }

        public ParseTreeRuleNode(Rule rule, IEnumerable<IParseTreeNode> children)
        {
            _children = new List<IParseTreeNode>(children);
            Rule = rule;
        }

        public void AppendTree(StringBuilder builder)
        {
            builder.Append("(<");
            builder.Append(Rule);
            builder.Append("> ");

            foreach (var child in Children)
            {
                child.AppendTree(builder);
                builder.Append(" ");
            }

            builder.Append(")");
        }
    }
}
