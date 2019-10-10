using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class WeaponHolder : MonoBehaviour, IData
{
    public Weapon weapon;
    public int key;

    void Update()
    {
        if(weapon == null)
        {
            return;
        }
        if (weapon.QueryHolder(key) == false)
        {
            Destroy(gameObject);
        }
        else
        {
            transform.position = weapon.transform.position;
            transform.rotation = weapon.transform.rotation;
        }
    }

    public string weaponKey;
    
    public void AfterLoad(DataStorage model)
    {
        this.weapon = GameObject.FindObjectsOfType<Weapon>().Where(w =>
        {
            var tag = w.GetComponent<TagId>();
            if (tag == null)
            {
                return false;
            }
            return tag.id == weaponKey;
        }).FirstOrDefault();
    }

    public void Load(BinaryReader reader)
    {
        weaponKey = reader.ReadString();
    }

    public void Save(BinaryWriter writer)
    {
        if (weapon.GetComponent<TagId>() == null)
        {
            throw new System.Exception("must have tagId");
        }
        writer.Write(weapon.GetComponent<TagId>().id);
    }
}
