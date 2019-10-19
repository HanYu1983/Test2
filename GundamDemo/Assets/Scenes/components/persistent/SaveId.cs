using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HanLib
{
    public class SaveId : MonoBehaviour
    {
        public string type;
        public string id;
        public bool isFromLoad;

        void Start()
        {
            if (isFromLoad)
            {
                return;
            }
        }
    }
}