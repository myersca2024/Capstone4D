using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuRotationController : MonoBehaviour
{
    public RaymarchCam rayCam;
    public Transform target;
    public float rotateSpeed;
    public float wTimeToLerp;
    public float wSpeed;
    public GameObject[] objectsToRotate;

    private float wTime = 0f;
    private float wMin = -2f;
    private float wMax = 2f;

    private void Update()
    {
        rayCam.transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);

        rayCam._wPosition = Mathf.Lerp(wMin, wMax, wTime);
        wTime += wSpeed * Time.deltaTime;
        if (wTime > wTimeToLerp)
        {
            float temp = wMax;
            wMax = wMin;
            wMin = temp;
            wTime = 0f;
        }
    }
}
