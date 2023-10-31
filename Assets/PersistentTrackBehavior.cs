using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentTrackBehavior : MonoBehaviour
{
    private static PersistentTrackBehavior instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }

        DontDestroyOnLoad(gameObject);
    }
}
