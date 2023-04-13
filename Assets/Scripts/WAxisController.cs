using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WAxisController : MonoBehaviour
{
    public float moveSpeed;
    public static bool isBusy = false;

    private RaymarchCam rc;
    private float targetWPos;
    private float lastWPos;
    private float deltaW;

    private void Start()
    {
        rc = FindObjectOfType<RaymarchCam>();
        isBusy = false;
    }

    private void Update()
    {
        if (isBusy)
        {
            if (lastWPos > targetWPos) { rc._wPosition = Mathf.Clamp(rc._wPosition - moveSpeed * Time.deltaTime, lastWPos - deltaW, lastWPos + deltaW); }
            else { rc._wPosition = Mathf.Clamp(rc._wPosition + moveSpeed * Time.deltaTime, lastWPos - deltaW, lastWPos + deltaW); }
            if (rc._wPosition == targetWPos) { isBusy = false; }
        }
    }

    public void UpdateWPosition(float pos)
    {
        if (!isBusy)
        {
            targetWPos = pos;
            lastWPos = rc._wPosition;
            deltaW = Mathf.Abs(lastWPos - targetWPos);
            isBusy = true;
        }
    }
}
