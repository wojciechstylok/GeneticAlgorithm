using System;
using System.Collections;
using System.IO;

namespace GeneticAlgorithm
{
    class Program
    {
        public static int arrayLength;
        public static int[,] distanceTable;

        private static int numberOfIndividualsInPopulation = 10;
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
            Population p1 = new Population(numberOfIndividualsInPopulation);
            // Showing population
            p1.ShowPopulation();

            int tournamentSelectionParamether = 3;

            Console.WriteLine("---------------------------------------------------------------------------");

            TournamentSelection(p1, tournamentSelectionParamether).ShowPopulation();


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
        private static Population TournamentSelection(Population inputPopulation, int k)
        {
            Random rnd = new Random();
            Population outputPopulation = new Population(numberOfIndividualsInPopulation);

            for (int i = 0; i < inputPopulation.population.Length; i++)
            {
                int randomIndex = rnd.Next(0, inputPopulation.population.Length - 1);
                int index = randomIndex;
                for (int j = 0; j < k; j++)
                {
                    randomIndex = rnd.Next(0, inputPopulation.population.Length - 1);
                    if (inputPopulation.population[randomIndex].rate > inputPopulation.population[index].rate)
                    {
                        index = randomIndex;
                    }
                }
                outputPopulation.population[i] = inputPopulation.population[index];
            }

            return outputPopulation;
        }
    }
}
