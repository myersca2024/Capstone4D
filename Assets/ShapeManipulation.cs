using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManipulation : MonoBehaviour
{
    public ObjectSelect objectSelect;
    public float xyzMin;
    public float xyzMax;
    public float wMin;
    public float wMax;

    public void MoveX(float x)
    {
        float newX = Mathf.Clamp(x, xyzMin, xyzMax);
        Vector3 pos = objectSelect.selectedObject.transform.position;
        objectSelect.selectedObject.transform.position = new Vector3(newX, pos.y, pos.z);
    }

    public void MoveY(float y)
    {
        float newY = Mathf.Clamp(y, xyzMin, xyzMax);
        Vector3 pos = objectSelect.selectedObject.transform.position;
        objectSelect.selectedObject.transform.position = new Vector3(pos.x, newY, pos.z);
    }

    public void MoveZ(float z)
    {
        float newZ = Mathf.Clamp(z, xyzMin, xyzMax);
        Vector3 pos = objectSelect.selectedObject.transform.position;
        objectSelect.selectedObject.transform.position = new Vector3(pos.x, pos.y, newZ);
    }

    public void MoveW(float w)
    {
        float newW = Mathf.Clamp(w, wMin, wMax);
        objectSelect.selectedObject.positionW = newW;
    }
}
