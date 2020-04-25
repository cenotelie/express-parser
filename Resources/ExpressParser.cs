/*
 * WARNING: this file has been generated by
 * Hime Parser Generator 3.4.0.0
 */
using System.Collections.Generic;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace Express
{
	/// <summary>
	/// Represents a parser
	/// </summary>
	internal class ExpressParser : LRkParser
	{
		/// <summary>
		/// The automaton for this parser
		/// </summary>
		private static readonly LRkAutomaton commonAutomaton = LRkAutomaton.Find(typeof(ExpressParser), "ExpressParser.bin");
		/// <summary>
		/// Contains the constant IDs for the variables and virtuals in this parser
		/// </summary>
		public class ID
		{
			/// <summary>
			/// The unique identifier for variable att_decl
			/// </summary>
			public const int VariableAttDecl = 0x0008;
			/// <summary>
			/// The unique identifier for variable entity_decl
			/// </summary>
			public const int VariableEntityDecl = 0x0009;
			/// <summary>
			/// The unique identifier for variable type_decl
			/// </summary>
			public const int VariableTypeDecl = 0x000A;
			/// <summary>
			/// The unique identifier for variable use_decl
			/// </summary>
			public const int VariableUseDecl = 0x000B;
			/// <summary>
			/// The unique identifier for variable schema_decl
			/// </summary>
			public const int VariableSchemaDecl = 0x000C;
			/// <summary>
			/// The unique identifier for variable schema_body
			/// </summary>
			public const int VariableSchemaBody = 0x000D;
			/// <summary>
			/// The unique identifier for variable comment_exp
			/// </summary>
			public const int VariableCommentExp = 0x000E;
			/// <summary>
			/// The unique identifier for variable root
			/// </summary>
			public const int VariableRoot = 0x000F;
		}
		/// <summary>
		/// The collection of variables matched by this parser
		/// </summary>
		/// <remarks>
		/// The variables are in an order consistent with the automaton,
		/// so that variable indices in the automaton can be used to retrieve the variables in this table
		/// </remarks>
		private static readonly Symbol[] variables = {
			new Symbol(0x0008, "att_decl"), 
			new Symbol(0x0009, "entity_decl"), 
			new Symbol(0x000A, "type_decl"), 
			new Symbol(0x000B, "use_decl"), 
			new Symbol(0x000C, "schema_decl"), 
			new Symbol(0x000D, "schema_body"), 
			new Symbol(0x000E, "comment_exp"), 
			new Symbol(0x000F, "root"), 
			new Symbol(0x0016, "__V22"), 
			new Symbol(0x001E, "__V30"), 
			new Symbol(0x0029, "__V41"), 
			new Symbol(0x002A, "__V42"), 
			new Symbol(0x002B, "__VAxiom") };
		/// <summary>
		/// The collection of virtuals matched by this parser
		/// </summary>
		/// <remarks>
		/// The virtuals are in an order consistent with the automaton,
		/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table
		/// </remarks>
		private static readonly Symbol[] virtuals = {
 };
		/// <summary>
		/// Represents a set of semantic actions in this parser
		/// </summary>
		public class Actions
		{
			/// <summary>
			/// The OnAttribute semantic action
			/// </summary>
			public virtual void OnAttribute(Symbol head, SemanticBody body) {}
			/// <summary>
			/// The OnEntity semantic action
			/// </summary>
			public virtual void OnEntity(Symbol head, SemanticBody body) {}
			/// <summary>
			/// The OnSelectType semantic action
			/// </summary>
			public virtual void OnSelectType(Symbol head, SemanticBody body) {}
			/// <summary>
			/// The OnPrimitiveType semantic action
			/// </summary>
			public virtual void OnPrimitiveType(Symbol head, SemanticBody body) {}
			/// <summary>
			/// The OnUse semantic action
			/// </summary>
			public virtual void OnUse(Symbol head, SemanticBody body) {}
			/// <summary>
			/// The OnSchema semantic action
			/// </summary>
			public virtual void OnSchema(Symbol head, SemanticBody body) {}

		}
		/// <summary>
		/// Represents a set of empty semantic actions (do nothing)
		/// </summary>
		private static readonly Actions noActions = new Actions();
		/// <summary>
		/// Gets the set of semantic actions in the form a table consistent with the automaton
		/// </summary>
		/// <param name="input">A set of semantic actions</param>
		/// <returns>A table of semantic actions</returns>
		private static SemanticAction[] GetUserActions(Actions input)
		{
			SemanticAction[] result = new SemanticAction[6];
			result[0] = new SemanticAction(input.OnAttribute);
			result[1] = new SemanticAction(input.OnEntity);
			result[2] = new SemanticAction(input.OnSelectType);
			result[3] = new SemanticAction(input.OnPrimitiveType);
			result[4] = new SemanticAction(input.OnUse);
			result[5] = new SemanticAction(input.OnSchema);
			return result;
		}
		/// <summary>
		/// Gets the set of semantic actions in the form a table consistent with the automaton
		/// </summary>
		/// <param name="input">A set of semantic actions</param>
		/// <returns>A table of semantic actions</returns>
		private static SemanticAction[] GetUserActions(Dictionary<string, SemanticAction> input)
		{
			SemanticAction[] result = new SemanticAction[6];
			result[0] = input["OnAttribute"];
			result[1] = input["OnEntity"];
			result[2] = input["OnSelectType"];
			result[3] = input["OnPrimitiveType"];
			result[4] = input["OnUse"];
			result[5] = input["OnSchema"];
			return result;
		}
		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		public ExpressParser(ExpressLexer lexer) : base (commonAutomaton, variables, virtuals, GetUserActions(noActions), lexer) { }
		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		/// <param name="actions">The set of semantic actions</param>
		public ExpressParser(ExpressLexer lexer, Actions actions) : base (commonAutomaton, variables, virtuals, GetUserActions(actions), lexer) { }
		/// <summary>
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		/// <param name="actions">The set of semantic actions</param>
		public ExpressParser(ExpressLexer lexer, Dictionary<string, SemanticAction> actions) : base (commonAutomaton, variables, virtuals, GetUserActions(actions), lexer) { }

		/// <summary>
		/// Visitor interface
		/// </summary>
		public class Visitor
		{
			public virtual void OnTerminalNewLine(ASTNode node) {}
			public virtual void OnTerminalWhiteSpace(ASTNode node) {}
			public virtual void OnTerminalSeparator(ASTNode node) {}
			public virtual void OnTerminalIdentifier(ASTNode node) {}
			public virtual void OnTerminalAnnotation(ASTNode node) {}
			public virtual void OnVariableAttDecl(ASTNode node) {}
			public virtual void OnVariableEntityDecl(ASTNode node) {}
			public virtual void OnVariableTypeDecl(ASTNode node) {}
			public virtual void OnVariableUseDecl(ASTNode node) {}
			public virtual void OnVariableSchemaDecl(ASTNode node) {}
			public virtual void OnVariableSchemaBody(ASTNode node) {}
			public virtual void OnVariableCommentExp(ASTNode node) {}
			public virtual void OnVariableRoot(ASTNode node) {}
		}

		/// <summary>
		/// Walk the AST using a visitor
		/// </summary>
		public static void Visit(ParseResult result, Visitor visitor)
		{
			VisitASTNode(result.Root, visitor);
		}

		/// <summary>
		/// Walk the AST using a visitor
		/// </summary>
		public static void VisitASTNode(ASTNode node, Visitor visitor)
		{
			for (int i = 0; i < node.Children.Count; i++)
				VisitASTNode(node.Children[i], visitor);
			switch(node.Symbol.ID)
			{
				case 0x0003: visitor.OnTerminalNewLine(node); break;
				case 0x0004: visitor.OnTerminalWhiteSpace(node); break;
				case 0x0005: visitor.OnTerminalSeparator(node); break;
				case 0x0006: visitor.OnTerminalIdentifier(node); break;
				case 0x0007: visitor.OnTerminalAnnotation(node); break;
				case 0x0008: visitor.OnVariableAttDecl(node); break;
				case 0x0009: visitor.OnVariableEntityDecl(node); break;
				case 0x000A: visitor.OnVariableTypeDecl(node); break;
				case 0x000B: visitor.OnVariableUseDecl(node); break;
				case 0x000C: visitor.OnVariableSchemaDecl(node); break;
				case 0x000D: visitor.OnVariableSchemaBody(node); break;
				case 0x000E: visitor.OnVariableCommentExp(node); break;
				case 0x000F: visitor.OnVariableRoot(node); break;
			}
		}
	}
}
