using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.Events;

public class HomeMenu : MonoBehaviour 
{
    public Text versionLabel;
    public Text scoreLabel;

    public UnityEvent OnSpaceBarDown;

    private void Start()
    {
        versionLabel.text = "v." + CurrentBundleVersion.version;
    }

    void Update()
    {
        if (OnSpaceBarDown != null && Input.GetKeyUp(KeyCode.Space))
        {
            OnSpaceBarDown.Invoke();
        }
    }
}
