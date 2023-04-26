using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPlacer4D : MonoBehaviour
{
    public Shape4D objectToPlace;
    [SerializeField] private GridObjectSelect gos;
    [SerializeField] private PlacementActionRecorder recorder;
    [SerializeField] private bool inPlaceState = true;
    public UnityEvent onShapePlaced;
    public UnityEvent onShapeDeleted;

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
            if (inPlaceState) { Place4DShape(objectToPlace); }
            else { Remove4DShape((int)gos.spotToPlaceShape.x, (int)gos.spotToPlaceShape.y, (int)gos.spotToPlaceShape.z, (int)(rc._wPosition / 2) + 1); }
        }
        if (inPlaceState && playerControls.Player.Rotate.WasPerformedThisFrame())
        {
            Rotate4DShape();
        }
    }

    public bool IsInPlaceState()
    {
        return inPlaceState;
    }

    public void SetPlaceState(bool val)
    {
        inPlaceState = val;
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
                            new Vector3(go.cellSize / 2f, obj.gameObject.transform.localScale.y, go.cellSize / 2f),
                            obj.transform.rotation);
        shape.gameObject.SetActive(true);
        shape.positionW = rc._wPosition;
        GridRailBehavior grb = shape.gameObject.GetComponent<GridRailBehavior>();
        grb.gridXYZ = spot;
        grb.gridW = (int)(shape.positionW / 2) + 1;
        go.grid.SetValue(grb, spot.x, spot.y, spot.z, (int)(rc._wPosition / 2) + 1); // CHANGE 1 TO FLOOR OF W_SIZE / 2 LATER
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

    private void Remove4DShape(int x, int y, int z, int w)
    {
        if (!go.grid.GetValue(x, y, z, w)) { return; }

        if (go.grid.GetValue(x, y + 1, z, w)) { Remove4DShape(x, y + 1, z, w); }
        GridRailBehavior shapeToDelete = go.grid.GetShape(x, y, z, w);
        go.grid.SetValue(null, x, y, z, w);
        shapeToDelete.DeleteShape(x, y, z, w);
        onShapeDeleted.Invoke();
    }
}
