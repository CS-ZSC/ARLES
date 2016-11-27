using System;
using System.Collections;
using System.Text;

namespace ARLES
{
	enum OpCode : byte
	{
		PUSH = 1,
		AND = 2,
		OR = 3,
		NOT = 4,
		HALT = 255
	};

	class Parser
	{
		private Lexer lexer = null;
		private Token curToken = null;
		private Token nextToken = null;

		public Parser(string input)
		{
			lexer = new Lexer(input);
			curToken = lexer.GetToken();
			nextToken = lexer.GetToken();
		}

		public byte[] Compile()
		{
			Stack program = new Stack ();
			Input (program);
			program.Push (OpCode.HALT);

			byte[] prog = new byte[program.Count];

			for (int i = program.Count - 1; i >= 0; --i)
				prog [i] = (byte) program.Pop ();

			return prog;
		}

		private byte IDX(byte c)
		{
			return (byte)(c - (byte)65);
		}

		private Token Eat()
		{
			Token token = curToken;
			curToken = nextToken;
			nextToken = lexer.GetToken ();

			return token;
		}

		/* Grammar Procedures */
		private void Input(Stack program)
		{
			Expression (program);
		}

		private void Expression(Stack program)
		{
			Term (program);

			if (curToken.type == TokenType.OR) {
				Eat ();
				Expression (program);
				program.Push (OpCode.OR);
			}
		}

		private void Term(Stack program)
		{
			Factor (program);
			if (curToken.type == TokenType.VAR || curToken.type == TokenType.LP) {
				Term (program);
				program.Push (OpCode.AND);
			}
		}

		private void Factor(Stack program)
		{
			if (curToken.type == TokenType.VAR) {
				program.Push (OpCode.PUSH);
				program.Push (IDX (Encoding.ASCII.GetBytes (curToken.text) [0]));

				Eat ();
			} else if (curToken.type == TokenType.LP) {
				Eat ();
				Expression (program);
				if (curToken.type == TokenType.RP) {
					Eat ();
				} else {
					/* TODO Error Handling */
				}
			} else {
				/* TODO Error Handling */
			}

			if (curToken.type == TokenType.COMPL) {
				program.Push (OpCode.NOT);
				Eat ();
			}
		}
	}
}

