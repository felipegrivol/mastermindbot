using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterMindBotChallenge.Models
{
    public class GuessRequest
    {
        public string code { get; set; }
        public string game_key { get; set; }
    }
}