using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using MasterMindBotChallenge.Models;
using System.Collections.Concurrent;
using MastermindBot.Bot;

namespace MasterMindBotChallenge
{
    public class MastermindHub : Hub
    {
        public static readonly ConcurrentDictionary<string, GameData> _games = new ConcurrentDictionary<string, GameData>();

        public void startGame(string name)
        {
            NewGameRequest request = new NewGameRequest() { user = name };
            NewGameResponse response = AxiomzenMastermind.newGame(request);

            GameData gameData = new GameData() { game_key = response.game_key };
            _games[Context.ConnectionId] = gameData;

            Clients.Client(Context.ConnectionId).postHistory("New game started", "Start guessing a 8 digit code, with values from 1 to 8.");
        }

        public void sendGuess(string guess)
        {
            if (!_games.Keys.Contains(Context.ConnectionId))
            {
                Clients.Client(Context.ConnectionId).postHistory("Error", "Start a new game before sending guesses");
                return;
            }

            int guessInt;
            if (guess.Length != 8 || !int.TryParse(guess, out guessInt))
            {
                Clients.Client(Context.ConnectionId).postHistory("Error", "Wrong guess format, must be 8 digit code, with values from 1 to 8");
                return;
            }

            GuessRequest request = new GuessRequest() { game_key = _games[Context.ConnectionId].game_key, code = AxiomzenMastermind.convertGuess(guess) };
            GuessResponse response = AxiomzenMastermind.guess(request);

            GameData gameData = _games[Context.ConnectionId];
            gameData.previousAttempts.Add(new Attempt() { Guess = guess.ToCharArray(), Exact = response.result.exact, Near = response.result.near });

            Clients.Client(Context.ConnectionId).postHistory("Guess", string.Format("Code: {0}, Exact: {1}, Near: {2}, Attempts: {3}", guess, response.result.exact, response.result.near, response.num_guesses));

            if (response.solved)
            {
                Clients.Client(Context.ConnectionId).postHistory("Congratulations!", "You guessed the secret code!");
            }
        }

        public void botGuess()
        {
            if (!_games.Keys.Contains(Context.ConnectionId))
            {
                Clients.Client(Context.ConnectionId).postHistory("Error", "Start a new game before sending guesses");
                return;
            }

            GameData gameData = _games[Context.ConnectionId];
            string guess;
            if (gameData.previousAttempts.Count == 0)
            {
                guess = "11112233"; // Fixed first guess;
            }
            else
            {
                MastermindFitnessCalculator mastermindFitnessCalculator = new MastermindFitnessCalculator();
                mastermindFitnessCalculator.setPreviousAttempts(gameData.previousAttempts);

                GeneticAlgorithm ga = new GeneticAlgorithm(mastermindFitnessCalculator);

                char[] guessArray = ga.calculateNextGuess(20);
                guess = new string(guessArray);
            }

            GuessRequest request = new GuessRequest() { game_key = _games[Context.ConnectionId].game_key, code = AxiomzenMastermind.convertGuess(guess) };
            GuessResponse response = AxiomzenMastermind.guess(request);

            gameData.previousAttempts.Add(new Attempt() { Guess = guess.ToCharArray(), Exact = response.result.exact, Near = response.result.near });

            Clients.Client(Context.ConnectionId).postHistory("Guess", string.Format("Code: {0}, Exact: {1}, Near: {2}, Attempts: {3}", guess, response.result.exact, response.result.near, response.num_guesses));

            if (response.solved)
            {
                Clients.Client(Context.ConnectionId).postHistory("Congratulations!", "You guessed the secret code!");
            }
        }
    }
}