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
                inputValue = BitOperations.RotateRight(inputValue, 81943 * Convert.ToInt32(character));
                inputValue = inputValue ^ character ^ current;

                sum += inputValue - BitOperations.RotateLeft((UInt64)current, 82031 * Convert.ToInt32(character));
                sum = BitOperations.RotateRight(sum, 82031 * Convert.ToInt32(current));
                current = character;
            }

            char[] formattedSum = sum.ToString().ToCharArray();
            char[] charList = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            string hash = string.Empty;

            for(int i = 0; i < 64; i++)
            {
                int salt = 0;

                if (formattedSum.Length > i)
                    salt = formattedSum[i] + i * 81943;
                else salt = formattedSum[i % formattedSum.Length] + i * 82031;

                if (salt >= charList.Length)
                    salt = salt % charList.Length;

                hash += charList[salt];
            }
            return hash;
        }
    }
}