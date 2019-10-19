using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanLib
{
    public class BombControlSystem : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var isActive = GetComponent<Weapon>() != null && GetComponent<Remover>() != null;
            gameObject.SetActive(isActive);

            var remover = GetComponent<Remover>();
            remover.OnRemove += OnRemove;
        }

        void OnRemove(Remover sender)
        {
            var weapon = GetComponent<Weapon>();
            weapon.Fire();
        }
    }
}