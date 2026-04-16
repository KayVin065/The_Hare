using UnityEngine;
using System.Collections.Generic;

public class SnowChunkPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 100;

    private List<GameObject> pool = new();

    void Awake()
    {
        for(int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public SnowChunk GetChunk()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
                return obj.GetComponent<SnowChunk>();
        }

        GameObject newObj = Instantiate(prefab, transform);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj.GetComponent<SnowChunk>();
    }
}
