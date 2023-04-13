using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitFps : MonoBehaviour
{
    public int frameRate = 30;

    void Start()
    {
        Application.targetFrameRate = frameRate;
    }
}
