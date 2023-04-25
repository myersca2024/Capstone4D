using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRailBehavior : MonoBehaviour
{
    public Vector3Int gridXYZ = new Vector3Int(-1, -1, -1);
    public int gridW = -1;
    public Int4[] directions = new Int4[2];
    public bool stackable;
    public bool deletable = true;

    private Int4[] connectedSpaces = new Int4[2];

    public void InitializePathways()
    {
        Int4 pos = new Int4(gridXYZ.x, gridXYZ.y, gridXYZ.z, gridW);
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 newDir = gameObject.transform.localRotation * new Vector3(directions[i].x, directions[i].y, directions[i].z);
            connectedSpaces[i] = new Int4(pos.x + (int)newDir.x, pos.y + (int)newDir.y, pos.z + (int)newDir.z, pos.w + directions[0].w);
        }
    }

    [System.Serializable]
    public class Int4
    {
        public int x;
        public int y;
        public int z;
        public int w;

        public Int4(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
