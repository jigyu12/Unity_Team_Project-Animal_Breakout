using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolTester : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private ObjectPool<GameObject> pool;

    public List<TestGoogleSheet> list= new();

    [System.Serializable]
    public class TestGoogleSheet
    {
        public string Direction;
        public string NextDirection;
        public int JustNumber;
    }

    private void Awake()
    {
        pool = ObjectPoolManager.Instance.CreateObjectPool(prefab);
    }

    private void Start()
    {
        StartCoroutine(DataTable.LoadGoogleSheet<TestGoogleSheet>(list, "https://docs.google.com/spreadsheets/d/1bHt6tEBq-L0aLtSUJqeuI5z9PeB6IwvNqxtvN6g46RI/export?format=tsv&gid=0"));
    }

    private void Update()
    {
        int k = 0;
    }
}
