
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DictionaryData : ScriptableObject {
    [SerializeField]
    private List<SerializableKeyValuePair<string, List<SpawnableInfo>>> serializedData = new List<SerializableKeyValuePair<string, List<SpawnableInfo>>>();

    public Dictionary<string, List<SpawnableInfo>> data = new Dictionary<string, List<SpawnableInfo>>();

    public void SerializeData() {
        serializedData.Clear();
        foreach (var pair in data) serializedData.Add(new SerializableKeyValuePair<string, List<SpawnableInfo>>(pair.Key, pair.Value));
    }

    public void DeserializeData() {
        data.Clear();
        foreach (var pair in serializedData) data.Add(pair.Key, pair.Value);
    }
}

[Serializable]
public class SerializableKeyValuePair<TKey, TValue> {
    public TKey Key;
    public TValue Value;

    public SerializableKeyValuePair(TKey key, TValue value) {
        Key = key;
        Value = value;
    }
}

