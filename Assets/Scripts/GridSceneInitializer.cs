using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSceneInitializer : MonoBehaviour
{
    public List<ObjectInitializerInstruction> instructions;
    public RaymarchCam rc;
    public GridObject go;

    private void Start()
    {
        InitializePuzzleObjects();
    }

    public void InitializePuzzleObjects()
    {
        foreach (ObjectInitializerInstruction instruction in instructions)
        {
            InitializeShape(instruction.shape, instruction.gridPosition, instruction.wPosition, instruction.rotation);
        }
    }

    private void InitializeShape(Shape4D obj, Vector3Int gridPos, int wPos, Vector3 objRot)
    {
        Vector3Int spot = new Vector3Int((int)gridPos.x, (int)gridPos.y, (int)gridPos.z);
        if (obj == null || go.grid.GetValue(spot.x, spot.y, spot.z, wPos)) { return; }

        Shape4D shape = Instantiate(obj,
                            go.grid.GetWorldPosition(spot.x, spot.y, spot.z) +
                            new Vector3(go.cellSize / 2f, obj.gameObject.transform.localScale.y + spot.y, go.cellSize / 2f),
                            obj.transform.rotation);
        shape.gameObject.SetActive(true);
        shape.positionW = (wPos - 1) * 2;
        GridRailBehavior grb = shape.gameObject.GetComponent<GridRailBehavior>();
        grb.gridXYZ = spot;
        grb.gridW = wPos;
        go.grid.SetValue(grb, spot.x, spot.y, spot.z, wPos); // CHANGE 1 TO FLOOR OF W_SIZE / 2 LATER
        shape.transform.eulerAngles += objRot;
        grb.InitializePathways();
    }

    [System.Serializable]
    public class ObjectInitializerInstruction
    {
        public Shape4D shape;
        public Vector3Int gridPosition;
        public int wPosition;
        public Vector3 rotation;
    }
}
