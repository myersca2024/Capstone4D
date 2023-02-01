using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelect : MonoBehaviour
{
    public Unity.Mathematics.PlayerRayMarchCollider prmc;
    public Camera pixelCamera;
    public float maxIterations;
    public float iterationDistance;

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
        if (playerControls.Player.ObjectsSelect.WasPressedThisFrame())
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
                    return;
                }
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
            Debug.Log(shape.gameObject.name);
            //collision
            return true;
        }

        return false;
    }
}