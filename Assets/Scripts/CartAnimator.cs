using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CartAnimator : MonoBehaviour
{
    public float moveSpeed;
    public float distanceToWaypoint;
    public float endWaitTime;
    public GameObject winEffect;
    public GameObject loseEffect;
    public AudioClip explosionSound;
    public AudioClip confettiSound;
    public UnityEvent onAnimationComplete;

    private Vector3 startingPos;
    private Quaternion startingRot;
    private WAxisController wAxisController;
    private List<Float4> waypoints;
    private RaymarchCam rc;
    private GridObject go;
    private int waypointIndex = 0;
    private bool animationStarted = false;
    private bool isAnimating = false;
    private Vector3 targetPos;
    private bool gameSuccess = false;
    private AudioSource audioSource;

    void Start()
    {
        startingPos = transform.position;
        startingRot = transform.rotation;
        rc = Camera.main.GetComponent<RaymarchCam>();
        waypoints = new List<Float4>();
        wAxisController = GameObject.FindGameObjectWithTag("WAxisController").GetComponent<WAxisController>();
        go = GameObject.FindGameObjectWithTag("4DGrid").GetComponent<GridObject>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!WAxisController.isBusy && isAnimating && !SceneManagement.isPaused)
        {
            if (!animationStarted)
            {
                transform.position = new Vector3(waypoints[0].x, waypoints[0].y, waypoints[0].z);
                animationStarted = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            // float angleToPoint = Vector3.Angle(transform.position, targetPos - transform.position);
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetPos - transform.position, Mathf.PI, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            if (Vector3.Distance(transform.position, targetPos) <= distanceToWaypoint)
            {
                if (waypointIndex + 1 < waypoints.Count)
                {
                    waypointIndex++;
                    targetPos = new Vector3(waypoints[waypointIndex].x, waypoints[waypointIndex].y, waypoints[waypointIndex].z);
                    wAxisController.UpdateWPosition((waypoints[waypointIndex].w - 1) * 2);
                }
                else
                {
                    CompleteAnimation();
                }
            }
        }
    }

    public void InitializeWaypoints(List<GridRailBehavior> objects, bool success)
    {
        gameSuccess = success;
        waypointIndex = 0;

        Vector3 startPos = GetGridCenter(objects[0].gridXYZ.x, objects[0].gridXYZ.y, objects[0].gridXYZ.z);
        waypoints.Add(new Float4(startPos.x, startPos.y, startPos.z, objects[0].gridW));

        GridRailBehavior.Int4 startFacingRail = objects[0].connectedSpaces[0];
        Vector3 startFacingPos = GetGridCenter(startFacingRail.x, startFacingRail.y, startFacingRail.z);
        Float4 startFacingPosf4 = new Float4((startPos.x + startFacingPos.x) / 2, startPos.y, (startPos.z + startFacingPos.z) / 2, startFacingRail.w);
        waypoints.Add(startFacingPosf4);

        for (int i = 1; i < objects.Count; i++)
        {
            if (objects[i].directions.Length == 2)
            {
                GridRailBehavior.Int4 previousRailPos = new GridRailBehavior.Int4(objects[i - 1].gridXYZ.x,
                                                                                  objects[i - 1].gridXYZ.y,
                                                                                  objects[i - 1].gridXYZ.z,
                                                                                  objects[i - 1].gridW);
                GridRailBehavior.Int4 facingRail = GetFacingRail(objects[i], previousRailPos);
                GridRailBehavior.Int4 backFacingRail = GetFacingRail(objects[i], facingRail);

                Vector3 currentPosv3 = GetGridCenter(objects[i].gridXYZ.x, objects[i].gridXYZ.y, objects[i].gridXYZ.z);
                Vector3 facingPosv3 = GetGridCenter(facingRail.x, facingRail.y, facingRail.z);
                Vector3 backFacingPosv3 = GetGridCenter(backFacingRail.x, backFacingRail.y, backFacingRail.z);

                Float4 currentPosf4 = new Float4(currentPosv3.x, (backFacingPosv3.y + facingPosv3.y) / 2, currentPosv3.z, objects[i].gridW);
                Float4 facingPosf4 = new Float4((currentPosv3.x + facingPosv3.x) / 2, facingPosv3.y, (currentPosv3.z + facingPosv3.z) / 2, facingRail.w);

                waypoints.Add(currentPosf4);
                waypoints.Add(facingPosf4);
            }
        }

        if (gameSuccess)
        {
            Vector3 endPos = GetGridCenter(objects[objects.Count - 1].gridXYZ.x,
                                           objects[objects.Count - 1].gridXYZ.y,
                                           objects[objects.Count - 1].gridXYZ.z);
            waypoints.Add(new Float4(endPos.x, endPos.y, endPos.z, objects[objects.Count - 1].gridW));
        }

        StartAnimation();
    }

    private void StartAnimation()
    {
        isAnimating = true;
        targetPos = new Vector3(waypoints[0].x, waypoints[0].y, waypoints[0].z);
        wAxisController.UpdateWPosition((waypoints[0].w - 1) * 2);
    }

    private void CompleteAnimation()
    {
        animationStarted = false;
        isAnimating = false;
        waypoints.Clear();
        if (gameSuccess) 
        { 
            Instantiate(winEffect, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
            LoadAndPlayAudio(confettiSound);
        }
        else 
        { 
            Instantiate(loseEffect, transform.position, transform.rotation);
            LoadAndPlayAudio(explosionSound);
            ResetTransform();
        }
        StartCoroutine(WaitThenInvoke());
    }

    private IEnumerator WaitThenInvoke()
    {
        yield return new WaitForSeconds(endWaitTime);
        onAnimationComplete.Invoke();
    }

    private void ResetTransform()
    {
        transform.position = startingPos;
        transform.rotation = startingRot;
    }

    private Vector3 GetGridCenter(int x, int y, int z)
    {
        Vector3 worldPos = go.grid.GetWorldPosition(x, y, z);
        worldPos += new Vector3(go.cellSize / 2f, 0.9f, go.cellSize / 2f);
        return worldPos;
    }

    private GridRailBehavior.Int4 GetFacingRail(GridRailBehavior currentRail, GridRailBehavior.Int4 pastRailPos)
    {
        GridRailBehavior.Int4 toReturn = null;

        GridRailBehavior.Int4 currentRailPos = new GridRailBehavior.Int4(currentRail.gridXYZ.x,
                                                                         currentRail.gridXYZ.y,
                                                                         currentRail.gridXYZ.z,
                                                                         currentRail.gridW);
        foreach (GridRailBehavior.Int4 connectingRailPos in currentRail.connectedSpaces)
        {
            if (!(connectingRailPos.x == currentRailPos.x &&
                connectingRailPos.z == currentRailPos.z &&
                connectingRailPos.w == currentRailPos.w) &&
                !(connectingRailPos.x == pastRailPos.x &&
                connectingRailPos.z == pastRailPos.z &&
                connectingRailPos.w == pastRailPos.w))
            {
                return connectingRailPos;
            }
        }

        if (toReturn == null) { Debug.Log("Could not find "); }
        return toReturn;
    }

    private void LoadAndPlayAudio(AudioClip audio)
    {
        audioSource.clip = audio;
        audioSource.Play();
    }

    [Serializable]
    public class Float4
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public Float4(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Float4 f41, Float4 f42)
        {
            if (ReferenceEquals(f41, f42)) { return true; }
            if (ReferenceEquals(f41, null)) { return false; }
            if (ReferenceEquals(f42, null)) { return false; }
            return f41.x == f42.x &&
                   f41.y == f42.y &&
                   f41.z == f42.z &&
                   f41.w == f42.w;
        }

        public static bool operator !=(Float4 f41, Float4 f42) => !(f41 == f42);

        public bool Equals(Float4 other)
        {
            if (ReferenceEquals(other, null)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            return x == other.x &&
                   y == other.y &&
                   z == other.z &&
                   w == other.w;
        }

        public override bool Equals(object obj) => Equals(obj as Float4);
    }
}
