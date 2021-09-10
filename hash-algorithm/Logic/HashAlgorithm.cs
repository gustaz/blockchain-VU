using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace hash_algorithm.Logic
{
    public class HashAlgorithm
    {
        public string ToHash(string inputString)
        {
            HashAlgorithm hashAlgorithm = new HashAlgorithm();
            UInt64 sum = 0;

            char current = inputString[0];
            foreach (char character in inputString)
            {
                UInt64 inputValue = Convert.ToUInt32(character);
                inputValue = BitOperations.RotateRight(inputValue, 51197 * Convert.ToInt32(character));
                inputValue = inputValue ^ character;
                //Console.WriteLine(inputValue);

                sum += inputValue - BitOperations.RotateLeft((UInt64)current, 51287 * Convert.ToInt32(character));
                current = character;
            }

            char[] formattedSum = sum.ToString().ToCharArray();
            char[] charList = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            string hash = string.Empty;

            for(int i = 0; i < 64; i++)
            {
                int salt = 0;

                if (formattedSum.Length > i)
                    salt = formattedSum[i] + i * 3;
                else salt = formattedSum[i % formattedSum.Length] + i * 7;

                if (salt >= charList.Length)
                    salt = salt % charList.Length;

                hash += charList[salt];
            }
            return hash;
        }
    }
}
