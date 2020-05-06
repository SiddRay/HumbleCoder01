using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Submission_Tracker.Model
{
    /// <summary>
    /// A data class which defines a result row.
    /// </summary>
    public class ResultRow 
    {
        public ResultRow()
        {
        }

        public Result SourceResult { get; set; } = new Result();

        public Result DestinationResult { get; set; } = new Result();


        public Comment Comment { get; set; } = new Comment();

        
    }
}
