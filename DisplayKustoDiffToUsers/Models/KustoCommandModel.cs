using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisplayKustoDiffToUsers.Models
{
    public class KustoCommandModel
    {

        public string KustoObjectName { get; set; } //name of function or table in command 
        public string PreviousPublicSchema { get; set; }

        public string UpdatedPublicSchema { get; set; }

        public string SuggestedTranslation { get; set; }

        public string CurrentAGCSchema { get; set; }

        public bool CommandSentSuccesfully = false;

        public KustoCommandModel(string name, string previousSchema, string newSchema, string suggestedTranslation, string currentAGC) 
        {
            KustoObjectName = name;
            PreviousPublicSchema = previousSchema;
            UpdatedPublicSchema = newSchema;
            SuggestedTranslation = suggestedTranslation;
            CurrentAGCSchema = currentAGC;
        }



    }
}
