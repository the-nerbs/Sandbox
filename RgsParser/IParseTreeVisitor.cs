using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RgsParser
{
    interface IParseTreeVisitor
    {
        void Visit(IParseTreeNode node);

        void VisitCompileUnit(ParseTreeRuleNode node);
        void VisitHiveExpression(ParseTreeRuleNode node);
        void VisitBlock(ParseTreeRuleNode node);

        void VisitRootKey(ParseTreeRuleNode node);
        void VisitRegistryExpression(ParseTreeRuleNode node);
        void VisitAddKey(ParseTreeRuleNode node);
        void VisitDeleteKey(ParseTreeRuleNode node);
        void VisitKeyName(ParseTreeRuleNode node);
        void VisitKeyValue(ParseTreeRuleNode node);
        void VisitKeyType(ParseTreeRuleNode node);
    }

    interface IParseTreeVisitor<TResult>
    {
        TResult DefaultResult { get; }


        TResult Visit(IParseTreeNode node);

        TResult VisitCompileUnit(ParseTreeRuleNode node);
        TResult VisitHiveExpression(ParseTreeRuleNode node);
        TResult VisitBlock(ParseTreeRuleNode node);

        TResult VisitRootKey(ParseTreeRuleNode node);
        TResult VisitRegistryExpression(ParseTreeRuleNode node);
        TResult VisitAddKey(ParseTreeRuleNode node);
        TResult VisitDeleteKey(ParseTreeRuleNode node);
        TResult VisitKeyName(ParseTreeRuleNode node);
        TResult VisitKeyValue(ParseTreeRuleNode node);
        TResult VisitKeyType(ParseTreeRuleNode node);
    }
}
