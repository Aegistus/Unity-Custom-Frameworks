using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Aegis
{
    public static class Popup
    {

        public static void CreateWorldPopup(string message, Vector3 position, int fontSize, Color color, float duration)
        {
            GameObject popup = new GameObject("Popup");
            TextMesh text = popup.AddComponent<TextMesh>();
            text.text = message;
            text.fontSize = fontSize;
            text.color = color;

            Timer.SetTimer(() => UnityEngine.Object.Destroy(popup), duration);
        }

        public static void CreateUIPopup(string message)
        {

        }
    }
}
