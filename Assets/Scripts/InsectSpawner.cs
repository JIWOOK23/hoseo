using System.Collections;
using UnityEngine;

public class InsectSpawner : MonoBehaviour
{
    public GameObject grade1Prefab;
    public GameObject grade2Prefab;
    public GameObject grade3Prefab;
    public GameObject grade4Prefab;
    public GameObject bombPrefab;

    public SpawnType spawnType = SpawnType.Static;
    public float staticInterval = 2f;
    public float randomIntervalMin = 1f;
    public float randomIntervalMax = 3f;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnRandomInsect();

            float waitTime = (spawnType == SpawnType.Static)
                ? staticInterval
                : Random.Range(randomIntervalMin, randomIntervalMax);

            yield return new WaitForSeconds(waitTime);
        }
    }

    void SpawnRandomInsect()
    {
        InsectGrade randomGrade = GetRandomInsectGrade();

        GameObject prefabToSpawn = GetPrefabByGrade(randomGrade);

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
        }
    }

    InsectGrade GetRandomInsectGrade()
    {
        // 확률 설정: 총합 100 기준
        int roll = Random.Range(0, 100);

        if (roll < 30) return InsectGrade.Grade1;      // 30%
        else if (roll < 55) return InsectGrade.Grade2; // 25%
        else if (roll < 75) return InsectGrade.Grade3; // 20%
        else if (roll < 90) return InsectGrade.Grade4; // 15%
        else return InsectGrade.Bomb;                 // 10%
    }

    GameObject GetPrefabByGrade(InsectGrade grade)
    {
        switch (grade)
        {
            case InsectGrade.Grade1: return grade1Prefab;
            case InsectGrade.Grade2: return grade2Prefab;
            case InsectGrade.Grade3: return grade3Prefab;
            case InsectGrade.Grade4: return grade4Prefab;
            case InsectGrade.Bomb:   return bombPrefab;
            default: return null;
        }
    }
}
public enum SpawnType
{
    Static,
    Random
}
