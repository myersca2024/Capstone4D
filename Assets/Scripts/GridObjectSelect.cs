using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GridObjectSelect : MonoBehaviour
{
    public Shape4D objectToPlace;
    public Vector3 spotToPlaceShape;
    public Unity.Mathematics.PlayerRayMarchCollider prmc;
    public Camera pixelCamera;
    public float maxIterations;
    public float iterationDistance;

    private RaymarchCam rc;
    private Shape4D selectedObject;
    private PlayerControls playerControls;
    private Camera cam;
    private GridObject go;
    private Plane plane;
    private Vector3 defaultVec = new Vector3(-1, -1, -1);

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        rc = GetComponent<RaymarchCam>();
        cam = GetComponent<Camera>();
        go = FindObjectOfType<GridObject>();
        plane = new Plane(Vector3.up, new Vector3(0, go.gameObject.transform.position.y, 0));
        spotToPlaceShape = defaultVec;
    }

    private void Update()
    {
        HoverGridSpace();
        if (spotToPlaceShape.x >= 0 && playerControls.Player.ObjectsSelect.WasPressedThisFrame() && !MouseInputUIBlocker.BlockedByUI)
        {
            Place4DShape();
        }
    }

    private void HoverGridSpace()
    {
        Shape4D shape = null;
        Vector2 mousePos = playerControls.Player.MousePosition.ReadValue<Vector2>();
        Vector3 rayPos = Vector3.zero;
        bool hit = false;

        Ray ray = new Ray(cam.transform.position, cam.transform.rotation * pixelCamera.ScreenPointToRay(mousePos).direction);
        Debug.DrawRay(ray.origin, ray.direction * maxIterations * iterationDistance, Color.red);

        // Does ray hit the grid plane?
        float distance = 0f;
        if (plane.Raycast(ray, out distance) && go.grid.ContainsCell(go.grid.GetXYZ(ray.GetPoint(distance)), 0))
        {
            spotToPlaceShape = go.grid.GetXYZ(ray.GetPoint(distance));
        }
        else
        {
            spotToPlaceShape = defaultVec;
        }

        /*
        // Does ray hit any 4D objects?
        for (int i = 0; i < maxIterations; i++)
        {
            rayPos = ray.origin + ray.direction * i;
            hit = MouseRayMarch(rayPos, out shape);

            if (hit && shape != null)
            {
                Vector3Int gl = shape.gridLocation;
                if (gl.x >= 0) { Debug.Log("Hit rod"); }
                if (go.grid.GetValue(gl.x, gl.y + 1, gl.z, (int)rc._wPosition))
                {
                    spotToPlaceShape = new Vector3Int(gl.x, gl.y + 1, gl.z);
                }
            }
        }
        */
    }

    private bool MouseRayMarch(Vector3 vec, out Shape4D shape)
    {
        //check hit
        float d = prmc.DistanceField(vec, out shape);

        if (d < 0) //hit
        {
            Debug.Log(shape.gameObject.name);
            //collision
            return true;
        }

        return false;
    }

    private void Place4DShape()
    {
        Vector3Int spot = new Vector3Int((int)spotToPlaceShape.x, (int)spotToPlaceShape.y, (int)spotToPlaceShape.z);
        if (objectToPlace == null || go.grid.GetValue(spot.x, spot.y, spot.z, (int)rc._wPosition)) { return; }

        Shape4D shape = Instantiate(objectToPlace, 
                        go.grid.GetWorldPosition(spot.x, spot.y, spot.z) + new Vector3(go.cellSize / 2f, go.cellSize / 2f, go.cellSize / 2f), 
                        objectToPlace.transform.rotation);
        shape.gridLocation = spot;
        go.grid.SetValue(true, spot.x, spot.y, spot.z, (int)rc._wPosition);
    }
}
