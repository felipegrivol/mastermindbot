using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MasterMindBotChallenge.Models;
using MastermindBot.Bot;

namespace MasterMindBotChallengeTest
{
    [TestClass]
    public class BotTest
    {
        [TestMethod]
        public void testGessTest()
        {
            char[] solution = { '5', '2', '1', '1', '7', '8', '5', '3' };
            char[] guess = { '5', '2', '1', '1', '7', '8', '5', '3' };

            Attempt attempt = testGuess(guess, solution);
            Assert.AreEqual(8, attempt.Exact);

            char[] guess2 = { '9', '2', '5', '9', '9', '8', '5', '7' };
            attempt = testGuess(guess2, solution);
            Assert.AreEqual(3, attempt.Exact);
            Assert.AreEqual(2, attempt.Near);
        }

        [TestMethod]
        public void testBot()
        {
            int attemptsCount = 0;

            char[] solution = { '5', '2', '1', '1', '7', '4', '5', '3' };
            Attempt attempt;

            MastermindFitnessCalculator mastermindFitnessCalculator = new MastermindFitnessCalculator();

            char[] firstGuess = { '1', '1', '1', '1', '2', '2', '3', '3' };
            attempt = testGuess(firstGuess, solution);
            mastermindFitnessCalculator.addPreviousAttempt(attempt);

            do
            {
                GeneticAlgorithm ga = new GeneticAlgorithm(mastermindFitnessCalculator);

                char[] guess = ga.calculateNextGuess(20);

                attempt = testGuess(guess, solution);
                mastermindFitnessCalculator.addPreviousAttempt(attempt);

                attemptsCount++;
                Console.WriteLine("Attempt: {0}, Guess: {1}, Exact: {2}, Near: {3}", attemptsCount, printArray(attempt.Guess), attempt.Exact, attempt.Near);
            }
            while (attempt.Exact != 8 && attemptsCount <= 25);

            Console.WriteLine("Number of Attemps: " + attemptsCount);

            Assert.AreEqual(8, attempt.Exact);
            Assert.IsTrue(attemptsCount <= 25, "Bot didn't guess the right secret code in less than 25 attempts.");
        }

        private Attempt testGuess(char[] guess, char[] solution)
        {
            Attempt attempt = new Attempt();
            attempt.Guess = guess;

            int count = 0;
            int[] result = new int[solution.Length];
            this.initializeArray(result, 0);

            int[] places = new int[solution.Length];
            int[] places2 = new int[solution.Length];

            this.initializeArray(places, -1);
            this.initializeArray(places2, -1);

            // match exact
            for (int i = 0; i < solution.Length; i++)
            {
                if (guess[i] == solution[i])
                {
                    attempt.Exact++;
                    result[count] = 1;
                    count++;

                    places[i] = 1;
                    places2[i] = 1;
                }
            }

            if (attempt.Exact == solution.Length)
                return attempt;

            // match near
            for (int i = 0; i < solution.Length; i++)
            {
                for (int j = 0; j < solution.Length; j++)
                {
                    if ((i != j) && (places[i] != 1) && (places2[j] != 1))
                    {
                        if (guess[i] == solution[j])
                        {
                            attempt.Near++;
                            result[count] = 2;
                            count++;
                            places[i] = 1;
                            places2[j] = 1;
                            break;
                        }
                    }
                }
            }

            return attempt;
        }

        private void initializeArray(int[] array, int value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
        }

        private string printArray(char[] array)
        {
            string result = "";
            for (int i = 0; i < array.Length; i++)
            {
                result += array[i];
            }
            return result;
        }
    }
}
