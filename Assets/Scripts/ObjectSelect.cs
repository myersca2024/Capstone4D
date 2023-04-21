using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ObjectSelect : MonoBehaviour
{
    public Shape4D selectedObject;
    public Unity.Mathematics.PlayerRayMarchCollider prmc;
    public Camera pixelCamera;
    public float maxIterations;
    public float iterationDistance;
    public UnityEvent onShapeSelected;
    public UnityEvent onShapeUnselected;

    private PlayerControls playerControls;
    private Camera cam;

    void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Player.Enable();

        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //DrawTheRay();
        if (playerControls.Player.ObjectsSelect.WasPressedThisFrame() && !MouseInputUIBlocker.BlockedByUI)
        {
            Shape4D shape = null;
            Vector2 mousePos = playerControls.Player.MousePosition.ReadValue<Vector2>();
            Vector3 rayPos = Vector3.zero;
            bool hit = false;

            for (int i = 0; i < maxIterations; i++)
            {
                Ray ray = new Ray(cam.transform.position, cam.transform.rotation * pixelCamera.ScreenPointToRay(mousePos).direction);
                rayPos = ray.origin + ray.direction * i;
                hit = MouseRayMarch(rayPos, out shape);

                if (hit && shape != null)
                {
                    Debug.Log("Moveable shape selected");
                    selectedObject = shape;
                    onShapeSelected.Invoke();
                    /*
                    else if (selectedObject != null)
                    {
                        selectedObject = null;
                        onShapeUnselected.Invoke();
                    }
                    */
                    return;
                }
            }

            if (selectedObject != null)
            {
                selectedObject = null;
                onShapeUnselected.Invoke();
            }
        }
    }

    void DrawTheRay()
    {
        Vector2 mousePos = playerControls.Player.MousePosition.ReadValue<Vector2>();
        Ray ray = new Ray(cam.transform.position, cam.transform.rotation * pixelCamera.ScreenPointToRay(mousePos).direction);
        Debug.DrawRay(ray.origin, ray.direction * 100f);
    }

    bool MouseRayMarch(Vector3 vec, out Shape4D shape)
    {
        //check hit
        float d = prmc.DistanceField(vec, out shape);

        if (d < 0) //hit
        {
            //collision
            return true;
        }

        return false;
    }
}
