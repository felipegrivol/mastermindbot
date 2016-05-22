using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterMindBotChallenge.Models
{
    public class Attempt
    {
        public char[] Guess { get; set; }
        public int Exact { get; set; }
        public int Near { get; set; }
    }
}