using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Data;
using System;

public class LevelButton : MonoBehaviour, LevelButtonInterface 
{
    private LevelStates status;
    public GameObject[] objectsState;
    public GameObject[] objectsRating;

    public int level {get; private set;}

    void Awake()
    {
        level = -1;
    }

    public void SetState(LevelStates status)
    {
        this.status = status;
        int i = 0;
        foreach (GameObject state in objectsState)
        {
            state.SetActive(i++ == (int)status);
        }
    }

    public void SetLevel(int level)
    {
        this.level = level;
        foreach (GameObject state in objectsState)
        {
            bool oldState = state.activeSelf;
            state.SetActive(true);
            Text label = state.GetComponentInChildren<Text>();
            Assert.NotNull(label, "You must provide a label");
            label.text = (level + 1).ToString();
            state.SetActive(oldState);
        }    
    }

    public void SetRating(LevelRating stars)
    {
        int index = (int)stars;
        for (int i = 0; i < objectsRating.Length; i++)
        {
            objectsRating[i].SetActive(i <= index);
        }
    }

    public void OnClick()
    {
        MainController.instance.OnClickLoadLevel(level);
    }
}
