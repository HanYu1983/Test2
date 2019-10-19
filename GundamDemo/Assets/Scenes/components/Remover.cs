using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace HanLib
{
    public class Remover : MonoBehaviour, IData
    {
        public float maxDistance;
        public float maxTime;
        public Vector3 originPosition;
        public float time;

        public delegate void OnRemoveDelete(Remover sender);
        public event OnRemoveDelete OnRemove = delegate { };

        bool isFromLoad;

        void Start()
        {
            if (isFromLoad)
            {
                return;
            }
            originPosition = transform.position;
        }

        void Update()
        {
            if (maxDistance > 0)
            {
                var v = transform.position - originPosition;
                if (v.magnitude > maxDistance)
                {
                    OnRemove(this);
                    Destroy(gameObject);
                    return;
                }
            }

            if (maxTime > 0)
            {
                if (time > maxTime)
                {
                    OnRemove(this);
                    Destroy(gameObject);
                    return;
                }
                time += Time.deltaTime;
            }
        }

        public void AfterLoad(DataStorage model)
        {
            isFromLoad = true;
        }

        public void Load(BinaryReader reader)
        {
            var x = reader.ReadDouble();
            var y = reader.ReadDouble();
            var z = reader.ReadDouble();
            var time = reader.ReadDouble();

            this.originPosition = new Vector3((float)x, (float)y, (float)z);
            this.time = (float)time;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write((double)originPosition.x);
            writer.Write((double)originPosition.y);
            writer.Write((double)originPosition.z);
            writer.Write((double)time);
        }
    }
}