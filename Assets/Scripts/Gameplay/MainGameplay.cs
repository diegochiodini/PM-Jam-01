using UnityEngine;
using System.Collections;
using GameUtils;
using Data;
using UnityEngine.Assertions;
using UnityEngine.Events;
using GameEvents;

public class MainGameplay : MonoBehaviour, IGameplay
{
    public FlowerSpawner flowerSpawner;

    private Camera gameCamera;
    private Vector3 viewportPosition;
    private GameplayStates _status;

    public GameplayStates GetStatus()
    {
        return _status;
    }

    public void SetStatus(GameplayStates status)
    {
        if (_status != status)
        {
            _status = status;
            switch (_status)
            {
                case GameplayStates.STARTED:
                    {
                        gameObject.SetActive(true);
                        Reset();
                        break;
                    }
                case GameplayStates.PLAYING:
                    {
                        gameObject.SetActive(true);
                        break;
                    }
                default:
                    {
                        gameObject.SetActive(false);
                        break;
                    }
            }
        }
    }

    void Awake()
    {
        Assert.IsNotNull(flowerSpawner);
    }

    void Start()
    {
        SetStatus(GameplayStates.STARTED);
        flowerSpawner.SpawnFlower();
        flowerSpawner.SpawnFlower();
        flowerSpawner.SpawnFlower();
        flowerSpawner.SpawnFlower();
        flowerSpawner.SpawnFlower();
        flowerSpawner.SpawnFlower();
    }

    void Update()
    {
        if (GetStatus() != GameplayStates.PLAYING)
        {
            return;
        }

        if (Input.GetKeyUp(KeyCode.R))
        {
            Reset();
        }


        //check exit condition
        //SetStatus(GameplayStates.GAME_OVER);        
    }

    private void Reset()
    {
        //player.Reset();
        SetStatus(GameplayStates.PLAYING);
    }

    public void OnScore()
    {
        flowerSpawner.SpawnFlower();
    }
}
