using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSelect : MonoBehaviour
{
    public Unity.Mathematics.PlayerRayMarchCollider prmc;
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
        DrawTheRay();
        if (playerControls.Player.ObjectsSelect.WasPressedThisFrame())
        {
            Shape4D shape = null;
            Vector2 mousePos = playerControls.Player.MousePosition.ReadValue<Vector2>();
            Vector3 rayPos = Vector3.zero;
            bool hit = false;

            for (int i = 0; i < maxIterations; i++)
            {
                rayPos = cam.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y)).direction;
                Debug.DrawRay(cam.transform.position, rayPos);
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
        Vector3 rayPos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 100f));
        Debug.DrawRay(cam.transform.position, rayPos);
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
