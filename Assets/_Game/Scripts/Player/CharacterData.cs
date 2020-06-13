using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Bird Buds/Character Data")]
public class CharacterData : ScriptableObject
{
    [System.Serializable]
    private struct DataItem
    {
        public string keyName;
        public float value;
    }

    [SerializeField]
    private DataItem[] data;

    public float GetValue(string keyName)
    {
        foreach(var d in data)
        {
            if (d.keyName == keyName)
            {
                return d.value;
            }
        }

        return 0;
    }
}
