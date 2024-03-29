using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHoverPreview : MonoBehaviour
{
    public GameObject hoverPreview;

    private Vector3 defaultPos = new Vector3(0, -100, 0);
    private GridObjectSelect gos;
    private GridObject go;
    private RaymarchCam rc;
    private ObjectPlacer4D op;

    void Start()
    {
        gos = Camera.main.GetComponent<GridObjectSelect>();
        rc = Camera.main.GetComponent<RaymarchCam>();
        go = GameObject.FindGameObjectWithTag("4DGrid").GetComponent<GridObject>();
        op = GetComponent<ObjectPlacer4D>();
    }

    private void Update()
    {
        if (hoverPreview == null || !op.IsInPlaceState()) { return; }

        Vector3 spot = gos.spotToPlaceShape;
        if (!WAxisController.isBusy && spot.x >= 0 && !go.grid.GetValue((int)spot.x, (int)spot.y, (int)spot.z, (int)(rc._wPosition / 2) + 1))
        {
            hoverPreview.transform.position = go.grid.GetWorldPosition((int)spot.x, (int)spot.y, (int)spot.z) + 
                                              new Vector3(go.cellSize / 2f, hoverPreview.transform.localScale.y / 2, go.cellSize / 2f);
        }
        else
        {
            hoverPreview.transform.position = defaultPos;
        }
    }

    public void InstantiateHoverPreview(Shape4DStorage data)
    {
        hoverPreview = Instantiate(data.hoverPreview, defaultPos, data.gridObject.transform.localRotation, transform);
    }

    public void DeleteHoverPreview()
    {
        if (hoverPreview != null) { Destroy(hoverPreview); }
    }

    public void SetObjectToPreview(Shape4DStorage data)
    {
        DeleteHoverPreview();
        InstantiateHoverPreview(data);
    }

    public void SetPreviewRotation(Vector3 rotation)
    {
        hoverPreview.transform.eulerAngles = rotation;
    }
}
