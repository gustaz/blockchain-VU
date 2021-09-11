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
            List<string> stringCouples;

            Console.WriteLine("Generating data ...");

            stringCouples = _dataGenerator.RunStringGenerator();

            if (!Directory.Exists(@"InputFiles\"))
            {
                Directory.CreateDirectory(@"InputFiles\");
            }

            File.WriteAllLines(@"InputFiles\stringpairs.txt", stringCouples);

            File.WriteAllText(@"InputFiles\1symbol1.txt", _dataGenerator.RunCharGenerator(1));

            File.WriteAllText(@"InputFiles\1symbol2.txt", _dataGenerator.RunCharGenerator(1));

            File.WriteAllText(@"InputFiles\symbols1.txt", _dataGenerator.RunCharGenerator(10000));

            File.WriteAllText(@"InputFiles\symbols2.txt", _dataGenerator.RunCharGenerator(10000));

            string symbols = _dataGenerator.RunCharGenerator(10000);
            string symbolsTemp = symbols;


            symbolsTemp = symbolsTemp.Insert(symbolsTemp.Length / 2, "e");
            File.WriteAllText(@"InputFiles\symbolsmiddleE.txt", symbolsTemp);

            symbols = symbols.Insert(symbols.Length / 2, "a");
            File.WriteAllText(@"InputFiles\symbolsmiddleA.txt", symbols);

            Console.WriteLine("All data generated and written!");

            return stringCouples;
        }
    }
}
