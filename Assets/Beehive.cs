using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using GameEvents;

public class Beehive : MonoBehaviour
{
    public Team team;

    public UnityEvent OnScore;


    public int pointsToWin;
    private Collider2D beehiveCollider;
    private int pollenInHive;

    private Transform[] honey = new Transform[3];

	// Use this for initialization
	void Start ()
    {
        pollenInHive = 0;

        honey[0] = transform.FindChild("honey_01");
        honey[1] = transform.FindChild("honey_02");
        honey[2] = transform.FindChild("honey_03");
        for(int i = 0; i < honey.Length; i++)
        {
            honey[i].gameObject.SetActive(false);
        }
        beehiveCollider = GetComponent<Collider2D>();       
	}
	
    void OnCollisionEnter2D (Collision2D other)
    {
        if (other.collider.CompareTag(GameConstants.TAG_PLAYER))
        {
            PollenCollector collector = other.transform.GetComponent<PollenCollector>();
            //Player player = collector.GetComponentInParent<Player>();
            if (collector.HasPollen)
            {
                AddPoint();
                collector.RemovePollen();
            }
        }
    }

    private void AddPoint()
    {
        Debug.Log("Point added!");
        pollenInHive++;

        honey[pollenInHive - 1].gameObject.SetActive(true);

        //if (OnScore != null)
        //{
        //    OnScore.Invoke();
        //}
        //else
        {
            MainGameplay gameplay = GetComponentInParent<MainGameplay>();
            gameplay.OnScore();
        }

        if (pollenInHive == pointsToWin)
        {
            GameOver(team);
        }
    }

    private void GameOver(Team winningTeam)
    {
        Debug.Log(winningTeam + " WON!!");
    }
}
