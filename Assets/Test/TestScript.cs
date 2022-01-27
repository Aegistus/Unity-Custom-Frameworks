using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Aegis;

public class TestScript : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Popup.CreateWorldPopup("Test", Vector3.zero, 12, Color.white, 5f);
            //Timer.CreateTimer(() => print("Timer test"), 4f);
        }
    }
}
