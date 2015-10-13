using System;

namespace SolveAvdoshin
{
	public class SolveAvdoshin
	{
		static string PrintEquation(int[] coefs)
		{
			string result = "";

			for(int i = 0; i < 9; i++) {
				result += i == 8 ? " = " : i == 0 ? "" : " ⨁ ";

				if(coefs[i] == -1)
					result += "___";
				else
					result += coefs[i].ToString("D");

				result += i == 8 ? "" : " x" + (7 - i).ToString("D");
			}

			return result;
		}

		static int ReadInt(string msg, Func<int, bool> constraint)
		{
			int res;

			Console.WriteLine("(ввести целое число от 0 до 255)");

			Console.Write(msg + ": ");

			while(!int.TryParse(Console.ReadLine(), out res) || !constraint(res)) {
				Console.Write("Хуйню ввел. Заново: ");
			}

			return res;
		}

		static int[] ConsoleInput()
		{
			//return new int[] { 220, 160, 85, 253, 210, 159, 103, 101, 72, };

			int[] coefs = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, };

			Console.Clear();

			for(int i = 0; i < 9; i++) {
				Console.WriteLine(PrintEquation(coefs) + "\n");

				coefs[i] = ReadInt("Вводи епт", x => 0 <= x && x <= 255);

				Console.Clear();
			}

			return coefs;
		}

		static int[] ReadArgs(string[] args)
		{
			int[] coefs = new int[9];

			if(args.Length == 0) {
				throw new ArgumentNullException();
			}
			else if(args.Length == 9) {

				for(int i = 0; i < 9; i++) {
					coefs[i] = int.Parse(args[i]);

					if(coefs[i] < 0 || coefs[i] > 255)
						throw new FormatException("Все аргументы должны быть целыми числами от 0 до 255");
				}
			}
			else {
				throw new FormatException("Аргументов то ли многовато, то ли маловато. Надо ровно 9");
			}

			return coefs;
		}

		static int Solve(int[] coefs, bool verbose)
		{
			Console.WriteLine(PrintEquation(coefs));

			int eqAnswer = AndXorEquation.SolveEq(coefs, verbose);

			return eqAnswer;
		}

		enum ExecutionMode { Interactive, CommandLine, NoEquation, };

		static bool ProcessCommandLineArgs(string[] args, out ExecutionMode? mode, out int a, out int b,
			out int[] coefs, out int n, out bool minOps, out bool showBlocks, out bool noLookBack, out bool verboseSystem)
		{
			coefs = new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, };
			mode = null;
			a = b = n = -1;
			minOps = showBlocks = noLookBack = verboseSystem = false;

			for(int i = 0; i < args.Length; i++) {
				switch(args[i]) {
				case "-i":
					if(mode != null)
						return false;
					
					mode = ExecutionMode.Interactive;

					break;

				case "-p":
					if(mode != null)
						return false;
					
					mode = ExecutionMode.CommandLine;

					if(args.Length - i - 1 < 9)
						return false;

					for(int j = 0; j < 9; j++) {
						if(!int.TryParse(args[i + j + 1], out coefs[j]))
							return false;
						
						if(!(0 <= coefs[j] && coefs[j] <= 255))
							return false;
					}

					i += 9;

					break;

				case "-f":
					if(mode != null)
						return false;
					
					mode = ExecutionMode.NoEquation;

					if(args.Length - i - 1 < 1)
						return false;
					
					if(!int.TryParse(args[i + 1], out n))
						return false;

					if(!(0 <= n && n <= 255))
						return false;

					i += 1;
					
					break;

				case "-ex":
					if(a != -1)
						return false;

					a = 0;

					if(args.Length - i - 1 < 1)
						return false;

					if(args[i + 1].Contains("-")) {
						string[] ab = args[i + 1].Split('-');

						if(!int.TryParse(ab[0], out a) || !int.TryParse(ab[1], out b))
							return false;

						if(!(0 <= a && a <= 255) || !(0 <= b && b <= 255))
							return false;
					}
					else {
						if(!int.TryParse(args[i + 1], out a))
							return false;

						if(!(0 <= a && a <= 255))
							return false;
					}

					i += 1;

					break;
				
				case "-mo":
					if(minOps)
						return false;

					minOps = true;

					break;

				case "-sb":
					if(showBlocks)
						return false;

					showBlocks = true;

					break;

				case "-vs":
					if(verboseSystem)
						return false;

					verboseSystem = true;

					break;

				case "-nlb":
					if(noLookBack)
						return false;

					noLookBack = true;

					break;

				default:
					int tmp;
					if(!int.TryParse(args[i], out tmp))
						return false;
					break;
				}
			}

			if(a == -1) {
				a = 1;
				b = 15;
			}

			return mode != null;
		}

		static void PrintAnswers(int n, int a, int b, bool minOps, bool showBlocks, bool noLookBack)
		{
			if(b == 1)
				return;

			var functions = new Action<int>[] {
				BooleanFunctions.PrintDerivatives, 
				BooleanFunctions.PrintExpressionsForDerivatives, 
				BooleanFunctions.Print2DirectionalDerivatives, 
				BooleanFunctions.PrintExpressionsFor2DirDerivatives, 
				BooleanFunctions.Print3DirectionalDerivative, 
				BooleanFunctions.PrintExpressionFor3DirDerivative, 
				BooleanFunctions.PrintMaclauren1XorAnd, 
				BooleanFunctions.PrintTailor1XorAnd, 
				BooleanFunctions.PrintMaclauren0EqOr, 
				BooleanFunctions.PrintTailor0EqOr, 
				BooleanFunctions.PrintClosedClasses, 
			};

			a = Math.Max(a, 3);
			b = Math.Max(b, 3);

			if(a <= 3) {
				Console.WriteLine("\n2-3.\n");

				BooleanFunctions.PrintMinimaInAvdoshinBases(n, minOps, showBlocks);
			}

			for(int i = Math.Max(a, 4); i <= Math.Min(b, 14); i++) {
				Console.WriteLine("\n" + i + ".\n");

				functions[i - 4](n);
			}

			if(b >= 15) {
				Console.WriteLine("\n15.\n");

				BooleanFunctions.PrintRepresentBinariesInF(n, noLookBack);
			}
		}

		public static void Main(string[] args)
		{
			int[] coefs = { -1, -1, -1, -1, -1, -1, -1, -1, -1, };

			ExecutionMode? mode;
			int a, b;
			int n;
			bool minOps, showBlocks, noLookBack, verboseSystem;

			if(ProcessCommandLineArgs(args, out mode, out a, out b, out coefs, out n, out minOps, out showBlocks,
										out noLookBack, out verboseSystem)) {
				switch(mode) {
				case ExecutionMode.Interactive:
					coefs = ConsoleInput();

					goto case ExecutionMode.CommandLine;

				case ExecutionMode.CommandLine:
					Console.WriteLine("\n1.\n");

					n = Solve(coefs, verboseSystem);

					Console.WriteLine("\nОтвет: " + n + "\n");

					break;
				case ExecutionMode.NoEquation:
					Console.WriteLine("\n1.\n\nДана функция: " + n + "\n");

					break;
				}

				if(a == -1 && b == -1)
					PrintAnswers(n, 3, 15, minOps, showBlocks, noLookBack);
				else if(a != -1 && b == -1)
					PrintAnswers(n, a, a, minOps, showBlocks, noLookBack);
				else
					PrintAnswers(n, a, b, minOps, showBlocks, noLookBack);

				Console.WriteLine();
			}
			else {
				Console.WriteLine(
					"usage: SolveAvdoshin [<options>] [<mode> ...]\n" +
					"\t-i\t\t\tввод коэффициентов ур-я с клавиатуры\n" +
					"\t-p <a7> ... <a1> <b>\tввод коэффициентов ур-я (чисел 9 штук)\n" +
					"\t-f <n>\t\t\tввод сразу номера функции\n" +
					"\n" +
					"\t-ex <a>\t\t\tрешать только <a>-е задание\n" +
					"\t-ex <a>-<b>\t\tрешать задания с <a> по <b> включительно\n" +
					"\t\t\t\t(прим.: всего заданий 15)\n\n" +
					"\t\tдля 1-го задания:\n" +
					"\t-vs \t\t\tотображать все этапы решения системы\n\n" +
					"\t\tдля 2-3 заданий:\n" +
					"\t-mo \t\t\tминимизировать выражения по кол-ву операций\n" +
					"\t-sb \t\t\tпоказывать кол-во блоков и сами блоки\n\n" +
					"\t\tдля 15-го задания:\n" +
					"\t-nlb \t\t\tне использовать при поиске выражений предыдущие\n" +
					"\t\t\t\tнайденные выражения\n");
			}
		}
	}
}

