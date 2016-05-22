using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MastermindBot.Bot
{
    public class GeneticAlgorithm
    {
        const float killFitnessRate = 0.0f;

        int populationSize = 2000;
        double mutationRate = 10; // percentage
        Random rand = new Random();
        MastermindFitnessCalculator mastermindFitnessCalculator;

        public GeneticAlgorithm(MastermindFitnessCalculator mastermindFitnessCalculator)
        {
            this.mastermindFitnessCalculator = mastermindFitnessCalculator;
        }

        public char[] calculateNextGuess(int generations)
        {
            Population population = new Population();
            population.initializePopulation(populationSize, mastermindFitnessCalculator);

            for (int i = 0; i < generations; i++)
            {
                population = makeNextGeneration(population);
            }

            return population.getFittest().getGene();
        }

        private Population makeNextGeneration(Population population)
        {
            Population nextPopulation = new Population();

            killPopulation(population);

            for (int i = 0; i < populationSize; i++)
            {
                Individual individual1 = population.getIndividual(rand.Next(population.getSize() - 1));
                Individual individual2 = population.getIndividual(rand.Next(population.getSize() - 1));
                Individual newIndiv1 = crossover(individual1, individual2);
                Individual newIndiv2 = crossover(individual2, individual1);

                Population family = new Population();
                family.addIndividual(individual1);
                family.addIndividual(individual2);
                family.addIndividual(newIndiv1);
                family.addIndividual(newIndiv2);

                Individual fittest = family.getFittest();
                Individual secondFittest = family.getSecondFittest();

                mutate(fittest);
                nextPopulation.addIndividual(fittest);

                mutate(secondFittest);
                nextPopulation.addIndividual(secondFittest);
            }

            return nextPopulation;
        }

        private Individual crossover(Individual individual1, Individual individual2)
        {
            char[] gene1 = individual1.getGene();
            char[] gene2 = individual2.getGene();
            char[] newGene = new char[gene1.Length];

            for (int i = 0; i < gene1.Length; i++)
            {
                if (i < gene1.Length / 2)
                    newGene[i] = gene1[i];
                else
                    newGene[i] = gene2[i];
            }

            Individual newChild = new Individual(mastermindFitnessCalculator);
            newChild.setGene(newGene);

            return newChild;
        }

        private void mutate(Individual individual)
        {
            char[] gene = individual.getGene();

            for (int i = 0; i < gene.Length; i++)
            {
                if (rand.Next(100) <= mutationRate)
                {
                    gene[i] = Individual.getRandomPossibleGeneValue();
                }
            }

            individual.setGene(gene);
        }

        private void killPopulation(Population population)
        {
            for (int i = 0; i < population.getSize(); i++)
            {
                if (population.getIndividual(i).getFitness() <= killFitnessRate)
                {
                    population.removeIndividual(i);
                    i--;
                }
            }
        }
    }
}