using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace hash_algorithm.Logic
{
    public class CustomHashAlgorithm
    {
        public string ToHash(string inputString)
        {
            UInt64 sum = 0;

            if (inputString == string.Empty || inputString == (string)null)
            {
                inputString = "eezgIENWG";
                sum = 895423;
            }

            char current = inputString[0];
            foreach (char character in inputString)
            {
                UInt64 inputValue = Convert.ToUInt32(character);
                inputValue = BitOperations.RotateRight(inputValue, 4001027 * Convert.ToInt32(character));
                inputValue = inputValue ^ character ^ current;

                sum += inputValue - BitOperations.RotateLeft((UInt64)current, 3012719 * Convert.ToInt32(character));
                sum = BitOperations.RotateRight(sum, 1013699 * Convert.ToInt32(current));
                current = character;
            }

            char[] formattedSum = sum.ToString().ToCharArray();
            char[] charList = "0123456789ABCDEF".ToCharArray();

            string hash = string.Empty;

            for(int i = 0; i < 64; i++)
            {
                int salt;

                if (formattedSum.Length > i)
                    salt = formattedSum[i] + i * 4001027;
                else salt = formattedSum[i % formattedSum.Length] + i * 3012719;

                if (salt >= charList.Length)
                    salt = salt % charList.Length;

                if (i != 0)
                    hash += charList[salt];
                else hash += charList[1];
            }
            return hash;
        }
        public string ToDefinedHash(HashAlgorithm definedHash, string input)
        {
            byte[] bytes = definedHash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}