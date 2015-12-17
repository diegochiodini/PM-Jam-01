using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Data;
using System.IO;
using GameUtils;

public class KeyValueToConst
{
    private class KeyValueDescription : Description
    {
        public string key;
        public string value;

        public override bool ParseCsvLine(string[] row)
        {
            key = row[0];
            value = row[1];
            return true;
        }

        public override string ToString()
        {
            return "\tpublic const string " + key.ToUpper() + " = \"" + value + "\";";
        }
    }

    [MenuItem("Chiodini/Tools/KeyValue To Const")]
    public static void CreateRandomLevels()
    {
        string fullPath = Application.streamingAssetsPath + "/" + CommonConstants.LOCAL_SOCIAL_DATA;
        Log.Print("Reading " + fullPath + "...");
        string csvString = System.IO.File.ReadAllText(fullPath);

        CSVReader csvReader = new CSVReader();
        csvReader.LoadFile(csvString);
        List<KeyValueDescription> pairs = csvReader.ParseObjects<KeyValueDescription>();


        string[] outLines = new string[pairs.Count + 3];
        outLines[0] = "public class SocialConstants";
        outLines[1] = "{";
        outLines[pairs.Count + 2] = "}";

        int i = 2;
        foreach (KeyValueDescription pair in pairs)
        {
            outLines[i++] = pair.ToString();
        }

        fullPath = CommonConstants.LOCAL_SOCIAL_CONST;
        Log.Print("Writing " + fullPath + "...");
        System.IO.File.WriteAllLines(fullPath, outLines);
    }
}

