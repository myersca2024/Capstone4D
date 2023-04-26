using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isPlayMode = false;

    public void SetPlayMode(bool val)
    {
        isPlayMode = val;
    }
}
