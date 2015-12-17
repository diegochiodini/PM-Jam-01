using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class HomeMenu : MonoBehaviour 
{
    public Text versionLabel;
    public Text scoreLabel;
    public Action hotKeyAction;

    private void Start()
    {
        versionLabel.text = "v." + CurrentBundleVersion.version;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (hotKeyAction != null)
            {
                hotKeyAction();
            }
        }

    }
}
