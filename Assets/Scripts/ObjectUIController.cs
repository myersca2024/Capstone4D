using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectUIController : MonoBehaviour
{
    [SerializeField] private RectTransform contentPanel;

    private List<Object4DUIController> UIObjects;
    private ObjectPlacer4D objectPlacer;
    private Shape4DStorage shapeData;

    private void Start()
    {
        UIObjects = new List<Object4DUIController>();
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            UIObjects.Add(contentPanel.GetChild(i).GetComponent<Object4DUIController>());
        }
        objectPlacer = GameObject.FindGameObjectWithTag("ObjectPlacer").GetComponent<ObjectPlacer4D>();
        objectPlacer.onShapePlaced.AddListener(DecrementCurrentShape);
        objectPlacer.onShapeDeleted.AddListener(UpdateUITexts);
        objectPlacer.onShapePlaced.AddListener(CheckObjectAvailability);
        contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, 0f);
    }

    public void SetShapeData(Shape4DStorage data)
    {
        shapeData = data;
    }

    public void DecrementCurrentShape()
    {
        if (GameManager.isPlayMode && shapeData != null) 
        { 
            shapeData.DecrementObjectCount();
            UpdateUITexts();
        }
    }

    public void CheckObjectAvailability()
    {
        if (GameManager.isPlayMode && shapeData.GetCurrentObjectCount() <= 0)
        {
            objectPlacer.SetObjectToPlace(null);
            shapeData = null;
        }
    }

    private void UpdateUITexts()
    {
        foreach (Object4DUIController uiController in UIObjects)
        {
            if (uiController.isActiveAndEnabled) { uiController.UpdateCountText(); }
        }
    }
}
