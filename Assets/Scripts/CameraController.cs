using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float cameraSpeed;

    private bool endGame = false;
    private PlayerControls playerControls;
    private Vector3 lastRecordedPosition;
    private Vector3 lastRecordedRotation;

    private void Start()
    {
        RecordTransform();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    private void Update()
    {
        if (!endGame)
        {
            if (playerControls.Player.CameraLock.WasPerformedThisFrame() || playerControls.Player.CameraLock.IsPressed())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Vector2 camMoveInput = playerControls.Player.CameraMove.ReadValue<Vector2>();
                CameraMove(camMoveInput);
            }
            else if (playerControls.Player.CameraLock.WasReleasedThisFrame())
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    private void CameraMove(Vector2 val)
    {
        transform.RotateAround(target.transform.position, Vector3.up, -val.x * cameraSpeed * Time.deltaTime);
        if (transform.eulerAngles.x >= 0 && transform.eulerAngles.x <= 75)
        {
            RecordTransform();
            transform.RotateAround(target.transform.position, transform.right, val.y * cameraSpeed * Time.deltaTime);
        }
        if (transform.eulerAngles.x > 75 || transform.eulerAngles.x < 0)
        {
            LoadTransform();
        }
    }

    private void RecordTransform()
    {
        lastRecordedPosition = transform.position;
        lastRecordedRotation = transform.eulerAngles;
    }

    private void LoadTransform()
    {
        transform.position = lastRecordedPosition;
        transform.eulerAngles = lastRecordedRotation;
    }
}
