using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelUIController : MonoBehaviour
{
    [SerializeField] private Button placeButton;
    [SerializeField] private Button deleteButton;
    [SerializeField] private Button runButton;
    [SerializeField] private Button[] wAxisButtons;
    [SerializeField] private Image buttonHighlight;

    private ObjectPlacer4D op;

    private void Start()
    {
        op = GameObject.FindGameObjectWithTag("ObjectPlacer").GetComponent<ObjectPlacer4D>();
    }

    public void UpdateHighlight(RectTransform transform)
    {
        buttonHighlight.rectTransform.anchoredPosition = transform.anchoredPosition;
    }

    public void SetButtonsActive(bool isActive)
    {
        placeButton.interactable = isActive;
        deleteButton.interactable = isActive;
        runButton.interactable = isActive;
        foreach (Button wAxisButton in wAxisButtons)
        {
            wAxisButton.interactable = isActive;
        }
    }
}
