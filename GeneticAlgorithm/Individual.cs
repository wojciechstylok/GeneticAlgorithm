using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgorithm
{
    public class Individual
    {
        public int[] individual;
        public int rate;
        private readonly int arrayLength = Program.arrayLength;
        public Individual()
        {
            this.individual = new int[arrayLength];
            foreach (int i in individual)
                individual[i] = -1;

            Random rnd = new Random();
            int randomValue;
            for (int i = 0; i < arrayLength; i++)
            {
                randomValue = rnd.Next(0, arrayLength - 1);
                while (Array.IndexOf(this.individual, randomValue) != -1)
                {
                    randomValue = rnd.Next(0, arrayLength - 1);
                    if (this.individual[arrayLength - 1] != -1)
                    {
                        break;
                    }
                }
                this.individual[i] = randomValue;
            }
            rate = SetRate();
        }

        public void ShowIndividual()
        {
            foreach (int i in individual)
                Console.Write(i + " ");
            Console.WriteLine("Rate: " + this.rate);
        }

        private int SetRate()
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
            return sum;
        }
    }
}
