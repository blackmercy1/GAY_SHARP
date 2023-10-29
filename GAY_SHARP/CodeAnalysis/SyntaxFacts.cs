namespace GAY_SHARP.CodeAnalysis;

internal static class SyntaxFacts
{
    public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
    {
        return kind switch
        {
            SyntaxKind.PlusToken => 1,
            SyntaxKind.MinusToken => 1,
            SyntaxKind.StarToken => 2,
            SyntaxKind.SlashToken => 2,
            _ => 0
        };
    }
}