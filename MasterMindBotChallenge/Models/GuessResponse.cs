using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterMindBotChallenge.Models
{
    public class GuessResponse
    {
        public int code_length { get; set; }
        public string[] colors { get; set; }
        public string game_key { get; set; }
        public string guess { get; set; }
        public int num_guesses { get; set; }
        public PastResults[] past_results { get; set; }
        public PastResults result { get; set; }
        public bool solved { get; set; }
    }
}