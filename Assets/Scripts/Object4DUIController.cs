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

    private ObjectUIController masterUIController;
    private ObjectPlacer4D objectPlacer;

    void Start()
    {
        gridObject.Init();
        objectPlacer = GameObject.FindGameObjectWithTag("ObjectPlacer").GetComponent<ObjectPlacer4D>();
        masterUIController = GameObject.FindGameObjectWithTag("ObjectSelectUI").GetComponent<ObjectUIController>();
        shapeImage.sprite = gridObject.objectImage;
        shapeCount.text = gridObject.GetCurrentObjectCount().ToString();
        if (GameManager.isPlayMode)
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

    public void UpdateCountText()
    {
        if (GameManager.isPlayMode) { shapeCount.text = gridObject.GetCurrentObjectCount().ToString(); }
    }

    public void SetShapeToPlace()
    {
        masterUIController.SetShapeData(gridObject);
        if (gridObject.GetCurrentObjectCount() > 0) { objectPlacer.SetObjectToPlace(gridObject); }
    }
}
