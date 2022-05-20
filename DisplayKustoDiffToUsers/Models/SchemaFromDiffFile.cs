using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisplayKustoDiffToUsers.Models
{
    public class SchemaFromDiffFile
    {
        //TODO: create a hash set for each cluster 
        public static HashSet<KustoCommandModel> schemaDiffCommands = new HashSet<KustoCommandModel>();
    }
}
