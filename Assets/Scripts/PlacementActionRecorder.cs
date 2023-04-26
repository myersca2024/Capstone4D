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

    }

    public void PopAction()
    {
        if (actionStack.Count == 0) { return; }
    }

    public class ActionObject
    {
        public ActionType action;
        public Shape4DStorage data;
        public GridRailBehavior.Int4 position;

        public ActionObject(ActionType action, Shape4DStorage data, GridRailBehavior.Int4 position)
        {
            this.action = action;
            this.data = data;
            this.position = position;
        }

        public enum ActionType { Place, Delete };
    }
}
