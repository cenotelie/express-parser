/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.4.0.0
 */
using System.Collections.Generic;
using System.IO;
using Hime.Redist;
using Hime.Redist.Lexer;

namespace Express
{
	/// <summary>
	/// Represents a lexer
	/// </summary>
	internal class ExpressLexer : ContextFreeLexer
	{
		/// <summary>
		/// The automaton for this lexer
		/// </summary>
		private static readonly Automaton commonAutomaton = Automaton.Find(typeof(ExpressLexer), "ExpressLexer.bin");
		/// <summary>
		/// Contains the constant IDs for the terminals for this lexer
		/// </summary>
		public class ID
		{
			/// <summary>
			/// The unique identifier for terminal NEW_LINE
			/// </summary>
			public const int TerminalNewLine = 0x0003;
			/// <summary>
			/// The unique identifier for terminal WHITE_SPACE
			/// </summary>
			public const int TerminalWhiteSpace = 0x0004;
			/// <summary>
			/// The unique identifier for terminal SEPARATOR
			/// </summary>
			public const int TerminalSeparator = 0x0005;
			/// <summary>
			/// The unique identifier for terminal IDENTIFIER
			/// </summary>
			public const int TerminalIdentifier = 0x0006;
			/// <summary>
			/// The unique identifier for terminal ANNOTATION
			/// </summary>
			public const int TerminalAnnotation = 0x0007;
			/// <summary>
			/// The unique identifier for terminal INTEGER_LITERAL_DECIMAL
			/// </summary>
			public const int TerminalIntegerLiteralDecimal = 0x0008;
			/// <summary>
			/// The unique identifier for terminal INTEGER_LITERAL_HEXA
			/// </summary>
			public const int TerminalIntegerLiteralHexa = 0x0009;
			/// <summary>
			/// The unique identifier for terminal REAL_LITERAL
			/// </summary>
			public const int TerminalRealLiteral = 0x000A;
			/// <summary>
			/// The unique identifier for terminal STRING_LITERAL
			/// </summary>
			public const int TerminalStringLiteral = 0x000B;
		}
		/// <summary>
		/// Contains the constant IDs for the contexts for this lexer
		/// </summary>
		public class Context
		{
			/// <summary>
			/// The unique identifier for the default context
			/// </summary>
			public const int Default = 0;
		}
		/// <summary>
		/// The collection of terminals matched by this lexer
		/// </summary>
		/// <remarks>
		/// The terminals are in an order consistent with the automaton,
		/// so that terminal indices in the automaton can be used to retrieve the terminals in this table
		/// </remarks>
		private static readonly Symbol[] terminals = {
			new Symbol(0x0001, "ε"),
			new Symbol(0x0002, "$"),
			new Symbol(0x0003, "NEW_LINE"),
			new Symbol(0x0004, "WHITE_SPACE"),
			new Symbol(0x0005, "SEPARATOR"),
			new Symbol(0x0006, "IDENTIFIER"),
			new Symbol(0x0007, "ANNOTATION"),
			new Symbol(0x0008, "INTEGER_LITERAL_DECIMAL"),
			new Symbol(0x0009, "INTEGER_LITERAL_HEXA"),
			new Symbol(0x000A, "REAL_LITERAL"),
			new Symbol(0x000B, "STRING_LITERAL"),
			new Symbol(0x0030, "USE"),
			new Symbol(0x0031, "FROM"),
			new Symbol(0x0032, ";"),
			new Symbol(0x0033, "SCHEMA"),
			new Symbol(0x0034, "TYPE"),
			new Symbol(0x0035, "="),
			new Symbol(0x0036, "END_TYPE"),
			new Symbol(0x0037, "SELECT"),
			new Symbol(0x0038, "("),
			new Symbol(0x0039, ","),
			new Symbol(0x003B, ")"),
			new Symbol(0x003C, "ENTITY"),
			new Symbol(0x003E, "END_ENTITY"),
			new Symbol(0x003F, "ABSTRACT"),
			new Symbol(0x0040, "SUBTYPE"),
			new Symbol(0x0041, "OF"),
			new Symbol(0x0043, "SUPERTYPE"),
			new Symbol(0x0044, ":"),
			new Symbol(0x0045, "OPTIONAL"),
			new Symbol(0x0046, "WHERE"),
			new Symbol(0x0048, "OR"),
			new Symbol(0x0049, "AND"),
			new Symbol(0x004A, "NOT"),
			new Symbol(0x004B, "EXISTS"),
			new Symbol(0x004C, "{"),
			new Symbol(0x004D, "SELF"),
			new Symbol(0x004E, "}"),
			new Symbol(0x004F, "TYPEOF"),
			new Symbol(0x0050, "SELF\\"),
			new Symbol(0x0051, "."),
			new Symbol(0x0053, "["),
			new Symbol(0x0054, "]"),
			new Symbol(0x0055, ":<>:"),
			new Symbol(0x0056, "IN"),
			new Symbol(0x0057, "<"),
			new Symbol(0x0058, "<="),
			new Symbol(0x0059, ">="),
			new Symbol(0x005A, ">"),
			new Symbol(0x005B, "ONEOF"),
			new Symbol(0x005D, "STRING"),
			new Symbol(0x005E, "NUMBER"),
			new Symbol(0x005F, "BOOLEAN"),
			new Symbol(0x0060, "LOGICAL"),
			new Symbol(0x0061, "BINARY"),
			new Symbol(0x0062, "INTEGER"),
			new Symbol(0x0063, "REAL"),
			new Symbol(0x0064, "true"),
			new Symbol(0x0065, "false") };
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public ExpressLexer(string input) : base(commonAutomaton, terminals, 0x0005, input) {}
		/// <summary>
		/// Initializes a new instance of the lexer
		/// </summary>
		/// <param name="input">The lexer's input</param>
		public ExpressLexer(TextReader input) : base(commonAutomaton, terminals, 0x0005, input) {}
	}
}
