using System;

namespace SolveAvdoshin
{
	public static class BooleanFunctions
	{
		public static void Main()
		{
			var ex = new OpSubexpression(BooleanOperation.Imp, new NotSubexpression(BooleanVariable.A), new OpSubexpression(BooleanOperation.Xor, BooleanVariable.B, BooleanVariable.C));

			Console.WriteLine(ex.ToString());
			Console.WriteLine(ex.Eval(false, true, true));
		}
	}

	enum BooleanOperation { Zero, NOR, NotBImp, NotA, NotImp, NotB, Xor, NAND, And, Eq, B, Imp, A, BImp, Or, One };
	enum BooleanVariable { A, B, C };

	abstract class BooleanExpression // TODO: Make private
	{
		
	}

	abstract class BooleanSubexpression
	{
		abstract public bool Eval(bool a, bool b, bool c);
		abstract new public string ToString();
	}

	class NotSubexpression : BooleanSubexpression
	{
		BooleanSubexpression Arg;

		public NotSubexpression(BooleanSubexpression arg)
		{
			Arg = arg;
		}

		public NotSubexpression(BooleanVariable variable)
		{
			Arg = new VarSubexpression(variable);
		}

		public override string ToString()
		{
			return "!" + Arg.ToString();
		}

		public override bool Eval(bool a, bool b, bool c)
		{
			return !Arg.Eval(a, b, c);
		}
	}

	class OpSubexpression : BooleanSubexpression
	{
		static readonly string[] OpSymbols = new string[] { "(Zero)", "(NOR)", "</=", "(NotA)", "=/>", "(NotB)", "⨁", "(NAND)", "&", "==", "(B)", "=>", "(A)", "<=", "|", "(One)", };

		BooleanOperation Op;
		BooleanSubexpression Left, Right;

		public OpSubexpression(BooleanOperation op, BooleanSubexpression left, BooleanSubexpression right)
		{
			Op = op;
			Left = left;
			Right = right;
		}

		public OpSubexpression(BooleanOperation op, BooleanVariable left, BooleanSubexpression right)
		{
			Op = op;
			Left = new VarSubexpression(left);
			Right = right;
		}

		public OpSubexpression(BooleanOperation op, BooleanSubexpression left, BooleanVariable right)
		{
			Op = op;
			Left = left;
			Right = new VarSubexpression(right);
		}

		public OpSubexpression(BooleanOperation op, BooleanVariable left, BooleanVariable right)
		{
			Op = op;
			Left = new VarSubexpression(left);
			Right = new VarSubexpression(right);
		}

		public override bool Eval(bool a, bool b, bool c)
		{
			bool aVal = Left.Eval(a, b, c);
			bool bVal = Right.Eval(a, b, c);

			switch(Op) {
			case BooleanOperation.Zero:
				return false;
			case BooleanOperation.NOR:
				return !(aVal || bVal);
			case BooleanOperation.NotBImp:
				return !aVal && bVal;
			case BooleanOperation.NotA:
				return !aVal;
			case BooleanOperation.NotImp:
				return aVal && !bVal;
			case BooleanOperation.NotB:
				return !bVal;
			case BooleanOperation.Xor:
				return aVal != bVal;
			case BooleanOperation.NAND:
				return !(aVal && bVal);
			case BooleanOperation.And:
				return aVal && bVal;
			case BooleanOperation.Eq:
				return aVal == bVal;
			case BooleanOperation.B:
				return bVal;
			case BooleanOperation.Imp:
				return !aVal || bVal;
			case BooleanOperation.A:
				return aVal;
			case BooleanOperation.BImp:
				return aVal || !bVal;
			case BooleanOperation.Or:
				return aVal || bVal;
			case BooleanOperation.One:
				return true;
			default:
				throw new ArgumentException();
			}
		}

		public override string ToString()
		{
			return "(" + Left.ToString() + " " + OpSymbols[(int)Op] + " " + Right.ToString() + ")";
		}
	}

	class VarSubexpression : BooleanSubexpression
	{
		static readonly string[] VarSymbols = new string[] { "A", "B", "C", };

		BooleanVariable Var;

		public VarSubexpression(BooleanVariable var)
		{
			Var = var;
		}

		public override bool Eval(bool a, bool b, bool c)
		{
			switch(Var) {
			case BooleanVariable.A:
				return a;
			case BooleanVariable.B:
				return b;
			case BooleanVariable.C:
				return c;
			default:
				throw new Exception("Nigga wat?");
			}
		}

		public override string ToString()
		{
			return VarSymbols[(int)Var];
		}
	}

	class BooleanFunction // TODO: Make private ?
	{
		int TruthTable;

		public BooleanFunction(int n)
		{
			TruthTable = n;
		}

		public int Eval(int a, int b, int c)
		{
			return TruthTable >> (7 - 4 * a - 2 * b - c) & 1;
		}

		public BooleanExpression GetFDNF() // СДНФ
		{
			return null;
		}
	}
}

