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
        private static readonly string[] SELECTED_FEATURES = new[] { "", "", "" };
        public List<CsvEntryModel> BaseEntries { get; set; }

        public LogRegEntryModel[] X
        {
            get
            {
                LogRegEntryModel[] entries = new LogRegEntryModel[BaseEntries.Count];
                for(int i = 0; i < BaseEntries.Count; ++i)
                {
                    for (int j = 0; j < SELECTED_FEATURES.Length; ++j)
                    {
                        entries[i].X[j] = (float)BaseEntries[i].GetType().GetField(SELECTED_FEATURES[0]).GetValue(null);
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
                        res[i] = 1;
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
                        res[i] = 1;
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
                        res[i] = 1;
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
                        res[i] = 1;
                    ++i;
                }
                return res;
            }
        }
    }

    public class LogRegEntryModel
    {
        public float[] X { get; set; }
    }
}
