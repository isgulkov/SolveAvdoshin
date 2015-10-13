using System;

namespace SolveAvdoshin
{
	public class SystemNonConsistentException : Exception
	{

	}

	public class AndXorEquation
	{
		public static int[] ConvertToBinary(int n)
		{
			int[] result = new int[(int)Math.Log(n, 2) + 1];
			int lastIndex = 0;

			while(n != 0) {
				result[lastIndex++] = n % 2;
				n /= 2;
			}

			return result;
		}

		public static int SolveEq(int[] coefs, bool verbose = false)
		{
			if(coefs.Length != 9)
				throw new ArgumentException("Input array shold be 9 elements long");

			int[,] binaryCoefs = new int[8, 9];

			if(verbose)
				Console.WriteLine("");

			for(int i = 0; i <= 8; i++) {
				int[] binary = ConvertToBinary(coefs[i]);

				if(verbose) {
					Console.Write(coefs[i] + "_10 = ");

					for(int j = 7 - binary.Length + 1; j < 8; j++)
						Console.Write(binary[7 - j]);

					Console.WriteLine("_2");
				}

				for(int j = 0; j < binary.Length; j++) {
					binaryCoefs[7 - j, i] = binary[j];
				}
			}

			var matrix = new AndXorSystem(binaryCoefs);

			try {
				return matrix.Solve(verbose);
			}
			catch(SystemNonConsistentException) {
				return -1;
			}
		}
	}

	public class AndXorSystem
	{
		int NumRows;
		int[,] Matrix;

		public AndXorSystem(int[,] matrix)
		{
			if(matrix.GetLength(0) + 1 != matrix.GetLength(1))
				throw new ArgumentException("The array supplied isn't an Nx(N+1) matrix");

			NumRows = matrix.GetLength(0);

			for(int i = 0; i < NumRows; i++) {
				for(int j = 0; j < NumRows + 1; j++) {
					if(matrix[i, j] != 0 && matrix[i, j] != 1)
						throw new ArgumentException("The array supplied contains non-binary elements");
				}
			}

			Matrix = matrix;
		}

		public override string ToString()
		{
			string result = "";

			for(int i = 0; i < NumRows; i++) {
				result += "( ";

				for(int j = 0; j < NumRows + 1; j++) {
					result += Matrix[i, j].ToString("D") + (j == 7 ? " | " : " ");
				}

				result += ")\n";
			}

			return result;
		}

		void SwapRows(int a, int b)
		{
			if(a < 0 || a >= NumRows || b < 0 || b >= NumRows)
				throw new ArgumentOutOfRangeException();

			for(int i = 0; i < NumRows + 1; i++) {
				int t = Matrix[a, i];
				Matrix[a, i] = Matrix[b, i];
				Matrix[b, i] = t;
			}
		}

		void XorRowIntoAnother(int a, int b)
		{
			if(a < 0 || a >= NumRows || b < 0 || b >= NumRows)
				throw new ArgumentOutOfRangeException();

			for(int i = 0; i < NumRows + 1; i++) {
				Matrix[b, i] ^= Matrix[a, i];
			}
		}

		void MakeCanonical(bool verbose)
		{
			// Реализуется алгоритм Гаусса, как Чернышев завещал

			int curRow = 0, curCol = 0;

			if(verbose)
				Console.WriteLine("\n" + this);

			while(curRow < NumRows && curCol < NumRows) {
				if(Matrix[curRow, curCol] != 0) {
					for(int i = 0; i < NumRows; i++) {
						if(i == curRow)
							continue;

						if(Matrix[i, curCol] == 1) {
							XorRowIntoAnother(curRow, i);

							if(verbose)
								Console.WriteLine("({0}) ⨁= ({1})", i, curRow);
						}
					}

					if(verbose)
						Console.WriteLine("\n" + this);

					curRow++;
					curCol++;
				}
				else {
					int nonZeroRow = curRow;

					for(int i = curRow + 1; i < NumRows; i++) {
						if(Matrix[i, curCol] != 0) {
							nonZeroRow = i;
							break;
						}
					}

					if(nonZeroRow != curRow) {
						SwapRows(curRow, nonZeroRow);

						if(verbose) {
							Console.WriteLine("меняем местами {0} и {1} столбцы\n", curRow, nonZeroRow);
							Console.WriteLine(this);
						}

						continue;
					}
					else {
						curCol++;
					}
				}
			}

			return;
		}

		bool CheckConsistency()
		{
			for(int i = 0; i < NumRows; i++) {
				bool nonZeroRow = false;

				for(int j = 0; j < NumRows; j++) {
					if(Matrix[i, j] != 0) {
						nonZeroRow = true;
						break;
					}
				}

				if(!nonZeroRow && (Matrix[i, 8] != 0))
					return false;
			}

			return true;
		}

		public int Solve(bool verbose = false)
		{
			MakeCanonical(verbose);

			if(!CheckConsistency())
				throw new SystemNonConsistentException();

			int result = 0, power = 1;
			for(int i = 0; i < 8; i++) {
				result += Matrix[7 - i, 8] * power;
				power *= 2;
			}

			return result;
		}
	}
}

