using System;
using hash_algorithm.Logic;

namespace hash_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            HashAlgorithm hashAlgorithm = new HashAlgorithm();

            Console.WriteLine(hashAlgorithm.ToHash("bbaa"));
            Console.WriteLine(hashAlgorithm.ToHash("baab"));
            Console.WriteLine(hashAlgorithm.ToHash("aabb"));
            Console.WriteLine(hashAlgorithm.ToHash("baba"));
        }
    }
}
