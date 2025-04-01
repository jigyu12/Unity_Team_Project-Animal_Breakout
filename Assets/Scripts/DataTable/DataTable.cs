using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using System.IO;
using System.Globalization;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine.Networking;
using Newtonsoft.Json;
using JetBrains.Annotations;

public abstract class DataTable
{
    public static readonly string FormatPath = "Tables/{0}";

    public bool IsReady
    {
        get;
        private set;
    }

    public int currentVersion = 0;

    public abstract void Load(string filename);

    public static List<T> LoadCSV<T>(string csv)
    {
        using (var reader = new StringReader(csv))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            return csvReader.GetRecords<T>().ToList<T>();
        }
    }

    public static void SaveCSV<T>(List<T> list, string path, bool addcsv=false)
    {
        if(addcsv)
        {
            path += ".csv";
        }

        using (var writer = new StreamWriter(path))
        using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteRecords(list);
        }
    }

    public static void SaveCSV(string text, string path, bool addcsv = false)
    {
        if (addcsv)
        {
            path += ".csv";
        }

        File.WriteAllText(path, text);
    }


}
