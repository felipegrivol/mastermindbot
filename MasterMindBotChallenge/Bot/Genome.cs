using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MastermindBot.Bot
{
    public class Genome
    {
        public long Length;
        public int CrossoverPoint;
        public int MutationIndex;
        public float CurrentFitness = 0.0f;

    }
}