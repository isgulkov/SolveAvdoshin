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

		public static void PrintMinimaInAvdoshinBases(int n, bool minOps)
		{
			Console.WriteLine("\nМинимальные представления f_" + n + " в базисах (для рисования в винлогике):");

			Console.WriteLine(" ☻   Базис             Выражение");
			Console.WriteLine();

			for(int i = 0; i < AvdoshinBases.Length; i++) {
				Console.Write("{0} — {1}", i.ToString("D2"), AvdoshinNames[i].PadRight(18));

				try {
					var ex = BooleanExpression.FindMininalExpressionInBasis(n, AvdoshinBases[i], AvdoshinVars[i],
					mode: minOps ? BooleanExpression.ExpressionSearchMode.CountOps :
						BooleanExpression.ExpressionSearchMode.CountBlocks);

					Console.WriteLine(ex.ToString());
				}
				catch(CouldntFindExpressionException) {
					Console.WriteLine("не нашлось");
				}
			}
		}

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

		static void PrintJustTruthTable(BooleanFunction f)
		{
			Console.WriteLine((new BooleanFunction(0x0F)).ToString("A"));
			Console.WriteLine((new BooleanFunction(0x33)).ToString("B"));
			Console.WriteLine((new BooleanFunction(0x55)).ToString("C"));

			Console.WriteLine(f.ToString("F"));
		}

		public static void PrintDerivatives(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			PrintJustTruthTable(f);

			for(int i = 0; i < 7; i++) {
				Console.WriteLine(f.Deriv(VarLists[i]).ToString(DerivStrings[i]));
			}

			Console.WriteLine();
		}

		public static void Print2DirectionalDerivatives(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			PrintJustTruthTable(f);

			for(int i = 3; i <= 5; i++) {
				Console.WriteLine(f.DirectionalDeriv(VarLists[i]).ToString(DirDerivStrings[i]));
			}

			Console.WriteLine();
		}

		public static void Print3DirectionalDerivative(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			PrintJustTruthTable(f);

			Console.WriteLine(f.DirectionalDeriv(VarLists[6]).ToString(DirDerivStrings[6]));

			Console.WriteLine();
		}

		public static void PrintExpressionsForDerivatives(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			for(int i = 0; i < 7; i++) {
				Console.WriteLine(DerivStrings[i] + " = " + BooleanExpression.FindMininalExpressionInBasis(
					f.Deriv(VarLists[i]).Eval(), AvdoshinBases[0], AvdoshinVars[0]).ToString());
			}

			Console.WriteLine();
		}

		public static void PrintExpressionsFor2DirDerivatives(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			for(int i = 3; i <= 5; i++) {
				Console.WriteLine(DirDerivStrings[i] + " = " + BooleanExpression.FindMininalExpressionInBasis(
					f.DirectionalDeriv(VarLists[i]).Eval(), AvdoshinBases[0], AvdoshinVars[0]).ToString());
			}

			Console.WriteLine();
		}

		public static void PrintExpressionFor3DirDerivative(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			Console.WriteLine(DirDerivStrings[6] + " = " + BooleanExpression.FindMininalExpressionInBasis(
				f.DirectionalDeriv(VarLists[6]).Eval(), AvdoshinBases[0], AvdoshinVars[0]).ToString());

			Console.WriteLine();
		}

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

		public static void PrintMaclauren1XorAnd(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			Console.WriteLine("F(A,B,C) = " + f.GetTailorStringXor(0));

			Console.WriteLine();
		}

		public static void PrintTailor1XorAnd(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			for(int i = 0; i < 8; i++) {
				Console.WriteLine(ReprStrings[i] + ": F(A,B,C) = " + f.GetTailorStringXor(i));
			}

			Console.WriteLine();
		}

		public static void PrintMaclauren0EqOr(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			Console.WriteLine("F(A,B,C) = " + f.GetTailorStringEq(7));

			Console.WriteLine();
		}

		public static void PrintTailor0EqOr(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			for(int i = 0; i < 8; i++) {
				Console.WriteLine(ReprStrings[i] + ": F(A,B,C) = " + f.GetTailorStringEq(i));
			}

			Console.WriteLine();
		}

		public static void PrintClosedClasses(int n)
		{
			BooleanFunction f = new BooleanFunction((byte)n);

			PostClosedClasses.CheckConservationZero(f, true);

			PostClosedClasses.CheckConservationOne(f, true);

			PostClosedClasses.CheckSelfDuality(f, true);

			PostClosedClasses.CheckMonotony(f, true);

			PostClosedClasses.CheckLinearity(f, true);

			Console.WriteLine();
		}

		static readonly Tuple<BooleanOperation, bool[]>[] BinariesAndClasses = {
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.Zero, new bool[5] { true, false, false, true, true, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.And, new bool[5] { true, true, false, true, false, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.CoImp, new bool[5] { true, false, false, false, false, }),
//			new Tuple<BooleanOperation, bool[]>(BooleanOperation.A, new bool[5] { false, false, false, false, false, } ),
//			new Tuple<BooleanOperation, bool[]>(BooleanOperation.BCoImp, new bool[5] { false, false, false, false, false, } ),
//			new Tuple<BooleanOperation, bool[]>(BooleanOperation.B, new bool[5] { false, false, false, false, false, } ),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.Xor, new bool[5] { true, false, false, false, true, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.Or, new bool[5] { true, true, false, true, false, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.NOR, new bool[5] { false, false, false, false, false, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.Eq, new bool[5] { false, true, false, false, true, }),
//			new Tuple<BooleanOperation, bool[]>(BooleanOperation.NotB, new bool[5] { false, false, false, false, false, } ),
//			new Tuple<BooleanOperation, bool[]>(BooleanOperation.BImp, new bool[5] { false, false, false, false, false, } ),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.NotA, new bool[5] { false, false, true, false, true, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.Imp, new bool[5] { false, true, false, false, false, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.NAND, new bool[5] { false, false, false, false, false, }),
			new Tuple<BooleanOperation, bool[]>(BooleanOperation.One, new bool[5] { false, true, false, true, true, }),
		};

		public static void PrintRepresentBinariesInF(int n)
		{
			bool[] functionClasses = PostClosedClasses.GetFunctionClasses(new BooleanFunction((byte)n));

			foreach(var t in BinariesAndClasses) {
				bool representable = true;

				for(int i = 0; i < 5; i++) {
					if(functionClasses[i] && !t.Item2[i]) {
						representable = false;
						break;
					}
				}

				if(!representable)
					continue;

				Console.Write((new OpExpression(t.Item1, BooleanVariable.A, BooleanVariable.B)).ToString() + " = ");

				try {
					Console.WriteLine(TertiaryOpExpression.FindMininalExpressionForBinary(t.Item1,
						new BooleanFunction((byte)n)));
				}
				catch(CouldntFindExpressionException) {
					Console.WriteLine("не нашлось");
				}
			}
		}
	}

	class TertiaryOpExpression : BooleanExpression
	{
		BooleanFunction F;
		BooleanExpression First, Second, Third;

		public TertiaryOpExpression(BooleanFunction f, BooleanExpression first, BooleanExpression second, BooleanExpression third)
		{
			F = f;
			First = first;
			Second = second;
			Third = third;
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("F({0},{1},{2})", First.ToString(), Second.ToString(), Third.ToString());
		}

		public override int CountOps()
		{
			return 1 + First.CountOps() + Second.CountOps() + Third.CountOps();
		}

		public override BooleanExpression Clone()
		{
			return new TertiaryOpExpression(F, First, Second, Third);
		}

		public override byte Eval()
		{
			int res = 0;

			for(int i = 0; i < 8; i++) {
				res *= 2;
				res += F.EvalAt(First.EvalAt(i) * 4 + Second.EvalAt(i) * 2 + Third.EvalAt(i));
			}

			return (byte)res;
		}

		public override HashSet<string> GetSetOfAllBlockStrings()
		{
			throw new NotImplementedException();
		}

		static public IEnumerable<TertiaryOpExpression> CombineTertiary(BooleanFunction f,
			BooleanExpression firstExpression, BooleanExpression anotherExpression, BooleanExpression thirdExpression)
		{
			yield return new TertiaryOpExpression(f, firstExpression.Clone(), anotherExpression.Clone(),
				thirdExpression.Clone());
			yield return new TertiaryOpExpression(f, firstExpression.Clone(), thirdExpression.Clone(),
				anotherExpression.Clone());
			yield return new TertiaryOpExpression(f, anotherExpression.Clone(), firstExpression.Clone(),
				thirdExpression.Clone());
			yield return new TertiaryOpExpression(f, thirdExpression.Clone(), firstExpression.Clone(),
				anotherExpression.Clone());
			yield return new TertiaryOpExpression(f, anotherExpression.Clone(), thirdExpression.Clone(),
				firstExpression.Clone());
			yield return new TertiaryOpExpression(f, thirdExpression.Clone(), anotherExpression.Clone(),
				firstExpression.Clone());
		}

		public static BooleanExpression FindMininalExpressionForBinary(BooleanOperation binary, BooleanFunction f)
		{
			var queue = new ImprovisedPriorityQueue<BooleanExpression>(20);
			var knownTruthTables = new HashSet<byte>();
			var knownExpressions = new HashSet<BooleanExpression>();

			var targetExpression = new OpExpression(binary, BooleanVariable.A, BooleanVariable.B);

			foreach(var variable in new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, }) {
				queue.TryEnqueue(new VarExpression(variable), 1);
			}

			while(queue.Count != 0) {
				var curExperession = queue.Dequeue();

				byte truthTable = curExperession.Eval();

				if(knownTruthTables.Contains(truthTable)) {
					continue;
				}

				if((curExperession.Eval() & 0xAA) == (targetExpression.Eval() & 0xAA)) {
					return curExperession;
				}

				knownExpressions.Add(curExperession);
				knownTruthTables.Add(truthTable);

				foreach(var anotherExpression in knownExpressions) {
					foreach(var thirdExpression in knownExpressions) {
						foreach(var neighbourExpression in CombineTertiary(f, curExperession, anotherExpression, thirdExpression)) {
							queue.TryEnqueue(neighbourExpression, neighbourExpression.CountOps());
						}
					}
				}
			}

			throw new CouldntFindExpressionException();
		}
	}

	static class PostClosedClasses
	{
		public static bool[] GetFunctionClasses(BooleanFunction f)
		{
			return new bool[] { CheckConservationZero(f), CheckConservationOne(f), CheckSelfDuality(f), CheckMonotony(f), CheckLinearity(f), };
		}

		public static bool CheckConservationZero(BooleanFunction f, bool verbose = false)
		{
			if(f.EvalAt(0) == 0) {
				if(verbose)
					Console.WriteLine("F ∈ T_0, т.к. F(0,0,0) = 0");
				
				return true;
			}
			else {
				if(verbose)
					Console.WriteLine("F ∉ T_0, т.к. F(0,0,0) = 1");
				
				return false;
			}
		}

		public static bool CheckConservationOne(BooleanFunction f, bool verbose = false)
		{
			if(f.EvalAt(7) == 1) {
				if(verbose)
					Console.WriteLine("F ∈ T_1, т.к. F(1,1,1) = 1");
				
				return true;
			}
			else {
				if(verbose)
					Console.WriteLine("F ∉ T_1, т.к. F(1,1,1) = 0");
				
				return false;
			}
		}

		static string PointAsString(int a)
		{
			return ((a >> 2) & 1) + "," + ((a >> 1) & 1) + "," + (a & 1);
		}

		public static bool CheckSelfDuality(BooleanFunction f, bool verbose = false)
		{
			for(int i = 0; i < 8; i++) {
				if(f.EvalAt(i) == f.EvalAt(7 - i)) {
					if(verbose)
						Console.WriteLine("F ∉ T_*, т.к. F({0}) = F({1}) = {2}", PointAsString(i), PointAsString(7 - i), f.EvalAt(i));
					
					return false;
				}
			}

			if(verbose)
				Console.WriteLine("F ∈ T_*, т.к. я на всех наборах проверял, честно!");
			
			return true;
		}

		static bool AreAdjacent(int a, int b)
		{
			bool res = false;

			for(int i = 0; i < 3; i++) {
				if(((a >> i) & 1) != ((b >> i) & 1)) {
					if(!res)
						res = true;
					else {
						res = false;
						break;
					}
				}
			}

			return res;
		}

		static bool IsLessThan(int a, int b)
		{
			int onesA = 0, onesB = 0;

			for(int i = 0; i < 3; i++) {
				onesA += (a >> i) & 1;
				onesB += (b >> i) & 1;
			}

			return onesA < onesB;
		}

		public static bool CheckMonotony(BooleanFunction f, bool verbose = false)
		{
			for(int i = 0; i < 8; i++) {
				for(int j = 0; j < 8; j++) {
					if(!AreAdjacent(i, j) || !IsLessThan(i, j))
						continue;
					
					if(f.EvalAt(i) > f.EvalAt(j)) {
						if(verbose)
							Console.WriteLine("F ∉ T_<=, т.к. F({0}) > F({1})", PointAsString(i), PointAsString(j));
						
						return false;
					}
				}
			}

			if(verbose)
				Console.WriteLine("F ∈ T_<=, т.к. я на всех наборах проверял, честно!");
			
			return true;
		}

		public static bool CheckLinearity(BooleanFunction f, bool verbose = false)
		{
			string zhegalkinRep = f.GetTailorStringXor(0);

			foreach(string s in new string[] { "AB", "BC", "AC", "ABC", }) {
				if(zhegalkinRep.Contains(s)) {
					if(verbose)
						Console.WriteLine("F ∉ T_L, т.к. F(A,B,C) = " + zhegalkinRep);
					
					return false;
				}
			}

			if(verbose)
				Console.WriteLine("F ∈ T_L, т.к. F(A,B,C) = " + zhegalkinRep);
			
			return true;
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

		public int EvalAt(int n)
		{
			return (TruthTable >> (7 - n)) & 1;
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

		static readonly BooleanVariable[][] VarLists = new BooleanVariable[][] {
			new BooleanVariable[] { },
			new BooleanVariable[] { BooleanVariable.A, },
			new BooleanVariable[] { BooleanVariable.B, },
			new BooleanVariable[] { BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, },
			new BooleanVariable[] { BooleanVariable.B, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.C, },
			new BooleanVariable[] { BooleanVariable.A, BooleanVariable.B, BooleanVariable.C, },
		};

		static readonly string[][] StringLists = new string[][] {
			new string[] { },
			new string[] { "A", },
			new string[] { "B", },
			new string[] { "C", },
			new string[] { "A", "B", },
			new string[] { "B", "C", },
			new string[] { "A", "C", },
			new string[] { "A", "B", "C", },
		};

		bool VariablePresentAt(int point, string variable)
		{
			switch(variable) {
			case "A":
				return ((point >> 2) & 1) == 1;
			case "B":
				return ((point >> 1) & 1) == 1;
			case "C":
				return (point & 1) == 1;
			default:
				throw new ArgumentException("VariablePresentAt only works for BooleanVariables A, B and C");
			}
		}

		public string GetTailorStringXor(int point)
		{
			List<string> outerTerms = new List<string>();

			if(((Eval() >> (7 - point)) & 1) == 1)
				outerTerms.Add("1");

			for(int i = 1; i < 8; i++) { // terms
				if(((Deriv(VarLists[i]).Eval() >> (7 - point)) & 1) == 1) {
					List<string> factors = new List<string>();

					foreach(var variable in StringLists[i]) {
						if(VariablePresentAt(point, variable))
							factors.Add("(" + variable + " ⊕ 1)");
						else
							factors.Add(variable);
					}

					outerTerms.Add(String.Join("", factors));
				}
			}

			return String.Join(" ⊕ ", outerTerms);
		}

		public string GetTailorStringEq(int point)
		{
			List<string> outerTerms = new List<string>();

			if(((Eval() >> (7 - point)) & 1) == 0)
				outerTerms.Add("0");

			for(int i = 1; i < 8; i++) { // terms
				if(((Deriv(VarLists[i]).Eval() >> (7 - point)) & 1) == 1) {
					List<string> factors = new List<string>();

					foreach(var variable in StringLists[i]) {
						if(!VariablePresentAt(point, variable))
							factors.Add("(" + variable + " ≡ 0)");
						else
							factors.Add(variable);
					}

					if(factors.Count == 1)
						outerTerms.Add(String.Join(" + ", factors));
					else
						outerTerms.Add("(" + String.Join(" + ", factors) + ")");
				}
			}

			return String.Join(" ≡ ", outerTerms);
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
		public static readonly string[] OpSymbols = new string[] { "0", "↓", "=/>", "(!A)", "</=", "(!B)", "⨁", "|", "&", "≡", "(B)", "<=", "(A)", "=>", "+", "1", };

		abstract public byte Eval();
		abstract override public string ToString();
		abstract public BooleanExpression Clone();
		abstract public int CountOps();

		public int EvalAt(int i)
		{
			return (Eval() >> (7 - i)) & 1;
		}

		public override int GetHashCode()
		{
			return ToString().GetHashCode();
		}

		public bool Equals(BooleanExpression obj)
		{
			return ToString() == obj.ToString();
		}

		public abstract HashSet<string> GetSetOfAllBlockStrings();

		public int CountBlocks()
		{
			var allBlocks = GetSetOfAllBlockStrings();

			return allBlocks != null ? allBlocks.Count : 0;
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

		public enum ExpressionSearchMode { CountOps, CountBlocks, };
		public static BooleanExpression FindMininalExpressionInBasis(int n, BooleanOperation[] ops,
			BooleanVariable[] vars, ExpressionSearchMode mode = ExpressionSearchMode.CountOps)
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
						queue.TryEnqueue(neighbourExpression, mode == ExpressionSearchMode.CountBlocks
							? neighbourExpression.CountBlocks() : neighbourExpression.CountOps());
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

		public override int CountOps()
		{
			return Left.CountOps() + Right.CountOps() + 1;
		}

		public override HashSet<string> GetSetOfAllBlockStrings()
		{
			var allBlocks = new HashSet<string>();
			allBlocks.Add(ToString());

			var leftBlocks = Left.GetSetOfAllBlockStrings();
			if(leftBlocks != null)
				allBlocks.UnionWith(leftBlocks);

			var rightBlocks = Right.GetSetOfAllBlockStrings();
			if(rightBlocks != null)
				allBlocks.UnionWith(rightBlocks);

			return allBlocks;
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

		public override int CountOps()
		{
			return 0;
		}

		public override HashSet<string> GetSetOfAllBlockStrings()
		{
			return null;
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

