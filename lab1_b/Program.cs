using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lab1_b
{
    class SimHashBuckets
    {
        static MD5 md5Hash = MD5.Create();
        static void Main(string[] args)
        {
            System.IO.StreamReader file = new System.IO.StreamReader("../../../IOFiles/lab1_b/test0/R.in");
            int N = int.Parse(file.ReadLine());

            List<bool[]> hashovi = new List<bool[]>();
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
                hashovi.Add(hash);

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

            Dictionary<int, List<int>> kandidati = new Dictionary<int, List<int>>();
            for (int i = 0; i < hashovi.Count; i++)
            {
                kandidati[i] = new List<int>();
            }
            for (int pojas = 0; pojas < 8; pojas++)
            {
                Dictionary<int, List<int>> pretinci = new Dictionary<int, List<int>>();
                for (int trenutni_id = 0; trenutni_id < hashovi.Count; trenutni_id++)
                {
                    bool[] hash = hashovi.ElementAt(trenutni_id);
                    var sb = new System.Text.StringBuilder();
                    for (int i = pojas * 16; i < 16 + pojas * 16; i++)
                    {
                        if (hash[i])
                        {
                            sb.Append(1.ToString());
                        }
                        else
                        {
                            sb.Append(0.ToString());
                        }
                    }
                    string s = sb.ToString().Trim();
                    int val = Convert.ToInt32(s, 2);
                    List<int> tekstovi_u_pretincu = new List<int>();
                    if (pretinci.ContainsKey(val))
                    {
                        tekstovi_u_pretincu = pretinci[val];
                        foreach (int text_id in tekstovi_u_pretincu)
                        {
                            if (!kandidati[trenutni_id].Contains(text_id))
                            {
                                kandidati[trenutni_id].Add(text_id);
                            }
                            if (!kandidati[text_id].Contains(trenutni_id))
                            {
                                kandidati[text_id].Add(trenutni_id);
                            }
                        }
                    }
                    else
                    {
                        pretinci[val] = new List<int>();
                    }
                    pretinci[val].Add(trenutni_id);
                }
            }

            int cnt = 0;
            foreach (int[] pair in qList)
            {
                cnt++;
                int L = pair[0];
                int K = pair[1];
                bool[] tmp = hashovi.ElementAt(L);
                List<int> tmpTwo = kandidati[L];
                int result = 0;
                foreach (int i in tmpTwo)
                {
                    if (i != L)
                    {
                        int distance = 0;
                        bool[] bytes = hashovi.ElementAt(i);
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