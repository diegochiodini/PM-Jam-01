using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FlowerSpawner : MonoBehaviour
{
    private GameObject[] flowerSpawnPoints = new GameObject[0];
    public GameObject flower;

	void Start ()
    {
       flowerSpawnPoints = (GameObject.FindGameObjectsWithTag(GameConstants.TAG_FLOWERSPAWNPOINT));
	}

    public void SpawnFlower()
    {
        if(flowerSpawnPoints.Length == 0)
        {
            Debug.LogError("There are no objects tagged with" + GameConstants.TAG_FLOWERSPAWNPOINT + " in the scene!");
            return;
        }
        else
        {
            int index = Random.Range(0, flowerSpawnPoints.Length);
            GameObject spawnPoint = flowerSpawnPoints[index];
            GameObject spawnedFlower = Instantiate(flower, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        }
    }
}
