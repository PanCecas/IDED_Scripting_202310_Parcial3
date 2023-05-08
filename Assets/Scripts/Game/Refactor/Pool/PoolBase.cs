using System.Collections.Generic;
using UnityEngine;

public abstract class PoolBase : MonoBehaviour, IPool
{
    [SerializeField]
    private int count = 0;

    [SerializeField]
    private GameObject basePrefab;

    private List<GameObject> instances = new List<GameObject>();

    public GameObject RetrieveInstance()
    {
        foreach (GameObject instance in instances)
        {
            if (!instance.activeInHierarchy)
            {
                instance.SetActive(true);
                return instance;
            }
        }

        GameObject newObject = Instantiate(basePrefab, transform.position, Quaternion.identity);
        instances.Add(newObject);
        newObject.SetActive(true);
        return newObject;
    }

    public void RecycleInstance(GameObject instance)
    {
        instance.SetActive(false);
    }

    private void Awake()
    {
        PopulatePool();
    }

    private void PopulatePool()
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newObject = Instantiate(basePrefab, transform.position, Quaternion.identity);
            newObject.SetActive(false);
            instances.Add(newObject);
        }
    }
}
