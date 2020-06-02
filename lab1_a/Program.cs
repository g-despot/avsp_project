using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lab1_a
{
    class SimHash
    {
        static MD5 md5Hash = MD5.Create();
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../IOFiles/lab1_a/R.in");
            int N = int.Parse(file.ReadLine());

            List<bool[]> hashes = new List<bool[]>();
            List<int[]> qList = new List<int[]>();

            for (int i = 0; i < N; i++)
            {
                string[] words = file.ReadLine().Trim().Split(' ');
                int[] sh = new int[128];
                bool[] hash = new bool[128];
                foreach (string word in words)
                {
                    byte[] byteHash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(word));
                    bool[] bits = byteHash.SelectMany(GetBits).ToArray();
                    for (int j = 0; j < bits.Length; j++)
                    {
                        if (bits[j])
                        {
                            sh[j] = sh[j] + 1;
                        }
                        else
                        {
                            sh[j] = sh[j] - 1;
                        }
                    }
                }
                for (int k = 0; k < sh.Length; k++)
                {
                    if (sh[k] >= 0)
                    {
                        sh[k] = 1;
                        hash[k] = true;
                    }
                    else
                    {
                        sh[k] = 0;
                        hash[k] = false;
                    }
                }
                hashes.Add(hash);

            }
            int Q = int.Parse(file.ReadLine());

            for (int i = 0; i < Q; i++)
            {
                string[] words = file.ReadLine().Trim().Split(' ');
                int[] numbers = new int[2];
                numbers[0] = int.Parse(words[0]);
                numbers[1] = int.Parse(words[1]);
                qList.Add(numbers);
            }

            int cnt = 0;
            foreach (int[] pair in qList)
            {
                cnt++;
                int L = pair[0];
                int K = pair[1];
                bool[] tmp = hashes.ElementAt(L);
                int result = 0;
                for (int i = 0; i < hashes.Count; i++)
                {
                    if (i != L)
                    {
                        int distance = 0;
                        bool[] bytes = hashes.ElementAt(i);
                        for (int j = 0; j < bytes.Length; j++)
                        {
                            if (bytes[j] != tmp[j])
                            {
                                distance++;
                            }
                        }
                        if (distance <= K)
                        {
                            result++;
                        }
                    }
                }
                Console.WriteLine(result);
            }
            Console.ReadLine();
        }

        static IEnumerable<bool> GetBits(byte b)
        {
            for (int i = 0; i < 8; i++)
            {
                yield return (b & 0x80) != 0;
                b *= 2;
            }
        }
    }
}