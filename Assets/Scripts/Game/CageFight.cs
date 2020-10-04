using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CageFight : MonoBehaviour
{
    public CollideCheck startCheck;

    public GameObject headachePrefab;

    public GameObject bossBar;

    public List<Transform> spawnPoints = new List<Transform>();
    private List<HeadacheAI> enemies = new List<HeadacheAI>();

    public Transform DoorLeft;
    public Transform DoorRight;

    bool started = false;
    bool ended = false;

    private void Update()
    {
        if (startCheck.colliding == true && started == false)
        {
            started = true;
            StartCoroutine(StartCageFight());
        } else if (startCheck.colliding && started == true)
        {
            DoorLeft.gameObject.SetActive(true);
            DoorRight.gameObject.SetActive(true);
        }

        if (started == true && PlayerEntity.instance == null && ended == false)
        {
            DoorLeft.gameObject.SetActive(false);
            DoorRight.gameObject.SetActive(false);
        }
    }

    IEnumerator StartCageFight()
    {
        DoorLeft.gameObject.SetActive(true);
        DoorRight.gameObject.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            Spawn(headachePrefab, spawnPoints[i]);
        }

        bool allDead = false;
        do
        {
            yield return null;

            int alive = 0;
            for (int i = 0; i < 3; i++)
            {
                if (enemies[i].Health > 0)
                    alive += 1;
            }
            if (alive == 0)
                allDead = true;
        } while (!allDead);

        enemies.Clear();

        //Boss
        Spawn(headachePrefab, spawnPoints[1]);
        HeadacheAI boss = enemies[0];
        boss.Health = 1000;
        boss.maxHealth = 1000;
        boss.walkSpeed = 2f;
        boss.transform.localScale = new Vector3(3, 3, 3);
        boss.attackRange = 5;

        GameObject oldHealthBar = boss.transform.Find("Enemy Healthbar").gameObject;
        Destroy(oldHealthBar);
        boss.healthbar = bossBar.GetComponent<BossHealthBar>();
        bossBar.SetActive(true);

        bool allDead2 = false;
        do
        {
            yield return null;

            int alive = 0;

            if (enemies[0].Health > 0)
                alive += 1;

            if (alive == 0)
                allDead2 = true;
        } while (!allDead2);

        bossBar.SetActive(false);

        ended = true;

        DoorLeft.gameObject.SetActive(false);
        DoorRight.gameObject.SetActive(false);

        Destroy(LevelManager.instance);
        SceneManager.LoadScene("FinalCutscene");
    }

    void Spawn(GameObject prefab, Transform spawnPoint)
    {
        GameObject enemy = Instantiate(prefab);
        enemy.transform.position = spawnPoint.position;

        HeadacheAI enemyAI = enemy.GetComponent<HeadacheAI>();

        enemyAI.targetRange = 30f;

        enemies.Add(enemyAI);
    }
}
