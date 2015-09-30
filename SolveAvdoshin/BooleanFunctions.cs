using System;
using System.Collections.Generic;

namespace SolveAvdoshin
{
	public static class BooleanFunctions
	{
		static readonly string[] AvdoshinNames = {
			"ОБЩИЙ",
			"NOR",
			"NAND",
			"0, IMP",
			"1, COIMP",
			"IMP, COIMP",
			"XOR, IMP",
			"EQV, COIMP",
			"NOT, IMP",
			"NOT, COIMP",
			"NOT, OR",
			"NOT, AND",
			"0, EQV, AND",
			"1, XOR, OR",
			"0, EQV, OR",
			"1, XOR, AND",
			"XOR, EQV, OR",
			"EQV, XOR, AND",
		};

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
			n = 253;

			Console.WriteLine("\nМинимальные представления f_" + n + " в базисах (для рисования в винлогике):");

			Console.WriteLine(" ☻ \tБазис        \t\tВыражение");
			Console.WriteLine();

			for(int i = 0; i < AvdoshinBases.Length; i++) {
				Console.Write("{0,2}\t{1}\t\t", i, AvdoshinNames[i].PadRight(13));

				try {
					var ex = FindMininalExpressionInBasis(n, AvdoshinBases[i], AvdoshinVars[i]);

					Console.WriteLine(ex.ToString());
				}
				catch(CouldntFindExpressionException) {
					Console.WriteLine("не нашлось");
				}
			}
		}

		static BooleanExpression FindMininalExpressionInBasis(int n, BooleanOperation[] ops, BooleanVariable[] vars)
		{
			var queue = new Queue<BooleanExpression>();
			var knownTruthTables = new HashSet<byte>();
			var knownExpressions = new HashSet<BooleanExpression>();

			foreach(var variable in vars) {
				queue.Enqueue(new VarExpression(variable));
			}

			while(queue.Count != 0) {
				var curExperession = queue.Dequeue();

				byte truthTable = curExperession.Eval();

				if(knownTruthTables.Contains(truthTable)) {
					continue;
				}

				if(curExperession.Eval() == n) {
					return curExperession;
				}

				knownExpressions.Add(curExperession);
				knownTruthTables.Add(truthTable);

				var newExpressions = new Stack<BooleanExpression>();

				foreach(var anotherExpression in knownExpressions) {
					foreach(var neighbourExpression in curExperession.CombineWith(anotherExpression, ops)) {
						newExpressions.Push(neighbourExpression);
					}
				}

				while(newExpressions.Count != 0) {
					queue.Enqueue(newExpressions.Pop());
				}
			}

			throw new CouldntFindExpressionException();
		}
	}

	class CouldntFindExpressionException : Exception
	{

	}



	enum BooleanOperation { Zero, NOR, NotCoImp, NotA, NotImp, NotB, Xor, NAND, And, Eq, B, Imp, A, CoImp, Or, One };
	enum BooleanVariable { A, B, C, Zero, One, };

	abstract class BooleanExpression
	{
		public static readonly string[] OpSymbols = new string[] { "0", "↓", "</=", "(!A)", "=/>", "(!B)", "⨁", "|", "&", "==", "(B)", "=>", "(A)", "<=", "|", "1", };

		abstract public byte Eval();
		abstract new public string ToString();
		abstract public BooleanExpression Clone();

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public bool Equals(BooleanExpression obj)
		{
			return ToString() == obj.ToString();
		}

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

		public IEnumerable<BooleanExpression> NeighbourExpressions(BooleanOperation[] ops,
			BooleanVariable[] vars)
		{
			foreach(var op in ops) {
				foreach(var variable in vars) {
					yield return new OpExpression(op, Clone(), new VarExpression(variable));

					yield return new OpExpression(op, new VarExpression(variable), Clone());
				}
			}
		}

		public IEnumerable<BooleanExpression> CombineWith(BooleanExpression anotherExpression,
			BooleanOperation[] ops)
		{
			foreach(var op in ops) {
				yield return new OpExpression(op, Clone(), anotherExpression.Clone());
				yield return new OpExpression(op, anotherExpression.Clone(), Clone());
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
	}
}

