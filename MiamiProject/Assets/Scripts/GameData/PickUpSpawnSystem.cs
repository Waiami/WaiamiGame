using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PickUpSpawnSystem : MonoBehaviour {

    [SerializeField] GameObject NearPlayer1PickUps;
    [SerializeField] GameObject NearPlayer2PickUps;
    [SerializeField] GameObject NearPlayer3PickUps;
    [SerializeField] GameObject NearPlayer4PickUps;
    [SerializeField] GameObject RandomPickUps;

    public void SpawnNewPickUps()
    {
        ClearPickUps();
        SpawnPickUps(NearPlayer1PickUps, 1);
        SpawnPickUps(NearPlayer2PickUps, 1);
        SpawnPickUps(NearPlayer3PickUps, 1);
        SpawnPickUps(NearPlayer4PickUps, 1);
        SpawnPickUps(RandomPickUps, GameStats.Instance.MaxPickUps);

    }

    private void ClearPickUps()
    {
        foreach (Transform child in NearPlayer1PickUps.transform)
        {
            if(child.childCount >= 0)
            {
                foreach(Transform pickUp in child)
                {
                    Destroy(pickUp.gameObject);
                }
            }
        }
        foreach (Transform child in NearPlayer2PickUps.transform)
        {
            if (child.childCount >= 0)
            {
                foreach (Transform pickUp in child)
                {
                    Destroy(pickUp.gameObject);
                }
            }
        }
        foreach (Transform child in NearPlayer3PickUps.transform)
        {
            if (child.childCount >= 0)
            {
                foreach (Transform pickUp in child)
                {
                    Destroy(pickUp.gameObject);
                }
            }
        }
        foreach (Transform child in RandomPickUps.transform)
        {
            if (child.childCount >= 0)
            {
                foreach (Transform pickUp in child)
                {
                    Destroy(pickUp.gameObject);
                }
            }
        }
    }

    private void SpawnPickUps(GameObject spawnObject, int max)
    {
        int count = spawnObject.transform.childCount;
        var rnd = new System.Random();
        var randomNumbers = Enumerable.Range(0, count).OrderBy(x => rnd.Next()).Take(max).ToList();

        for (int i = 0; i < max; i++)
        {
            Transform child = spawnObject.transform.GetChild(randomNumbers[i]);
            Instantiate(GameStats.Instance.PickUp, child);
        }
    }
}
