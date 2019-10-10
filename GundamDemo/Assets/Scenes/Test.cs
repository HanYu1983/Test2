using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public GameObject target;
    public float thrust = 10;

    public void Update()
    {
        float y = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        if (y != 0 || x != 0)
        {
            var rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(Vector3.forward * y * thrust);
            rigid.AddForce(Vector3.right * x * thrust);
            if ( rigid.velocity.sqrMagnitude > 25)
            {
                rigid.velocity = rigid.velocity.normalized * 5;
            }
        }

        if (Input.GetButtonDown("Vertical"))
        {
            Debug.Log(Input.mousePosition);
        }
    }
}