using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterMindBotChallenge.Models
{
    public class GameData
    {
        private List<Attempt> _previousAttempts;

        public string game_key { get; set; }
        public List<Attempt> previousAttempts
        {
            get
            {
                if (_previousAttempts == null)
                    _previousAttempts = new List<Attempt>();

                return _previousAttempts;
            }
            set
            {
                _previousAttempts = value;
            }
        }

    }
}