using System;
using hash_algorithm.Logic;

namespace hash_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            HashAlgorithm hashAlgorithm = new HashAlgorithm();

            Console.WriteLine("Ivestis: {0} ir gauta isvestis: {1}", "input", hashAlgorithm.ToHash("input"));
            Console.WriteLine("Ivestis: {0} ir gauta isvestis: {1}", "jnput", hashAlgorithm.ToHash("jnput"));
            Console.WriteLine("Ivestis: {0} ir gauta isvestis: {1}", "lietuva", hashAlgorithm.ToHash("lietuva"));
            Console.WriteLine("Ivestis: {0} ir gauta isvestis: {1}", "Lietuva", hashAlgorithm.ToHash("Lietuva"));
            Console.WriteLine("Ivestis: {0} ir gauta isvestis: {1}", "Lietuva!", hashAlgorithm.ToHash("Lietuva!"));
        }
    }
}
