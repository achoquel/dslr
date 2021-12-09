using logreg_train.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using visualize.Models;

namespace visualize.Controllers
{
    public class TrainController : BaseController
    {
        public IActionResult Index(TrainIndexViewModel vm)
        {
            if (mainDataset != null && mainDataset.IsValidDataset() && trainingResults != null)
            {
                var newVm = new TrainIndexViewModel()
                {
                    Dataset = mainDataset,
                    TrainingResults = trainingResults,
                    LossChart = ChartsController.GenerateLossChart(),
                    Error = vm != null && vm.Error,
                    ErrorMessage = vm?.ErrorMessage
                };
                return View(newVm);
            }
            return RedirectToAction("Index", "Home", routeValues: new HomeIndexViewModel() { Error = true, ErrorMessage = "An error occurred." });
        }

        [HttpPost]
        public IActionResult Index(int epochsInput, string lrInput)
        {
            try
            {
                if (mainDataset != null && mainDataset.IsValidDataset())
                {
                    if (!float.TryParse(lrInput, out float lr))
                    {
                        lr = 0.1f;
                    }

                    trainingResults = LogregController.Train(mainDataset, progressBar: false, epochs: epochsInput, lr: lr);
                    var lossChart = ChartsController.GenerateLossChart();
                    return View(new TrainIndexViewModel() { Dataset = mainDataset, TrainingResults = trainingResults, LossChart = lossChart });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("Index", "Home", routeValues: new HomeIndexViewModel() { Error = true, ErrorMessage = "An error occurred ! " + e.Message });
            }
            return RedirectToAction("Index", "Home", routeValues: new HomeIndexViewModel() { Error = true, ErrorMessage = "An error occurred." });

        }
    }
}
