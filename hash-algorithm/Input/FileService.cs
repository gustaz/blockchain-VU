using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hash_algorithm.InputGeneration;

namespace hash_algorithm.Input
{
    public class FileService
    {
        readonly DataGenerator _dataGenerator;

        public FileService(DataGenerator dataGenerator)
        {
            _dataGenerator = dataGenerator;
        }

        public List<string> GenerateData()
        {
            List<string> stringCouples = new List<string>();

            Console.WriteLine("Generating data ...");

            stringCouples = _dataGenerator.RunStringGenerator();

            if (!Directory.Exists(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\"))
            {
                Directory.CreateDirectory(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\");
            }

            File.WriteAllLines(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\stringpairs.txt", stringCouples);

            File.WriteAllText(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\1symbol1.txt", _dataGenerator.RunCharGenerator(1));

            File.WriteAllText(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\1symbol2.txt", _dataGenerator.RunCharGenerator(1));

            File.WriteAllText(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\symbols1.txt", _dataGenerator.RunCharGenerator(10000));

            File.WriteAllText(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\symbols2.txt", _dataGenerator.RunCharGenerator(10000));

            string symbols = _dataGenerator.RunCharGenerator(10000);
            string symbolsTemp = symbols;

            symbolsTemp.Insert(symbolsTemp.Length / 2, "e");
            File.WriteAllText(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\symbolsmiddleE.txt", symbolsTemp);

            symbols.Insert(symbols.Length / 2, "a");
            File.WriteAllText(@"C:\Users\gusta\Desktop\VU ISI 1\2 kursas\Blockchain\blockchain-VU\hash-algorithm\InputFiles\symbolsmiddleA.txt", symbols);

            Console.WriteLine("All data generated and written!");

            return stringCouples;
        }
    }
}
