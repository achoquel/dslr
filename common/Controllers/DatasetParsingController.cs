using common.Enumerations;
using common.Models;
using System;
using System.IO;
using System.Linq;
using System.Text;
using TinyCsvParser;

namespace common.Controllers
{
    public class DatasetParsingController
    {
        /// <summary>
        /// Parses a dataset from a file
        /// </summary>
        /// <param name="path">The path of the file</param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static DatasetModel ParseDatasetFromFile(string path, ExecutionModeEnum mode)
        {
            if (File.Exists(path))
            {
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                var csvParser = new CsvParser<CsvEntryModel>(csvParserOptions, new CsvEntryModelMapping());
                var records = csvParser.ReadFromFile(path, Encoding.UTF8).Select(x => x.Result).ToList().ToList().Where(x => x != null).ToList();
                if (records != null)
                {
                    var dataset = new DatasetModel(records) { Path = path, Mode = mode };
                    if (dataset.IsValidDataset())
                    {
                        return dataset;
                    }
                    else
                    {
                        throw new Exception("Parsing error: Dataset is not well formated or doesn't have enough entries.");
                    }
                }
                throw new Exception("Parsing error: The dataset doesn't contain the required ammount of data or is not well formated.");
            }
            else
            {
                throw new Exception("Parsing error: File doesn't exists.");
            }
        }

        /// <summary>
        /// Parses a dataset from a string
        /// </summary>
        /// <param name="datas">The string that contains the dataset</param>
        /// <returns></returns>
        public static DatasetModel ParseDatasetFromString(string datas, ExecutionModeEnum mode)
        {
            if (!string.IsNullOrEmpty(datas))
            {
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine, "\n" });
                var csvParser = new CsvParser<CsvEntryModel>(csvParserOptions, new CsvEntryModelMapping());
                var records = csvParser.ReadFromString(csvReaderOptions, datas).Select(x => x.Result).ToList().Where(x => x != null).ToList();
                if (records != null)
                {
                    var dataset = new DatasetModel(records) { Mode = mode };
                    if (dataset.IsValidDataset())
                    {
                        return dataset;
                    }
                    else
                    {
                        throw new Exception("Parsing error: Dataset is not well formated or doesn't have enough entries.");
                    }
                }
                throw new Exception("Parsing error: The dataset doesn't contain the required ammount of data or is not well formated.");
            }
            else
            {
                throw new Exception("Parsing error: File doesn't exists.");
            }
        }
    }
}
