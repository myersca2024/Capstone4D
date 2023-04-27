using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomLevelButtonController : MonoBehaviour
{
    public string fileName;

    public void LoadLevelName()
    {
        GameManager.levelToLoad = fileName;
        Debug.Log(fileName);
    }
}
