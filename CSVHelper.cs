using CsvHelper;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Text;
using System.Globalization;
using System.Linq;

namespace fdb.apollo.cfidselect.mkdataservice
{
    public static class CSVHelper
    {
        public static DataTable jsonStringToTable(string jsonContent)
        {
            DataTable dt = JsonConvert.DeserializeObject<DataTable>(jsonContent);
            return dt;
        }
        public static string jsonToCSV(string jsonContent, string delimiter)
        {

            var expandos = JsonConvert.DeserializeObject<ExpandoObject[]>(jsonContent);

            using (var writer = new StringWriter())
            {
                using (var csv = new CsvWriter(writer, CultureInfo.CurrentCulture))
                {
                    csv.WriteRecords(expandos as IEnumerable<dynamic>);
                }

                return writer.ToString();
            }

        }
        public static string jsonStringToCSV(string jsonContent)
        {
            var dataTable = (DataTable)JsonConvert.DeserializeObject(jsonContent, (typeof(DataTable)));

            //Datatable to CSV
            var lines = new List<string>();
            string[] columnNames = dataTable.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);
            var valueLines = dataTable.AsEnumerable()
                               .Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);
            return lines.ToString();
        }
    }
    }
