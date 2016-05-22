using MasterMindBotChallenge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Web;

namespace MasterMindBotChallenge
{
    public static class AxiomzenMastermind
    {
        const string serviceUrl = "http://az-mastermind.herokuapp.com/";

        public static NewGameResponse newGame(NewGameRequest newGameRequest)
        {
            try
            {
                string url = serviceUrl + "new_game?user=" + newGameRequest.user;
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "text/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    DataContractJsonSerializer jsonResponseSerializer = new DataContractJsonSerializer(typeof(NewGameRequest));
                    jsonResponseSerializer.WriteObject(streamWriter.BaseStream, newGameRequest);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                    DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(NewGameResponse));
                    object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                    NewGameResponse jsonResponse = objResponse as NewGameResponse;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static GuessResponse guess(GuessRequest guessRequest)
        {
            try
            {
                string url = serviceUrl + "guess?game_key=" + guessRequest.game_key
                    + "&code=" + guessRequest.code;

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.Method = "POST";
                request.ContentType = "text/json";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    DataContractJsonSerializer jsonResponseSerializer = new DataContractJsonSerializer(typeof(GuessRequest));
                    jsonResponseSerializer.WriteObject(streamWriter.BaseStream, guessRequest);
                    streamWriter.Flush();
                    streamWriter.Close();
                }

                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        throw new Exception(String.Format("Server error (HTTP {0}: {1}).", response.StatusCode, response.StatusDescription));

                    DataContractJsonSerializer jsonResponseSerializer = new DataContractJsonSerializer(typeof(GuessResponse));
                    object objResponse = jsonResponseSerializer.ReadObject(response.GetResponseStream());
                    GuessResponse jsonResponse = objResponse as GuessResponse;
                    return jsonResponse;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public static string convertGuess(string guess)
        {
            string guessConverted = "";

            char[] chars = { 'R', 'B', 'G', 'Y', 'P', 'O', 'C', 'M' };

            for (int i = 0; i < guess.Length; i++)
            {
                guessConverted += chars[Int16.Parse(guess[i].ToString()) - 1];
            }

            return guessConverted;
        }
    }
}