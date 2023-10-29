namespace GAY_SHARP.CodeAnalysis;

public sealed class SyntaxTree
{
    public SyntaxTree(ExpressionSyntax expressionSyntax, SyntaxToken endOfFileToken)
    {
        EndOfFileToken = endOfFileToken;
        Root = expressionSyntax;
    }

    public ExpressionSyntax Root { get; }
    public SyntaxToken EndOfFileToken { get; }

    public static SyntaxTree Parse(string text)
    {
        var parser = new Parser(text);
        return parser.Parse();
    }
}