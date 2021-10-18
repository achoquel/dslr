using common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logreg_train.Models
{
    public class LogRegDatasModel
    {
        public static readonly string[] SELECTED_FEATURES = new[] { "Herbology", "Arithmancy", "Defense against the Dark Arts", "Divination", "Charms", "Ancient Runes" };
        public List<CsvEntryModel> BaseEntries { get; set; }

        public LogRegEntryModel[] X
        {
            get
            {
                LogRegEntryModel[] entries = new LogRegEntryModel[BaseEntries.Count];
                for(int i = 0; i < BaseEntries.Count; ++i)
                {
                    entries[i].X = new float[SELECTED_FEATURES.Length];
                    for (int j = 0; j < SELECTED_FEATURES.Length; ++j)
                    {
                        var type = typeof(CsvEntryModel);
                        entries[i].X[j] = (float)type.GetProperty(SELECTED_FEATURES[j]).GetValue(BaseEntries[i], null);
                    }
                }
                return entries;
            }
        }

        public int[] YForGryffindor
        {
            get
            {
                int[] res = new int[BaseEntries.Count];
                for (int i = 0; i < BaseEntries.Count; ++i)
                {
                    if (BaseEntries[i].House == "Gryffindor")
                        res[i] = 1;
                    else
                        res[i] = 0;
                    ++i;
                }
                return res;
            }
        }

        public int[] YForHufflepuff
        {
            get
            {
                int[] res = new int[BaseEntries.Count];
                for (int i = 0; i < BaseEntries.Count; ++i)
                {
                    if (BaseEntries[i].House == "Hufflepuff")
                        res[i] = 1;
                    else
                        res[i] = 0;
                    ++i;
                }
                return res;
            }
        }

        public int[] YForSlytherin
        {
            get
            {
                int[] res = new int[BaseEntries.Count];
                for (int i = 0; i < BaseEntries.Count; ++i)
                {
                    if (BaseEntries[i].House == "Slytherin")
                        res[i] = 1;
                    else
                        res[i] = 0;
                    ++i;
                }
                return res;
            }
        }

        public int[] YForRavenclaw
        {
            get
            {
                int[] res = new int[BaseEntries.Count];
                for (int i = 0; i < BaseEntries.Count; ++i)
                {
                    if (BaseEntries[i].House == "Ravenclaw")
                        res[i] = 1;
                    else
                        res[i] = 0;
                    ++i;
                }
                return res;
            }
        }
    }

    public class LogRegEntryModel
    {
        public float[] X { get; set; } = new float[LogRegDatasModel.SELECTED_FEATURES.Length];
    }
}
