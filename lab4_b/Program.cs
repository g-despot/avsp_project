using System;
using System.Collections.Generic;
using System.Linq;

namespace lab4_b
{
    class ClosestBlackNode
    {
        public static int[] color;
        public static Dictionary<int, List<int>> found = new Dictionary<int, List<int>>();
        public static Dictionary<int, List<int>> toCheck = new Dictionary<int, List<int>>();
        public static Dictionary<int, SortedSet<int>> adjacentNodes = new Dictionary<int, SortedSet<int>>();
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../IOFiles/lab4_b/mtest2/R.in");
            string[] inputString = file.ReadLine().Trim().Split(' ');
            int n = Convert.ToInt32(inputString[0]);
            double e = Convert.ToInt32(inputString[1]);
            color = new int[n];
            for (int i = 0; i < n; i++)
            {
                color[i] = Convert.ToInt32(file.ReadLine().Trim());
            }
            for (int i = 0; i < e; i++)
            {
                int[] inputNumbers = Array.ConvertAll(file.ReadLine().Split(' '), int.Parse);
                if (!adjacentNodes.ContainsKey(inputNumbers[0]))
                {
                    adjacentNodes.Add(inputNumbers[0], new SortedSet<int>());
                }
                adjacentNodes[inputNumbers[0]].Add(inputNumbers[1]);

                if (!adjacentNodes.ContainsKey(inputNumbers[1]))
                {
                    adjacentNodes.Add(inputNumbers[1], new SortedSet<int>());
                }
                adjacentNodes[inputNumbers[1]].Add(inputNumbers[0]);
            }

            for (int i = 0; i < n; i++)
            {
                List<int> visited = new List<int>();
                int[] res;
                if (color[i] == 1)
                {
                    res = new int[] { i, 0 };
                }
                else
                {
                    toCheck.Add(0, new List<int>());
                    toCheck[0].Add(i);
                    res = Black(i, 1, visited);
                }
                Console.WriteLine(res[0] + " " + res[1]);
                toCheck.Clear();
                found.Clear();
            }
            Console.ReadLine();
        }

        public static int[] Black(int node, int depth, List<int> visited)
        {
            if (depth < 10)
            {
                toCheck[depth - 1].Remove(node);
                visited.Add(node);
                int[] adjacent = adjacentNodes[node].ToArray();

                for (int i = 0; i < adjacent.Length; i++)
                {
                    if (color[adjacent[i]] == 1)
                    {
                        return new int[] { adjacent[i], depth };
                    }
                    else
                    {
                        if (!toCheck.ContainsKey(depth))
                        {
                            toCheck.Add(depth, new List<int>());
                        }
                        if (!visited.Contains(adjacent[i]))
                        {
                            toCheck[depth].Add(adjacent[i]);
                        }
                    }
                }
                if (found.ContainsKey(depth) && found[depth].Count > 0 || toCheck[depth - 1].Count > 0)
                {
                    return new int[] { -1, -1 };
                }
                toCheck[depth].Sort((a, b) => a.CompareTo(b));
                List<int> mc = new List<int>(toCheck[depth]);
                for (int i = 0; i < mc.Count; i++)
                {
                    int[] tmpi = Black(mc.ElementAt(i), depth + 1, visited);
                    if (tmpi[0] != -1 && tmpi[0] != -2)
                    {
                        if (!found.ContainsKey(tmpi[1]))
                        {
                            found.Add(tmpi[1], new List<int>());
                        }
                        found[tmpi[1]].Add(tmpi[0]);
                    }
                }
                if (found.Count > 0)
                {
                    int min = found.Keys.Min();
                    if (found[min].Count > 0)
                    {
                        return new int[] { found[min].Min(), min };
                    }
                    else
                    {
                        return new int[] { -1, -1 };
                    }
                }
                return new int[] { -1, -1 };
            }
            else
            {
                return new int[] { -1, -1 };
            }
        }
    }
}