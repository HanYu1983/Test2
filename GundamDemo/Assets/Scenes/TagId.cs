using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TagId : MonoBehaviour, IData
{
    public string id;
    static int seqId;

    void Start()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = "" + seqId++;
        }
    }

    public void Load(BinaryReader reader)
    {
        id = reader.ReadString();
    }
    public void Save(BinaryWriter writer)
    {
        writer.Write(id);
    }

    public void AfterLoad(DataStorage model)
    {

    }
}
