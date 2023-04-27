using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CustomLevelPanelController : MonoBehaviour
{
    public Button buttonPrefab;
    public GameObject scrollContent;
    public SceneManagement sm;

    private void Start()
    {
        string path = Application.streamingAssetsPath + "/CustomLevels/";
        foreach (string file in Directory.GetFiles(path))
        {
            string[] paths = file.Split("/");
            string fileName = paths[paths.Length - 1];
            if (file.Substring(Mathf.Max(0, file.Length - 4)) == ".txt")
            {
                Button button = Instantiate(buttonPrefab, scrollContent.transform);
                fileName = fileName.Substring(0, fileName.Length - 4);
                button.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = fileName;
                fileName = "CustomLevels/" + fileName;
                button.GetComponent<CustomLevelButtonController>().fileName = fileName;
                button.onClick.AddListener(sm.LoadGameScene);
            }
        }
    }
}
