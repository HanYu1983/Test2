using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanLib
{
    public class MaxSpeed : MonoBehaviour
    {
        public Rigidbody rigid;
        public float maxSpeed;

        void Start()
        {
            if (rigid == null)
            {
                rigid = GetComponent<Rigidbody>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (rigid.velocity.magnitude > maxSpeed)
            {
                rigid.velocity = rigid.velocity.normalized * maxSpeed;
            }
        }
    }
}