using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using GameUtils;
using Data;

public class SimpleMenu : SwitchMenu 
{
    protected enum MenuTags
    {
        MENU_MAIN = 0,
        MENU_HUD,
        MENU_PAUSE,
        MENU_WIN,
        MENU_GAMEOVER,
        MENU_LEVEL,
        MENU_LOADING,
        MENU_MSG_BOX,
        MENU_CHARACTER,
    }

    public GameObject uiBackground;
    public GameObject gameBackground;
    public PaletteBehaviour palette;

    private int score = 0;

    //private MainController mainController;

    void Awake()
    {
        Loading();
    }

    protected override void RefreshChildren(int index)
    {
        base.RefreshChildren(index);

        if (index == (int)MenuTags.MENU_HUD || index == (int)MenuTags.MENU_PAUSE)
        {
            uiBackground.SetActive(false);
            gameBackground.SetActive(true);
            VertexColors background = gameBackground.GetComponent<VertexColors>();
            if (palette != null && Application.isPlaying) //TODO fix a bug in the editor where palette == null
            {
                background.startColor = palette.currentPalette.bottomBackground;
                background.endColor = palette.currentPalette.topBackground;
            }
        }
        else
        {
            uiBackground.SetActive(true);
            gameBackground.SetActive(false);
        }
    }

    public void PlayGame()
    {
        Log.Print("PlayGame");
        activeChild = (int)MenuTags.MENU_HUD;
        uiBackground.SetActive(false);
        gameBackground.SetActive(true);
    }

    public void CharacterSelection()
    {
        activeChild = (int)MenuTags.MENU_CHARACTER;
        SelectionMenu menu = GetActiveObjectComponent<SelectionMenu>();
    }

    public void Home()
    {
        activeChild = (int)MenuTags.MENU_MAIN;
        HomeMenu homeMenu = GetActiveObjectComponent<HomeMenu>();
        UpdateScoreLabel(homeMenu.scoreLabel);
    }

    public void Restart()
    {
        activeChild = (int)MenuTags.MENU_HUD;
        uiBackground.SetActive(false);
        gameBackground.SetActive(true);
    }

    public void Pause()
    {
        activeChild = (int)MenuTags.MENU_PAUSE;
    }

    public void Unpause()
    {
        activeChild = (int)MenuTags.MENU_HUD;
    }

    public void Win(int currentLevel)
    {
        activeChild = (int)MenuTags.MENU_WIN;
        WinMenu winMenu = GetActiveObjectComponent<WinMenu>();
        winMenu.levelLabel.text = "LVL. " + (currentLevel + 1).ToString();
        UpdateScoreLabel(winMenu.scoreLabel);
    }

    public void GameOver(int currentLevel)
    {
        activeChild = (int)MenuTags.MENU_GAMEOVER;
        GameOverMenu gameOverMenu = GetActiveObjectComponent<GameOverMenu>();
        gameOverMenu.levelLabel.text = "LVL. " + currentLevel.ToString();
        UpdateScoreLabel(gameOverMenu.scoreLabel);
    }

    public void Levels(LevelProviderInterface levelProvider)
    {
        activeChild = (int)MenuTags.MENU_LEVEL;
        LevelSelectionMenu menu = GetActiveObjectComponent<LevelSelectionMenu>();
        menu.FillPageByCurrentLevel(levelProvider);
    }

    public void Loading()
    {
        activeChild = (int)MenuTags.MENU_LOADING;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    private void UpdateScoreLabel(Text label)
    {
        label.text = CommonConstants.LABEL_SCORE + score.ToString();
    }

    public void MessageBox(string message, Action[] actions)
    {
        activeChild = (int)MenuTags.MENU_MSG_BOX;
        MessageBox menu = GetActiveObjectComponent<MessageBox>();
        menu.Show(message, actions);
    }
}
