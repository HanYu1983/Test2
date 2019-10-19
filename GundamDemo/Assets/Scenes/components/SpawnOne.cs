using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanLib
{
    public class SpawnOne : MonoBehaviour
    {
        public GameObject target;

        void Start()
        {
            var obj = Instantiate(target, transform, false);
            obj.SetActive(true);
        }
    }
}