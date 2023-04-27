using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ObjectTracker4D : MonoBehaviour
{
    public Shape4DStorage[] shapeData;
    public int[] shapeTotals;
    public List<ObjectInstruction> instructions;

    private bool loaded = false;

    void Start()
    {
        if (!loaded)
        {
            instructions = new List<ObjectInstruction>();
            shapeTotals = new int[shapeData.Length];
        }
    }

    public void SetObjectCount(int id, int amount)
    {
        if (id >= 0 && id < shapeTotals.Length)
        {
            shapeTotals[id] = amount;
        }
    }

    public bool AddObjectInstruction(Shape4D shape)
    {
        GridRailBehavior rail = shape.gameObject.GetComponent<GridRailBehavior>();
        GridRailBehavior.Int4 pos = new GridRailBehavior.Int4(rail.gridXYZ.x, rail.gridXYZ.y, rail.gridXYZ.z, rail.gridW);
        foreach (ObjectInstruction inst in instructions)
        {
            if (inst.position.x == pos.x && 
                inst.position.y == pos.y && 
                inst.position.z == pos.z && 
                inst.position.w == pos.w)
            {
                return false;
            }
        }
        ObjectInstruction oi = new ObjectInstruction(rail.GetData().ID, pos, shape.gameObject.transform.eulerAngles);
        instructions.Add(oi);
        return true;
    }

    public bool RemoveObjectInstruction(int x, int y, int z, int w)
    {
        foreach (ObjectInstruction inst in instructions)
        {
            if (inst.position.x == x && 
                inst.position.y == y && 
                inst.position.z == z && 
                inst.position.w == w)
            {
                instructions.Remove(inst);
                return true;
            }
        }
        return false;
    }

    public void WriteInstructionsToFile(string fileName)
    {
        if (fileName == "") { return; }

        string path = Application.streamingAssetsPath + "/CustomLevels/" + fileName + ".txt";
        File.WriteAllText(path, "");
        StreamWriter writer = new StreamWriter(path, true);
        foreach (ObjectInstruction inst in instructions)
        {
            string line = "I ";
            line += inst.objectID.ToString() + " ";
            line += inst.position.x.ToString() + " " + inst.position.y.ToString() + " " + inst.position.z.ToString() + " " + inst.position.w.ToString() + " ";
            line += ((int)inst.rotation.x).ToString() + " " + ((int)inst.rotation.y).ToString() + " " + ((int)inst.rotation.z).ToString() + " ";
            writer.WriteLine(line);
        }
        for (int i = 2; i < shapeTotals.Length; i++)
        {
            if (shapeTotals[i] > 0)
            {
                string line = "C ";
                line += i.ToString() + " ";
                line += shapeTotals[i].ToString();
                writer.WriteLine(line);
            }
        }
        writer.Close();
    }

    public void ReadInstructionsFromFile(string fileName)
    {
        if (fileName == "") { return; }

        instructions = new List<ObjectInstruction>();
        shapeTotals = new int[shapeData.Length];
        loaded = true;
        
        string path = Application.streamingAssetsPath + "/" + fileName + ".txt";
        StreamReader reader = new StreamReader(path);

        for (int i = 2; i < shapeTotals.Length; i++)
        {
            shapeTotals[i] = 0;
            shapeData[i].objectCount = 0;
        }

        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] fields = line.Split(' ');

            if (fields[0] == "I")
            {
                int objectID = Int32.Parse(fields[1]);
                int posX = Int32.Parse(fields[2]);
                int posY = Int32.Parse(fields[3]);
                int posZ = Int32.Parse(fields[4]);
                int posW = Int32.Parse(fields[5]);
                int rotX = Int32.Parse(fields[6]);
                int rotY = Int32.Parse(fields[7]);
                int rotZ = Int32.Parse(fields[8]);
                GridRailBehavior.Int4 pos = new GridRailBehavior.Int4(posX, posY, posZ, posW);
                Vector3 rot = new Vector3(rotX, rotY, rotZ);
                ObjectInstruction newInstruction = new ObjectInstruction(objectID, pos, rot);
                instructions.Add(newInstruction);
            }
            else if (fields[0] == "C")
            {
                int objectID = Int32.Parse(fields[1]);
                int count = Int32.Parse(fields[2]);
                shapeTotals[objectID] = count;
                shapeData[objectID].objectCount = count;
            }
        }
    }

    [System.Serializable]
    public struct ObjectInstruction
    {
        public int objectID;
        public GridRailBehavior.Int4 position;
        public Vector3 rotation;

        public ObjectInstruction(int id, GridRailBehavior.Int4 pos, Vector3 rot)
        {
            objectID = id;
            position = pos;
            rotation = rot;
        }
    }
}
