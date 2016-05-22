using MasterMindBotChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MastermindBot.Bot
{
    public class MastermindFitnessCalculator
    {
        private List<Attempt> previousAttempts = new List<Attempt>();

        public float getFitness(Individual individual)
        {
            float fitness = 0;

            foreach (var previousAttempt in previousAttempts)
            {
                int[] comparedScore = calculateScore(individual.getGene(), previousAttempt);
                int quantityMatched = compareScore(comparedScore, getResultScoreArray(previousAttempt));

                fitness += (float)quantityMatched / (float)individual.getGene().Length;
            }

            fitness += .01f;

            return fitness;
        }

        private int[] calculateScore(char[] individualGene, Attempt attempt)
        {
            int exactMatches = 0;
            int nearMatches = 0;
            int count = 0;

            int[] result = new int[individualGene.Length];
            this.initializeArray(result, 0);

            int[] places = new int[individualGene.Length];
            int[] places2 = new int[individualGene.Length];

            this.initializeArray(places, -1);
            this.initializeArray(places2, -1);

            // match exact
            for (int i = 0; i < individualGene.Length; i++)
            {
                if (individualGene[i] == attempt.Guess[i])
                {
                    exactMatches++;
                    result[count] = 1;
                    count++;

                    places[i] = 1;
                    places2[i] = 1;
                }
            }

            if (exactMatches == individualGene.Length)
                return result;

            // match near
            for (int i = 0; i < individualGene.Length; i++)
            {
                for (int j = 0; j < individualGene.Length; j++)
                {
                    if ((i != j) && (places[i] != 1) && (places2[j] != 1))
                    {
                        if (individualGene[i] == attempt.Guess[j])
                        {
                            nearMatches++;
                            result[count] = 2;
                            count++;
                            places[i] = 1;
                            places2[j] = 1;
                            break; 
                        }
                    }
                }
            }

            return result;
        }

        private int compareScore(int[] individualScore, int[] attemptScore)
        {
            int matches = 0;

            for (int i = 0; i < individualScore.Length; i++)
            {
                if (attemptScore[i] == individualScore[i])
                    matches++;
            }

            return matches;
        }

        public int[] getResultScoreArray(Attempt attempt)
        {
            int[] result = new int[attempt.Guess.Length];
            int count = 0;

            for (int i = 0; i < attempt.Exact; i++)
            {
                result[count] = 1;
                count++;
            }

            for (int i = 0; i < attempt.Near; i++)
            {
                result[count] = 2;
                count++;
            }

            return result;
        }

        public void setPreviousAttempts(List<Attempt> attempts)
        {
            previousAttempts = attempts;
        }

        public void addPreviousAttempt(Attempt attempt)
        {
            previousAttempts.Add(attempt);
        }

        private void initializeArray(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }
    }
}