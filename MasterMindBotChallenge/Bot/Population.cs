using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MastermindBot.Bot
{
    public class Population
    {
        bool isSorted = false;
        ArrayList individuals = new ArrayList();

        public void initializePopulation(int size, MastermindFitnessCalculator mastermindFitnessCalculator)
        {
            for (int i = 0; i < size; i++)
            {
                individuals.Add(new Individual(mastermindFitnessCalculator).generateIndividual());
            }
        }

        public void sortPopulation()
        {
            individuals.Sort(new IndividualComparer());
            isSorted = true;
        }

        public Individual getFittest()
        {
            if (!isSorted)
                sortPopulation();

            return (Individual)individuals[individuals.Count - 1];
        }

        public Individual getSecondFittest()
        {
            if (!isSorted)
                sortPopulation();

            return (Individual)individuals[individuals.Count - 2];
        }

        public int getSize()
        {
            return individuals.Count;
        }

        public Individual getIndividual(int index)
        {
            return (Individual)individuals[index];
        }

        public void removeIndividual(int index)
        {
            individuals.RemoveAt(index);
        }

        public void addIndividual(Individual individual)
        {
            individuals.Add(individual);
            isSorted = false;
        }
    }
}