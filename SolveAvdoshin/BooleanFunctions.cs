using System;

namespace SolveAvdoshin
{
	public static class BooleanFunctions
	{
		public static void Main()
		{
//			Console.WriteLine(ex.ToString());
//			Console.WriteLine(ex.Eval(true, true, true));

//			var f = new BooleanFunction(134);
//
//			Console.WriteLine(f.ToString());
//			Console.WriteLine(f.ToString("L"));

			var ex = (new BooleanFunction(26)).GetFDNF();

			Console.WriteLine(ex.ToString());
		}
	}

	class BooleanFunction // TODO: Make private ?
	{
		int TruthTable;

		public BooleanFunction(int n)
		{
			TruthTable = n;
		}

		public bool Eval(bool a, bool b, bool c)
		{
			return (TruthTable >> (7 - (a ? 4 : 0) - (b ? 2 : 0) - (c ? 1 : 0)) & 1) == 1;
		}

		public string ToString(string format)
		{
			if(format == "L") {
				string[] lines = new string[] { "A ", "B ", "C ", "--", "F ", };

				for(int i = 0; i < 8; i++) {
					lines[0] += (i >> 2 & 1) + " ";
					lines[1] += (i >> 1 & 1) + " ";
					lines[2] += (i & 1) + " ";
					lines[3] += "--";
					lines[4] += (Eval((i >> 2 & 1) == 1, (i >> 1 & 1) == 1, (i & 1) == 1) ? 1 : 0) + " ";
				}

				string result = "";

				foreach(string line in lines) result += line + "\n";

				return result;
			}
			else if(format == "S") {
				return "BooleanFunction(" + TruthTable + ")";
			}
			else
				throw new ArgumentException("Invalid format string. Use L for long or S for short");
		}

		public override string ToString()
		{
			return ToString("S");
		}

		public BooleanExpression GetFDNF() // СДНФ
		{
			BooleanExpression ex = null, cons1;

			for(int i = 0; i < 8; i++) {
				if(!Eval((i >> 2 & 1) == 1, (i >> 1 & 1) == 1, (i & 1) == 1))
					continue;
					
				BooleanExpression varA, varB, varC;

				if((i >> 2 & 1) == 1)
					varA = new VarExpression(BooleanVariable.A);
				else
					varA = new NotExpression(BooleanVariable.A);

				if((i >> 1 & 1) == 1)
					varB = new VarExpression(BooleanVariable.B);
				else
					varB = new NotExpression(BooleanVariable.B);

				if((i & 1) == 1)
					varC = new VarExpression(BooleanVariable.C);
				else
					varC = new NotExpression(BooleanVariable.C);

				cons1 = new OpExpression(BooleanOperation.And, new OpExpression(BooleanOperation.And, varA, varB), varC);

				ex = ex != null ? new OpExpression(BooleanOperation.Or, cons1, ex) : cons1;
			}

			return ex;
		}
	}

	enum BooleanOperation { Zero, NOR, NotBImp, NotA, NotImp, NotB, Xor, NAND, And, Eq, B, Imp, A, BImp, Or, One };
	enum BooleanVariable { A, B, C };

	abstract class BooleanExpression
	{
		abstract public bool Eval(bool a, bool b, bool c);
		abstract new public string ToString();
	}

	class NotExpression : BooleanExpression
	{
		BooleanExpression Arg;

		public NotExpression(BooleanExpression arg)
		{
			Arg = arg;
		}

		public NotExpression(BooleanVariable variable)
		{
			Arg = new VarExpression(variable);
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

	class OpExpression : BooleanExpression
	{
		static readonly string[] OpSymbols = new string[] { "(Zero)", "(NOR)", "</=", "(NotA)", "=/>", "(NotB)", "⨁", "(NAND)", "&", "==", "(B)", "=>", "(A)", "<=", "|", "(One)", };

		BooleanOperation Op;
		BooleanExpression Left, Right;

		public OpExpression(BooleanOperation op, BooleanExpression left, BooleanExpression right)
		{
			Op = op;
			Left = left;
			Right = right;
		}

		public OpExpression(BooleanOperation op, BooleanVariable left, BooleanExpression right)
		{
			Op = op;
			Left = new VarExpression(left);
			Right = right;
		}

		public OpExpression(BooleanOperation op, BooleanExpression left, BooleanVariable right)
		{
			Op = op;
			Left = left;
			Right = new VarExpression(right);
		}

		public OpExpression(BooleanOperation op, BooleanVariable left, BooleanVariable right)
		{
			Op = op;
			Left = new VarExpression(left);
			Right = new VarExpression(right);
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

	class VarExpression : BooleanExpression
	{
		static readonly string[] VarSymbols = new string[] { "A", "B", "C", };

		BooleanVariable Var;

		public VarExpression(BooleanVariable var)
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
}

