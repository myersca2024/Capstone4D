using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPlacer4D : MonoBehaviour
{
    public Shape4D objectToPlace;
    [SerializeField] private GridObjectSelect gos;
    [SerializeField] private PlacementActionRecorder recorder;
    public UnityEvent onShapePlaced;

    private PlayerControls playerControls;
    private RaymarchCam rc;
    private GridObject go;
    private ObjectHoverPreview ohp;

    void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();
        go = FindObjectOfType<GridObject>();
        rc = FindObjectOfType<RaymarchCam>();
        ohp = gameObject.GetComponent<ObjectHoverPreview>();
    }

    void Update()
    {

        if (gos.spotToPlaceShape.x >= 0 && playerControls.Player.ObjectsSelect.WasPressedThisFrame() && !MouseInputUIBlocker.BlockedByUI)
        {
            Place4DShape(objectToPlace);
        }
        if (playerControls.Player.Rotate.WasPerformedThisFrame())
        {
            Rotate4DShape();
        }
    }

    public void SetObjectToPlace(Shape4DStorage data)
    {
        if (data == null)
        {
            objectToPlace = null;
            ohp.DeleteHoverPreview();
        }
        else
        {
            objectToPlace = data.gridObject;
            ohp.SetObjectToPreview(data);
        }
    }

    private void Place4DShape(Shape4D obj)
    {
        Vector3Int spot = new Vector3Int((int)gos.spotToPlaceShape.x, (int)gos.spotToPlaceShape.y, (int)gos.spotToPlaceShape.z);
        if (obj == null || go.grid.GetValue(spot.x, spot.y, spot.z, (int)(rc._wPosition / 2) + 1)) { return; }

        Shape4D shape = Instantiate(obj,
                            go.grid.GetWorldPosition(spot.x, spot.y, spot.z) +
                            new Vector3(go.cellSize / 2f, obj.gameObject.transform.localScale.y + spot.y, go.cellSize / 2f),
                            obj.transform.rotation);
        shape.gameObject.SetActive(true);
        shape.positionW = rc._wPosition;
        GridRailBehavior grb = shape.gameObject.GetComponent<GridRailBehavior>();
        grb.gridXYZ = spot;
        grb.gridW = (int)(shape.positionW / 2) + 1;
        go.grid.SetValue(true, spot.x, spot.y, spot.z, (int)(rc._wPosition / 2) + 1); // CHANGE 1 TO FLOOR OF W_SIZE / 2 LATER
        grb.InitializePathways();
        // gosb.CurrentShapeUsed();
        // recorder.PushAction(shape, gosb.GetCurrentShapeID());
        onShapePlaced.Invoke();
    }

    private void Rotate4DShape()
    {
        if (objectToPlace == null || gos.spotToPlaceShape.x < 0) { return; }
        objectToPlace.gameObject.transform.eulerAngles += new Vector3(0, 90, 0);
        ohp.SetPreviewRotation(objectToPlace.gameObject.transform.eulerAngles);
    }
}
