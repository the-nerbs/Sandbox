using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    enum Rule
    {
        CompileUnit,
        HiveExpression,
        Block,

        RootKey,
        RegistryExpression,
        AddKey,
        DeleteKey,
        KeyName,
        KeyValue,
        KeyType,
    }

    class Parser
    {
        private readonly Lexer _lex;


        public Parser(Lexer lexer)
        {
            _lex = lexer;
        }


        public ParseTreeRuleNode Parse(Rule rule)
        {
            switch (rule)
            {
                case Rule.CompileUnit:
                    return ParseCompileUnit();

                case Rule.HiveExpression:
                    return ParseHiveExpression();

                case Rule.Block:
                    return ParseBlock();

                case Rule.RootKey:
                    return ParseRootKey();

                case Rule.RegistryExpression:
                    return ParseRegistryExpression();

                case Rule.AddKey:
                    return ParseAddKey();

                case Rule.DeleteKey:
                    return ParseDeleteKey();

                case Rule.KeyName:
                    return ParseKeyName();

                case Rule.KeyValue:
                    return ParseKeyValue();

                case Rule.KeyType:
                    return ParseKeyType();

                default:
                    throw new ArgumentException($"{rule} is not a recognized rule.");
            }
        }


        public ParseTreeRuleNode ParseCompileUnit()
        {
            var children = new List<IParseTreeNode>();

            var node = ParseHiveExpression();
            while (node != null)
            {
                children.Add(node);
                node = ParseHiveExpression();
            }

            return new ParseTreeRuleNode(Rule.CompileUnit, children);
        }

        public ParseTreeRuleNode ParseHiveExpression()
        {
            var children = new List<IParseTreeNode>();

            var node = ParseRootKey();
            if (node != null)
            {
                children.Add(node);

                node = ParseBlock();
                if (node != null)
                {
                    children.Add(node);

                    return new ParseTreeRuleNode(Rule.HiveExpression, children);
                }
            }

            return null;
        }

        public ParseTreeRuleNode ParseRootKey()
        {
            Token tok = _lex.AcceptAny(
                TokenType.HKeyClassesRoot, 
                TokenType.HKeyCurrentConfig,
                TokenType.HKeyCurrentUser,
                TokenType.HKeyDynData,
                TokenType.HKeyLocalMachine,
                TokenType.HKeyPerformanceData,
                TokenType.HKeyUsers
            );

            if (tok != null)
            {
                return new ParseTreeRuleNode(Rule.RootKey,
                    new ParseTreeTokenNode(tok));
            }

            return null;
        }

        public ParseTreeRuleNode ParseBlock()
        {
            var children = new List<IParseTreeNode>();

            var tok = _lex.Accept(TokenType.LBrace);
            if (tok != null)
            {
                children.Add(new ParseTreeTokenNode(tok));

                var node = ParseRegistryExpression();
                while (node != null)
                {
                    children.Add(node);

                    tok = _lex.Accept(TokenType.RBrace);
                    if (tok != null)
                    {
                        children.Add(new ParseTreeTokenNode(tok));
                        return new ParseTreeRuleNode(Rule.Block, children);
                    }
                    else
                    {
                        node = ParseRegistryExpression();
                    }
                }

            }

            return null;
        }

        public ParseTreeRuleNode ParseRegistryExpression()
        {
            IParseTreeNode node = ParseAddKey();

            if (node != null)
            {
                return new ParseTreeRuleNode(Rule.RegistryExpression, node);
            }

            node = ParseDeleteKey();
            if (node != null)
            {
                return new ParseTreeRuleNode(Rule.RegistryExpression, node);
            }

            return null;
        }

        public ParseTreeRuleNode ParseAddKey()
        {
            var children = new List<IParseTreeNode>();

            var tok = _lex.AcceptAny(TokenType.ForceRemove, TokenType.NoRemove, TokenType.Val);
            if (tok != null)
            {
                children.Add(new ParseTreeTokenNode(tok));
            }

            var node = ParseKeyName();

            if (node == null)
                goto Failure;

            children.Add(node);

            tok = _lex.Accept(TokenType.Equals);
            if (tok != null)
            {
                children.Add(new ParseTreeTokenNode(tok));

                node = ParseKeyValue();
                if (node == null)
                    goto Failure;

                children.Add(node);
            }

            node = ParseBlock();
            if (node != null)
            {
                children.Add(node);
            }

            // Success
            return new ParseTreeRuleNode(Rule.AddKey, children);

        Failure:
            return null;
        }

        public ParseTreeRuleNode ParseDeleteKey()
        {
            var children = new List<IParseTreeNode>();

            var tok = _lex.Accept(TokenType.Delete);
            if (tok != null)
            {
                children.Add(new ParseTreeTokenNode(tok));

                var node = ParseKeyName();
                if (node != null)
                {
                    children.Add(node);

                    return new ParseTreeRuleNode(Rule.DeleteKey, children);
                }
            }

            return null;
        }

        public ParseTreeRuleNode ParseKeyName()
        {
            var tok = _lex.Accept(TokenType.Name);

            if (tok != null)
            {
                return new ParseTreeRuleNode(Rule.KeyName, new ParseTreeTokenNode(tok));
            }

            return null;
        }

        public ParseTreeRuleNode ParseKeyValue()
        {
            var children = new List<IParseTreeNode>();

            var node = ParseKeyType();
            if (node != null)
            {
                children.Add(node);
            }

            var tok = _lex.Accept(TokenType.Value);
            if (tok != null)
            {
                children.Add(new ParseTreeTokenNode(tok));

                return new ParseTreeRuleNode(Rule.KeyValue, children);
            }

            return null;
        }

        public ParseTreeRuleNode ParseKeyType()
        {
            var tok = _lex.AcceptAny(
                TokenType.Type_String,
                TokenType.Type_Dword,
                TokenType.Type_MultiString,
                TokenType.Type_Binary
            );

            if (tok != null)
            {
                return new ParseTreeRuleNode(Rule.KeyType,
                    new ParseTreeTokenNode(tok));
            }

            return null;
        }
    }
}
