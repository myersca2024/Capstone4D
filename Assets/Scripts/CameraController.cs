using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera vcam;
    public float cameraSpeed;

    private bool endGame = false;
    private PlayerControls playerControls;
    private CinemachineOrbitalTransposer transposer;

    private void Start()
    {
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    // Update is called once per frame
    private void Update()
    {
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
        }
    }
    private void CameraMove(float val)
    {
        transposer.m_XAxis.Value += val * cameraSpeed * Time.deltaTime;
    }
}
