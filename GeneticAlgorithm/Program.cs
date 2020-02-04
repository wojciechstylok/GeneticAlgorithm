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
        private static Random random = new Random();
        private static Random rnd = new Random();
        private static int numberOfIndividualsInPopulation = 70;
        private static int k = 3;
        private static int pk = 7500; // 100% = 10 000
        private static int pm = 100; // 100% = 10 000
        private static int iterations = 15000000;
        static void Main(string[] args)
        {
            var fileName = "bays29.txt";
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
                lineCounter++;
            }

            sr.Close();

            Population p1 = new Population();
            Population p2 = new Population();
            Population p3 = new Population(numberOfIndividualsInPopulation);
            Individual bestIndividual = new Individual();

            #region Główna pętla
            p1 = new Population(numberOfIndividualsInPopulation);
            for (int i = 0; i < iterations; i++)
            {
                
                //p1.population[0] = bestIndividual;
                p2 = TournamentSelection(p1);
                p2.population.CopyTo(p3.population, 0);
                p3 = PmxCrossing(p3);
                p3 = MutationByInversion(p3);
                p3.SearchForTheBestIndividual();

                if (p3.bestIndividual.rate < bestIndividual.rate)
                {
                    bestIndividual = p3.bestIndividual;
                }

                if (i % 5000 == 0)
                {
                    bestIndividual.ShowIndividual();
                    pm += 50;
                }
                if (i % 500000 == 0)
                {
                    Console.WriteLine($"{DateTime.Now} iteracja: {i}");
                }
            }

            Console.WriteLine("---WINNER---");
            bestIndividual.ShowIndividual();

            p3.population.CopyTo(p1.population, 0);

            #endregion // Główna pętla
        }
        private static Population TournamentSelection(Population inputPopulation)
        {
            Population outputPopulation = new Population(numberOfIndividualsInPopulation);

            for (int i = 0; i < inputPopulation.population.Length; i++)
            {
                int randomIndex = rnd.Next(0, inputPopulation.population.Length); // otwarte z prawej strony
                int index = randomIndex;
                for (int j = 0; j < k; j++)
                {
                    randomIndex = rnd.Next(0, inputPopulation.population.Length);
                    if (inputPopulation.population[randomIndex].rate > inputPopulation.population[index].rate)
                    {
                        index = randomIndex;
                    }
                }
                inputPopulation.population[index].individual.CopyTo(outputPopulation.population[i].individual, 0);
                outputPopulation.population[i].rate = inputPopulation.population[index].rate;
            }

            return outputPopulation;
        }

        private static Population PmxCrossing(Population inputPopulation)
        {
            Population outputPopulation = inputPopulation;
            for (int i = 0; i < outputPopulation.population.Length - 2; i += 2)
            {
                if (i % 2 == 0)
                {
                    if (random.Next(0, 9999) < pk)
                    {
                        int ppp = rand.Next(0, individualLength - 2);
                        int dpp = rand.Next(ppp + 1, individualLength);

                        //Console.WriteLine("ppp: " + ppp + " dpp: " + dpp + " " + individualLength);

                        Individual child1 = inputPopulation.population[i];
                        Individual child2 = inputPopulation.population[i + 1];

                        #region Podmiana środków

                        int[] ch1 = new int[child1.individual.Length];
                        int[] ch2 = new int[child2.individual.Length];

                        List<int> ch1swap = new List<int>();
                        List<int> ch2swap = new List<int>();

                        for (int p = 0; p < ch1.Length; p++)
                        {
                            if (p >= ppp && p <= dpp)
                            {
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

                        #region Wstawianie na początku i końcu

                        Rewrite(ch1, child1.individual, child2.individual, 0, ppp, ch1swap);
                        Rewrite(ch1, child1.individual, child2.individual, dpp + 1, ch1.Length, ch1swap);

                        Rewrite(ch2, child2.individual, child1.individual, 0, ppp, ch2swap);
                        Rewrite(ch2, child2.individual, child1.individual, dpp + 1, ch2.Length, ch2swap);

                        ch1.CopyTo(child1.individual, 0);
                        ch2.CopyTo(child2.individual, 0);

                        #endregion // Wstawianie na początku i końcuw

                        child1.SetRate();
                        child2.SetRate();

                        child1.individual.CopyTo(outputPopulation.population[i].individual, 0);
                        child2.individual.CopyTo(outputPopulation.population[i + 1].individual, 0);
                    }
                }
            }

            return outputPopulation;
        }

        private static void Rewrite(int[] child, int[] parent1, int[] parent2, int start, int koniec, List<int> swap)
        {
            for (int i = start; i < koniec; i++)
            {
                int gene = parent1[i];
                int geneIndex;
                while (swap.Contains(gene))
                {
                    geneIndex = Array.IndexOf(parent1, gene);
                    gene = parent2[geneIndex];
                }
                swap.Add(gene);
                child[i] = gene;
            }
        }

        private static Population MutationByInversion(Population inputPopulation)
        {
            Population outputPopulation = inputPopulation;
            for (int i = 0; i < outputPopulation.population.Length; i++)
            {
                for (int j = 0; j<outputPopulation.population[i].individual.Length; j++)
                {
                    if (random.Next(9998, 9999) < pm)
                    {
                        int indZam = rand.Next(0, individualLength - 1);
                        int tmp = outputPopulation.population[i].individual[j];
                        outputPopulation.population[i].individual[j]= outputPopulation.population[i].individual[indZam];
                        outputPopulation.population[i].individual[indZam]=tmp;
                    }
                }
                
                if (random.Next(0, 9999) < pm)
                {
                    int ppp = rand.Next(0, individualLength - 2);
                    int dpp = rand.Next(ppp + 1, individualLength);

                    Individual child = outputPopulation.population[i];

                    //int[] swap = new int[dpp - ppp + 1];
                    //int index = 0;
                    for (int g = ppp, gg = dpp; g<gg; g++, gg--)
                    {
                        int tmp = child.individual[g];
                        child.individual[g] = child.individual[gg];
                        child.individual[gg] = tmp;
                    }
                    //for (int g = ppp; g <= dpp; g++)
                    //{
                    //    swap[index] = child.individual[g];
                    //    index++;
                    //}

                    //Array.Reverse(swap);

                    //swap.CopyTo(child.individual, ppp);
                    child.SetRate();
                }
                //else
                //{
                //    int[] poczatek = new int[ppp - 1];
                //    int[] koniec = new int[child.individual.Length - dpp - 1];
                //    int indexPoczatek = 0;
                //    int indexKoniec = 0;
                //    for(int x = 0; x < ppp; x++)
                //    {
                //        poczatek[indexPoczatek] = child.individual[x - 1];
                //        indexPoczatek++;
                //    }
                //    for (int y = 0; y < ppp; y++)
                //    {
                //        koniec[indexKoniec] = child.individual[y];
                //        indexKoniec++;
                //    }
                //    swap.CopyTo(child.individual, 0);
                //    koniec.CopyTo(child.individual, swap.Length - 1);
                //    poczatek.CopyTo(child.individual, swap.Length + koniec.Length - 1);
                //    child.SetRate();
                //}
            }

            return outputPopulation;
        }
    }
}