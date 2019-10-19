using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanLib
{
    public class PlayerControlSystem : MonoBehaviour
    {
        public int lastWeaponKey;

        void Update()
        {
            var isActive = GetComponentsInChildren<Weapon>().Length >= 2 && GetComponentsInChildren<Thrust>().Length >= 2;
            if (isActive == false)
            {
                return;
            }

            float y = Input.GetAxis("Vertical");
            float x = Input.GetAxis("Horizontal");
            if (y != 0 || x != 0)
            {
                var thrust = GetComponentsInChildren<Thrust>()[0];
                thrust.thrust(1 * y);

                var thrust2 = GetComponentsInChildren<Thrust>()[1];
                thrust2.thrust(1 * x);
            }

            if (Input.GetButtonDown("Fire1"))
            {
                var weapon = GetComponentsInChildren<Weapon>()[0];
                var heading = Matrix4x4.Rotate(transform.rotation).MultiplyVector(Vector3.forward);
                weapon.Fire(10);
            }

            if (Input.GetButtonDown("Fire2"))
            {
                var weapon = GetComponentsInChildren<Weapon>()[1];
                var heading = Matrix4x4.Rotate(transform.rotation).MultiplyVector(Vector3.forward);
                lastWeaponKey = weapon.Fire(10);
            }

            if (Input.GetButtonUp("Fire2"))
            {
                var weapon = GetComponentsInChildren<Weapon>()[1];
                weapon.StopFire(lastWeaponKey);
            }


        }
    }
}