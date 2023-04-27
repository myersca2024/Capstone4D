using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GridObjectSelect : MonoBehaviour
{
    public Vector3 spotToPlaceShape;
    public Unity.Mathematics.PlayerRayMarchCollider prmc;
    public Camera pixelCamera;
    public PlacementActionRecorder recorder;
    public GridObjectStorageBehavior gosb;
    public float maxIterations;
    public float iterationDistance;

    private PlayerControls playerControls;
    private ObjectPlacer4D op;
    private RaymarchCam rc;
    private Camera cam;
    private GridObject go;
    private Plane[] planes;
    private Vector3 defaultVec = new Vector3(-1, -1, -1);

    private void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        cam = GetComponent<Camera>();
        go = FindObjectOfType<GridObject>();
        rc = Camera.main.GetComponent<RaymarchCam>();
        op = GameObject.FindGameObjectWithTag("ObjectPlacer").GetComponent<ObjectPlacer4D>();
        planes = new Plane[go.sizeY];
        for (int i = 0; i < go.sizeY; i++)
        {
            planes[i] = new Plane(Vector3.up, new Vector3(0, go.gameObject.transform.position.y + (i * go.cellSize), 0));
        }
        spotToPlaceShape = defaultVec;
    }

    private void Update()
    {
        if (!WAxisController.isBusy)
        {
            if (op.IsInPlaceState()) { PlaceStateHover(); }
            else { RemoveStateHover(); }
        }
    }

    private void PlaceStateHover()
    {
        Vector2 mousePos = playerControls.Player.MousePosition.ReadValue<Vector2>();

        Ray ray = new Ray(cam.transform.position, cam.transform.rotation * pixelCamera.ScreenPointToRay(mousePos).direction);
        Debug.DrawRay(ray.origin, ray.direction * maxIterations * iterationDistance, Color.red);

        // Does ray hit the grid plane?
        float distance = 0f;
        for (int i = go.sizeY - 1; i >= 0; i--) {
            planes[i].Raycast(ray, out distance);
            if (i > 0)
            {
                Vector3Int pos = go.grid.GetXYZ(ray.GetPoint(distance));
                if (go.grid.ContainsCell(pos, 0))
                {
                    Vector3Int belowPos = new Vector3Int(pos.x, pos.y - 1, pos.z);
                    int wPos = (int)(rc._wPosition / 2) + 1;
                    if (go.grid.GetValue(belowPos.x, belowPos.y, belowPos.z, wPos) && 
                        go.grid.GetShape(belowPos.x, belowPos.y, belowPos.z, wPos).stackable)
                    {
                        spotToPlaceShape = pos;
                        return;
                    }
                }
            }
            else
            {
                spotToPlaceShape = go.grid.ContainsCell(go.grid.GetXYZ(ray.GetPoint(distance)), 0) ? go.grid.GetXYZ(ray.GetPoint(distance)) : defaultVec;
            }
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

    private void RemoveStateHover()
    {
        Vector2 mousePos = playerControls.Player.MousePosition.ReadValue<Vector2>();

        Ray ray = new Ray(cam.transform.position, cam.transform.rotation * pixelCamera.ScreenPointToRay(mousePos).direction);
        Debug.DrawRay(ray.origin, ray.direction * maxIterations * iterationDistance, Color.red);

        // Does ray hit the grid plane?
        float distance = 0f;
        for (int i = go.sizeY - 1; i >= 0; i--)
        {
            planes[i].Raycast(ray, out distance);
            Vector3Int pos = go.grid.GetXYZ(ray.GetPoint(distance));
            if (go.grid.ContainsCell(pos, 0))
            {
                int wPos = (int)(rc._wPosition / 2) + 1;
                if (go.grid.GetValue(pos.x, pos.y, pos.z, wPos) &&
                    go.grid.GetShape(pos.x, pos.y, pos.z, wPos).deletable)
                {
                    spotToPlaceShape = pos;
                    // Debug.Log(pos);
                    return;
                }
            }
            else
            {
                spotToPlaceShape = defaultVec;
            }
        }
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
}
