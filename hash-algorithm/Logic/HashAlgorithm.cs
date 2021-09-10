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

            foreach (char character in inputString)
            {
                UInt64 inputValue = Convert.ToUInt32(character);
                inputValue = BitOperations.RotateRight(inputValue, 51197 * Convert.ToInt32(character));
                inputValue = inputValue ^ character;
                //Console.WriteLine(inputValue);
           
                sum += inputValue;
            }

            string hash = sum.ToString();
            return hash;
        }
    }
}
