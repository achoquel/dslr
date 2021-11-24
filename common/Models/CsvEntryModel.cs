using System;
using TinyCsvParser.Mapping;

namespace common.Models
{
    public class CsvEntryModel
    {
        public int Index { get; set; }

        public string House { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime? Birthday { get; set; }

        public string BestHand { get; set; }

        public float? Arithmancy { get; set; }

        public float? Astronomy { get; set; }

        public float? Herbology { get; set; }

        public float? DefenseAgainstTheDarkArts { get; set; }

        public float? Divination { get; set; }

        public float? MuggleStudies { get; set; }

        public float? AncientRunes { get; set; }

        public float? HistoryOfMagic { get; set; }

        public float? Transfiguration { get; set; }

        public float? Potions { get; set; }

        public float? CareOfMagicalCreatures { get; set; }

        public float? Charms { get; set; }

        public float? Flying { get; set; }

        internal bool IsFull()
        {
            return Arithmancy.HasValue
                && Astronomy.HasValue
                && Herbology.HasValue
                && DefenseAgainstTheDarkArts.HasValue
                && Divination.HasValue
                && MuggleStudies.HasValue
                && AncientRunes.HasValue
                && HistoryOfMagic.HasValue
                && Transfiguration.HasValue
                && Potions.HasValue
                && CareOfMagicalCreatures.HasValue
                && Charms.HasValue
                && Flying.HasValue;
        }
    }

    public class CsvEntryModelMapping : CsvMapping<CsvEntryModel>
    {
        public CsvEntryModelMapping() : base()
        {
            MapProperty(0, x => x.Index);
            MapProperty(1, x => x.House);
            MapProperty(2, x => x.FirstName);
            MapProperty(3, x => x.LastName);
            MapProperty(4, x => x.Birthday);
            MapProperty(5, x => x.BestHand);
            MapProperty(6, x => x.Arithmancy);
            MapProperty(7, x => x.Astronomy);
            MapProperty(8, x => x.Herbology);
            MapProperty(9, x => x.DefenseAgainstTheDarkArts);
            MapProperty(10, x => x.Divination);
            MapProperty(11, x => x.MuggleStudies);
            MapProperty(12, x => x.AncientRunes);
            MapProperty(13, x => x.HistoryOfMagic);
            MapProperty(14, x => x.Transfiguration);
            MapProperty(15, x => x.Potions);
            MapProperty(16, x => x.CareOfMagicalCreatures);
            MapProperty(17, x => x.Charms);
            MapProperty(18, x => x.Flying);
        }
    }
}
