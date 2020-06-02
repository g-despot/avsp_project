using System;
using System.Collections.Generic;
using System.Linq;

namespace lab3
{
    class CF
    {
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../IOFiles/lab3/R.in");
            int[] inputNumbers = Array.ConvertAll(file.ReadLine().Split(' '), int.Parse);
            int N = inputNumbers[0];
            int M = inputNumbers[1];
            double[,] IUMatrix = new double[100, 100];
            double[,] UIMatrix = new double[100, 100];
            double[] matrixRow = new double[100];
            double[] IURowSum = new double[100];
            double[] IURowNumberOfElements = new double[100];
            double[] IURowMean = new double[100];
            int numberOfElements = 0;
            double sum = 0;

            for (int i = 0; i < N; i++)
            {
                string[] matrixRowString = file.ReadLine().Split(' ').Select(s => s.Replace("X", "0")).ToArray();
                matrixRow = Array.ConvertAll(matrixRowString, double.Parse);
                for (int j = 0; j < matrixRow.Length; j++)
                {
                    IUMatrix[i, j] = matrixRow[j];
                    if (matrixRow[j] != 0)
                    {
                        numberOfElements++;
                        sum += matrixRow[j];
                    }
                }
                IURowSum[i] = sum;
                IURowNumberOfElements[i] = numberOfElements;
                IURowMean[i] = sum / numberOfElements;
                matrixRow = new double[100];
                numberOfElements = 0;
                sum = 0;
            }
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < M; j++)
                {
                    UIMatrix[i, j] = IUMatrix[j, i];
                }
            }
            double[] UIRowSum = new double[100];
            double[] UIRowNumberOfElements = new double[100];
            double[] UIRowMean = new double[100];
            for (int i = 0; i < M; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    if (UIMatrix[i, j] != 0)
                    {
                        numberOfElements++;
                        sum += UIMatrix[i, j];
                    }
                }
                UIRowSum[i] = sum;
                UIRowNumberOfElements[i] = numberOfElements;
                UIRowMean[i] = sum / numberOfElements;
                numberOfElements = 0;
                sum = 0;
            }

            int Q = int.Parse(file.ReadLine());

            for (int i = 0; i < Q; i++)
            {
                inputNumbers = Array.ConvertAll(file.ReadLine().Split(' '), int.Parse);
                int I = inputNumbers[0];
                int J = inputNumbers[1];
                int T = inputNumbers[2];
                int K = inputNumbers[3];

                double[,] matrix;
                double[] rowMean;

                if (T == 0)
                {
                    I -= 1;
                    J -= 1;
                    matrix = IUMatrix;
                    rowMean = IURowMean;
                }
                else
                {
                    int tmpI = I;
                    I = J - 1;
                    J = tmpI - 1;
                    matrix = UIMatrix;
                    rowMean = UIRowMean;
                }
                double mean = rowMean[I];
                double[,] matrixSubMean = new double[100, 100];

                for (int l = 0; l < matrix.GetLength(0); l++)
                {
                    for (int k = 0; k < matrix.GetLength(1); k++)
                    {
                        if (matrix[l, k] != 0)
                        {
                            matrixSubMean[l, k] = matrix[l, k] - rowMean[l];
                        }
                    }
                }

                Dictionary<int, double> cosine = new Dictionary<int, double>();
                for (int l = 0; l < matrix.GetLength(0); l++)
                {
                    cosine[l] = CosineSim(GetRow(matrixSubMean, l), GetRow(matrixSubMean, I));
                }
                Dictionary<int, double> orderedCosine = cosine.OrderByDescending(x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                double aSum = 0, bSum = 0;
                int count = 0;
                foreach (int o in orderedCosine.Keys)
                {

                    if (count != 0 && orderedCosine[o] > 0)
                    {
                        if (matrix[o, J] == 0) continue;
                        aSum += orderedCosine[o] * matrix[o, J];
                        bSum += orderedCosine[o];
                    }
                    if (count == K) break;
                    count++;
                }
                Console.WriteLine(String.Format("{0:0.000}", Math.Round(aSum / bSum, 3, MidpointRounding.AwayFromZero)));
            }
            Console.ReadLine();
        }
        public static double CosineSim(double[] a, double[] b)
        {
            double ab = 0, aSquaredSum = 0, bSquaredSum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                ab += a[i] * b[i];
                aSquaredSum += a[i] * a[i];
                bSquaredSum += b[i] * b[i];
            }
            return ab / (Math.Sqrt(aSquaredSum) * Math.Sqrt(bSquaredSum));
        }

        public static double[] GetColumn(double[,] matrix, int columnNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(0))
                    .Select(x => matrix[x, columnNumber])
                    .ToArray();
        }

        public static double[] GetRow(double[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        public static void PrintMatrix(double[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    Console.Write(Math.Round(matrix[i, j], 3) + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static void PrintArray(double[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(Math.Round(array[i], 3) + " ");
            }
            Console.WriteLine();
        }

        public static void PrintDictionary(Dictionary<int, double> dictionary)
        {
            foreach (int i in dictionary.Keys)
            {
                Console.Write(Math.Round(dictionary[i], 3) + " ");
            }
            Console.WriteLine();
        }
    }
}