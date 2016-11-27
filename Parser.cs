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
			Queue program = new Queue ();
			Queue log = new Queue ();
			Input (program, log);
			program.Enqueue (OpCode.HALT);
			log.Enqueue ("HALT");

			Log.Debug ("# Compiler Output:");

			foreach (string s in log) {
				Log.Debug ("\t" + s);
			}

			byte[] code = new byte[program.Count];

			int i = 0;
			foreach (byte b in program) {
				code [i++] = b;
			}

			return code;
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
		private void Input(Queue program, Queue log)
		{
			Expression (program, log);
		}

		private void Expression(Queue program, Queue log)
		{
			Term (program, log);

			if (curToken.type == TokenType.OR) {
				Eat ();
				Expression (program, log);
				program.Enqueue (OpCode.OR);
				log.Enqueue ("OR");
			}
		}

		private void Term(Queue program, Queue log)
		{
			Factor (program, log);
			if (curToken.type == TokenType.VAR || curToken.type == TokenType.LP) {
				Term (program, log);
				program.Enqueue (OpCode.AND);
				log.Enqueue ("AND");
			}
		}

		private void Factor(Queue program, Queue log)
		{
			if (curToken.type == TokenType.VAR) {
				program.Enqueue (OpCode.PUSH);
				program.Enqueue (IDX (Encoding.ASCII.GetBytes (curToken.text) [0]));
				log.Enqueue (string.Format ("PUSH {0}", curToken.text));

				Eat ();
			} else if (curToken.type == TokenType.LP) {
				Eat ();
				Expression (program, log);
				if (curToken.type == TokenType.RP) {
					Eat ();
				} else {
					/* TODO Error Handling */
				}
			} else {
				/* TODO Error Handling */
			}

			if (curToken.type == TokenType.COMPL) {
				program.Enqueue (OpCode.NOT);
				log.Enqueue ("NOT");
				Eat ();
			}
		}
	}
}

