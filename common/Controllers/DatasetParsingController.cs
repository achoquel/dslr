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
        public static DatasetModel ParseDatasetFromFile(string path)
        {
            if (File.Exists(path))
            {
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                var csvParser = new CsvParser<CsvEntryModel>(csvParserOptions, new CsvEntryModelMapping());
                var records = csvParser.ReadFromFile(path, Encoding.UTF8).Select(x => x.Result).ToList().ToList().Where(x => x != null).ToList();
                if (records != null)
                {
                    return new DatasetModel(records) { Path = path };
                }
                throw new Exception("Parsing error: The dataset doesn't contain the required ammount of data or is not well formated.");
            }
            else
            {
                throw new Exception("Parsing error: File doesn't exists.");
            }
        }

        public static DatasetModel ParseDatasetFromString(string datas)
        {
            if (!string.IsNullOrEmpty(datas))
            {
                CsvParserOptions csvParserOptions = new CsvParserOptions(true, ',');
                CsvReaderOptions csvReaderOptions = new CsvReaderOptions(new[] { Environment.NewLine, "\n" });
                var csvParser = new CsvParser<CsvEntryModel>(csvParserOptions, new CsvEntryModelMapping());
                var records = csvParser.ReadFromString(csvReaderOptions, datas).Select(x => x.Result).ToList().Where(x => x != null).ToList();
                if (records != null)
                {
                    return new DatasetModel(records);
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
