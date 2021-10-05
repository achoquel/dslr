using Alba.CsConsoleFormat;
using Alba.CsConsoleFormat.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Path { get; set; }

        public List<CsvEntryModel> Entries { get; set; }

        public List<NumericalFeatureModel> Features { get; set; }


        public void Describe()
        {
            var headerThickness = new LineThickness(LineWidth.Single, LineWidth.Single);
            var tableThickness = new LineThickness(LineWidth.Single, LineWidth.Single);

            var doc = new Document(
                "\n",
                new Span("Dataset: ") 
                { 
                    Color = ConsoleColor.Yellow 
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
                        GridLength.Star(1), 
                        GridLength.Star(1), 
                        GridLength.Star(1), 
                        GridLength.Star(1),
                        GridLength.Star(1)
                    },
                    Children = 
                    {
                        new Cell("") { Stroke = headerThickness},
                        Features.Select(f => new [] {
                            new Cell(f.FeatureName) { Stroke = headerThickness, Color = ConsoleColor.Yellow, TextAlign = TextAlign.Center, TextWrap = TextWrap.WordWrap, VerticalAlign = VerticalAlign.Center}
                        }),
                        new Cell("Count") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left},
                        Features.Select(f => new[]
                        {
                            new Cell(f.Count) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("Mean") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.Mean) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("Std") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.StdDev) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("Min") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.Min) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("25%") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.Q1) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("50%") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.Med) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("75%") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.Q3) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        }),
                        new Cell("Max") { Stroke = tableThickness, Color = ConsoleColor.DarkYellow, TextAlign = TextAlign.Left },
                        Features.Select(f => new[]
                        {
                            new Cell(f.Max) { Stroke = tableThickness, TextAlign = TextAlign.Right}
                        })
                    }
                }
            );
            ConsoleRenderer.RenderDocument(doc);
        }

        private void InitializeFeatures()
        {
            Features = new List<NumericalFeatureModel>()
            {
                new NumericalFeatureModel("Arithmancy", Entries.Select(e => e.Arithmancy).ToList()),
                new NumericalFeatureModel("Astromomy", Entries.Select(e => e.Astronomy).ToList()),
                new NumericalFeatureModel("Herbology", Entries.Select(e => e.Herbology).ToList()),
                new NumericalFeatureModel("Def. against the Dark Arts", Entries.Select(e => e.DefenseAgainstTheDarkArts).ToList()),
                new NumericalFeatureModel("Divination", Entries.Select(e => e.Divination).ToList()),
                new NumericalFeatureModel("Muggle Studies", Entries.Select(e => e.MuggleStudies).ToList()),
                new NumericalFeatureModel("Ancient Runes", Entries.Select(e => e.AncientRunes).ToList()),
                new NumericalFeatureModel("History of Magic", Entries.Select(e => e.HistoryOfMagic).ToList()),
                new NumericalFeatureModel("Transfiguration", Entries.Select(e => e.Transfiguration).ToList()),
                new NumericalFeatureModel("Potions", Entries.Select(e => e.Potions).ToList()),
                new NumericalFeatureModel("Care of Magical Creatures", Entries.Select(e => e.CareOfMagicalCreatures).ToList()),
                new NumericalFeatureModel("Charms", Entries.Select(e => e.Charms).ToList()),
                new NumericalFeatureModel("Flying", Entries.Select(e => e.Flying).ToList())
            };
        }
    }
}
