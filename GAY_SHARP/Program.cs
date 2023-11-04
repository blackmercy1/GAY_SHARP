namespace GAY_SHARP.CodeAnalysis.Syntax;

public class Program
{
    static void Main(string[] args)
    {
        var showTree = false;
        while (true)
        {
            var line = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(line))
                return;

            if (line == "#showTree")
            {
                showTree = !showTree;
                Console.WriteLine(showTree ? "showing parse trees ." : "Not showing parse trees");
                continue;
            }

            if (line == "#cls")
            {
                Console.Clear();
                continue;
            }

            var syntaxTree = SyntaxTree.Parse(line);

            if (showTree)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;
                PrettyPrint(syntaxTree.Root);
                Console.ForegroundColor = color;
            }

            var evaluator = new Evaluator(syntaxTree.Root);
            var result = evaluator.Evaluate();
            Console.WriteLine(result);
        }
    }

    static void PrettyPrint(SyntaxNode node, string indent = "")
    {
        Console.Write(indent);
        Console.Write(node.Kind);

        var nodeType = node as SyntaxToken;

        if (nodeType != null && nodeType.Value != null)
        {
            Console.Write("");
            Console.Write(nodeType.Value);
        }

        Console.WriteLine();
        indent += "   ";
        foreach (var child in node.GetChildren())
        {
            PrettyPrint(child, indent);
        }
    }
}

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

public sealed class ParenthesizedExpressionSyntax : ExpressionSyntax
{
    public ExpressionSyntax Expression { get; private set; }
    public SyntaxToken CloseParenthesizedToken { get; private set; }
    public SyntaxNode OpenParenthesizedToken { get; private set; }
    public override SyntaxKind Kind => SyntaxKind.ParenthesizedExpression;

    public ParenthesizedExpressionSyntax(SyntaxNode openParenthesizedToken, ExpressionSyntax expression,
        SyntaxToken closeParenthesizedToken)
    {
        Expression = expression;
        CloseParenthesizedToken = closeParenthesizedToken;
        OpenParenthesizedToken = openParenthesizedToken;
    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        yield return OpenParenthesizedToken;
        yield return Expression;
        yield return CloseParenthesizedToken;
    }
}