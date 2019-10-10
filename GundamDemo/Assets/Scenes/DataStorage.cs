using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.IO;
using System;

public class DataStorage : MonoBehaviour
{
    public GameObject asset;
    static int seqId;

    public GameObject Query(string key)
    {
        return null;
    }


    byte[] store;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.W))
        {
            store = Save();
        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            Load(store);
        }
    }

    public byte[] Save()
    {
        var saveObjects = GetComponentsInChildren<SaveId>();

        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(saveObjects.Length);
                foreach (var saveId in saveObjects)
                {
                    var id = saveId.id;
                    if (string.IsNullOrEmpty(id))
                    {
                        id = ""+seqId++;
                    }
                    writer.Write(id);
                    writer.Write(saveId.type);
                    foreach (var d in saveId.gameObject.GetComponentsInChildren<IData>())
                    {
                        d.Save(writer);
                    }
                }
            }
            stream.Flush();
            return stream.GetBuffer();
        }
    }

    public void Load(byte[] bytes)
    {
        var saveObjects = GetComponentsInChildren<SaveId>();
        foreach (var o in saveObjects)
        {
            Destroy(o.gameObject);
        }

        var root = transform;

        var dict = new Dictionary<string, GameObject>();
        using (MemoryStream stream = new MemoryStream(bytes))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                var cnt = reader.ReadInt32();
                for (var i = 0; i < cnt; ++i)
                {
                    var key = reader.ReadString();
                    if (dict.ContainsKey(key))
                    {
                        Debug.LogError("save key " + key);
                        continue;
                    }
                    var type = reader.ReadString();
                    var objT = asset.transform.Find(type);
                    if (objT == null)
                    {
                        Debug.LogWarning(type + " not found!!");
                        continue;
                    }
                    GameObject obj = Instantiate(objT.gameObject, root);
                    obj.GetComponent<SaveId>().id = key;
                    obj.GetComponent<SaveId>().isFromLoad = true;
                    foreach (var d in obj.GetComponentsInChildren<IData>())
                    {
                        d.Load(reader);
                    }
                    dict[key] = obj;
                }
            }
        }

        foreach (var obj in dict.Values)
        {
            obj.SetActive(true);
        }

        StartCoroutine(delayActive(()=>
        {
            foreach (var obj in dict.Values)
            {
                foreach (var d in obj.GetComponentsInChildren<IData>())
                {
                    d.AfterLoad(this);
                }
            }
        }));
    }

    IEnumerator delayActive(Action fn)
    {
        yield return null;
        fn();
    }
}
