using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterMindBotChallenge.Models
{
    public class NewGameResponse
    {
        public string[] colors { get; set; }
        public int code_length { get; set; }
        public string game_key { get; set; }
        public int num_guesses { get; set; }
        public string[] past_results { get; set; }
        public bool solved { get; set; }
    }
}