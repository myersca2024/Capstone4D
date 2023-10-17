using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAnimator : MonoBehaviour
{
    public float moveSpeed;
    public float distanceToWaypoint;
    public float endWaitTime;

    private WAxisController wAxisController;
    private List<GridRailBehavior.Int4> waypoints;
    private RaymarchCam rc;
    private GridObject go;
    private int waypointIndex = 0;
    private bool animationStarted = false;
    private bool isAnimating = false;
    private Vector3 targetPos;

    void Start()
    {
        rc = Camera.main.GetComponent<RaymarchCam>();
        waypoints = new List<GridRailBehavior.Int4>();
        wAxisController = GameObject.FindGameObjectWithTag("WAxisController").GetComponent<WAxisController>();
        go = GameObject.FindGameObjectWithTag("4DGrid").GetComponent<GridObject>();
    }

    void Update()
    {
        if (!WAxisController.isBusy && isAnimating)
        {
            if (!animationStarted)
            {
                transform.position = GetGridCenter(waypoints[0].x, waypoints[0].y, waypoints[0].z);
                animationStarted = true;
            }
            // if (waypointIndex >= waypoints.Count) { isAnimating = false; }
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetPos - transform.position, moveSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
            if (Vector3.Distance(transform.position, targetPos) <= distanceToWaypoint)
            {
                waypointIndex++;
                targetPos = GetGridCenter(waypoints[waypointIndex].x, waypoints[waypointIndex].y, waypoints[waypointIndex].z);
                if (Vector3.Distance(transform.position, targetPos) <= distanceToWaypoint)
                {
                    wAxisController.UpdateWPosition((waypoints[waypointIndex].w - 1) * 2);
                    waypointIndex++;
                }
            }
        }
    }

    public void InitializeWaypoints(List<GridRailBehavior.Int4> positions)
    {
        /*
        waypointIndex = 0;
        waypoints = positions;
        isAnimating = true;
        targetPos = GetGridCenter(waypoints[0].x, waypoints[0].y, waypoints[0].z);
        wAxisController.UpdateWPosition((waypoints[0].w - 1) * 2);
        */
    }

    private Vector3 GetGridCenter(int x, int y, int z)
    {
        Vector3 worldPos = go.grid.GetWorldPosition(x, y, z);
        worldPos += new Vector3(go.cellSize / 2f, 0.9f, go.cellSize / 2f);
        return worldPos;
    }
}
