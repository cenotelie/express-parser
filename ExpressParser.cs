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
			/// The unique identifier for variable root
			/// </summary>
			public const int VariableRoot = 0x000D;
			/// <summary>
			/// The unique identifier for variable comment_exp
			/// </summary>
			public const int VariableCommentExp = 0x000E;
			/// <summary>
			/// The unique identifier for variable schema_body
			/// </summary>
			public const int VariableSchemaBody = 0x000F;
			/// <summary>
			/// The unique identifier for variable use_decl
			/// </summary>
			public const int VariableUseDecl = 0x0010;
			/// <summary>
			/// The unique identifier for variable schema_decl
			/// </summary>
			public const int VariableSchemaDecl = 0x0011;
			/// <summary>
			/// The unique identifier for variable type_decl
			/// </summary>
			public const int VariableTypeDecl = 0x0012;
			/// <summary>
			/// The unique identifier for variable select_decl
			/// </summary>
			public const int VariableSelectDecl = 0x0013;
			/// <summary>
			/// The unique identifier for variable entity_decl
			/// </summary>
			public const int VariableEntityDecl = 0x0014;
			/// <summary>
			/// The unique identifier for variable enum_decl
			/// </summary>
			public const int VariableEnumDecl = 0x0015;
			/// <summary>
			/// The unique identifier for variable abstract_decl
			/// </summary>
			public const int VariableAbstractDecl = 0x0016;
			/// <summary>
			/// The unique identifier for variable supertype_decl
			/// </summary>
			public const int VariableSupertypeDecl = 0x0017;
			/// <summary>
			/// The unique identifier for variable subtype_decl
			/// </summary>
			public const int VariableSubtypeDecl = 0x0018;
			/// <summary>
			/// The unique identifier for variable derive_decl
			/// </summary>
			public const int VariableDeriveDecl = 0x0019;
			/// <summary>
			/// The unique identifier for variable derive_exp
			/// </summary>
			public const int VariableDeriveExp = 0x001A;
			/// <summary>
			/// The unique identifier for variable attr_read_exp
			/// </summary>
			public const int VariableAttrReadExp = 0x001B;
			/// <summary>
			/// The unique identifier for variable prop_decl
			/// </summary>
			public const int VariablePropDecl = 0x001C;
			/// <summary>
			/// The unique identifier for variable prop_exp
			/// </summary>
			public const int VariablePropExp = 0x001D;
			/// <summary>
			/// The unique identifier for variable list_exp
			/// </summary>
			public const int VariableListExp = 0x001E;
			/// <summary>
			/// The unique identifier for variable set_exp
			/// </summary>
			public const int VariableSetExp = 0x001F;
			/// <summary>
			/// The unique identifier for variable array_exp
			/// </summary>
			public const int VariableArrayExp = 0x0020;
			/// <summary>
			/// The unique identifier for variable optional_decl
			/// </summary>
			public const int VariableOptionalDecl = 0x0021;
			/// <summary>
			/// The unique identifier for variable where_decl
			/// </summary>
			public const int VariableWhereDecl = 0x0022;
			/// <summary>
			/// The unique identifier for variable or_exp
			/// </summary>
			public const int VariableOrExp = 0x0023;
			/// <summary>
			/// The unique identifier for variable and_exp
			/// </summary>
			public const int VariableAndExp = 0x0024;
			/// <summary>
			/// The unique identifier for variable unary_exp
			/// </summary>
			public const int VariableUnaryExp = 0x0025;
			/// <summary>
			/// The unique identifier for variable atom_exp
			/// </summary>
			public const int VariableAtomExp = 0x0026;
			/// <summary>
			/// The unique identifier for variable exists_exp
			/// </summary>
			public const int VariableExistsExp = 0x0027;
			/// <summary>
			/// The unique identifier for variable limits_exp
			/// </summary>
			public const int VariableLimitsExp = 0x0028;
			/// <summary>
			/// The unique identifier for variable comp_exp
			/// </summary>
			public const int VariableCompExp = 0x0029;
			/// <summary>
			/// The unique identifier for variable comp_opd
			/// </summary>
			public const int VariableCompOpd = 0x002A;
			/// <summary>
			/// The unique identifier for variable typeof_opd
			/// </summary>
			public const int VariableTypeofOpd = 0x002B;
			/// <summary>
			/// The unique identifier for variable access_opd
			/// </summary>
			public const int VariableAccessOpd = 0x002C;
			/// <summary>
			/// The unique identifier for variable comp_op
			/// </summary>
			public const int VariableCompOp = 0x002D;
			/// <summary>
			/// The unique identifier for variable eq_op
			/// </summary>
			public const int VariableEqOp = 0x002E;
			/// <summary>
			/// The unique identifier for variable in_op
			/// </summary>
			public const int VariableInOp = 0x002F;
			/// <summary>
			/// The unique identifier for variable num_op
			/// </summary>
			public const int VariableNumOp = 0x0030;
			/// <summary>
			/// The unique identifier for variable lt_comp_op
			/// </summary>
			public const int VariableLtCompOp = 0x0031;
			/// <summary>
			/// The unique identifier for variable gt_comp_op
			/// </summary>
			public const int VariableGtCompOp = 0x0032;
			/// <summary>
			/// The unique identifier for variable selector_exp
			/// </summary>
			public const int VariableSelectorExp = 0x0033;
			/// <summary>
			/// The unique identifier for variable type_id
			/// </summary>
			public const int VariableTypeId = 0x0034;
			/// <summary>
			/// The unique identifier for variable pt_keyword
			/// </summary>
			public const int VariablePtKeyword = 0x0035;
			/// <summary>
			/// The unique identifier for variable literal
			/// </summary>
			public const int VariableLiteral = 0x0036;
			/// <summary>
			/// The unique identifier for variable numeric_lit
			/// </summary>
			public const int VariableNumericLit = 0x0037;
			/// <summary>
			/// The unique identifier for variable schema_id
			/// </summary>
			public const int VariableSchemaId = 0x0038;
			/// <summary>
			/// The unique identifier for variable select_or_entity_id
			/// </summary>
			public const int VariableSelectOrEntityId = 0x0039;
			/// <summary>
			/// The unique identifier for variable prop_id
			/// </summary>
			public const int VariablePropId = 0x003A;
			/// <summary>
			/// The unique identifier for variable rule_id
			/// </summary>
			public const int VariableRuleId = 0x003B;
		}
		/// <summary>
		/// The collection of variables matched by this parser
		/// </summary>
		/// <remarks>
		/// The variables are in an order consistent with the automaton,
		/// so that variable indices in the automaton can be used to retrieve the variables in this table
		/// </remarks>
		private static readonly Symbol[] variables = {
			new Symbol(0x000D, "root"), 
			new Symbol(0x000E, "comment_exp"), 
			new Symbol(0x000F, "schema_body"), 
			new Symbol(0x0010, "use_decl"), 
			new Symbol(0x0011, "schema_decl"), 
			new Symbol(0x0012, "type_decl"), 
			new Symbol(0x0013, "select_decl"), 
			new Symbol(0x0014, "entity_decl"), 
			new Symbol(0x0015, "enum_decl"), 
			new Symbol(0x0016, "abstract_decl"), 
			new Symbol(0x0017, "supertype_decl"), 
			new Symbol(0x0018, "subtype_decl"), 
			new Symbol(0x0019, "derive_decl"), 
			new Symbol(0x001A, "derive_exp"), 
			new Symbol(0x001B, "attr_read_exp"), 
			new Symbol(0x001C, "prop_decl"), 
			new Symbol(0x001D, "prop_exp"), 
			new Symbol(0x001E, "list_exp"), 
			new Symbol(0x001F, "set_exp"), 
			new Symbol(0x0020, "array_exp"), 
			new Symbol(0x0021, "optional_decl"), 
			new Symbol(0x0022, "where_decl"), 
			new Symbol(0x0023, "or_exp"), 
			new Symbol(0x0024, "and_exp"), 
			new Symbol(0x0025, "unary_exp"), 
			new Symbol(0x0026, "atom_exp"), 
			new Symbol(0x0027, "exists_exp"), 
			new Symbol(0x0028, "limits_exp"), 
			new Symbol(0x0029, "comp_exp"), 
			new Symbol(0x002A, "comp_opd"), 
			new Symbol(0x002B, "typeof_opd"), 
			new Symbol(0x002C, "access_opd"), 
			new Symbol(0x002D, "comp_op"), 
			new Symbol(0x002E, "eq_op"), 
			new Symbol(0x002F, "in_op"), 
			new Symbol(0x0030, "num_op"), 
			new Symbol(0x0031, "lt_comp_op"), 
			new Symbol(0x0032, "gt_comp_op"), 
			new Symbol(0x0033, "selector_exp"), 
			new Symbol(0x0034, "type_id"), 
			new Symbol(0x0035, "pt_keyword"), 
			new Symbol(0x0036, "literal"), 
			new Symbol(0x0037, "numeric_lit"), 
			new Symbol(0x0038, "schema_id"), 
			new Symbol(0x0039, "select_or_entity_id"), 
			new Symbol(0x003A, "prop_id"), 
			new Symbol(0x003B, "rule_id"), 
			new Symbol(0x003C, "__V60"), 
			new Symbol(0x003D, "__V61"), 
			new Symbol(0x0048, "__V72"), 
			new Symbol(0x004B, "__V75"), 
			new Symbol(0x004F, "__V79"), 
			new Symbol(0x0052, "__V82"), 
			new Symbol(0x0055, "__V85"), 
			new Symbol(0x005B, "__V91"), 
			new Symbol(0x0063, "__V99"), 
			new Symbol(0x0072, "__V114"), 
			new Symbol(0x007C, "__VAxiom") };
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
		/// Initializes a new instance of the parser
		/// </summary>
		/// <param name="lexer">The input lexer</param>
		public ExpressParser(ExpressLexer lexer) : base (commonAutomaton, variables, virtuals, null, lexer) { }

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
			public virtual void OnTerminalInlineAnnotation(ASTNode node) {}
			public virtual void OnTerminalIntegerLiteralDecimal(ASTNode node) {}
			public virtual void OnTerminalIntegerLiteralHexa(ASTNode node) {}
			public virtual void OnTerminalRealLiteral(ASTNode node) {}
			public virtual void OnTerminalStringLiteral(ASTNode node) {}
			public virtual void OnVariableRoot(ASTNode node) {}
			public virtual void OnVariableCommentExp(ASTNode node) {}
			public virtual void OnVariableSchemaBody(ASTNode node) {}
			public virtual void OnVariableUseDecl(ASTNode node) {}
			public virtual void OnVariableSchemaDecl(ASTNode node) {}
			public virtual void OnVariableTypeDecl(ASTNode node) {}
			public virtual void OnVariableSelectDecl(ASTNode node) {}
			public virtual void OnVariableEntityDecl(ASTNode node) {}
			public virtual void OnVariableEnumDecl(ASTNode node) {}
			public virtual void OnVariableAbstractDecl(ASTNode node) {}
			public virtual void OnVariableSupertypeDecl(ASTNode node) {}
			public virtual void OnVariableSubtypeDecl(ASTNode node) {}
			public virtual void OnVariableDeriveDecl(ASTNode node) {}
			public virtual void OnVariableDeriveExp(ASTNode node) {}
			public virtual void OnVariableAttrReadExp(ASTNode node) {}
			public virtual void OnVariablePropDecl(ASTNode node) {}
			public virtual void OnVariablePropExp(ASTNode node) {}
			public virtual void OnVariableListExp(ASTNode node) {}
			public virtual void OnVariableSetExp(ASTNode node) {}
			public virtual void OnVariableArrayExp(ASTNode node) {}
			public virtual void OnVariableOptionalDecl(ASTNode node) {}
			public virtual void OnVariableWhereDecl(ASTNode node) {}
			public virtual void OnVariableOrExp(ASTNode node) {}
			public virtual void OnVariableAndExp(ASTNode node) {}
			public virtual void OnVariableUnaryExp(ASTNode node) {}
			public virtual void OnVariableAtomExp(ASTNode node) {}
			public virtual void OnVariableExistsExp(ASTNode node) {}
			public virtual void OnVariableLimitsExp(ASTNode node) {}
			public virtual void OnVariableCompExp(ASTNode node) {}
			public virtual void OnVariableCompOpd(ASTNode node) {}
			public virtual void OnVariableTypeofOpd(ASTNode node) {}
			public virtual void OnVariableAccessOpd(ASTNode node) {}
			public virtual void OnVariableCompOp(ASTNode node) {}
			public virtual void OnVariableEqOp(ASTNode node) {}
			public virtual void OnVariableInOp(ASTNode node) {}
			public virtual void OnVariableNumOp(ASTNode node) {}
			public virtual void OnVariableLtCompOp(ASTNode node) {}
			public virtual void OnVariableGtCompOp(ASTNode node) {}
			public virtual void OnVariableSelectorExp(ASTNode node) {}
			public virtual void OnVariableTypeId(ASTNode node) {}
			public virtual void OnVariablePtKeyword(ASTNode node) {}
			public virtual void OnVariableLiteral(ASTNode node) {}
			public virtual void OnVariableNumericLit(ASTNode node) {}
			public virtual void OnVariableSchemaId(ASTNode node) {}
			public virtual void OnVariableSelectOrEntityId(ASTNode node) {}
			public virtual void OnVariablePropId(ASTNode node) {}
			public virtual void OnVariableRuleId(ASTNode node) {}
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
				case 0x0008: visitor.OnTerminalInlineAnnotation(node); break;
				case 0x0009: visitor.OnTerminalIntegerLiteralDecimal(node); break;
				case 0x000A: visitor.OnTerminalIntegerLiteralHexa(node); break;
				case 0x000B: visitor.OnTerminalRealLiteral(node); break;
				case 0x000C: visitor.OnTerminalStringLiteral(node); break;
				case 0x000D: visitor.OnVariableRoot(node); break;
				case 0x000E: visitor.OnVariableCommentExp(node); break;
				case 0x000F: visitor.OnVariableSchemaBody(node); break;
				case 0x0010: visitor.OnVariableUseDecl(node); break;
				case 0x0011: visitor.OnVariableSchemaDecl(node); break;
				case 0x0012: visitor.OnVariableTypeDecl(node); break;
				case 0x0013: visitor.OnVariableSelectDecl(node); break;
				case 0x0014: visitor.OnVariableEntityDecl(node); break;
				case 0x0015: visitor.OnVariableEnumDecl(node); break;
				case 0x0016: visitor.OnVariableAbstractDecl(node); break;
				case 0x0017: visitor.OnVariableSupertypeDecl(node); break;
				case 0x0018: visitor.OnVariableSubtypeDecl(node); break;
				case 0x0019: visitor.OnVariableDeriveDecl(node); break;
				case 0x001A: visitor.OnVariableDeriveExp(node); break;
				case 0x001B: visitor.OnVariableAttrReadExp(node); break;
				case 0x001C: visitor.OnVariablePropDecl(node); break;
				case 0x001D: visitor.OnVariablePropExp(node); break;
				case 0x001E: visitor.OnVariableListExp(node); break;
				case 0x001F: visitor.OnVariableSetExp(node); break;
				case 0x0020: visitor.OnVariableArrayExp(node); break;
				case 0x0021: visitor.OnVariableOptionalDecl(node); break;
				case 0x0022: visitor.OnVariableWhereDecl(node); break;
				case 0x0023: visitor.OnVariableOrExp(node); break;
				case 0x0024: visitor.OnVariableAndExp(node); break;
				case 0x0025: visitor.OnVariableUnaryExp(node); break;
				case 0x0026: visitor.OnVariableAtomExp(node); break;
				case 0x0027: visitor.OnVariableExistsExp(node); break;
				case 0x0028: visitor.OnVariableLimitsExp(node); break;
				case 0x0029: visitor.OnVariableCompExp(node); break;
				case 0x002A: visitor.OnVariableCompOpd(node); break;
				case 0x002B: visitor.OnVariableTypeofOpd(node); break;
				case 0x002C: visitor.OnVariableAccessOpd(node); break;
				case 0x002D: visitor.OnVariableCompOp(node); break;
				case 0x002E: visitor.OnVariableEqOp(node); break;
				case 0x002F: visitor.OnVariableInOp(node); break;
				case 0x0030: visitor.OnVariableNumOp(node); break;
				case 0x0031: visitor.OnVariableLtCompOp(node); break;
				case 0x0032: visitor.OnVariableGtCompOp(node); break;
				case 0x0033: visitor.OnVariableSelectorExp(node); break;
				case 0x0034: visitor.OnVariableTypeId(node); break;
				case 0x0035: visitor.OnVariablePtKeyword(node); break;
				case 0x0036: visitor.OnVariableLiteral(node); break;
				case 0x0037: visitor.OnVariableNumericLit(node); break;
				case 0x0038: visitor.OnVariableSchemaId(node); break;
				case 0x0039: visitor.OnVariableSelectOrEntityId(node); break;
				case 0x003A: visitor.OnVariablePropId(node); break;
				case 0x003B: visitor.OnVariableRuleId(node); break;
			}
		}
	}
}
