using common.Models;
using Highsoft.Web.Mvc.Charts.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace visualize.Models
{
    public class TrainIndexViewModel
    {
        public DatasetModel Dataset { get; set; }

        public LogRegTrainingResultsModel TrainingResults { get; set; }

        public List<string> Courses
        {
            get
            {
                return Dataset?.Features?.Select(f => f.FeatureName).Distinct().ToList();
            }
        }

        public HighchartsRenderer LossChart { get; set; }

        public bool Error { get; set; }

        public string ErrorMessage { get; set; }
    }
}
