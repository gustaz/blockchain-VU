using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace hash_algorithm.InputGeneration
{
    public class DataGenerator
    {
        public List<string> RunStringGenerator()
        {
            List<string> PairsOfStrings = new List<string>();

            Parallel.For(0, 25000,
                action => {
                    PairsOfStrings
                        .Add(RandomString(10));

                    PairsOfStrings
                        .Add(RandomString(100));

                    PairsOfStrings
                        .Add(RandomString(500));

                    PairsOfStrings
                        .Add(RandomString(1000));
                });

            PairsOfStrings.Sort((a, b) => a.Length.CompareTo(b.Length));

            return PairsOfStrings;
        }

        public string RunCharGenerator(int length)
        {
            string result = RandomString(length);
            return result;
        }

        private string RandomString(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder result = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    result.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            return result.ToString();
        }
    }
}
