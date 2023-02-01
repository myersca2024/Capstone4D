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
    public float jumpForce;
    public float wAxisSpeed;

    public float DeathDistance;

    private Vector3 StartPos;
    private bool endGame = false;
    private Transform model;
    private PlayerControls playerControls;

    private CinemachineOrbitalTransposer transposer;

    private PlayerRayMarchCollider prmc;
    private bool isJumping;
    private float velocity = 0f;

    private float yPosDifference;
    private Vector3 previouslyTrackedPosition;

    private Shape4D nan = null; // null shape

    private void Start()
    {
        model = transform;
        StartPos = transform.position;
        transposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        prmc = gameObject.GetComponent<PlayerRayMarchCollider>();

        playerControls = new PlayerControls();
        playerControls.Player.Enable();
    }

    // Update is called once per frame
    private void Update()
    {
        TrackYPos();

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
            if (playerControls.Player.Jump.WasPerformedThisFrame() || isJumping) { Jump(); }

            float waxis = playerControls.Player.WAxis.ReadValue<float>();
            if (waxis != 0) { WAxisMove(waxis); }
        }
    }

    private void Move(Vector3 input)
    {
        transform.position += Matrix4x4.Rotate(Quaternion.Euler(0, cam.transform.eulerAngles.y, 0)).MultiplyPoint3x4(input) * 
            input.normalized.magnitude * playerSpeed * Time.deltaTime;
    }

    private void Look(Vector3 input)
    {
        Quaternion rot = Quaternion.LookRotation(Matrix4x4.Rotate(
            Quaternion.Euler(0, cam.transform.eulerAngles.y, 0)).MultiplyPoint3x4(input), 
            Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, turnSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (!isJumping)
        {
            isJumping = true;
            transform.position += new Vector3(0, jumpForce * Time.deltaTime, 0);
        }
        else
        {
            transform.position += new Vector3(0, jumpForce * Time.deltaTime, 0);
            if (yPosDifference <= 0f && prmc.DistanceField(transform.position, out nan) < 0.1f)
            {
                isJumping = false;
            }
        }
    }

    private void WAxisMove(float val)
    {
        cam._wPosition = Mathf.Clamp(cam._wPosition + val * wAxisSpeed * Time.deltaTime, -10, 10);
    }

    private void CameraMove(float val)
    {
        transposer.m_XAxis.Value += val * cameraSpeed * Time.deltaTime;
    }

    private void TrackYPos()
    {
        yPosDifference = transform.position.y - previouslyTrackedPosition.y;
        previouslyTrackedPosition = transform.position;
    }

    public void EndGame()
    {
        endGame = true;
    }
}