using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace HanLib
{
    public class BasicSaver : MonoBehaviour, IData
    {

        public void AfterLoad(DataStorage model)
        {

        }

        public void Load(BinaryReader reader)
        {
            {
                var x = reader.ReadDouble();
                var y = reader.ReadDouble();
                var z = reader.ReadDouble();

                var velocity = new Vector3((float)x, (float)y, (float)z);
                transform.position = velocity;
            }

            {
                var x = reader.ReadDouble();
                var y = reader.ReadDouble();
                var z = reader.ReadDouble();

                var eular = new Vector3((float)x, (float)y, (float)z);
                transform.rotation = Quaternion.Euler(eular);
            }
            var rigid = GetComponent<Rigidbody>();
            if (rigid)
            {
                var x = reader.ReadDouble();
                var y = reader.ReadDouble();
                var z = reader.ReadDouble();

                var velocity = new Vector3((float)x, (float)y, (float)z);
                rigid.velocity = velocity;
            }
        }

        public void Save(BinaryWriter writer)
        {
            {
                writer.Write((double)transform.position.x);
                writer.Write((double)transform.position.y);
                writer.Write((double)transform.position.z);
            }

            {
                writer.Write((double)transform.rotation.eulerAngles.x);
                writer.Write((double)transform.rotation.eulerAngles.y);
                writer.Write((double)transform.rotation.eulerAngles.z);
            }

            var rigid = GetComponent<Rigidbody>();
            if (rigid)
            {
                writer.Write((double)rigid.velocity.x);
                writer.Write((double)rigid.velocity.y);
                writer.Write((double)rigid.velocity.z);
            }
        }
    }
}