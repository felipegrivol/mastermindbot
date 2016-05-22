using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterMindBotChallenge.Models
{
    public class PastResults
    {
        public int exact { get; set; }
        public string guess { get; set; }
        public int near { get; set; }
    }
}