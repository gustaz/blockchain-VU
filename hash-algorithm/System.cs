using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using hash_algorithm.Input;
using hash_algorithm.InputGeneration;
using hash_algorithm.Logic;
using System.Collections;

namespace hash_algorithm
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> arguments = args.ToList();
            string inputParam = string.Empty;
            string outputParam = string.Empty;
            string hashParam = string.Empty;
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

                if (outputParam == "-md5" || outputParam == "-sha256")
                {
                    hashParam = outputParam;
                    arguments.Remove(outputParam);
                    outputParam = arguments.Last();
                    arguments.Remove(outputParam);
                }
            }
            catch (Exception)
            {
                PrintUsageWarning();
            }

            List<Tuple<string, string>> hashes = new List<Tuple<string, string>>();
            List<string> stringCouples = new List<string>();

            CustomHashAlgorithm hashAlgorithm = new CustomHashAlgorithm();
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

                Parallel.For(0, 100000, x =>
                {
                    string input = dataGenerator.RunCharGenerator(1000);
                    StringBuilder s1 = new StringBuilder(input);
                    StringBuilder s2 = new StringBuilder(input);

                    s1.Insert(x % 1000, dataGenerator.RunCharGenerator(1));
                    s2.Insert(x % 1000, dataGenerator.RunCharGenerator(1));

                    string str1 = s1.ToString();
                    string str2 = s2.ToString();

                    if(str1 != str2)
                    {
                        if(hashParam == string.Empty)
                        {
                            avalanchePairs.Add(new Tuple<string, string>(hashAlgorithm.ToHash(str1), hashAlgorithm.ToHash(str2)));
                        }
                        else
                        {
                            if(hashParam == "-md5")
                            {
                                avalanchePairs.Add(new Tuple<string, string>(hashAlgorithm.ToDefinedHash(MD5.Create(), str1), hashAlgorithm.ToDefinedHash(MD5.Create(), str2)));
                            }
                            else if(hashParam == "-sha256")
                            {
                                avalanchePairs.Add(new Tuple<string, string>(hashAlgorithm.ToDefinedHash(SHA256.Create(), str1), hashAlgorithm.ToDefinedHash(MD5.Create(), str2)));
                            }
                        }
                    }
                });

                Console.WriteLine("Data generation done!");

                Console.WriteLine("\nPerforming similarity calculation in HEX...");
                foreach(Tuple<string, string> pair in avalanchePairs)
                {
                    similarityPercentage.Add(similarityCalculator.CalculateSimilarity(pair.Item1, pair.Item2));
                }

                //Parallel.ForEach(similarityPercentage, x => { if (x < min) min = x; if (x > max) max = x; avg += x; });

                foreach(var occ in similarityPercentage)
                {
                    if (occ < min)
                        min = occ;
                    if (occ > max)
                        max = occ;

                    avg += occ;
                }

                Console.WriteLine("Minimum detected hex difference: {0:F2}%\nMaximum detected difference: {1:F2}%\nAverage difference: {2:F2}%", min, max, avg / similarityPercentage.Count());

                min = 100;
                max = 0;
                avg = 0;
                    
                Console.WriteLine("\nPerforming similarity calculation in BINARY...");
                foreach (Tuple<string, string> pair in avalanchePairs)
                {

                    BitArray bitArray1 = new BitArray(Encoding.UTF8.GetBytes(pair.Item1));
                    BitArray bitArray2 = new BitArray(Encoding.UTF8.GetBytes(pair.Item2));

                    bitArray1.Xor(bitArray2);

                    double bits = 0;
                    foreach (bool bit in bitArray1)
                        if (bit) bits++;

                    double difference = 100 * (bits / 512);

                    if (difference < min)
                        min = difference;

                    if (difference > max)
                        max = difference;

                    avg += difference;
                }

                Console.WriteLine("Minimum detected binary difference: {0:F2}%\nMaximum detected difference: {1:F2}%\nAverage difference: {2:F2}%", min, max, avg / similarityPercentage.Count());
            }
            else if (inputParam == "-c")
            {
                try
                {
                    stringCouples = File.ReadAllLines(@"InputFiles\stringpairs.txt").ToList();
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("File not found! Generating data instead..");
                    stringCouples = fileService.GenerateData();
                }

                string prev = stringCouples.First();
                bool collided = false;
                stringCouples.Remove(prev);

                Console.WriteLine("Performing collission test...");
                foreach (string occurence in stringCouples)
                {
                    if (occurence.Length == prev.Length)
                    {
                        if (hashParam == "-md5")
                        {
                            MD5 md5Hash = MD5.Create();

                            if(hashAlgorithm.ToDefinedHash(md5Hash, occurence) == hashAlgorithm.ToDefinedHash(md5Hash, prev))
                                collided = true;
                        }
                        if (hashParam == "-sha256")
                        {
                            SHA256 sha256Hash = SHA256.Create();

                            if (hashAlgorithm.ToDefinedHash(sha256Hash, occurence) == hashAlgorithm.ToDefinedHash(sha256Hash, prev))
                                collided = true;
                        }
                        else if (hashAlgorithm.ToHash(occurence) == hashAlgorithm.ToHash(prev))
                            collided = true;
                    }
                    //Console.WriteLine("Comparing {0} and {1}", hashAlgorithm.ToHash(occurence), hashAlgorithm.ToHash(prev));
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

                if (hashParam == "-md5")
                {
                    MD5 md5Hasher = MD5.Create();
                    arguments.ForEach(x => { hashes.Add(new Tuple<string, string>(x, hashAlgorithm.ToDefinedHash(md5Hasher, x))); });
                }
                else if (hashParam == "-sha256")
                {
                    SHA256 sha256Hasher = SHA256.Create();
                    arguments.ForEach(x => { hashes.Add(new Tuple<string, string>(x, hashAlgorithm.ToDefinedHash(sha256Hasher, x))); });
                } 
                else
                {
                    arguments.ForEach(x => { hashes.Add(new Tuple<string, string>(x, hashAlgorithm.ToHash(x))); });
                }

                timeSpan = stopWatch.Elapsed;
            }
            else if (inputParam == "-if")
            {
                stopWatch.Start();
                foreach (string argument in arguments)
                {
                    try
                    {
                        string data = File.ReadAllText(argument);
                        
                        if (hashParam == "-md5")
                        {
                            MD5 md5Hasher = MD5.Create();
                            hashes.Add(new Tuple<string, string>(argument, hashAlgorithm.ToDefinedHash(md5Hasher, data)));
                        }
                        else if(hashParam == "-sha256")
                        {
                            SHA256 sha256Hasher = SHA256.Create();
                            hashes.Add(new Tuple<string, string>(argument, hashAlgorithm.ToDefinedHash(sha256Hasher, data)));
                        }
                        else
                        {
                            hashes.Add(new Tuple<string, string>(argument, hashAlgorithm.ToHash(data)));
                        }  
                    }

                    catch(FileNotFoundException)
                    {
                        Console.WriteLine("File not found! Exiting..");
                        Environment.Exit(1);
                    }
                }
                timeSpan = stopWatch.Elapsed;
            }
            else if (arguments.Count != 0)
            {
                PrintUsageWarning();
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
                    PrintUsageWarning();
                }
            }
        }
        private static void PrintUsageWarning()
        {
            Console.WriteLine("Usage: .\\hash-algorithm <-i | -if | -c | -g | -a> <input> <-o | -of> [-md5 | -sha256]");
            Console.WriteLine("-i allows input through the command line.");
            Console.WriteLine("-if allows input through a file.");
            Console.WriteLine("-c allows collission resistance testing.");
            Console.WriteLine("-g allows for generation of files.");
            Console.WriteLine("-a allows for Avalanche-effect testing.");
            Console.WriteLine("input requires string(s) to be input if the -i flag is selected and path(s) if the -if flag is selected.");
            Console.WriteLine("-o outputs to the command line.");
            Console.WriteLine("-of outputs to a file.");
            Console.WriteLine("-md5 runs the program using MD5 instead of the built-in hashing algorithm.");
            Console.WriteLine("-sha256 runs the program using SHA256 instead of the built-in hashing algorithm.");
        }
    }

    class Levenshtein
    {
        public int LevenshteinDistance(string a, string b)
        {
            if (String.IsNullOrEmpty(a) && String.IsNullOrEmpty(b))
            {
                return 0;
            }
            if (String.IsNullOrEmpty(a))
            {
                return b.Length;
            }
            if (String.IsNullOrEmpty(b))
            {
                return a.Length;
            }
            int lengthA = a.Length;
            int lengthB = b.Length;
            var distances = new int[lengthA + 1, lengthB + 1];
            for (int i = 0; i <= lengthA; distances[i, 0] = i++) ;
            for (int j = 0; j <= lengthB; distances[0, j] = j++) ;

            for (int i = 1; i <= lengthA; i++)
                for (int j = 1; j <= lengthB; j++)
                {
                    int cost = b[j - 1] == a[i - 1] ? 0 : 1;
                    distances[i, j] = Math.Min
                        (
                        Math.Min(distances[i - 1, j] + 1, distances[i, j - 1] + 1),
                        distances[i - 1, j - 1] + cost
                        );
                }
            return distances[lengthA, lengthB];
        }

        public double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 100.0;
            int stepsToSame = LevenshteinDistance(source, target);
            return 100 - ((double)Math.Max(source.Length, target.Length) - (double)stepsToSame) / (double)Math.Max(source.Length, target.Length) * 100;
        }
    }
}

