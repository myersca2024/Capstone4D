using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectSelectUIController : MonoBehaviour
{
    public TMP_Text[] shapeCounts;
    public GridObjectStorageBehavior gosb;

    private void Start()
    {
        for (int i = 0; i < gosb.totalShapeCounts.Length; i++)
        {
            UpdateShapeText(i);
        }
    }

    public void UpdateShapeText(int id)
    {
        shapeCounts[id].text = gosb.GetCurrentShapeCount(id).ToString();
    }
}
