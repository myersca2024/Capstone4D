using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Credit to CodeMonkey for grid framework
public class Grid4D
{
    private int sizeX;
    private int sizeY;
    private int sizeZ;
    private int sizeW;
    private float cellSize;
    private Vector3 offset;
    private bool[,,,] gridArray;

    public Grid4D(int sizeX, int sizeY, int sizeZ, int sizeW, float cellSize, Vector3 offset)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        this.sizeZ = sizeZ;
        this.sizeW = sizeW;
        this.cellSize = cellSize;
        this.offset = offset;
        gridArray = new bool[sizeX, sizeY, sizeZ, sizeW];

        // DebugDrawGrid();
    }

    private void DebugDrawGrid()
    {
        for (int i = 0; i < gridArray.GetLength(0); i++)
        {
            for (int j = 0; j < gridArray.GetLength(1); j++)
            {
                for (int k = 0; k < gridArray.GetLength(2); k++)
                {
                    Debug.DrawLine(GetWorldPosition(i, j, k), GetWorldPosition(i, j, k + 1), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j, k), GetWorldPosition(i + 1, j, k), Color.red, 100f);
                    Debug.DrawLine(GetWorldPosition(i, j, k), GetWorldPosition(i, j + 1, k), Color.red, 100f);
                }
            }
        }
        Debug.DrawLine(GetWorldPosition(0, 0, sizeZ), GetWorldPosition(sizeX, 0, sizeZ), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(sizeX, 0, 0), GetWorldPosition(sizeX, 0, sizeZ), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(0, sizeY, sizeZ), GetWorldPosition(sizeX, sizeY, sizeZ), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(sizeX, sizeY, 0), GetWorldPosition(sizeX, sizeY, sizeZ), Color.red, 100f);
        Debug.DrawLine(GetWorldPosition(sizeX, 0, sizeZ), GetWorldPosition(sizeX, sizeY, sizeZ), Color.red, 100f);
    }

    public Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + offset;
    }

    public Vector3Int GetXYZ(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - offset.x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.y - offset.y) / cellSize);
        int z = Mathf.FloorToInt((worldPosition.z - offset.z) / cellSize);
        return new Vector3Int(x, y, z);
    }

    public bool ContainsCell(Vector3Int pos, int w)
    {
        bool existsOnX = (pos.x < sizeX) && (pos.x >= 0);
        bool existsOnY = (pos.y < sizeY) && (pos.y >= 0);
        bool existsOnZ = (pos.z < sizeZ) && (pos.z >= 0);
        bool existsOnW = (w < sizeW) && (w >= 0);
        return existsOnX && existsOnY && existsOnZ && existsOnW;
    }
}
