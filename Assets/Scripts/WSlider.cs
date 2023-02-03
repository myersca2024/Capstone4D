using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//******************The slider can move the world W position ****************

public class WSlider : MonoBehaviour
{
    private RaymarchCam cam;
    public Slider wSlider;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<RaymarchCam>();
    }

    // Update is called once per frame
    void Update()
    {
        wSlider.value = cam._wPosition;
    }

    public void ChangeValue()
    {
        cam._wPosition = wSlider.value;
    }
}
