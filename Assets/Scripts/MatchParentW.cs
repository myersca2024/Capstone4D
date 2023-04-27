using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchParentW : MonoBehaviour
{
    private Shape4D parent;
    private Shape4D thisShape;

    private void Start()
    {
        thisShape = GetComponent<Shape4D>();
        parent = transform.parent.gameObject.GetComponent<Shape4D>();
    }

    private void Update()
    {
        thisShape.positionW = parent.positionW;
    }
}
