using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    class ParseTreeVisitorBase : IParseTreeVisitor
    {
        public virtual void Visit(IParseTreeNode node)
        {
            var ruleNode = node as ParseTreeRuleNode;
            if (ruleNode != null)
            {
                switch (ruleNode.Rule)
                {
                    case Rule.CompileUnit:
                        VisitCompileUnit(ruleNode);
                        break;

                    case Rule.HiveExpression:
                        VisitHiveExpression(ruleNode);
                        break;

                    case Rule.Block:
                        VisitBlock(ruleNode);
                        break;

                    case Rule.RootKey:
                        VisitRootKey(ruleNode);
                        break;

                    case Rule.RegistryExpression:
                        VisitRegistryExpression(ruleNode);
                        break;

                    case Rule.AddKey:
                        VisitAddKey(ruleNode);
                        break;

                    case Rule.DeleteKey:
                        VisitDeleteKey(ruleNode);
                        break;

                    case Rule.KeyName:
                        VisitKeyName(ruleNode);
                        break;

                    case Rule.KeyValue:
                        VisitKeyValue(ruleNode);
                        break;

                    case Rule.KeyType:
                        VisitKeyType(ruleNode);
                        break;

                    default:
                        break;
                }
            }
        }


        public virtual void VisitCompileUnit(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitHiveExpression(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitBlock(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitRootKey(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitRegistryExpression(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitAddKey(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitDeleteKey(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitKeyName(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitKeyValue(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }

        public virtual void VisitKeyType(ParseTreeRuleNode node)
        {
            VisitChildren(node);
        }


        protected void VisitChildren(ParseTreeRuleNode node)
        {
            foreach (var item in node.Children)
            {
                Visit(item);
            }
        }
    }

    class ParseTreeVisitorBase<TResult> : IParseTreeVisitor<TResult>
    {
        public TResult DefaultResult { get; set; }


        public virtual TResult Visit(IParseTreeNode node)
        {
            var ruleNode = node as ParseTreeRuleNode;
            if (ruleNode != null)
            {
                switch (ruleNode.Rule)
                {
                    case Rule.CompileUnit:
                        return VisitCompileUnit(ruleNode);

                    case Rule.HiveExpression:
                        return VisitHiveExpression(ruleNode);

                    case Rule.Block:
                        return VisitBlock(ruleNode);

                    case Rule.RootKey:
                        return VisitRootKey(ruleNode);

                    case Rule.RegistryExpression:
                        return VisitRegistryExpression(ruleNode);

                    case Rule.AddKey:
                        return VisitAddKey(ruleNode);

                    case Rule.DeleteKey:
                        return VisitDeleteKey(ruleNode);

                    case Rule.KeyName:
                        return VisitKeyName(ruleNode);

                    case Rule.KeyValue:
                        return VisitKeyValue(ruleNode);

                    case Rule.KeyType:
                        return VisitKeyType(ruleNode);

                    default:
                        break;
                }
            }

            return DefaultResult;
        }


        public virtual TResult VisitCompileUnit(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitHiveExpression(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitBlock(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitRootKey(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitRegistryExpression(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitAddKey(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitDeleteKey(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitKeyName(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitKeyValue(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }

        public virtual TResult VisitKeyType(ParseTreeRuleNode node)
        {
            return VisitChildren(node);
        }


        protected TResult VisitChildren(ParseTreeRuleNode node)
        {
            TResult result = DefaultResult;

            foreach (var item in node.Children)
            {
                TResult second = Visit(item);
                result = AggregateResults(result, second);
            }

            return result;
        }

        protected virtual TResult AggregateResults(TResult first, TResult second)
        {
            return second;
        }
    }
}
