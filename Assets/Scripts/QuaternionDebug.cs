using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuaternionDebug : MonoBehaviour
{
    public GameObject objectTracking;
    public TMP_Text text;

    private void Update()
    {
        text.text = objectTracking.transform.rotation.ToString();
    }
}
