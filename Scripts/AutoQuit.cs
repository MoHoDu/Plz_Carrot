using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoQuit : MonoBehaviour
{
    bool quit = false;

    [SerializeField]
    GameObject[] quitPanel;

    void Start()
    {
        
    }

    private void Update()
    {
        #if UNITY_ANDROID
        if (quitPanel.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape) && quit == false)
            {
                quitPanel[0].SetActive(true);
                QuitPanelOnOff();
            }
        }
        #endif
    }

    public void QuitPanelOnOff()
    {
        if (quit == true)
        {
            quit = false;
        }
        else
        {
            quit = true;
        }
    }
}
