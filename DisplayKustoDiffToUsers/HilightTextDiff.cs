using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace DisplayKustoDiffToUsers
{
    public class HilightTextDiff
    {
         private readonly ISideBySideDiffBuilder diffBuilder;


        public void getDiff(string oldText, string newText)
        {
            var model = diffBuilder.BuildDiffModel(oldText ?? string.Empty, newText ?? string.Empty);

        }



    }
}
