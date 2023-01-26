using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

// ****************** Player 3D Movement ****************** 

public class PlayerController : MonoBehaviour
{
    public RaymarchCam cam;
    public CinemachineVirtualCamera vcam;
    public float playerSpeed;
    public float cameraSpeed;
    public float turnSpeed;
    public float wAxisSpeed;
    public float DeathDistance;

    private Vector3 StartPos;
    private bool endGame = false;
    private Transform model;
    private PlayerControls playerControls;
    private CinemachineOrbitalTransposer transposer;

    private void Start()
    {
        model = transform;
        StartPos = transform.position;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < DeathDistance)
        {
            transform.position = StartPos;
        }
        if (!endGame)
        {
            if (playerControls.Player.CameraLock.WasPerformedThisFrame() || playerControls.Player.CameraLock.IsPressed())
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                float camMoveInput = playerControls.Player.CameraMove.ReadValue<float>();
                CameraMove(camMoveInput);
            }
            else if (playerControls.Player.CameraLock.WasReleasedThisFrame())
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }

            Vector2 moveInput = playerControls.Player.Move.ReadValue<Vector2>();
            Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
            if (direction.x != 0 || direction.z != 0)
            {
                Look(direction);
                Move(direction);
            }
            float waxis = playerControls.Player.WAxis.ReadValue<float>();
            if (waxis != 0) { WAxisMove(waxis); }
        }
    }

    void Move(Vector3 input)
    {
        transform.position += Matrix4x4.Rotate(Quaternion.Euler(0, cam.transform.eulerAngles.y, 0)).MultiplyPoint3x4(input) * 
            input.normalized.magnitude * playerSpeed * Time.deltaTime;
    }

    void Look(Vector3 input)
    {
        Quaternion rot = Quaternion.LookRotation(Matrix4x4.Rotate(
            Quaternion.Euler(0, cam.transform.eulerAngles.y, 0)).MultiplyPoint3x4(input), 
            Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, turnSpeed * Time.deltaTime);
    }

    void WAxisMove(float val)
    {
        cam._wPosition = Mathf.Clamp(cam._wPosition + val * wAxisSpeed * Time.deltaTime, -10, 10);
    }

    void CameraMove(float val)
    {
        transposer.m_XAxis.Value += val * cameraSpeed * Time.deltaTime;
    }

    public void EndGame()
    {
        endGame = true;
    }
}