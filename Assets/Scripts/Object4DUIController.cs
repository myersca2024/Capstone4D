using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Object4DUIController : MonoBehaviour
{
    [SerializeField] private Shape4DStorage gridObject;
    [SerializeField] private Image shapeImage;
    [SerializeField] private TMP_Text shapeCount;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private bool amountSelectOverride;

    private ObjectUIController masterUIController;
    private ObjectPlacer4D objectPlacer;

    void Start()
    {
        gridObject.Init();
        objectPlacer = GameObject.FindGameObjectWithTag("ObjectPlacer").GetComponent<ObjectPlacer4D>();
        masterUIController = GameObject.FindGameObjectWithTag("ObjectSelectUI").GetComponent<ObjectUIController>();
        shapeImage.sprite = gridObject.objectImage;
        shapeCount.text = gridObject.GetCurrentObjectCount().ToString();
        if (GameManager.isPlayMode || amountSelectOverride)
        {
            inputField.gameObject.SetActive(false);
            if (gridObject.GetCurrentObjectCount() == 0)
            {
                gameObject.SetActive(false);
                return;
            }
        }
        else { shapeCount.gameObject.SetActive(false); }
        // objectPlacer.onShapePlaced.AddListener(UpdateCountText);
    }

    public void UpdateRailCount()
    {
        int cur = Int32.Parse(inputField.text);
        cur = Mathf.Clamp(cur, 0, 99);
        inputField.text = cur.ToString();
        masterUIController.UpdateTrackerCounts(gridObject.ID, cur);
    }

    public void IncrementInputField()
    {
        int cur = Int32.Parse(inputField.text);
        cur = Mathf.Clamp(cur + 1, 0, 99);
        inputField.text = cur.ToString();
    }

    public void DecrementInputField()
    {
        int cur = Int32.Parse(inputField.text);
        cur = Mathf.Clamp(cur - 1, 0, 99);
        inputField.text = cur.ToString();
    }

    public void UpdateCountText()
    {
        if (GameManager.isPlayMode || amountSelectOverride) { shapeCount.text = gridObject.GetCurrentObjectCount().ToString(); }
    }

    public void SetShapeToPlace()
    {
        masterUIController.SetShapeData(gridObject);
        if (gridObject.GetCurrentObjectCount() > 0) { objectPlacer.SetObjectToPlace(gridObject); }
    }
}
