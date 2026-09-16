using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSetting : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;

    private bool isActive = false;

    void Start()
    {
        settingPanel.SetActive(false);
        isActive = false;
    }

    void Update()
    {
        if ((Input.GetKey(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.BackQuote)) && Input.GetKeyDown(KeyCode.I))
        {
            OpenPanel();
        }
    }

    public void OpenPanel()
    {
        if(isActive)
        {
            settingPanel.SetActive(false);
            isActive = false;
            Time.timeScale = 1f;
        }
        else
        {
            settingPanel.SetActive(true);
            isActive = true;
            Time.timeScale = 0f;
        }
    }
}
