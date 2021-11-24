using common.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace visualize.Controllers
{
    public class BaseController : Controller
    {
        public static DatasetModel mainDataset;

        public static LogRegTrainingResultsModel trainingResults;

        public static Tuple<int, string>[] predictionResults;
    }
}
