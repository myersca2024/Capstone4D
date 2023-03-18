using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShapeManipUIControl : MonoBehaviour
{
    public ShapeManipulation shapeManip;
    public GameObject shapeManipPanel;
    public TMP_InputField[] inputs;

    public void PopulateInputFields()
    {
        inputs[0].text = shapeManip.objectSelect.selectedObject.transform.position.x.ToString();
        inputs[1].text = shapeManip.objectSelect.selectedObject.transform.position.y.ToString();
        inputs[2].text = shapeManip.objectSelect.selectedObject.transform.position.z.ToString();
        inputs[3].text = shapeManip.objectSelect.selectedObject.positionW.ToString();
    }

    public void SetActiveUIPanel(bool isActive)
    {
        shapeManipPanel.SetActive(isActive);
    }

    public void HandleXInput(float change)
    {
        shapeManip.MoveX(float.Parse(inputs[0].text) + change);
        inputs[0].text = shapeManip.objectSelect.selectedObject.transform.position.x.ToString();
    }

    public void HandleYInput(float change)
    {
        shapeManip.MoveY(float.Parse(inputs[1].text) + change);
        inputs[1].text = shapeManip.objectSelect.selectedObject.transform.position.y.ToString();
    }

    public void HandleZInput(float change)
    {
        shapeManip.MoveZ(float.Parse(inputs[2].text) + change);
        inputs[2].text = shapeManip.objectSelect.selectedObject.transform.position.z.ToString();
    }

    public void HandleWInput(float change)
    {
        shapeManip.MoveW(float.Parse(inputs[3].text) + change);
        inputs[3].text = shapeManip.objectSelect.selectedObject.positionW.ToString();
    }
}