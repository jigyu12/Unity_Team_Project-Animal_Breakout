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
       
       var rewardItemsDataTable = new RewardItemsDataTable();
       rewardItemsDataTable.Load(Utils.RewardItemsTableName);
       tables.Add(Utils.RewardItemsTableName, rewardItemsDataTable);
    }
    
    public static MapObjectsDataTable mapObjectsDataTable => Get<MapObjectsDataTable>(Utils.MapObjectsTableName);
    public static RewardItemsDataTable rewardItemsDataTable => Get<RewardItemsDataTable>(Utils.RewardItemsTableName);


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
