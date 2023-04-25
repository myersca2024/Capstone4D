using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjectStorageBehavior : MonoBehaviour
{
    public int[] totalShapeCounts;
    public int[] currentShapeCounts;
    public ObjectSelectUIController osuc;

    private ObjectPlacer4D gos;
    private List<Shape4D> shapes;
    private int currentID;

    private void Awake()
    {
        shapes = new List<Shape4D>();
        for (int i = 0; i < transform.childCount; i++)
        {
            shapes.Add(transform.GetChild(i).gameObject.GetComponent<Shape4D>());
        }

        currentShapeCounts = new int[totalShapeCounts.Length];
        for (int i = 0; i < totalShapeCounts.Length; i++)
        {
            currentShapeCounts[i] = totalShapeCounts[i];
        }

        gos = Camera.main.gameObject.GetComponent<ObjectPlacer4D>();
    }

    public void SetObjectToPlace(int id)
    {
        if (currentShapeCounts[id] <= 0) { return; }
        currentID = id;
        gos.objectToPlace = shapes[currentID];
    }

    public void CurrentShapeUsed()
    {
        currentShapeCounts[currentID]--;
        osuc.UpdateShapeText(currentID);
        if (currentShapeCounts[currentID] <= 0)
        {
            gos.objectToPlace = null;
        }
    }

    public int GetCurrentShapeCount(int id)
    {
        return currentShapeCounts[id];
    }

    public int GetCurrentShapeID()
    {
        return currentID;
    }

    public void IncrementShapeCount(int id)
    {
        currentShapeCounts[id]++;
        osuc.UpdateShapeText(id);
    }
}
