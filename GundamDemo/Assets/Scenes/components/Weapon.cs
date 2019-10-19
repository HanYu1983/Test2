using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanLib
{
    public class Weapon : MonoBehaviour
    {
        public enum Type { Ray, Object }
        public Type type;
        public Transform spawnPosition;
        public GameObject bullet;
        public Transform bulletLayer;
        public Vector3 force;

        Dictionary<int, bool> holder = new Dictionary<int, bool>();
        int seqId = 0;
        int NextSeqId()
        {
            return seqId++;
        }

        void Start()
        {
            if (spawnPosition == null)
            {
                spawnPosition = transform;
            }
        }

        public bool QueryHolder(int key)
        {
            return holder.ContainsKey(key);
        }

        public void StopFire(int key)
        {
            holder.Remove(key);
        }

        public int Fire()
        {
            switch (type)
            {
                case Type.Ray:
                    {
                        var id = NextSeqId();
                        holder[id] = true;
                        var obj = Instantiate(bullet, spawnPosition.transform.position, spawnPosition.transform.rotation, bulletLayer);
                        obj.SetActive(true);
                        var weaponHolder = obj.GetComponent<WeaponHolder>();
                        if (weaponHolder == null)
                        {
                            throw new System.Exception("xxxx");
                        }
                        weaponHolder.weapon = this;
                        weaponHolder.key = id;
                        var rigid = obj.GetComponent<Rigidbody>();
                        if (rigid)
                        {
                            Debug.Log(force);
                            rigid.AddForce(force);
                        }
                        return id;
                    }
                case Type.Object:
                    {
                        var obj = Instantiate(bullet, spawnPosition.transform.position, spawnPosition.transform.rotation, bulletLayer);
                        obj.SetActive(true);
                        var rigid = obj.GetComponent<Rigidbody>();
                        if (rigid)
                        {
                            var heading = spawnPosition.transform.TransformVector(force);
                            rigid.AddForce(heading);
                        }
                    }
                    break;
            }
            return 0;
        }
    }
}