using System;

namespace ARLES
{
	public enum TokenType : int {VAR = 1, OR, COMPL, LP, RP, EOI, INV = -1};

	class Token 
	{
		public TokenType type;
		public string text;

		public Token(TokenType type = TokenType.INV, String text = null)
		{
			this.type = type;
			this.text = text;
		}
	}

	class Lexer
	{
		private readonly string lexInput = null;
		private int posInString = 0;

		public Lexer(String input)
		{
			lexInput = input;
		}

		private char GetChar()
		{
			return lexInput [posInString];
		}

		public Token GetToken()
		{
			char c;
			state0:
			if (posInString >= lexInput.Length)
				return new Token (TokenType.EOI);

			c = GetChar ();
			posInString++;

			switch (c) {
			case '\'':
				return new Token (TokenType.COMPL, "\'");
			case '+':
				return new Token (TokenType.OR, "+");
			case '(':
				return new Token (TokenType.LP, "(");
			case ')':
				return new Token (TokenType.RP, ")");
			case '\t':
			case ' ':
				goto state0;
			case '\n':
			case '\0':
				return new Token (TokenType.EOI);
			}

			if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')) {
				return new Token (TokenType.VAR, ("" + c).ToUpper ());
			}

			return new Token (TokenType.INV);
		}
	}
}