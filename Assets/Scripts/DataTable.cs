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
using static ObjectPoolTester;
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

    public static void SaveCSV<T>(List<T> list, string path)
    {
        using (var writer = new StreamWriter(path + ".csv"))
        using (var csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
        {
            csvWriter.WriteRecords(list);
        }
    }

    //public static List<T> LoadGoogleSheet(string sheetURL)
    //{
    //    using (UnityWebRequest url = UnityWebRequest.Get(sheetURL))
    //    {

    //    }
    //}

    public static string ConvertTSVToJson(string tsv)
    {
        string[] lines = tsv.Replace("\r\n", "\n").Split('\n');
        if (lines.Length < 2) return "[]";

        string[] headers = lines[0].Split('\t');
        List<Dictionary<string, string>> jsonList = new List<Dictionary<string, string>>();

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i]))
            {
                continue;
            }

            string[] values = lines[i].Split('\t');
            Dictionary<string, string> jsonObject = new Dictionary<string, string>();

            for (int j = 0; j < headers.Length && j < values.Length; j++)
            {
                jsonObject[headers[j]] = values[j];
            }

            jsonList.Add(jsonObject);
        }
        return JsonConvert.SerializeObject(jsonList, Formatting.Indented);
    }

    //public static string ConvertGoogleSheetURLToTSVURL(string sheetURL)
    //{

    //    string googleSheet = "https://docs.google.com/spreadsheets/d/";
    //    string sheetTSVFormat = "{0}/export?format=tsv&gid={1}";

    //    int lastIndex = sheetURL.IndexOf('/', googleSheet.Length);
    //    string id = sheetURL.Substring(googleSheet.Length, lastIndex);
    //    x
    //}

    public static IEnumerator LoadGoogleSheet<T>(List<T> list, string sheetURL)//, DataTable table)
    {
        UnityWebRequest www = UnityWebRequest.Get(sheetURL);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log("Err" + www.error);
        }
        else
        {
            string tsv = www.downloadHandler.text;
            string json = ConvertTSVToJson(tsv);
            list = JsonConvert.DeserializeObject<List<T>>(json);
            Debug.Log("Success");
        }
    }

}
