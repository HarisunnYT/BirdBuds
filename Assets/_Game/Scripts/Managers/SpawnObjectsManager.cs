using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpawnObjectsManager : Singleton<SpawnObjectsManager>
{
    [SerializeField]
    private SpawnableObjectData[] spawnableObjectData;

    public GameObject GetPrefab(string key)
    {
        foreach(var data in spawnableObjectData)
        {
            if (data.name == key)
            {
                return data.Prefab;
            }
        }

        throw new Exception("Object doesn't exist: " + key);
    }
}
