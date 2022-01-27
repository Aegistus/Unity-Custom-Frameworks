using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Aegis
{
    public class Timer
    {
        private static GameObject timerObject;

        private class ComponentHook : MonoBehaviour
        {
            public Action OnFinish;
            public Action<float> OnTick;
            public float timerLength;
            private float timer = 0f;

            private void Update()
            {
                if (timer >= timerLength)
                {
                    OnFinish();
                    Destroy(this);
                }
                else
                {
                    if (OnTick != null)
                    {
                        OnTick.Invoke(Time.deltaTime);
                    }
                    timer += Time.deltaTime;
                }
            }
        }

        public static void SetTimer(Action OnFinish, float timerLength)
        {
            if (timerObject == null)
            {
                timerObject = new GameObject("Timers");
            }
            ComponentHook hook = timerObject.AddComponent<ComponentHook>();
            hook.timerLength = timerLength;
            hook.OnFinish = OnFinish;
        }

        public static void SetTimer(Action OnFinish, Action<float> OnTick, float timerLength)
        {
            if (timerObject == null)
            {
                timerObject = new GameObject("Timers");
            }
            ComponentHook hook = timerObject.AddComponent<ComponentHook>();
            hook.timerLength = timerLength;
            hook.OnFinish = OnFinish;
            hook.OnTick = OnTick;
        }

    }
}
