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
				BooleanOperation.One,
				BooleanOperation.NotA,
				BooleanOperation.NotB,
				BooleanOperation.NOR,
				BooleanOperation.CoImp,
				BooleanOperation.BCoImp,
				BooleanOperation.Xor,
				BooleanOperation.NAND,
				BooleanOperation.And,
				BooleanOperation.Eq,
				BooleanOperation.B,
				BooleanOperation.BImp,
				BooleanOperation.A,
				BooleanOperation.Imp,
				BooleanOperation.Or,
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
					var ex = BooleanExpression.FindMininalExpressionInBasis(n, AvdoshinBases[i], AvdoshinVars[i]);

					Console.WriteLine(ex.ToString());
				}
				catch(CouldntFindExpressionException) {
					Console.WriteLine("не нашлось");
				}
			}
		}

		public static void PrintAllDerivatives(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			DerivativePrinting.PrintDerivatives(f);

			DerivativePrinting.PrintExpressionsForDerivatives(f, AvdoshinBases[0], AvdoshinVars[0]);

			DerivativePrinting.Print2DirectionalDerivatives(f);

			DerivativePrinting.PrintExpressionsFor2DirDerivatives(f, AvdoshinBases[0], AvdoshinVars[0]);

			DerivativePrinting.Print3DirectionalDerivative(f);

			DerivativePrinting.PrintExpressionFor3DirDerivative(f, AvdoshinBases[0], AvdoshinVars[0]);
		}

		public static void PrintAllRepresentations(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			RepresentationPrinting.PrintMaclauren1XorAnd(f);

			RepresentationPrinting.PrintTailor1XorAnd(f);

			RepresentationPrinting.PrintMaclauren0EqOr(f);

			RepresentationPrinting.PrintTailor0EqOr(f);
		}
	}

	static class DerivativePrinting
	{
		static readonly BooleanVariable[][] VarLists = new BooleanVariable[][] {
			new BooleanVariable[] { BooleanVariable.A, },
			new BooleanVariable[] { BooleanVariable.B, },
			new BooleanVariable[] { BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, },
			new BooleanVariable[] { BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
		};

		static readonly string[] DerivStrings = new string[] {
			"F'_A",
			"F'_B",
			"F'_C",
			"F''_AB",
			"F''_BC",
			"F''_AC",
			"F'''_ABC",
		};

		static readonly string[] DirDerivStrings = new string[] {
			"F'_A",
			"F'_B",
			"F'_C",
			"F'_(A,B)",
			"F'_(B,C)",
			"F'_(A,C)",
			"F'_(A,B,C)",
		};

		public static void PrintJustTruthTable(BooleanFunction f)
		{
			Console.WriteLine((new BooleanFunction(0x0F)).ToString("A"));
			Console.WriteLine((new BooleanFunction(0x33)).ToString("B"));
			Console.WriteLine((new BooleanFunction(0x55)).ToString("C"));

			Console.WriteLine(f.ToString("F"));
		}

		public static void PrintDerivatives(BooleanFunction f)
		{
			PrintJustTruthTable(f);

			for(int i = 0; i < 7; i++) {
				Console.WriteLine(f.Deriv(VarLists[i]).ToString(DerivStrings[i]));
			}

			Console.WriteLine();
		}

		public static void Print2DirectionalDerivatives(BooleanFunction f)
		{
			PrintJustTruthTable(f);

			for(int i = 3; i <= 5; i++) {
				Console.WriteLine(f.DirectionalDeriv(VarLists[6]).ToString(DirDerivStrings[i]));
			}

			Console.WriteLine();
		}

		public static void Print3DirectionalDerivative(BooleanFunction f)
		{
			PrintJustTruthTable(f);

			Console.WriteLine(f.DirectionalDeriv(VarLists[6]).ToString(DirDerivStrings[6]));

			Console.WriteLine();
		}

		public static void PrintExpressionsForDerivatives(BooleanFunction f, BooleanOperation[] ops, BooleanVariable[] vars)
		{
			for(int i = 0; i < 7; i++) {
				Console.WriteLine(DerivStrings[i] + " = " + BooleanExpression.FindMininalExpressionInBasis(
					f.Deriv(VarLists[i]).Eval(), ops, vars).ToString());
			}

			Console.WriteLine();
		}

		public static void PrintExpressionsFor2DirDerivatives(BooleanFunction f, BooleanOperation[] ops, BooleanVariable[] vars)
		{
			for(int i = 3; i <= 5; i++) {
				Console.WriteLine(DirDerivStrings[i] + " = " + BooleanExpression.FindMininalExpressionInBasis(f.DirectionalDeriv(VarLists[i]).Eval(), ops, vars).ToString());
			}

			Console.WriteLine();
		}

		public static void PrintExpressionFor3DirDerivative(BooleanFunction f, BooleanOperation[] ops, BooleanVariable[] vars)
		{
			Console.WriteLine(DirDerivStrings[6] + " = " + BooleanExpression.FindMininalExpressionInBasis(
				f.DirectionalDeriv(VarLists[6]).Eval(), ops, vars).ToString());

			Console.WriteLine();
		}
	}

	static class RepresentationPrinting
	{
		static readonly string[] ReprStrings = new string[] {
			"(0,0,0)",
			"(0,0,1)",
			"(0,1,0)",
			"(0,1,1)",
			"(1,0,0)",
			"(1,0,1)",
			"(1,1,0)",
			"(1,1,1)",
		};

		public static void PrintMaclauren1XorAnd(BooleanFunction f)
		{
			Console.WriteLine("F(A,B,C) = " + 
				f.GetTailor(0, BooleanVariable.One, BooleanOperation.Xor, BooleanOperation.And));

			Console.WriteLine();
		}

		public static void PrintTailor1XorAnd(BooleanFunction f)
		{
			for(int i = 0; i < 8; i++) {
				Console.WriteLine(ReprStrings[i] + ": F(A,B,C) = " +
					f.GetTailor(i, BooleanVariable.One, BooleanOperation.Xor, BooleanOperation.And));
			}

			Console.WriteLine();
		}

		public static void PrintMaclauren0EqOr(BooleanFunction f)
		{
			Console.WriteLine("F(A,B,C) = " + 
				f.GetTailor(0, BooleanVariable.Zero, BooleanOperation.Eq, BooleanOperation.Or));

			Console.WriteLine();
		}

		public static void PrintTailor0EqOr(BooleanFunction f)
		{
			for(int i = 0; i < 8; i++) {
				Console.WriteLine(ReprStrings[i] + ": F(A,B,C) = " +
					f.GetTailor(i, BooleanVariable.Zero, BooleanOperation.Eq, BooleanOperation.Or));
			}

			Console.WriteLine();
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

		public BooleanExpression GetTailor(int point, BooleanVariable one, BooleanOperation xor, BooleanOperation and)
		{
			return new OpExpression(BooleanOperation.NOR, BooleanVariable.A, BooleanVariable.B);
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
				
	class CouldntFindExpressionException : Exception
	{

	}

	enum BooleanOperation { Zero, NOR, CoImp, NotA, BCoImp, NotB, Xor, NAND, And, Eq, B, BImp, A, Imp, Or, One };
	enum BooleanVariable { A, B, C, Zero, One, };

	abstract class BooleanExpression
	{
		public static readonly string[] OpSymbols = new string[] { "0", "↓", "=/>", "(!A)", "</=", "(!B)", "⨁", "|", "&", "==", "(B)", "<=", "(A)", "=>", "+", "1", };

		abstract public byte Eval();
		abstract override public string ToString();
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

		public static BooleanExpression FindMininalExpressionInBasis(int n, BooleanOperation[] ops, BooleanVariable[] vars)
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

