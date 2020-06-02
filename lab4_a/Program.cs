using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4_a
{
    class NodeRank
    {
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../IOFiles/lab4_a/mtest2/R.in");
            string[] inputString = file.ReadLine().Trim().Split(' ');
            int N = Convert.ToInt32(inputString[0]);
            double b = Convert.ToDouble(inputString[1]);
            int[,] adjacentNodes = new int[N, 15];
            InitArray(adjacentNodes, -1);

            for (int i = 0; i < N; i++)
            {
                int[] inputNumbers = Array.ConvertAll(file.ReadLine().Split(' '), int.Parse);
                for (int j = 0; j < inputNumbers.Length; j++)
                {
                    adjacentNodes[i, j] = inputNumbers[j];
                }
            }

            int Q = Convert.ToInt32(file.ReadLine().Trim());

            List<int[]> requests = new List<int[]>();
            int[] iterations = new int[Q];

            for (int i = 0; i < Q; i++)
            {
                int[] inputNumbers = Array.ConvertAll(file.ReadLine().Split(' '), int.Parse);
                requests.Add(inputNumbers);
                iterations[i] = inputNumbers[1];
            }

            List<List<double>> r = new List<List<double>>();
            List<double> results = new List<double>();
            int max = iterations.Max();

            for (int i = 0; i < max + 1; i++)
            {
                r.Add(new List<double>());
                FillList(r.ElementAt(i), N, (1 - b) / N);
            }

            for (int iteration = 0; iteration < max; iteration++)
            {
                for (int i = 0; i < N; i++)
                {
                    int[] dest = GetRow(adjacentNodes, i);
                    int di = dest.Count(x => x != -1);
                    for (int j = 0; j < di; j++)
                    {
                        r.ElementAt(iteration + 1)[dest[j]] += b * r.ElementAt(iteration).ElementAt(i) / di;
                    }
                }
            }

            for (int q = 0; q < Q; q++)
            {
                int requestNode = requests.ElementAt(q)[0];
                int reqestIteration = requests.ElementAt(q)[1];
                results.Add(r.ElementAt(reqestIteration)[requestNode]);
            }

            foreach (double result in results)
            {
                Console.WriteLine(String.Format("{0:0.0000000000}", Math.Round(result, 10)));
            }
            Console.ReadLine();
        }

        public static int[] GetRow(int[,] matrix, int rowNumber)
        {
            return Enumerable.Range(0, matrix.GetLength(1))
                    .Select(x => matrix[rowNumber, x])
                    .ToArray();
        }

        public static void InitArray(int[,] array, int initValue)
        {
            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    array[i, j] = initValue;
                }
            }
        }

        public static void FillList(List<double> list, int length, double value)
        {
            for (int i = 0; i < length; i++)
            {
                list.Add(value);
            }
        }
    }
}