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

		static int[] ConsoleInput()
		{
			return new int[] { -1, -1, -1, -1, -1, -1, -1, -1, -1, }; // TODO: Sdielat
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

		public static void Solve(int[] coefs)
		{
			Console.WriteLine(PrintEquation(coefs));

			int eqAnswer = AndXorEquation.SolveEq(coefs);

			Console.WriteLine("\nОтвет: " + eqAnswer);
		}

		public static void Main(string[] args)
		{
			int[] coefs = { -1, -1, -1, -1, -1, -1, -1, -1, -1, };	

			try {
				coefs = ReadArgs(args);

				Solve(coefs);

				throw new DivideByZeroException("Матибал");
			}
			catch(ArgumentNullException) {
				coefs = ConsoleInput();

				Solve(coefs);
			}
			catch(FormatException e) {
				Console.WriteLine("Osheebka: " + e.Message);
			}
			catch(Exception e) { // TODO: Разобраться с экссепшенами
				Console.WriteLine("Непойманное исключение: " + e.Message);
				Console.WriteLine("\n" + e.StackTrace);
			}
		}
	}
}

