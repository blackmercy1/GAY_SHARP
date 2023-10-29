namespace GAY_SHARP.CodeAnalysis;

public class SyntaxToken : SyntaxNode
{
    public override SyntaxKind Kind { get; }
    public readonly string Text;
    public readonly int Position;
    public readonly object? Value;

    public SyntaxToken(SyntaxKind kind, int position, string text, object? value)
    {
        Kind = kind;
        Position = position;
        Text = text;
        Value = value;
    }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
        return Enumerable.Empty<SyntaxNode>();
    }
}