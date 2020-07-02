using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //Vars
    //Data
    private bool bShowSettings;
    //UI Objects
    //Buttons
    [Header("Buttons")]
    public Button settingsButton;

    [Header("Windows")]
    public GameObject settingsWindow;

    // Start is called before the first frame update
    void Start()
    {
        bShowSettings = false;
        settingsWindow.SetActive(false);
}

    // Update is called once per frame
    void Update()
    {
        
    }

    //Toggle Settings Menu
    public void ToggleSettings()
    {
        bShowSettings = !bShowSettings;
        if (bShowSettings)
        {
            settingsWindow.SetActive(true);
        }
        else
        {
            settingsWindow.SetActive(false);
        }
    }
}
