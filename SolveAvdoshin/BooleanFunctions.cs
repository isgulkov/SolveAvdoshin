using System;
using System.Collections.Generic;

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

//			var ex = (new BooleanFunction(26)).GetFDNF();
//
//			Console.WriteLine(ex.ToString());
//
//			PrintDerivatives(new BooleanFunction(26));

//			foreach(var t in BooleanExpression.AllTrees(3)) {
//				t.SetIthVar(0, BooleanVariable.B);
//				t.setIthOp(0, BooleanOperation.Xor);
//				Console.WriteLine(t.ToString());
//			}

//			foreach(var v in Combinatorics.AllNTuples(new BooleanOperation[] { BooleanOperation.And,
//				BooleanOperation.Or }, 3)) {
//				foreach(var b in v) {
//					Console.Write("{0} ", b);
//				}
//				Console.WriteLine();
//			}

			BooleanOperation[] ops = { BooleanOperation.And, BooleanOperation.Or };
			BooleanVariable[] vars = { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C };

			foreach(var v in BooleanExpression.AllExpressions(3, ops, vars)) {
				Console.WriteLine(v.ToString());
			}
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
			
		public int Eval(int a, int b, int c)
		{
			return TruthTable >> (7 - a * 4 - b * 2 - c) & 1;
		}

		public static string VarRows()
		{
			string[] lines = new string[] { "A ", "B ", "C ", };

			for(int i = 0; i < 8; i++) {
				lines[0] += (i >> 2 & 1) + " ";
				lines[1] += (i >> 1 & 1) + " ";
				lines[2] += (i & 1) + " ";
			}

			return String.Join("\n", lines) + "\n";
		}

		public static string DelimRow()
		{
			string line = "--";

			for(int i = 0; i < 8; i++) {
				line += "--";
			}

			return line + "\n";
		}

		public string FuncRow()
		{
			string line = "F ";

			for(int i = 0; i < 8; i++) {
				line += (Eval((i >> 2 & 1) == 1, (i >> 1 & 1) == 1, (i & 1) == 1) ? 1 : 0) + " ";
			}

			return line + "\n";
		}

		public string ToString(string format)
		{
			if(format == "L") {
				string result = "";

				result += VarRows();
				result += DelimRow();
				result += FuncRow();

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

				varA = new VarExpression(BooleanVariable.A, (i >> 2 & 1) != 1);
				varB = new VarExpression(BooleanVariable.B, (i >> 1 & 1) != 1);
				varC = new VarExpression(BooleanVariable.C, (i & 1) != 1);

				cons1 = new OpExpression(BooleanOperation.And,
					new OpExpression(BooleanOperation.And, varA, varB), varC);

				ex = ex != null ? new OpExpression(BooleanOperation.Or, cons1, ex) : cons1;
			}

			return ex;
		}

		public BooleanExpression MininalExpressionInBasis()
		{
			throw new NotImplementedException();

//			for(int i = 2; i < 15; i++) {
//				foreach(var ex in BooleanExpression.AllExpressions(i)) {
//					
//				}
//			}
		}
	}

	enum BooleanOperation { Zero, NOR, NotBImp, NotA, NotImp, NotB, Xor, NAND, And, Eq, B, Imp, A, BImp, Or, One };
	enum BooleanVariable { A, B, C };

	abstract class BooleanExpression
	{
		abstract public bool Eval(bool a, bool b, bool c);
		abstract new public string ToString();
		abstract public int CountVariables();
		abstract public void SetIthVar(int i, BooleanVariable value);
		abstract public int setIthOp(int i, BooleanOperation value);

		public static IEnumerable<BooleanExpression> AllTrees(int size)
		{
			if(size == 0) {
				yield return new VarExpression(BooleanVariable.A);
			}
			else {
				for(int i = 0; i < size; i++) {
					foreach(var l in AllTrees(i)) {
						foreach(var r in AllTrees(size - 1 - i)) {
							yield return new OpExpression(BooleanOperation.And, l, r);
						}
					}
				}
			}
		}

		public static IEnumerable<BooleanExpression> AllExpressions(int size, BooleanOperation[] ops,
			BooleanVariable[] vars)
		{
			foreach(var ex in AllTrees(size)) {
				foreach(var opList in Combinatorics.AllNTuples(ops, size)) {
					foreach(var varList in Combinatorics.AllNTuples(vars, size + 1)) {
						for(int i = 0; i < size; i++) {
							ex.setIthOp(i, opList[i]);
						}

						for(int i = 0; i < size + 1; i++) {
							ex.SetIthVar(i, varList[i]);
						}

						yield return ex;
					}
				}
			}
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

		public override int CountVariables()
		{
			int res = 0;

			res += Left.CountVariables();
			res += Right.CountVariables();

			return res;
		}

		public override void SetIthVar(int i, BooleanVariable value)
		{
			if(i >= Left.CountVariables()) {
				Right.SetIthVar(i - Left.CountVariables(), value);
			}
			else {
				Left.SetIthVar(i, value);
			}
		}

		public override int setIthOp(int i, BooleanOperation value)
		{
			if(i < 0) {
				return 0;
			}
			else {
				if(i == 0) {
					Op = value;
				}

				int res = 1;

				res += Left.setIthOp(i - res, value);
				res += Right.setIthOp(i - res, value);

				return res;
			}
		}
	}

	class VarExpression : BooleanExpression
	{
		static readonly string[] VarSymbols = new string[] { "A", "B", "C", };

		BooleanVariable Var;
		bool Not;

		public VarExpression(BooleanVariable var, bool not = false)
		{
			Var = var;
			Not = not;
		}

		public override bool Eval(bool a, bool b, bool c)
		{
			switch(Var) {
			case BooleanVariable.A:
				return a != Not;
			case BooleanVariable.B:
				return b != Not;
			case BooleanVariable.C:
				return c != Not;
			default:
				throw new Exception("Nigga wat?");
			}
		}

		public override string ToString()
		{
			return (Not ? "!" : "") + VarSymbols[(int)Var];
		}

		public override int CountVariables()
		{
			return 1;
		}

		public override void SetIthVar(int i, BooleanVariable variable)
		{
			this.Var = variable;
		}

		public override int setIthOp(int i, BooleanOperation value)
		{
			return 0;
		}
	}
}

