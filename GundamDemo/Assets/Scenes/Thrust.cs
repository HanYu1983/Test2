using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrust : MonoBehaviour
{
    public string key;
    public Rigidbody rigid;
    public Transform direction;
    public float force;
    public float activeValue;

    void Start()
    {
        if(rigid == null)
        {
            rigid = GetComponent<Rigidbody>();
        }
        if(direction == null)
        {
            direction = transform;
        }
    }

    public void thrust(float factor)
    {
        var heading = Matrix4x4.Rotate(direction.rotation).MultiplyVector(Vector3.forward);

        rigid.AddForce(heading * force * factor);
    }

    void Update()
    {
        if (activeValue != 0)
        {
            thrust(activeValue);
        }
    }
}
