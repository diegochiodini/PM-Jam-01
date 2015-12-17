using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System;

public class MessageBox : MonoBehaviour
{
    public Text message;
    public Button[] actionButtons;

    private bool abortClose = false;

    void Start()
    {
        //TODO: used to deal with disabled editor components on start (they will not call Awake). Look for a better alternative.
        if (!abortClose)
        {
            Close();
        }
    }

    void OnDisable()
    {
        if (actionButtons != null)
        {
            foreach (Button button in actionButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }
    }

    public void Show(string message, Action[] actions = null)
    {
        abortClose = true;
        gameObject.SetActive(true);
        Assert.That(actionButtons != null && actionButtons.Length > 0, "There must be at least one button");

        this.message.text = message;

        if (actionButtons.Length == 1 && actions == null)
        {
            actionButtons[0].onClick.AddListener(Close);
        }
        else
        {
            Assert.That(actions != null && actionButtons.Length == actions.Length, "Must be an action for every assigned button");
            int i = 0;
            foreach (Button button in actionButtons)
            {
                button.onClick.AddListener(new UnityAction (actions[i++]));
            }
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
