using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Timer
{
    private static GameObject timerObject;

    private class ComponentHook : MonoBehaviour
    {
        public Action OnFinish;
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
                timer += Time.deltaTime;
            }
        }
    }

    public static void CreateTimer(Action OnFinish, float timerLength)
    {
        if (timerObject == null)
        {
            timerObject = new GameObject("Timers");
        }
        ComponentHook hook = timerObject.AddComponent<ComponentHook>();
        hook.timerLength = timerLength;
        hook.OnFinish = OnFinish;
    }

}
