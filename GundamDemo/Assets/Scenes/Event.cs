using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace HanLib
{
    public class Event
    {
        public static Event instance = new Event();

        public ReplaySubject<GameObject> OnGameObjectStart = new ReplaySubject<GameObject>();
        public ReplaySubject<GameObject> OnGameObjectDestroy = new ReplaySubject<GameObject>();
    }
}