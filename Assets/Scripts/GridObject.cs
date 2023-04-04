using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public Grid4D grid;
    public int sizeX;
    public int sizeY;
    public int sizeZ;
    public int sizeW;
    public float cellSize;
    public float lineThickness;
    public Color lineColor;
    public Shader lineShader;

    void Start()
    {
        grid = new Grid4D(sizeX, sizeY, sizeZ, sizeW, cellSize, transform.position);
        DrawGrid();
    }

    public void DrawGrid()
    {
        Vector3 buffer = new Vector3(0, 0.1f, 0);
        for (int i = 0; i <= sizeZ; i++)
        {
            DrawLine(grid.GetWorldPosition(i, 0, 0) + buffer, grid.GetWorldPosition(i, 0, sizeZ) + buffer, Color.red);
        }
        for (int i = 0; i <= sizeX; i++)
        {
            DrawLine(grid.GetWorldPosition(0, 0, i) + buffer, grid.GetWorldPosition(sizeX, 0, i) + buffer, Color.red);
        }
    }

    // https://answers.unity.com/questions/8338/how-to-draw-a-line-using-script.html
    void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(lineShader);
        lr.startColor = lineColor;
        lr.endColor = lineColor;
        lr.startWidth = lineThickness;
        lr.endWidth = lineThickness;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
