using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    class Program
    {
        private const string TestRgs = @"HKCR
{
    AtlObj1ProgId.1 = s 'AtlObj1 Class'
    {
        CLSID = s '{D15A646A-4F2F-42C2-BA8B-780AABCFB133}'
    }
    AtlObj1ProgId = s 'AtlObj1 Class'
    {       
        CurVer = s 'AtlObj1ProgId.1'
    }
    NoRemove CLSID
    {
        ForceRemove {D15A646A-4F2F-42C2-BA8B-780AABCFB133} = s 'AtlObj1 Class'
        {
            ProgID = s 'AtlObj1ProgId.1'
            VersionIndependentProgID = s 'AtlObj1ProgId'
            ForceRemove Programmable
            InprocServer32 = s '%MODULE%'
            {
                val ThreadingModel = s 'Neutral'
            }
            TypeLib = s '{85D8EC5E-3C24-4151-83D9-34CCE9A1E534}'
            Version = s '1.0'
        }
    }

    AltObj2ProgId.1 = s 'AtlObj2 Class'
    {
        CLSID = s '{C208B430-8E12-4C65-AA5A-899F6AB13C4B}'
    }
    AltObj2ProgId = s 'AtlObj2 Class'
    {       
        CurVer = s 'AltObj2ProgId.1'
    }
    NoRemove CLSID
    {
        ForceRemove {C208B430-8E12-4C65-AA5A-899F6AB13C4B} = s 'AtlObj2 Class'
        {
            ProgID = s 'AltObj2ProgId.1'
            VersionIndependentProgID = s 'AltObj2ProgId'
            ForceRemove Programmable
            InprocServer32 = s '%MODULE%'
            {
                val ThreadingModel = s 'Neutral'
            }
            TypeLib = s '{85D8EC5E-3C24-4151-83D9-34CCE9A1E534}'
            Version = s '1.0'
        }
    }
}";

        static void Main(string[] args)
        {
            var source = new Source(TestRgs);
            var lex = new Lexer(source);
            var parser = new Parser(lex);

            var root = parser.ParseCompileUnit();

            var builder = new StringBuilder();
            root.AppendTree(builder);
            string treeForm = builder.ToString();

            var visitor = new PrinterVisiter();
            visitor.IndentChars = "    ";
            visitor.Visit(root);

            string result = visitor.Result;

            Debugger.Break();
        }


        private class PrinterVisiter : ParseTreeVisitorBase
        {
            private int _indent = 0;
            private StringBuilder _builder = new StringBuilder();

            public string Result
            {
                get { return _builder.ToString(); }
            }

            public string IndentChars { get; set; } = "    ";


            public override void VisitCompileUnit(ParseTreeRuleNode node)
            {
                _indent = 0;
                _builder.Clear();

                VisitChildren(node);
            }

            public override void VisitRootKey(ParseTreeRuleNode node)
            {
                var tokNode = (ParseTreeTokenNode)node.Children[0];
                var tok = tokNode.Token;

                _builder.Append(tok.Value);
            }

            public override void VisitBlock(ParseTreeRuleNode node)
            {
                _builder.AppendLine();
                AppendIndent();

                _builder.AppendLine("{");
                _indent++;
                AppendIndent();

                VisitChildren(node);

                _indent--;

                // hack: if we're already indented, just unindent.
                // I'm, sure there's a better way to do this, buy w/e
                while (_builder[_builder.Length-1] != '\r' &&
                       _builder[_builder.Length-1] != '\n')
                {
                    _builder.Length--;
                }
                AppendIndent();

                _builder.Append("}");
            }

            public override void VisitRegistryExpression(ParseTreeRuleNode node)
            {
                VisitChildren(node);
                _builder.AppendLine();
                AppendIndent();
            }

            public override void VisitAddKey(ParseTreeRuleNode node)
            {
                int index = 0;

                var tokNode = node.Children[index] as ParseTreeTokenNode;
                if (tokNode != null)
                {
                    index++;
                    _builder.Append(tokNode.Token.Value);
                    _builder.Append(' ');
                }

                Visit(node.Children[index++]);

                if (index < node.ChildCount &&
                    node.Children[index] is ParseTreeTokenNode)
                {
                    tokNode = (ParseTreeTokenNode)node.Children[index++];
                    _builder.Append(' ');
                    _builder.Append(tokNode.Token.Value);
                    _builder.Append(' ');

                    Visit(node.Children[index++]);
                    _builder.Append(' ');
                }

                if (index < node.ChildCount)
                {
                    Visit(node.Children[index]);
                }
            }

            public override void VisitDeleteKey(ParseTreeRuleNode node)
            {
                var tokNode = (ParseTreeTokenNode)node.Children[0];
                _builder.Append(tokNode.Token.Value);

                Visit(tokNode.Children[1]);
            }

            public override void VisitKeyName(ParseTreeRuleNode node)
            {
                var tokNode = (ParseTreeTokenNode)node.Children[0];
                _builder.Append(tokNode.Token.Value);
            }

            public override void VisitKeyValue(ParseTreeRuleNode node)
            {
                int index = 0;

                var typeNode = node.Children[index] as ParseTreeRuleNode;
                if (typeNode != null)
                {
                    index++;
                    Visit(typeNode);
                    _builder.Append(' ');
                }

                var tokNode = (ParseTreeTokenNode)node.Children[index];
                _builder.Append(tokNode.Token.Value);
            }

            public override void VisitKeyType(ParseTreeRuleNode node)
            {
                var tokNode = (ParseTreeTokenNode)node.Children[0];
                _builder.Append(tokNode.Token.Value);
            }


            private void AppendIndent()
            {
                for (int i = 0; i < _indent; i++)
                {
                    _builder.Append(IndentChars);
                }
            }
        }
    }
}
