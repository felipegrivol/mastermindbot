using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MastermindBot.Bot
{
    public class Individual
    {
        static char[] possibleValues = { '1', '2', '3', '4', '5', '6', '7', '8' };
        const int geneSize = 8;
        static Random random = new Random();

        char[] gene = new char[geneSize];
        float fitness = 0;

        MastermindFitnessCalculator mastermindFitnessCalculator;

        public Individual(MastermindFitnessCalculator mastermindFitnessCalculator)
        {
            this.mastermindFitnessCalculator = mastermindFitnessCalculator;
        }

        public Individual generateIndividual()
        {
            for (int i = 0; i < geneSize; i++)
            {
                this.gene[i] = possibleValues[random.Next(0, possibleValues.Length - 1)];
            }

            return this;
        }

        public float getFitness()
        {
            if (this.fitness == 0)
            {
                this.fitness = this.mastermindFitnessCalculator.getFitness(this);
            }

            return this.fitness;
        }

        public char[] getGene()
        {
            return this.gene;
        }

        public void setGene(char[] value)
        {
            this.gene = value;
            this.fitness = 0;
        }

        public static char getRandomPossibleGeneValue()
        {
            return possibleValues[random.Next(0, possibleValues.Length - 1)];
        }
    }
}