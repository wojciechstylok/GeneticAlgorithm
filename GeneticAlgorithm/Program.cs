using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace GeneticAlgorithm
{
    class Program
    {
        public static int individualLength;
        public static int[,] distanceTable;

        private static Random rand = new Random();
        private static Random rando = new Random();
        private static Random rnd = new Random();
        private static int numberOfIndividualsInPopulation = 30;
        private static int k = 3;
        private static int pk = 7500; // 100% = 10 000
        static void Main(string[] args)
        {
            var fileName = "berlin52.txt";
            var path = @"C:\Users\Wojciech\Desktop\" + fileName;
            StreamReader sr = new StreamReader(path);

            string line;
            int lineCounter = 0;
            if ((line = sr.ReadLine()) != null)
            {
                individualLength = int.Parse(line.Trim());
            }

            distanceTable = new int[individualLength, individualLength];

            while ((line = sr.ReadLine()) != null)
            {
                var singleLine = line.Trim().Split(" ");
                for (int i = 0; i < singleLine.Length; i++)
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

            Console.WriteLine("---------------------------------------------------------------------------");

            Population p2 = new Population(numberOfIndividualsInPopulation);
            p2 = TournamentSelection(p1);
            p2.ShowPopulation();

            Console.WriteLine("---------------------------------------------------------------------------");

            Population p3 = new Population(numberOfIndividualsInPopulation);
            p2.population.CopyTo(p3.population, 0);
            p3 = PmxCrossing(p3);
            //p3.ShowPopulation();


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
        private static Population TournamentSelection(Population inputPopulation)
        {
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
                //outputPopulation.population[i] = inputPopulation.population[index];
                inputPopulation.population[index].individual.CopyTo(outputPopulation.population[i].individual, 0);
                outputPopulation.population[i].rate = inputPopulation.population[index].rate;
            }

            return outputPopulation;
        }

        private static Population PmxCrossing(Population inputPopulation)
        {
            Population outputPopulation = new Population(numberOfIndividualsInPopulation);
            for (int i = 0; i < outputPopulation.population.Length - 2; i += 2)
            {
                if (i % 2 == 0)
                {
                    if (rando.Next(0, 9999) < pk)
                    {
                        int ppp = rand.Next(0, individualLength);
                        int dpp = rand.Next(ppp + 1, individualLength);

                        Console.WriteLine("ppp: " + ppp + " dpp: " + dpp + " " + individualLength);

                        Individual child1 = inputPopulation.population[i];
                        Individual child2 = inputPopulation.population[i + 1];

                        Console.WriteLine("---POCZĄTEK-------------------------------------------------------------------");
                        child1.ShowIndividual();
                        child2.ShowIndividual();

                        #region Podmiana środków

                        int[] ch1 = new int[child1.individual.Length];
                        int[] ch2 = new int[child2.individual.Length];

                        List<int> ch1swap = new List<int>();
                        List<int> ch2swap = new List<int>();

                        for (int p = 0; p < ch1.Length; p++)
                        {
                            if (p >= ppp && p <= dpp)
                            {
                                var sadda = child2.individual[p];
                                var asadasdasd = child1.individual[p];
                                ch1[p] = child2.individual[p];
                                ch2[p] = child1.individual[p];
                                ch1swap.Add(ch1[p]);
                                ch2swap.Add(ch2[p]);
                            }
                            else
                            {
                                ch1[p] = -1;
                                ch2[p] = -1;
                            }
                        }

                        #endregion //Podmiana środków   

                        //while (areChildrenNotMixed)
                        //{
                        //    for (int index = 0; index < ppp; index++)
                        //    {
                        //        tmpCh1 = new int[ch1.Length];
                        //        tmpCh2 = new int[ch2.Length];

                        //        if (ch2swap.Contains(ch1[index]))
                        //        {
                        //            var indexOf1 = Array.IndexOf(ch2, ch1[index]);
                        //            var indexValue = ch2[indexOf1];
                        //            ch1[indexOf1] = indexValue;
                        //        }
                        //    }
                        //}


                        //child1.individual = ch1;
                        ch1.CopyTo(child1.individual, 0);
                        //child2.individual = ch2;
                        ch2.CopyTo(child2.individual, 0);

                        //outputPopulation.population[i] = child1;
                        child1.individual.CopyTo(outputPopulation.population[i].individual, 0);
                        //outputPopulation.population[i + 1] = child2;
                        child2.individual.CopyTo(outputPopulation.population[i + 1].individual, 0);

                        //child1.SetRate();
                        //child2.SetRate();

                        Console.WriteLine("---KONIEC-------------------------------------------------------------------");
                        child1.ShowIndividual();
                        child2.ShowIndividual();
                    }
                }
            }

            return outputPopulation;
        }
        /// <summary>
        /// Sprawdzanie czy dana liczba może być przepisana
        /// </summary>
        /// <param name="child1">Dziecko do wypełnienia</param>
        /// <param name="child2">Dziecko przed swapem środków</param>
        /// <param name="searchingNumber">Poszukiwana wartość z child1 do przepisania</param>
        private void CheckIfContains(int[] child1, int[] child2, int searchingNumber)
        {
            while (!Array.Exists<int>(child1, x => x == searchingNumber))
            {
                var index = Array.IndexOf(child1, searchingNumber);
                child1[index]
            }
        }
    }
}
