using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Shape4DStorage", order = 1)]
public class Shape4DStorage : ScriptableObject
{
    public Shape4D gridObject;
    public GameObject hoverPreview;
    public int objectCount;
    public Sprite objectImage;

    private int currObjects;

    public void Init()
    {
        currObjects = objectCount;
    }

    public int GetCurrentObjectCount()
    {
        return currObjects;
    }

    public void DecrementObjectCount()
    {
        if (currObjects > 0) { currObjects--; }
    }

    public void IncrementObjectCount()
    {
        currObjects++;
    }
}
