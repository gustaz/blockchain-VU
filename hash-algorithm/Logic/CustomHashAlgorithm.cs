﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace hash_algorithm.Logic
{
    public class CustomHashAlgorithm
    {
        public string ToHash(string inputString)
        {
            //if (inputString == string.Empty || inputString == (string)null) return "00000000";
            UInt64 sum = 0;

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
            char[] charList = "123456789ABCDEF".ToCharArray();

            string hash = string.Empty;

            for(int i = 0; i < 64; i++)
            {
                int salt;

                if (formattedSum.Length > i)
                    salt = formattedSum[i] + i * 4001027;
                else salt = formattedSum[i % formattedSum.Length] + i * 3012719;

                if (salt >= charList.Length)
                    salt = salt % charList.Length;

                hash += charList[salt];
            }
            return hash;
        }
    }
}