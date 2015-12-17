using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using Data;
using Utils;
using Extensions;
using UnityEngine.Assertions;

public class LevelSelectionMenu : MonoBehaviour 
{
    public GridLayoutGroup grid;
    public GameObject elementTemplate;
    public int rows = 4;
    public int columns = 4;

    private int currentPage = -1;
    private LevelProviderInterface levelProvider;

    public int maxElementsPerPage
    {
        get
        {
            return rows * columns;
        }

        private set { }
    }

    void Awake()
    {
        grid.CalculateLayoutInputHorizontal();
    }
        
    public void IncrementPage()
    {
        //remeber this is an index and it starts from zero
        currentPage = Mathf.Min(currentPage + 1, Mathf.CeilToInt(levelProvider.GetTotalLevels() / maxElementsPerPage) - 1);
        FillPage(levelProvider, currentPage);
    }

    public void DecrementPage()
    {
        currentPage = Mathf.Max(0, currentPage - 1);
        FillPage(levelProvider, currentPage);
    }

    public void FillPageByCurrentLevel(LevelProviderInterface provider)
    {
        FillPage(provider, provider.GetCurrentLevel() / maxElementsPerPage);
    }

    public void FillPage(LevelProviderInterface provider, int index)
    {
        if (levelProvider == null)
        {
            levelProvider = provider;
            currentPage = index;
        }

        int firstLockedLevel = provider.GetMaxLevelUnlocked() + 1;
        GameObject element;
        int beginElement = index * maxElementsPerPage;
        int endElement = Mathf.Min(provider.GetTotalLevels(), beginElement + maxElementsPerPage);

        int childCount = grid.transform.childCount; //constant within the frame
        bool createElements = (endElement - beginElement) > childCount;

        int childIndex = 0;
        for (int i = beginElement; i < endElement; i++)
        {
            if (createElements && childIndex >= childCount)
            {
                element = GameObject.Instantiate(elementTemplate);
                element.transform.SetParent(grid.transform, false);
            }
            else
            {
                element = grid.transform.GetChild(childIndex).gameObject;
                element.SetActive(true);
            }
            LevelButtonInterface levelInterface = element.GetInterface<LevelButtonInterface>();
            Assert.IsNotNull(levelInterface, "elementTemplate must implement LevelButtonInterface interface");
            levelInterface.SetLevel(i);
            LevelStates levelStatus = provider.GetLevelStatus(i);
            levelInterface.SetState(levelStatus);
            levelInterface.SetRating(provider.GetLevelRating(i));
            childIndex++;
        }

        if (endElement == provider.GetTotalLevels())
        {
            for (; childIndex < grid.transform.childCount; childIndex++)
            {
                element = grid.transform.GetChild(childIndex).gameObject;
                element.SetActive(false);
            }
        }

    }

    public void FillGrid(LevelProviderInterface provider)
    {
        int firstLockedLevel = provider.GetMaxLevelUnlocked() + 1;
        GameObject element;
        bool createElements = grid.transform.childCount == 0;
        for (int i = 0; i < provider.GetTotalLevels(); i++)
        {
            if (createElements)
            {
                element = GameObject.Instantiate(elementTemplate);
                element.transform.SetParent(grid.transform, false);
            }
            else
            {
                element = grid.transform.GetChild(i).gameObject;
            }
            LevelButtonInterface levelInterface = element.GetInterface<LevelButtonInterface>();
            Assert.IsNotNull(levelInterface, "elementTemplate must implement LevelButtonInterface interface");
            levelInterface.SetLevel(i);
            LevelStates levelStatus = provider.GetLevelStatus(i);
            levelInterface.SetState(levelStatus);
            levelInterface.SetRating(provider.GetLevelRating(i));
        }

    }
}
