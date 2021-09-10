using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using hash_algorithm.Input;
using hash_algorithm.InputGeneration;
using hash_algorithm.Logic;

namespace hash_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length == 0)
            {
                Console.WriteLine("No arguments detected!");
                Console.WriteLine("Valid arguments are:\n");
                Console.WriteLine("-generate Used to trigger file generation");
                Console.WriteLine("-input Used to take input from the command line");
            }
            HashAlgorithm hashAlgorithm = new HashAlgorithm();
            DataGenerator dataGenerator = new DataGenerator();
            FileService fileService = new FileService(dataGenerator);

            foreach (string arguments in args)
            {
                if (arguments.Contains("-generate"))
                {
                    fileService.GenerateData();
                }
            }

            Console.WriteLine(hashAlgorithm.ToHash(null));
            /*foreach (var tuple in stringCouples)
            {
                Console.WriteLine("Hash1: {0}, Hash2: {1}", hashAlgorithm.ToHash(tuple.Item1), hashAlgorithm.ToHash(tuple.Item2));
                if (hashAlgorithm.ToHash(tuple.Item1) == hashAlgorithm.ToHash(tuple.Item2))
                { 
                    Console.WriteLine("Detected a collission!");
                    throw new Exception();
                }
*/
            //}
        }
    }
}
