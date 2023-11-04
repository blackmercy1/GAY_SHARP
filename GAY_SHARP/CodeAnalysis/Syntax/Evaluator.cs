using GAY_SHARP.CodeAnalysis.Syntax;

namespace GAY_SHARP;

public sealed class Evaluator
{
    private readonly ExpressionSyntax _root;

    public Evaluator(ExpressionSyntax root)
    {
        _root = root;
    }

    public int Evaluate()
    {
        return EvaluateExpression(_root);
    }

    private int EvaluateExpression(ExpressionSyntax node)
    {
        var unaryExpression = node as UnaryExpressionSyntax;
        if (unaryExpression != null)
        {
            var operand = EvaluateExpression(unaryExpression.Operand);

            if (unaryExpression.OperatorToken.Kind == SyntaxKind.MinusToken)
                return -operand;
            if (unaryExpression.OperatorToken.Kind == SyntaxKind.PlusToken)
                return operand;
            else
                throw new Exception($"Unecpected unray operator {unaryExpression.OperatorToken.Kind}");
        }
        
        var rootLink = node as LiteralExpressionSyntax;
        if (rootLink != null)
            return (int) rootLink.LiteralToken.Value!;

        var binaryExpression = node as BinaryExpressionSyntax;
        if (binaryExpression != null)
        {
            var left = EvaluateExpression(binaryExpression.Left);
            var right = EvaluateExpression(binaryExpression.Right);

            return binaryExpression.OperatorToken.Kind switch
            {
                SyntaxKind.PlusToken => left + right,
                SyntaxKind.MinusToken => left - right,
                SyntaxKind.StarToken => left * right,
                SyntaxKind.SlashToken => left / right,
                _ => throw new Exception($"Unexpected, kind of token {binaryExpression.Kind}")
            };
        }

        if (node is ParenthesizedExpressionSyntax parenthesizedExpressionSyntax)
            return EvaluateExpression(parenthesizedExpressionSyntax.Expression);

        throw new Exception($"Unexpected, node {node.Kind}");
    }
}