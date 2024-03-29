using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridRailBehavior : MonoBehaviour
{
    public Vector3Int gridXYZ = new Vector3Int(-1, -1, -1);
    public int gridW = -1;
    public Int4[] directions;
    public bool stackable;
    public bool deletable = true;
    public bool isStart;
    public bool isEnd;
    [SerializeField] private Shape4DStorage data;
    public Int4[] connectedSpaces;

    private void Start()
    {
        if (isStart)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().startPoint = this;
        }
        else if (isEnd)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().endPoint = this;
        }
    }

    private void Update()
    {
        //if (reinitialize) { Debug.Log("Reinitialize"); InitializePathways(); }
    }

    public void InitializePathways()
    {
        Int4 pos = new Int4(gridXYZ.x, gridXYZ.y, gridXYZ.z, gridW);
        connectedSpaces = new Int4[directions.Length];
        for (int i = 0; i < directions.Length; i++)
        {
            Vector3 newDir = gameObject.transform.localRotation * new Vector3(directions[i].x, directions[i].y, directions[i].z);
            // if (newDir == Vector3.zero) { reinitialize = true; }
            // else { reinitialize = false; }
            Debug.Log(newDir);
            connectedSpaces[i] = new Int4(pos.x + Mathf.RoundToInt(newDir.x), pos.y + Mathf.RoundToInt(newDir.y), pos.z + Mathf.RoundToInt(newDir.z), pos.w + directions[i].w);
        }
    }

    public Int4 GetConnectedSpace(Int4 coord)
    {
        for (int i = 0; i < connectedSpaces.Length; i++)
        {
            Int4 space = connectedSpaces[i];
            if (space.x == coord.x &&
                space.y == coord.y &&
                space.z == coord.z &&
                space.w == coord.w)
            {
                int indexToReturn = i == 0 ? 1 : 0;
                return connectedSpaces[indexToReturn];
            }
        }
        return null;
    }

    public bool IsConnectedToSpace(Int4 coord)
    {
        for (int i = 0; i < connectedSpaces.Length; i++)
        {
            Int4 space = connectedSpaces[i];
            if (space.x == coord.x &&
                space.y == coord.y &&
                space.z == coord.z &&
                space.w == coord.w)
            {
                int indexToReturn = i == 0 ? 1 : 0;
                return true;
            }
        }
        return false;
    }

    public void DeleteShape(int x, int y, int z, int w)
    {
        data.IncrementObjectCount();
        Destroy(gameObject);
    }

    public Shape4DStorage GetData()
    {
        return data;
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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(Int4 i41, Int4 i42)
        {
            if (ReferenceEquals(i41, i42)) { return true; }
            if (ReferenceEquals(i41, null)) { return false; }
            if (ReferenceEquals(i42, null)) { return false; }
            return i41.x == i42.x &&
                   i41.y == i42.y &&
                   i41.z == i42.z &&
                   i41.w == i42.w;
        }

        public static bool operator !=(Int4 i41, Int4 i42) => !(i41 == i42);

        public bool Equals(Int4 other)
        {
            if (ReferenceEquals(other, null)) { return false; }
            if (ReferenceEquals(this, other)) { return true; }
            return x == other.x &&
                   y == other.y &&
                   z == other.z &&
                   w == other.w;
        }

        public override bool Equals(object obj) => Equals(obj as Int4);
    }
}
