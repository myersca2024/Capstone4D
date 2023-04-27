using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExportUIController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;

    private ObjectTracker4D ot;

    void Start()
    {
        ot = GameObject.FindGameObjectWithTag("ObjectTracker").GetComponent<ObjectTracker4D>();
    }

    public void WriteToFile()
    {
        Debug.Log(inputField.text);
        ot.WriteInstructionsToFile(inputField.text);
    }
}
