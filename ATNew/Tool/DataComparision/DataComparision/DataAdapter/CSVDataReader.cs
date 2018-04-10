using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataComparision.DataAdapter
{
    //public class CSVDataReader : IDataReader
    //{
    //    private StreamReader stream;
    //    private Dictionary<string, int> columnsByName = new Dictionary<string, int>();
    //    private Dictionary<int, string> columnsByOrdinal = new Dictionary<int, string>();
    //    private string[] currentRow;
    //    private bool _isClosed = true;

    //    public CSVDataReader(string fileName)
    //    {
    //        if (!File.Exists(fileName))
    //            throw new Exception("File [" + fileName + "] does not exist.");

    //        this.stream = new StreamReader(fileName);

    //        string[] headers = stream.ReadLine().Split(',');
    //        for (int i = 0; i < headers.Length; i++)
    //        {
    //            columnsByName.Add(headers[i], i);
    //            columnsByOrdinal.Add(i, headers[i]);
    //        }

    //        _isClosed = false;
    //    }

    //    public void Close()
    //    {
    //        if (stream != null) stream.Close();
    //        _isClosed = true;
    //    }

    //    public int FieldCount
    //    {
    //        get { return columnsByName.Count; }
    //    }

    //    /// <summary>
    //    /// This is the main function that does the work - it reads in the next line of data and parses the values into ordinals.
    //    /// </summary>
    //    /// <returns>A value indicating whether the EOF was reached or not.</returns>
    //    public bool Read()
    //    {
    //        if (stream == null) return false;
    //        if (stream.EndOfStream) return false;

    //        currentRow = stream.ReadLine().Split(',');
    //        return true;
    //    }

    //    public object GetValue(int i)
    //    {
    //        return currentRow[i];
    //    }

    //    public string GetName(int i)
    //    {
    //        return columnsByOrdinal[i];
    //    }

    //    public int GetOrdinal(string name)
    //    {
    //        return columnsByName[name];
    //    }

    //}
}
