using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    class Population
    {
        public Individual[] population;
        public Individual bestIndividual;

        public Population() { }
        public Population(int numberOfIndividuals)
        {
            Individual[] pop = new Individual[numberOfIndividuals];
            for (int i = 0; i < numberOfIndividuals; i++)
            {
                pop[i] = new Individual();
            }
            this.population = pop;
        }

        public void ShowPopulation()
        {
            foreach (Individual i in population)
            {
                i.ShowIndividual();
                Console.WriteLine();
            }
        }

        public void SearchForTheBestIndividual()
        {
            var currentBest = population[0];
            foreach (Individual i in population)
            {
                if (i.rate < currentBest.rate)
                {
                    currentBest = i;
                }
            }
            bestIndividual = currentBest;
        }
    }
}
