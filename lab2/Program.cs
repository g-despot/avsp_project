using System;
using System.Collections.Generic;
using System.Linq;

namespace lab2
{
    class PCY
    {
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../IOFiles/lab2/R.in");

            int N = int.Parse(file.ReadLine());
            double s = double.Parse(file.ReadLine());
            int b = int.Parse(file.ReadLine());
            int treshold = (int)Math.Floor(s * N);

            List<int[]> buckets = new List<int[]>();
            for (int i = 0; i < N; i++)
            {
                int[] line = file.ReadLine().Trim().Split(' ').Select(int.Parse).ToArray();
                buckets.Add(line);
            }

            Dictionary<int, int> numberOfItems = new Dictionary<int, int>();
            foreach (int[] bucket in buckets)
            {
                foreach (int item in bucket)
                {
                    IncreaseOrCreateDictionary<int>(numberOfItems, item);
                }
            }

            int numberOfUniqueItems = numberOfItems.Keys.Count;
            Dictionary<int, int> compartments = new Dictionary<int, int>();
            foreach (int[] bucket in buckets)
            {
                for (int i = 0; i < (bucket.Length - 1); i++)
                {
                    for (int j = i + 1; j < bucket.Length; j++)
                    {
                        if ((numberOfItems[bucket[i]] >= treshold) && (numberOfItems[bucket[j]] >= treshold))
                        {
                            int h = ((bucket[i] * numberOfUniqueItems + bucket[j]) % b);
                            IncreaseOrCreateDictionary<int>(compartments, h);
                        }
                    }
                }
            }

            Dictionary<string, int> pairs = new Dictionary<string, int>();
            foreach (int[] bucket in buckets)
            {
                for (int i = 0; i < (bucket.Length - 1); i++)
                {
                    for (int j = i + 1; j < bucket.Length; j++)
                    {
                        if ((numberOfItems[bucket[i]] >= treshold) && (numberOfItems[bucket[j]] >= treshold))
                        {
                            int h = ((bucket[i] * numberOfUniqueItems + bucket[j]) % b);
                            if (compartments[h] >= treshold)
                            {
                                string key = (bucket[i] + "-" + bucket[j]).ToString();
                                IncreaseOrCreateDictionary<string>(pairs, key);
                            }
                        }
                    }
                }
            }

            Console.WriteLine(numberOfUniqueItems * (numberOfUniqueItems - 1) / 2);
            Console.WriteLine(pairs.Count);

            List<int> values = pairs.Values.ToList();
            values.Sort((x, y) => y.CompareTo(x));
            foreach (int value in values)
            {
                Console.WriteLine(value);
            }
            Console.ReadLine();
        }
        static void IncreaseOrCreateDictionary<T>(Dictionary<T, int> dictionary, T key)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, 1);
            }
            else
            {
                dictionary[key] = dictionary[key] + 1;
            }
        }
    }
}