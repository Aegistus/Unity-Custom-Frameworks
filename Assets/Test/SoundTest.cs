using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public string[] soundNames;

    private int[] soundIDs;

    SoundManager soundManager;

    void Start()
    {
        soundManager = SoundManager.Instance;
        soundIDs = new int[soundNames.Length];
        // for (int i = 0; i < soundNames.Length; i++)
        // {
        //     soundIDs[i] = soundManager.GetSoundID(soundNames[i]);
        // }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            soundManager.PlaySoundGlobal(soundIDs[0]);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            soundManager.PlaySoundAtPosition(soundIDs[0], Vector3.zero);
        }
    }

}
