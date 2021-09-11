using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hash_algorithm.Input;
using hash_algorithm.InputGeneration;
using hash_algorithm.Logic;

namespace hash_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> arguments = args.ToList();
            string inputParam = string.Empty;
            string outputParam = string.Empty;
            Stopwatch stopWatch = new Stopwatch();
            TimeSpan timeSpan = new TimeSpan();

            try
            {
                inputParam = arguments[0];
                arguments.Remove(inputParam);
                
                if (inputParam != "-c" && inputParam != "-g" && inputParam != "-a")
                {
                
                    outputParam = arguments.Last();
                    arguments.Remove(outputParam);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nNo valid launch parameters detected!\nValid arguments are:\n");
                Console.WriteLine("-a Used to benchmark the Avalanche effect");
                Console.WriteLine("-c Used to check for collissions");
                Console.WriteLine("-g Used to trigger file generation");
                Console.WriteLine("-i Used to take input from the command line");
                Console.WriteLine("-if Used to take input from a file");
                Console.WriteLine("-o Used to output to the command line");
                Console.WriteLine("-of Used to output to a file");
            }

            List<Tuple<string, string>> hashes = new List<Tuple<string, string>>();
            List<string> stringCouples = new List<string>();

            HashAlgorithm hashAlgorithm = new HashAlgorithm();
            DataGenerator dataGenerator = new DataGenerator();
            FileService fileService = new FileService(dataGenerator);

            if (inputParam == "-a")
            {
                List<Tuple<string, string>> avalanchePairs = new List<Tuple<string, string>>();
                List<double> similarityPercentage = new List<double>();
                Levenshtein similarityCalculator = new Levenshtein();

                double min = 100;
                double max = 0;
                double avg = 0;

                Console.WriteLine("Performing data generation...");

                Parallel.For(0, 10000, x =>
                {
                    StringBuilder s1 = new StringBuilder(dataGenerator.RunCharGenerator(1000));
                    StringBuilder s2 = new StringBuilder(dataGenerator.RunCharGenerator(1000));

                    s1.Insert(x % 1000, dataGenerator.RunCharGenerator(1));
                    s2.Insert(x % 1000, dataGenerator.RunCharGenerator(1));

                    string str1 = s1.ToString();
                    string str2 = s2.ToString();

                    avalanchePairs.Add(new Tuple<string, string>(hashAlgorithm.ToHash(str1), hashAlgorithm.ToHash(str2)));
                });

                Console.WriteLine("Data generation done!");
                //Parallel.ForEach(avalanchePairs, x => { similarityPercentage.Add(similarityCalculator.CalculateSimilarity(x.Item1, x.Item2)); });

                Console.WriteLine("\nPerforming similarity calculation in HEX...");
                foreach(Tuple<string, string> pair in avalanchePairs)
                {
                    similarityPercentage.Add(similarityCalculator.CalculateSimilarity(pair.Item1, pair.Item2));
                }

                int i = 0;
                Parallel.ForEach(similarityPercentage, x => { if (x < min) min = x; if (x > max) max = x; avg += x; i++; });

                Console.WriteLine("Minimum detected hex difference: {0}\nMaximum detected difference: {1}\nAverage difference: {2}", min, max, avg / i);
            }
            else if (inputParam == "-c")
            {
                try
                {
                    stringCouples = File.ReadAllLines(@"InputFiles\stringpairs.txt").ToList();
                }
                catch (Exception ex)
                {
                    if (ex is FileNotFoundException)
                    {
                        Console.WriteLine("File not found! Generating data instead..");
                        stringCouples = fileService.GenerateData();
                    }
                }
                string prev = stringCouples.First();
                bool collided = false;
                stringCouples.Remove(prev);

                foreach (string occurence in stringCouples)
                {
                    if (occurence.Length == prev.Length)
                    {
                        if (hashAlgorithm.ToHash(occurence) == hashAlgorithm.ToHash(prev))
                            collided = true;
                    }
                    Console.WriteLine("Comparing {0} and {1}", hashAlgorithm.ToHash(occurence), hashAlgorithm.ToHash(prev));
                    prev = occurence;
                }

                if (collided) Console.WriteLine("A collission has occured!");
                else Console.Write("No collissions occured.");
            }
            else if (inputParam == "-g")
            {
                fileService.GenerateData();
            }
            else if (inputParam == "-i")
            {
                stopWatch.Start();
                arguments.ForEach(x => { hashes.Add(new Tuple<string, string>(x, hashAlgorithm.ToHash(x))); });
                timeSpan = stopWatch.Elapsed;
            }
            else if (inputParam == "-if")
            {
                stopWatch.Start();
                foreach (string argument in arguments)
                {
                    string data = File.ReadAllText(argument);
                    hashes.Add(new Tuple<string, string>(argument, hashAlgorithm.ToHash(data)));
                }
                timeSpan = stopWatch.Elapsed;
            }
            else if (arguments.Count != 0)
            {
                Console.WriteLine("\nNo valid launch input parameters detected!\nValid arguments are:\n");
                Console.WriteLine("-a Used to benchmark the Avalanche effect");
                Console.WriteLine("-c Used to check for collissions");
                Console.WriteLine("-g Used to trigger file generation");
                Console.WriteLine("-i Used to take input from the command line");
                Console.WriteLine("-if Used to take input from a file");
            }

            if(hashes.Count != 0)
            {
                if(outputParam == "-o")
                {
                    hashes.ForEach(x => { Console.WriteLine("Input: {0} and the resulting output: {1}", x.Item1, x.Item2); });
                    Console.WriteLine("Time spent hashing: {0}ms", timeSpan.TotalMilliseconds);
                }
                else if(outputParam == "-of")
                {
                    Console.WriteLine("Writing to files...");
                    if (!Directory.Exists(@"OutputFiles\"))
                    {
                        Directory.CreateDirectory(@"OutputFiles\");
                    }

                    if (File.Exists(@"OutputFiles\Output.txt"))
                    {
                        File.Delete(@"Output.txt");
                    }
                    hashes.ForEach(x => { File.AppendAllText(@"OutputFiles\Output.txt", String.Format("Input: {0} and the resulting output: {1}\n", x.Item1, x.Item2)); });
                    Console.WriteLine("Time spent hashing: {0}ms", timeSpan.TotalMilliseconds);
                    Console.WriteLine("Done!");
                }
                else
                {
                    Console.WriteLine("\nNo valid launch output parameters detected!\nValid arguments are:\n");
                    Console.WriteLine("-o Used to output to the command line");
                    Console.WriteLine("-of Used to take input from the command line");
                }
            }
        }
    }

    class Levenshtein
    {
        public int LevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        public double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = LevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }
    }
}

