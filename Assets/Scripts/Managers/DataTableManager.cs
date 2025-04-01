using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public static class DataTableManager
{
    private static readonly Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableManager()
    {
       var mapObjectsDataTable = new MapObjectsDataTable();
       mapObjectsDataTable.Load(Utils.MapObjectsTableName);
       tables.Add(Utils.MapObjectsTableName, mapObjectsDataTable);
    }
    
    public static MapObjectsDataTable mapObjectsDataTable => Get<MapObjectsDataTable>(Utils.MapObjectsTableName);


    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
        {
            Debug.LogError($"No Table found for {id}");
            return null;
        }
        return tables[id] as T;
    }
}
