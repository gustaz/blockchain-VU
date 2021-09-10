using System;
using hash_algorithm.Logic;

namespace hash_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            HashAlgorithm hashAlgorithm = new HashAlgorithm();
            string test = "testing";

            Console.WriteLine("Input string is {0}", test);

            Console.WriteLine("Output string is {0}", hashAlgorithm.ToHash(test));
        }
    }
}
