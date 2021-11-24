using Alba.CsConsoleFormat;
using common.Controllers;
using common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace common.Models
{
    public class DatasetModel
    {
        public DatasetModel() { }

        public DatasetModel(List<CsvEntryModel> entries)
        {
            Entries = entries;
            InitializeFeatures();
        }

        /// <summary>
        /// Path of the dataset's file that we are using
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// The mode of the running program (Training, Predicting, Describe or Visualize)
        /// </summary>
        public ExecutionModeEnum Mode { get; set; }

        /// <summary>
        /// Entries from the parsed dataset
        /// </summary>
        public List<CsvEntryModel> Entries { get; set; }

        /// <summary>
        /// List of all the numerical features, without null marks
        /// </summary>
        public List<NumericalFeatureModel> Features { get; set; }

        /// <summary>
        /// List of all the numerical features, without entries that contain a null mark
        /// </summary>
        public List<NumericalFeatureModel> FullFeatures { get; set; }

        /// <summary>
        /// List of all the numerical features where null marks are filled by the mean of the feature
        /// </summary>
        public List<NumericalFeatureModel> FilledFeatures { get; set; }

        #region Public Methods

        /// <summary>
        /// Outputs a table that describes the dataset by providing statistical informations about it
        /// </summary>
        public void Describe()
        {
            var headerThickness = new LineThickness(LineWidth.Single, LineWidth.Single);
            var tableThickness = new LineThickness(LineWidth.Single, LineWidth.Single);
            var bday = Entries.Select(e => e.Birthday).Where(b => b.HasValue).Select(e => e.Value).ToList();
            var bdayStd = MathUtils.StdDev(bday);

            var doc = new Document(
                "\n",
                new Span("Dataset: ")
                {
                    Color = ConsoleColor.Red
                },
                Path,
                new Grid
                {
                    Color = ConsoleColor.Gray,
                    Columns =
                    {
                        GridLength.Auto,
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1),
                        GridLength.Star(1)
                    },
                    Children =
                    {
                       new Cell("") { Stroke = headerThickness},
                       new Cell("Count") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("Unique") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("Mean") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("Std") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("Min") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("25%") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("50%") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("75%") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new Cell("Max") { Stroke = tableThickness, Color = ConsoleColor.Red, TextAlign = TextAlign.Center },
                       new[]{ new Cell("Birthday"){ Stroke = tableThickness, Color = ConsoleColor.DarkRed, TextAlign = TextAlign.Left, TextWrap = TextWrap.WordWrap, VerticalAlign = VerticalAlign.Center },
                                                  new Cell(MathUtils.Count(Entries.Where(e => e.Birthday != null)).ToString("n0")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Count(Entries.Select(e => e.Birthday).Distinct()).ToString("n0")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Mean(bday).ToString("dd MMMM yyyy")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell($"{bdayStd.Year} years {bdayStd.Month} months {bdayStd.Day} days"){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Min(bday).ToString("dd MMMM yyyy")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Q1(bday).ToString("dd MMMM yyyy")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Med(bday).ToString("dd MMMM yyyy")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Q3(bday).ToString("dd MMMM yyyy")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                  new Cell(MathUtils.Max(bday).ToString("dd MMMM yyyy")){ Stroke = tableThickness, TextAlign = TextAlign.Right}
                       },
                       Features.Select(f => new[]{ new Cell(f.FeatureName) { Stroke = tableThickness, Color = ConsoleColor.DarkRed, TextAlign = TextAlign.Left, TextWrap = TextWrap.WordWrap, VerticalAlign = VerticalAlign.Center },
                                                   new Cell(f.Count.ToString("n0")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Values.Select(v => v.Value).Distinct().Count().ToString("n0")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Mean.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.StdDev.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Min.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Q1.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Med.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Q3.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right},
                                                   new Cell(f.Max.ToString("n6")){ Stroke = tableThickness, TextAlign = TextAlign.Right}
                       }),
                    }
                }
            );
            ConsoleRenderer.RenderDocument(doc);
        }

        /// <summary>
        /// Verifies that the dataset is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValidDataset()
        {
            //Not enough entries
            if (Entries.Count < 5)
            {
                return false;
            }
            //Empty or null entry
            if (Features.Any(f => f == null || f.Count < 5))
            {
                return false;
            }
            //An entry has a house specified in a prediction dataset
            if (Mode == ExecutionModeEnum.PREDICTION && Entries.Any(e => !string.IsNullOrEmpty(e.House)))
            {
                return false;
            }
            //An entry doesn't have a house specified in a training dataset
            if ((Mode == ExecutionModeEnum.DESCRIBE || Mode == ExecutionModeEnum.TRAINING || Mode == ExecutionModeEnum.VISUALIZE) && Entries.Any(e => string.IsNullOrEmpty(e.House)))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Initialize the NumericalFeatureModels that will be later used for describing, training, vizualisation and prediction.
        /// </summary>
        private void InitializeFeatures()
        {
            Features = new List<NumericalFeatureModel>()
            {
                new NumericalFeatureModel("Arithmancy", Entries.Where(e => e.Arithmancy.HasValue).Select(e => (e.Arithmancy.Value, e.House)).ToList()),
                new NumericalFeatureModel("Astronomy", Entries.Where(e => e.Astronomy.HasValue).Select(e => (e.Astronomy.Value, e.House)).ToList()),
                new NumericalFeatureModel("Herbology", Entries.Where(e => e.Herbology.HasValue).Select(e => (e.Herbology.Value, e.House)).ToList()),
                new NumericalFeatureModel("Defense against the Dark Arts", Entries.Where(e => e.DefenseAgainstTheDarkArts.HasValue).Select(e=> (e.DefenseAgainstTheDarkArts.Value, e.House)).ToList()),
                new NumericalFeatureModel("Divination", Entries.Where(e => e.Divination.HasValue).Select(e => (e.Divination.Value, e.House)).ToList()),
                new NumericalFeatureModel("Muggle Studies", Entries.Where(e =>e.MuggleStudies.HasValue).Select(e => (e.MuggleStudies.Value, e.House)).ToList()),
                new NumericalFeatureModel("Ancient Runes", Entries.Where(e => e.AncientRunes.HasValue).Select(e => (e.AncientRunes.Value, e.House)).ToList()),
                new NumericalFeatureModel("History of Magic", Entries.Where(e => e.HistoryOfMagic.HasValue).Select(e => (e.HistoryOfMagic.Value, e.House)).ToList()),
                new NumericalFeatureModel("Transfiguration", Entries.Where(e => e.Transfiguration.HasValue).Select(e => (e.Transfiguration.Value, e.House)).ToList()),
                new NumericalFeatureModel("Potions", Entries.Where(e => e.Potions.HasValue).Select(e => (e.Potions.Value, e.House)).ToList()),
                new NumericalFeatureModel("Care of Magical Creatures", Entries.Where(e => e.CareOfMagicalCreatures.HasValue).Select(e => (e.CareOfMagicalCreatures.Value, e.House)).ToList()),
                new NumericalFeatureModel("Charms", Entries.Where(e => e.Charms.HasValue).Select(e => (e.Charms.Value, e.House)).ToList()),
                new NumericalFeatureModel("Flying", Entries.Where(e => e.Flying.HasValue).Select(e => (e.Flying.Value, e.House)).ToList())
            };
            FullFeatures = new List<NumericalFeatureModel>()
            {
                new NumericalFeatureModel("Arithmancy", Entries.Where(e => e.IsFull()).Select(e => (e.Arithmancy.Value, e.House)).ToList()),
                new NumericalFeatureModel("Astronomy", Entries.Where(e => e.IsFull()).Select(e => (e.Astronomy.Value, e.House)).ToList()),
                new NumericalFeatureModel("Herbology", Entries.Where(e => e.IsFull()).Select(e => (e.Herbology.Value, e.House)).ToList()),
                new NumericalFeatureModel("Defense against the Dark Arts", Entries.Where(e => e.IsFull()).Select(e=> (e.DefenseAgainstTheDarkArts.Value, e.House)).ToList()),
                new NumericalFeatureModel("Divination", Entries.Where(e => e.IsFull()).Select(e => (e.Divination.Value, e.House)).ToList()),
                new NumericalFeatureModel("Muggle Studies", Entries.Where(e =>e.IsFull()).Select(e => (e.MuggleStudies.Value, e.House)).ToList()),
                new NumericalFeatureModel("Ancient Runes", Entries.Where(e => e.IsFull()).Select(e => (e.AncientRunes.Value, e.House)).ToList()),
                new NumericalFeatureModel("History of Magic", Entries.Where(e => e.IsFull()).Select(e => (e.HistoryOfMagic.Value, e.House)).ToList()),
                new NumericalFeatureModel("Transfiguration", Entries.Where(e => e.IsFull()).Select(e => (e.Transfiguration.Value, e.House)).ToList()),
                new NumericalFeatureModel("Potions", Entries.Where(e => e.IsFull()).Select(e => (e.Potions.Value, e.House)).ToList()),
                new NumericalFeatureModel("Care of Magical Creatures", Entries.Where(e => e.IsFull()).Select(e => (e.CareOfMagicalCreatures.Value, e.House)).ToList()),
                new NumericalFeatureModel("Charms", Entries.Where(e => e.IsFull()).Select(e => (e.Charms.Value, e.House)).ToList()),
                new NumericalFeatureModel("Flying", Entries.Where(e => e.IsFull()).Select(e => (e.Flying.Value, e.House)).ToList())
            };
            FilledFeatures = new List<NumericalFeatureModel>()
            {
                new NumericalFeatureModel("Arithmancy", Entries.Select(e => (e.Arithmancy ?? Features[0].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Astronomy", Entries.Select(e => (e.Astronomy ?? Features[1].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Herbology", Entries.Select(e => (e.Herbology ?? Features[2].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Defense against the Dark Arts", Entries.Select(e => (e.DefenseAgainstTheDarkArts ?? Features[3].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Divination", Entries.Select(e => (e.Divination ?? Features[4].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Muggle Studies", Entries.Select(e => (e.MuggleStudies ?? Features[5].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Ancient Runes", Entries.Select(e => (e.AncientRunes ?? Features[6].Mean, e.House)).ToList()),
                new NumericalFeatureModel("History of Magic", Entries.Select(e => (e.HistoryOfMagic ?? Features[7].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Transfiguration", Entries.Select(e => (e.Transfiguration ?? Features[8].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Potions", Entries.Select(e => (e.Potions ?? Features[9].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Care of Magical Creatures", Entries.Select(e => (e.CareOfMagicalCreatures ?? Features[10].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Charms", Entries.Select(e => (e.Charms ?? Features[11].Mean, e.House)).ToList()),
                new NumericalFeatureModel("Flying", Entries.Select(e => (e.Flying ?? Features[12].Mean, e.House)).ToList()),
            };
        }
        #endregion
    }
}
