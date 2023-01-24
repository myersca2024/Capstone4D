using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

// ****************** Player 3D Movement ****************** 

public class PlayerController : MonoBehaviour
{
    public RaymarchCam cam;
    public float playerSpeed;
    public float turnSpeed;
    public float wAxisSpeed;
    public float DeathDistance;

    private Vector3 StartPos;
    private bool endGame = false;
    private Transform model;
    private PlayerInput playerInput;
    private PlayerControls playerControls;

    private void Start()
    {
        model = transform;
        StartPos = transform.position;

        playerInput = GetComponent<PlayerInput>();
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
            Vector2 input = playerControls.Player.Move.ReadValue<Vector2>();
            Vector3 direction = new Vector3(input.x, 0, input.y);
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
        transform.position += input.ToIso() * input.normalized.magnitude * playerSpeed * Time.deltaTime;
    }

    void Look(Vector3 input)
    {
        Quaternion rot = Quaternion.LookRotation(input.ToIso(), Vector3.up);
        model.rotation = Quaternion.RotateTowards(model.rotation, rot, turnSpeed * Time.deltaTime);
    }

    void WAxisMove(float val)
    {
        cam._wPosition = Mathf.Clamp(cam._wPosition + val * wAxisSpeed * Time.deltaTime, -10, 10);
    }

    public void EndGame()
    {
        endGame = true;
    }
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -28, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}