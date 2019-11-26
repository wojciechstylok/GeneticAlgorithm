using System;
using System.Collections;
using System.IO;

namespace GeneticAlgorithm
{
    class Program
    {
        public static int arrayLength;
        public static int[,] distanceTable;
        static void Main(string[] args)
        {
            var fileName = "berlin52.txt";
            var path = @"C:\Users\Wojciech\Desktop\" + fileName;
            StreamReader sr = new StreamReader(path);

            string line;
            int lineCounter = 0;
            if ((line = sr.ReadLine()) != null)
            {
                arrayLength = int.Parse(line.Trim());
            }

            distanceTable = new int[arrayLength, arrayLength];

            while ((line = sr.ReadLine()) != null)
            {
                var singleLine = line.Trim().Split(" ");
                for(int i = 0; i < singleLine.Length; i++)
                {
                    distanceTable[lineCounter, i] = int.Parse(singleLine[i]);
                    distanceTable[i, lineCounter] = int.Parse(singleLine[i]);
                }
                //System.Console.WriteLine(line);
                lineCounter++;
            }

            sr.Close();

            // Creating population of n individuals
            int numberOfIndividualsInPopulation = 8;
            Population p1 = new Population(numberOfIndividualsInPopulation);
            // Showing population
            p1.ShowPopulation();

            // Test individual
            //Individual i1 = new Individual();
            //i1.ShowIndividual();
            
            // Display distance table
            /*for (int i = 0; i<distanceTable.GetLength(0); i++)
            {
                for(int j = 0; j < distanceTable.GetLength(1); j++)
                {
                    Console.Write(distanceTable[i, j] + " ");
                }
                Console.WriteLine();
            }*/
        }
    }
}
