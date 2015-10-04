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
				BooleanOperation.CoImp,
				BooleanOperation.NotA,
				BooleanOperation.BCoImp,
				BooleanOperation.NotB,
				BooleanOperation.Xor,
				BooleanOperation.NAND,
				BooleanOperation.And,
				BooleanOperation.Eq,
				BooleanOperation.B,
				BooleanOperation.BImp,
				BooleanOperation.A,
				BooleanOperation.Imp,
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

			Console.WriteLine(" ☻   Базис             Выражение");
			Console.WriteLine();

			for(int i = 0; i < AvdoshinBases.Length; i++) {
				Console.Write("{0} — {1}", i.ToString("D2"), AvdoshinNames[i].PadRight(18));

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
			var queue = new ImprovisedPriorityQueue<BooleanExpression>(20);
			var knownTruthTables = new HashSet<byte>();
			var knownExpressions = new HashSet<BooleanExpression>();

			foreach(var variable in vars) {
				queue.TryEnqueue(new VarExpression(variable), 1);
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

				foreach(var anotherExpression in knownExpressions) {
					foreach(var neighbourExpression in curExperession.CombineWith(anotherExpression, ops)) {
						queue.TryEnqueue(neighbourExpression, neighbourExpression.getSize());
					}
				}
			}

			throw new CouldntFindExpressionException();
		}

		public static void Main()
		{
			var f = new BooleanFunction(26);

			Console.WriteLine(f.ToString());

			Console.WriteLine();
			Console.WriteLine(f.Deriv(BooleanVariable.A).ToString("F'_A"));
			Console.WriteLine(f.Deriv(BooleanVariable.B).ToString("F'_B"));
			Console.WriteLine(f.Deriv(BooleanVariable.C).ToString("F'_C"));

			Console.WriteLine();
			Console.WriteLine(f.Deriv(BooleanVariable.A, BooleanVariable.B).ToString("F''_AB"));
			Console.WriteLine(f.Deriv(BooleanVariable.B, BooleanVariable.C).ToString("F''_BC"));
			Console.WriteLine(f.Deriv(BooleanVariable.A, BooleanVariable.C).ToString("F''_AC"));

			Console.WriteLine();
			Console.WriteLine(f.Deriv(BooleanVariable.A, BooleanVariable.B, BooleanVariable.C)
				.ToString("F'''_ABC"));

			Console.WriteLine();
			Console.WriteLine(f.DirectionalDeriv(BooleanVariable.A, BooleanVariable.B).ToString("F'_(A,B)"));
			Console.WriteLine(f.DirectionalDeriv(BooleanVariable.B, BooleanVariable.C).ToString("F'_(B,C)"));
			Console.WriteLine(f.DirectionalDeriv(BooleanVariable.A, BooleanVariable.C).ToString("F'_(A,C)"));

			Console.WriteLine();
			Console.WriteLine(f.DirectionalDeriv(BooleanVariable.A, BooleanVariable.B, BooleanVariable.C)
				.ToString("F'_(A,B,C)"));
		}
	}

	class BooleanFunction
	{
		byte TruthTable;

		public BooleanFunction(byte n)
		{
			TruthTable = n;
		}

		public byte Eval()
		{
			return TruthTable;
		}

		public byte EvalAt(int n)
		{
			return (byte)((TruthTable >> (7 - n)) & 1);
		}

		public new string ToString()
		{
			return ToString("F");
		}

		public string ToString(string caption)
		{
			int n = TruthTable;
			char[] digits = new char[8];

			for(int i = 0; i < 8; i++) {
				digits[i] += (n % 2).ToString()[0];
				n /= 2;
			}

			Array.Reverse(digits);

			return caption.PadRight(10) + " " + String.Join(" ", digits);
		}

		public BooleanFunction DirectionalDeriv(params BooleanVariable[] variables)
		{
			Dictionary<BooleanVariable, bool> values;

			int res = 0;

			for(int i = 0; i < 8; i++) {
				res *= 2;

				values = new Dictionary<BooleanVariable, bool> { { BooleanVariable.A, i / 4 == 1 },
					{ BooleanVariable.B, i / 2 % 2 == 1 },
					{ BooleanVariable.C, i % 2 == 1 },
				};

				BooleanFunction leftOp = this;
				BooleanFunction rightOp = this;

				foreach(var variable in variables) {
					leftOp = leftOp.FixVar(variable, values[variable] ? 1 : 0);
					rightOp = rightOp.FixVar(variable, values[variable] ? 0 : 1);
				}

				res += leftOp.EvalAt(i) ^ rightOp.EvalAt(i);
			}

			return new BooleanFunction((byte)res);
		}

		public BooleanFunction Deriv(params BooleanVariable[] variables)
		{
			BooleanFunction res = this;

			foreach(var variable in variables) {
				res = new BooleanFunction((byte)(res.FixVar(variable, 0).Eval() ^ res.FixVar(variable, 1).Eval()));
			}

			return res;
		}

		BooleanFunction FixVar(BooleanVariable variable, int value)
		{
			int res = 0;

			switch(variable) {
			case BooleanVariable.A:
				res = FixA(value);
				break;
			case BooleanVariable.B:
				res = FixB(value);
				break;
			case BooleanVariable.C:
				res = FixC(value);
				break;
			default:
				throw new ArgumentException("The variable argument for FixVar should be A, B or C");
			}

			return new BooleanFunction((byte)res);
		}

		byte FixA(int value)
		{
			int res = 0;

			if(value == 0) {
				res = TruthTable >> 4;
				res += (TruthTable >> 4) * 16;
			}
			else if(value == 1) {
				res = TruthTable & 15;
				res += (TruthTable & 15) * 16;
			}
			else {
				throw new ArgumentException("Value for Fix[Variable] should be either 0 or 1");
			}

			return (byte)res;
		}

		byte FixB(int value)
		{
			int res = 0;

			if(value == 0) {
				res += (TruthTable >> 2) & 3;
				res += ((TruthTable >> 2) & 3) * 4;
				res += ((TruthTable >> 6) & 3) * 16;
				res += ((TruthTable >> 6) & 3) * 64;
			}
			else if(value == 1) {
				res += TruthTable & 3;
				res += (TruthTable & 3) * 4;
				res += ((TruthTable >> 4) & 3) * 16;
				res += ((TruthTable >> 4) & 3) * 64;
			}
			else {
				throw new ArgumentException("Value for Fix[Variable] should be either 0 or 1");
			}

			return (byte)res;
		}

		byte FixC(int value)
		{
			int res = 0;

			if(value == 0) {
				res += (TruthTable >> 1) & 1;
				res += ((TruthTable >> 1) & 1) * 2;
				res += ((TruthTable >> 3) & 1) * 4;
				res += ((TruthTable >> 3) & 1) * 8;
				res += ((TruthTable >> 5) & 1) * 16;
				res += ((TruthTable >> 5) & 1) * 32;
				res += ((TruthTable >> 7) & 1) * 64;
				res += ((TruthTable >> 7) & 1) * 128;
			}
			else if(value == 1) {
				res += TruthTable & 1;
				res += (TruthTable & 1) * 2;
				res += ((TruthTable >> 2) & 1) * 4;
				res += ((TruthTable >> 2) & 1) * 8;
				res += ((TruthTable >> 4) & 1) * 16;
				res += ((TruthTable >> 4) & 1) * 32;
				res += ((TruthTable >> 6) & 1) * 64;
				res += ((TruthTable >> 6) & 1) * 128;
			}
			else {
				throw new ArgumentException("Value for Fix[Variable] should be either 0 or 1");
			}

			return (byte)res;
		}
	}

	class ImprovisedPriorityQueue<T>
	{
		public int Capacity, Count;
		Queue<T>[] Queues;

		public ImprovisedPriorityQueue(int capacity)
		{
			Capacity = capacity;
			Count = 0;

			Queues = new Queue<T>[capacity + 1];

			for(int i = 1; i <= capacity; i++) {
				Queues[i] = new Queue<T>();
			}
		}

		public bool TryEnqueue(T item, int priority)
		{
			if(priority < 1 || priority > Capacity)
				return false;

			Count += 1;
			Queues[priority].Enqueue(item);
			return true;
		}

		public T Dequeue()
		{
			if(Count == 0)
				throw new Exception("ImprovisedPriorityQueue is empty");

			for(int i = 1; i <= Capacity; i++) {
				if(Queues[i].Count != 0) {
					Count -= 1;
					return Queues[i].Dequeue();
				}
			}

			throw new Exception("ImprovisedPriorityQueue is empty");
		}
	}

	class CouldntFindExpressionException : Exception
	{

	}

	enum BooleanOperation { Zero, NOR, CoImp, NotA, BCoImp, NotB, Xor, NAND, And, Eq, B, BImp, A, Imp, Or, One };
	enum BooleanVariable { A, B, C, Zero, One, };

	abstract class BooleanExpression
	{
		public static readonly string[] OpSymbols = new string[] { "0", "↓", "=/>", "(!A)", "</=", "(!B)", "⨁", "|", "&", "==", "(B)", "<=", "(A)", "=>", "+", "1", };

		abstract public byte Eval();
		abstract new public string ToString();
		abstract public BooleanExpression Clone();
		abstract public int getSize();

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public bool Equals(BooleanExpression obj)
		{
			return ToString() == obj.ToString();
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

		public override int getSize()
		{
			return Left.getSize() + Right.getSize() + 1;
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
			case BooleanOperation.CoImp:
				res = aVal & ~bVal;
				break;
			case BooleanOperation.NotA:
				res = ~aVal;
				break;
			case BooleanOperation.BCoImp:
				res = ~aVal & bVal;
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
			case BooleanOperation.BImp:
				res = aVal | ~bVal;
				break;
			case BooleanOperation.A:
				res = aVal;
				break;
			case BooleanOperation.Imp:
				res = ~aVal | bVal;
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

		public override int getSize()
		{
			return 0;
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

