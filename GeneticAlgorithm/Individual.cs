using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class Individual
    {
        public int[] individual;
        public int rate;
        private readonly int arrayLength = Program.individualLength;
        public Individual()
        {
            this.individual = new int[arrayLength];
            for(int i = 0; i < arrayLength; i++)
            {
                individual[i] = -1;
            }                

            Random rnd = new Random();
            int randomValue;
            for (int i = 0; i < arrayLength; i++)
            {
                randomValue = rnd.Next(0, arrayLength);
                while (Array.IndexOf(this.individual, randomValue) != -1)
                {
                    if (this.individual[arrayLength - 1] != -1)
                    {
                        break;
                    }
                    randomValue = rnd.Next(0, arrayLength);
                }
                this.individual[i] = randomValue;
            }
            rate = SetRate();
        }

        public void ShowIndividual()
        {
            foreach (int i in individual)
                Console.Write(i + "-");
            Console.WriteLine(this.rate);
        }

        public int SetRate()
        {
            int sum = 0;
            for(int i = 0; i < this.individual.Length; i++)
            {
                if (i + 1 != this.individual.Length)
                {
                    sum += Program.distanceTable[individual[i], individual[i + 1]];
                }
                else if (i + 1 == this.individual.Length)
                {
                    sum += Program.distanceTable[individual[0], individual[i]];
                }
            }
            this.rate = sum;
            return sum;
        }

        public void SortIndividual()
        {
            Array.Sort(individual);
            Console.Write("Sorted individual: ");
            foreach (int i in individual)
                Console.Write(i + " ");
        }
    }
}
