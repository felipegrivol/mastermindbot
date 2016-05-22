using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MastermindBot.Bot
{
    public class IndividualComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            if (((Individual)x).getFitness() > ((Individual)y).getFitness())
                return 1;
            else if (((Individual)x).getFitness() == ((Individual)y).getFitness())
                return 0;
            else
                return -1;
        }
    }
}