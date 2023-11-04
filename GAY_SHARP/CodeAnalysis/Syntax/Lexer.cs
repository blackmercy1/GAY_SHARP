namespace GAY_SHARP.CodeAnalysis.Syntax;

internal class Lexer
{
    private readonly string _text;

    private List<string> _diagnostics = new List<string>();
    private int _position;

    public Lexer(string text)
    {
        _text = text;
    }

    public IEnumerable<string> Diagnostics => _diagnostics;

    private char Current
    {
        get
        {
            if (_position >= _text.Length)
                return '\0';
            return _text[_position];
        }
    }

    private void Next()
    {
        _position++;
    }

    public virtual SyntaxToken NextToken()
    {
        //<numbers>
        // + - * /
        // <whitespace>

        if (_position >= _text.Length)
            return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);

        if (char.IsDigit(Current))
        {
            var start = _position;
            while (char.IsDigit(Current))
            {
                Next();
            }

            var lenght = _position - start;
            var text = _text.Substring(start, lenght);
            int.TryParse(text, out var value);
            return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
        }

        if (char.IsWhiteSpace(Current))
        {
            var start = _position;
            while (char.IsWhiteSpace(Current))
            {
                Next();
            }

            var lenght = _position - start;
            var text = _text.Substring(start, lenght);
            int.TryParse(text, out var value);
            return new SyntaxToken(SyntaxKind.WhiteSpaceToken, start, text, value);
        }

        return Current == '+' ? new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null) :
            Current == '-' ? new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null) :
            Current == '*' ? new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null) :
            Current == '/' ? new SyntaxToken(SyntaxKind.SlashToken, _position++, "/", null) :
            Current == '(' ? new SyntaxToken(SyntaxKind.OpenParenthesisToken, _position++, "(", null) :
            Current == ')' ? new SyntaxToken(SyntaxKind.CloseParenthesisToken, _position++, ")", null) :
            new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);

    }
}