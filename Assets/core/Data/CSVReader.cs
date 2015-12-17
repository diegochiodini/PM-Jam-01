using UnityEngine;
using System.Collections.Generic;
using System.Linq; 
using System;
 
public class CSVReader 
{
    private Dictionary<string, List<string>> cachedDictionary;
    private int linesCount;

    public CSVReader()
	{
	}

    public void LoadFile(string csvString)
    {
        Assert.That(!string.IsNullOrEmpty(csvString), "You are trying to parse an empty csv string. Please check the file exsists.");
        Dictionary<string, List<string>> data = new Dictionary<string, List<string>>();
        string[] lines = csvString.Split("\n"[0]);
        linesCount = lines.Length;

        string[] keys = null;
        for (int i=0; i<lines.Length; i++)
        {
            string line = lines[i];
            if (line.Length == 0)
            {
                linesCount--;
                continue;
            }

            line = line.Trim('\r');
            string[] values = SplitCsvLine(line);
            if (values[0][0] == '#')
            {
                //key declaration line
                values[0] = values[0].Substring(1);
                keys = values;
                foreach (string key in keys)
                {
                    data.Add(key, new List<string>());
                }
                linesCount--;
                continue;
            }

            if (keys != null)
            {
                int k = 0;
                foreach (string value in values)
                {
                    string key = keys[k++];
                    List<string> list = data[key];
                    list.Add(value);
                }
            }
        }
        cachedDictionary = data;
    }

    public List<T> ParseObjects<T>() where T: Description, new()
    {
        List<T> resultList = new List<T>();

        string[] keys = cachedDictionary.Keys.ToArray<string>();
        for (int i = 0; i < linesCount; i++)
        {
            List<string> row = new List<string>();
            foreach (string key in keys)
            {
                row.Add(cachedDictionary[key][i]);
            }
            T parsedObject = new T();
            parsedObject.ParseCsvLine(row.ToArray());
            resultList.Add(parsedObject);
        }
        return resultList;
    }

    public void Print()
    {
        string[] keys = cachedDictionary.Keys.ToArray<string>();
        for (int i = 0; i < linesCount; i++)
        {
            string row = "";
            foreach (string key in keys)
            {
                row += string.Format("{0}: {1} ", key, cachedDictionary[key][i]);
            }
            Debug.Log(row);
        }
    }
 
	// splits a CSV row 
    private string[] SplitCsvLine(string line)
	{
        //string[] result = (from System.Text.RegularExpressions.Match m in System.Text.RegularExpressions.Regex.Matches(line,
        //@"(((?<x>(?=[,\r\n]+))|""(?<x>([^""]|"""")+)""|(?<x>[^,\r\n]+)),?)", 
        //System.Text.RegularExpressions.RegexOptions.ExplicitCapture)
        //select m.Groups[1].Value).ToArray();
        //return result;

        return line.Split(',');
	}
}