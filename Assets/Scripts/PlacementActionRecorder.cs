using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementActionRecorder : MonoBehaviour
{
    public Stack<ActionObject> actionStack;
    public GridObjectStorageBehavior gosb;
    public GridObject go;

    private void Start()
    {
        actionStack = new Stack<ActionObject>();
    }

    public void PushAction(Shape4D shape, int shapeID)
    {
        ActionObject ao = new ActionObject(shape, shapeID);
        actionStack.Push(ao);
    }

    public void PopAction()
    {
        if (actionStack.Count == 0) { return; }
        
        ActionObject ao = actionStack.Peek();
        gosb.IncrementShapeCount(ao.shapeID);
        GridRailBehavior grb = ao.objectPlaced.gameObject.GetComponent<GridRailBehavior>();
        go.grid.SetValue(false, grb.gridXYZ.x, grb.gridXYZ.y, grb.gridXYZ.z, grb.gridW);
        Destroy(ao.objectPlaced.gameObject);
        actionStack.Pop();
    }

    public class ActionObject
    {
        public Shape4D objectPlaced;
        public int shapeID;

        public ActionObject(Shape4D objectPlaced, int shapeID)
        {
            this.objectPlaced = objectPlaced;
            this.shapeID = shapeID;
        }
    }
}
