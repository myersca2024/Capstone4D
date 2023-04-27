using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static string levelToLoad = "";
    public static bool isPlayMode = false;
    public GridRailBehavior startPoint;
    public GridRailBehavior endPoint;
    public CartAnimator cart;

    private List<GridRailBehavior.Int4> trackForAnimations;
    private ObjectTracker4D ot;
    private GridObject go;

    private void Awake()
    {
        ot = GameObject.FindGameObjectWithTag("ObjectTracker").GetComponent<ObjectTracker4D>();
        if (isPlayMode) { ot.ReadInstructionsFromFile(levelToLoad); }
        else { SetShapeDataToOne(); }
    }

    private void Start()
    {
        go = GameObject.FindGameObjectWithTag("4DGrid").GetComponent<GridObject>();
        if (isPlayMode) { InitializeLevel(); }
    }

    public void InitializeLevel()
    {
        foreach (ObjectTracker4D.ObjectInstruction inst in ot.instructions)
        {
            InitializeShape(ot.shapeData[inst.objectID].gridObject, 
                            inst.position.x, inst.position.y, inst.position.z, inst.position.w, 
                            (int)inst.rotation.x, (int)inst.rotation.y, (int)inst.rotation.z);
        }
    }

    private void InitializeShape(Shape4D obj, int x, int y, int z, int w, int rotX, int rotY, int rotZ)
    {
        if (obj == null || go.grid.GetValue(x, y, z, w)) { return; }

        Shape4D shape = Instantiate(obj,
                            go.grid.GetWorldPosition(x, y, z) +
                            new Vector3(go.cellSize / 2f, obj.gameObject.transform.localScale.y, go.cellSize / 2f),
                            Quaternion.Euler(rotX, rotY, rotZ));
        shape.gameObject.SetActive(true);
        shape.positionW = (w - 1) * 2;
        GridRailBehavior grb = shape.gameObject.GetComponent<GridRailBehavior>();
        grb.gridXYZ = new Vector3Int(x, y, z);
        grb.gridW = w;
        go.grid.SetValue(grb, x, y, z, w); // CHANGE 1 TO FLOOR OF W_SIZE / 2 LATER
        grb.deletable = false;
        grb.InitializePathways();
    }

    public void CheckWin()
    {
        GridRailBehavior.Int4 endPos = new GridRailBehavior.Int4(endPoint.gridXYZ.x, endPoint.gridXYZ.y, endPoint.gridXYZ.z, endPoint.gridW);
        GridRailBehavior.Int4 currPos = new GridRailBehavior.Int4(startPoint.gridXYZ.x, startPoint.gridXYZ.y, startPoint.gridXYZ.z, startPoint.gridW);
        GridRailBehavior.Int4 aboveCurrPos = new GridRailBehavior.Int4(currPos.x, currPos.y + 1, currPos.z, currPos.w);
        GridRailBehavior.Int4 nextMove = startPoint.connectedSpaces[0];
        trackForAnimations = new List<GridRailBehavior.Int4>();
        while (!Int4Equal(currPos, endPos))
        {
            Debug.Log("Current: " + "[" + currPos.x + ", " + currPos.y + ", " + currPos.z + ", " + currPos.w + "]");
            Debug.Log("Next: " + "[" + nextMove.x + ", " + nextMove.y + ", " + nextMove.z + ", " + nextMove.w + "]");
            trackForAnimations.Add(currPos);
            aboveCurrPos = new GridRailBehavior.Int4(currPos.x, currPos.y + 1, currPos.z, currPos.w);
            // Normal connection
            if (go.grid.GetValue(nextMove.x, nextMove.y, nextMove.z, nextMove.w) &&
                go.grid.GetShape(nextMove.x, nextMove.y, nextMove.z, nextMove.w).IsConnectedToSpace(currPos))
            {
                if (Int4Equal(nextMove, endPos))
                {
                    Debug.Log("success!");
                    trackForAnimations.Add(endPos);
                    cart.InitializeWaypoints(trackForAnimations);
                    return;
                }
                else
                {
                    GridRailBehavior.Int4 connectedSpace = go.grid.GetShape(nextMove.x, nextMove.y, nextMove.z, nextMove.w).GetConnectedSpace(currPos);
                    if (connectedSpace == null)
                    {
                        Debug.Log("failure!");
                        cart.InitializeWaypoints(trackForAnimations);
                        return;
                    }
                    currPos = nextMove;
                    nextMove = connectedSpace;
                }
            }
            // Up a ramp
            else if (go.grid.GetValue(nextMove.x, nextMove.y, nextMove.z, nextMove.w) &&
                     go.grid.GetShape(nextMove.x, nextMove.y, nextMove.z, nextMove.w).IsConnectedToSpace(aboveCurrPos))
            {
                if (Int4Equal(nextMove, endPos))
                {
                    Debug.Log("success!");
                    trackForAnimations.Add(endPos);
                    cart.InitializeWaypoints(trackForAnimations);
                    return;
                }
                else
                {
                    GridRailBehavior.Int4 connectedSpace = go.grid.GetShape(nextMove.x, nextMove.y, nextMove.z, nextMove.w).GetConnectedSpace(aboveCurrPos);
                    if (connectedSpace == null)
                    {
                        Debug.Log("failure!");
                        cart.InitializeWaypoints(trackForAnimations);
                        return;
                    }
                    currPos = nextMove;
                    nextMove = connectedSpace;
                }
            }
            // Down a ramp
            else if (go.grid.GetValue(nextMove.x, nextMove.y - 1, nextMove.z, nextMove.w) &&
                     go.grid.GetShape(nextMove.x, nextMove.y - 1, nextMove.z, nextMove.w).IsConnectedToSpace(currPos))
            {
                Debug.Log("down a ramp");
                nextMove = new GridRailBehavior.Int4(nextMove.x, nextMove.y - 1, nextMove.z, nextMove.w);
                if (Int4Equal(nextMove, endPos))
                {
                    Debug.Log("success!");
                    trackForAnimations.Add(endPos);
                    cart.InitializeWaypoints(trackForAnimations);
                    return;
                }
                else
                {
                    GridRailBehavior.Int4 connectedSpace = go.grid.GetShape(nextMove.x, nextMove.y, nextMove.z, nextMove.w).GetConnectedSpace(currPos);
                    if (connectedSpace == null)
                    {
                        Debug.Log("failure!");
                        cart.InitializeWaypoints(trackForAnimations);
                        return;
                    }
                    currPos = nextMove;
                    nextMove = connectedSpace;
                }
            }
            // ramp to ramp
            else if (go.grid.GetValue(nextMove.x, nextMove.y - 1, nextMove.z, nextMove.w) &&
                     go.grid.GetShape(nextMove.x, nextMove.y - 1, nextMove.z, nextMove.w).IsConnectedToSpace(aboveCurrPos))
            {
                Debug.Log("ramp to ramp");
                nextMove = new GridRailBehavior.Int4(nextMove.x, nextMove.y - 1, nextMove.z, nextMove.w);
                if (Int4Equal(nextMove, endPos))
                {
                    Debug.Log("success!");
                    trackForAnimations.Add(endPos);
                    cart.InitializeWaypoints(trackForAnimations);
                    return;
                }
                else
                {
                    GridRailBehavior.Int4 connectedSpace = go.grid.GetShape(nextMove.x, nextMove.y, nextMove.z, nextMove.w).GetConnectedSpace(aboveCurrPos);
                    if (connectedSpace == null)
                    {
                        Debug.Log("failure!");
                        cart.InitializeWaypoints(trackForAnimations);
                        return;
                    }
                    currPos = nextMove;
                    nextMove = connectedSpace;
                }
            }
            else
            {
                Debug.Log("failure!");
                cart.InitializeWaypoints(trackForAnimations);
                return;
            }
        }
    }

    private bool Int4Equal(GridRailBehavior.Int4 s1, GridRailBehavior.Int4 s2)
    {
        return s1.x == s2.x &&
               s1.y == s2.y &&
               s1.z == s2.z &&
               s1.w == s2.w;
    }

    public void SetShapeDataToOne()
    {
        foreach (Shape4DStorage data in ot.shapeData)
        {
            data.objectCount = 1;
        }
    }

    public void SetLevel(string fileName)
    {
        levelToLoad = fileName;
    }

    public void SetPlayMode(bool val)
    {
        isPlayMode = val;
    }
}
