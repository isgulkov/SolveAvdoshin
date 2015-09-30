using System;
using System.Collections.Generic;

namespace SolveAvdoshin
{
	public static class BooleanFunctions
	{
		static readonly BooleanOperation[][] AvdoshinBases = {
			new BooleanOperation[] {
				BooleanOperation.Zero,
				BooleanOperation.NOR,
				BooleanOperation.NotCoImp,
				BooleanOperation.NotA,
				BooleanOperation.NotImp,
				BooleanOperation.NotB,
				BooleanOperation.Xor,
				BooleanOperation.NAND,
				BooleanOperation.And,
				BooleanOperation.Eq,
				BooleanOperation.B,
				BooleanOperation.Imp,
				BooleanOperation.A,
				BooleanOperation.CoImp,
				BooleanOperation.Or,
				BooleanOperation.One
			},
			new BooleanOperation[] { BooleanOperation.NOR, },
			new BooleanOperation[] { BooleanOperation.NAND, },
			new BooleanOperation[] { BooleanOperation.Imp, },
			new BooleanOperation[] { BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.Imp, BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.Imp, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.Imp, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.CoImp, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.NotA, BooleanOperation.And, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.And, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.And, },
			new BooleanOperation[] { BooleanOperation.Xor, BooleanOperation.Eq, BooleanOperation.Or, },
			new BooleanOperation[] { BooleanOperation.Eq, BooleanOperation.Xor, BooleanOperation.And, },
		};

		static readonly BooleanVariable[][] AvdoshinVars = {
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.Zero, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.One, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.Zero, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.One, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.Zero, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, BooleanVariable.One, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
		};

		public static void PrintMinimaInAvdoshinBases(int n)
		{
			Console.WriteLine("\nМинимальные представления f_" + n + " в базисах (для рисования в винлогике):");

			Console.WriteLine(" ☻ \tБазис\t\tВыражение");
			Console.WriteLine();

			for(int i = 0; i < AvdoshinBases.Length; i++) {
				Console.Write("{0,2}\t", i);

				if(i != 0) {
					foreach(var op in AvdoshinBases[i]) {
						Console.Write(BooleanExpression.PrintOperation(op) + " ");
					}
				}
				else {
					Console.Write("ОБЩИЙ");
				}

				Console.Write("\t\t");

				try {
					var ex = FindMininalExpressionInBasis(n, AvdoshinBases[i], AvdoshinVars[i]);

					Console.WriteLine(ex.ToString());
				}
				catch(TooLongToSearchForExpressionException) {
					Console.WriteLine("долго искать, пропускаем");
				}
			}
		}

		static BooleanExpression FindMininalExpressionInBasis(int n, BooleanOperation[] ops, BooleanVariable[] vars)
		{
			int depthLimit = ops.Length == 1 ? 10 : ops.Length == 2 ? 7 : 5;

			for(int i = 2; i <= depthLimit; i++) {
				foreach(var ex in BooleanExpression.AllExpressions(i, ops, vars)) {
					if(n == ex.Eval()) {
						return ex;
					}
				}
			}

			throw new TooLongToSearchForExpressionException();
		}

		public static void Main1()
		{
			foreach(var op in AvdoshinBases[0]) {
				var ex = new OpExpression(op, BooleanVariable.B, BooleanVariable.C);

				Console.WriteLine(ex.Eval() % 16);
			}

//			foreach(var variable in AvdoshinVars[0]) {
//				Console.WriteLine((new VarExpression(variable)).Eval());
//			}
		}
	}

	class TooLongToSearchForExpressionException : Exception
	{

	}



	enum BooleanOperation { Zero, NOR, NotCoImp, NotA, NotImp, NotB, Xor, NAND, And, Eq, B, Imp, A, CoImp, Or, One };
	enum BooleanVariable { A, B, C, Zero, One, };

	abstract class BooleanExpression
	{
		public static readonly string[] OpSymbols = new string[] { "0", "↓", "</=", "(!A)", "=/>", "(!B)", "⨁", "|", "&", "==", "(B)", "=>", "(A)", "<=", "|", "1", };

		abstract public byte Eval();
		abstract new public string ToString();
		abstract public int CountVariables();
		abstract public void SetIthVar(int i, BooleanVariable value);
		abstract public int SetIthOp(int i, BooleanOperation value);
		abstract public BooleanExpression Clone();

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

		public static readonly BooleanVariable[] AllVars = { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, };

		public static bool CheckVarList(BooleanVariable[] varList)
		{
			foreach(var variable in BooleanExpression.AllVars) {
				bool varPresent = false;

				foreach(var v in varList) {
					if(v == variable) {
						varPresent = true;
						break;
					}
				}

				if(!varPresent) return false;
			}

			return true;
		}

		public static IEnumerable<BooleanExpression> AllExpressions(int size, BooleanOperation[] ops,
			BooleanVariable[] vars)
		{
			foreach(var ex in AllTrees(size)) {
				foreach(var varList in Combinatorics.AllNTuples(vars, size + 1)) {
//					if(!CheckVarList(varList, vars))
//						continue;
					
					foreach(var opList in Combinatorics.AllNTuples(ops, size)) {
						for(int i = 0; i < size; i++) {
							ex.SetIthOp(i, opList[i]);
						}

						for(int i = 0; i < size + 1; i++) {
							ex.SetIthVar(i, varList[i]);
						}

						yield return ex;
					}
				}
			}
		}

		public static string PrintOperation(BooleanOperation op)
		{
			return OpSymbols[(int)op];
		}
	}

	class OpExpression : BooleanExpression
	{
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

		public override BooleanExpression Clone()
		{
			return new OpExpression(Op, Left.Clone(), Right.Clone());
		}

		public override byte Eval()
		{
			byte aVal = Left.Eval();
			byte bVal = Right.Eval();

			int res;

			switch(Op) {
			case BooleanOperation.Zero:
				res = 0x00;
				break;
			case BooleanOperation.NOR:
				res = ~(aVal | bVal);
				break;
			case BooleanOperation.NotCoImp:
				res = ~aVal & bVal;
				break;
			case BooleanOperation.NotA:
				res = ~aVal;
				break;
			case BooleanOperation.NotImp:
				res = aVal & ~bVal;
				break;
			case BooleanOperation.NotB:
				res = ~bVal;
				break;
			case BooleanOperation.Xor:
				res = aVal ^ bVal;
				break;
			case BooleanOperation.NAND:
				res = ~(aVal & bVal);
				break;
			case BooleanOperation.And:
				res = aVal & bVal;
				break;
			case BooleanOperation.Eq:
				res = aVal ^ (~bVal);
				break;
			case BooleanOperation.B:
				res = bVal;
				break;
			case BooleanOperation.Imp:
				res = ~aVal | bVal;
				break;
			case BooleanOperation.A:
				res = aVal;
				break;
			case BooleanOperation.CoImp:
				res = aVal | ~bVal;
				break;
			case BooleanOperation.Or:
				res = aVal | bVal;
				break;
			case BooleanOperation.One:
				res = 0xFF;
				break;
			default:
				throw new ArgumentException();
			}

			return (byte)res;
		}

		public override string ToString()
		{
			switch(Op) {
			case BooleanOperation.A:
				return Left.ToString();
			case BooleanOperation.B:
				return Right.ToString();
			case BooleanOperation.One:
			case BooleanOperation.Zero:
				return PrintOperation(Op);
			case BooleanOperation.NotA:
				return "!" + Left.ToString();
			case BooleanOperation.NotB:
				return "!" + Right.ToString();
			default:
				return "(" + Left.ToString() + " " + PrintOperation(Op) + " " + Right.ToString() + ")";
			}
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

		public override int SetIthOp(int i, BooleanOperation value)
		{
			if(i < 0) {
				return 0;
			}
			else {
				if(i == 0) {
					Op = value;
				}

				int res = 1;

				res += Left.SetIthOp(i - res, value);
				res += Right.SetIthOp(i - res, value);

				return res;
			}
		}
	}

	class VarExpression : BooleanExpression
	{
		static readonly string[] VarSymbols = new string[] { "A", "B", "C", "0", "1", };

		BooleanVariable Var;

		public VarExpression(BooleanVariable var, bool not = false)
		{
			Var = var;
		}

		public override BooleanExpression Clone()
		{
			return new VarExpression(Var);
		}

		public override byte Eval()
		{
			switch(Var) {
			case BooleanVariable.A:
				return 0x0F;
			case BooleanVariable.B:
				return 0x33;
			case BooleanVariable.C:
				return 0x55;
			case BooleanVariable.Zero:
				return 0x00;
			case BooleanVariable.One:
				return 0xFF;
			default:
				throw new Exception("Nigga wat?");
			}
		}

		public override string ToString()
		{
			return VarSymbols[(int)Var];
		}

		public override int CountVariables()
		{
			return 1;
		}

		public override void SetIthVar(int i, BooleanVariable variable)
		{
			this.Var = variable;
		}

		public override int SetIthOp(int i, BooleanOperation value)
		{
			return 0;
		}
	}
}

