using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UniRx;

public interface IData
{
    void Load(BinaryReader reader);
    void Save(BinaryWriter writer);
    void AfterLoad(DataStorage model);
}

public class Model : MonoBehaviour
{
    public GameObject asset;
    public List<GameObject> saveObjects;
    int seqId;

    void Start()
    {
        Event.instance.OnGameObjectStart.Subscribe(obj =>
        {
            if(obj.GetComponent<SaveId>() != null)
            {
                if (string.IsNullOrEmpty(obj.GetComponent<SaveId>().id))
                {
                    obj.GetComponent<SaveId>().id = "" + seqId++;
                }
                saveObjects.Add(obj);
            }
        });

        Event.instance.OnGameObjectDestroy.Subscribe(obj =>
        {
            if (obj.GetComponent<SaveId>() != null)
            {
                saveObjects.Remove(obj);
            }
        });
    }

    private Dictionary<string, GameObject> saveObjectsDict = new Dictionary<string, GameObject>();
    public GameObject Query(string key)
    {
        return saveObjectsDict[key];
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
        using (MemoryStream stream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write(saveObjects.Count);
                foreach (var blt in saveObjects)
                {
                    writer.Write(blt.GetComponent<SaveId>().id);
                    writer.Write(blt.GetComponent<SaveId>().type);
                    foreach (var d in blt.GetComponentsInChildren<IData>())
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
        /*
        foreach(var o in saveObjects)
        {
            Destroy(o);
        }
        saveObjects.Clear();
        */

        var dict = new Dictionary<string, GameObject>();
        foreach (var obj in saveObjects)
        {
            dict.Add(obj.GetComponent<SaveId>().id, obj);
        }
        

        using (MemoryStream stream = new MemoryStream(bytes))
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                var cnt = reader.ReadInt32();
                for (var i = 0; i < cnt; ++i)
                {
                    var key = reader.ReadString();
                    var type = reader.ReadString();
                    /*
                    if (dict.ContainsKey(key) == false)
                    {
                        var type = reader.ReadString();
                        var objT = asset.transform.Find(type);
                        if (objT == null)
                        {
                            Debug.LogWarning(type + " not found!!");
                            continue;
                        }
                        GameObject blt = Instantiate(objT.gameObject, transform, false);
                        dict[key] = blt;
                    }
                    */
                    Debug.Log(key);
                    var obj = dict[key];
                    Debug.Log(obj.name);
                    obj.GetComponent<SaveId>().id = key;
                    obj.GetComponent<SaveId>().isFromLoad = true;
                    foreach (var d in obj.GetComponentsInChildren<IData>())
                    {
                        d.Load(reader);
                    }
                }
            }
        }

        foreach(var obj in dict.Values)
        {
            Debug.Log(obj.name);
            foreach (var d in obj.GetComponentsInChildren<IData>())
            {
                //d.AfterLoad(this);
            }
            obj.gameObject.SetActive(true);
        }

        //saveObjects.AddRange(dict.Values);
        saveObjectsDict = dict;
    }
}
