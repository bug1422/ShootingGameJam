using Assets.Script.Enemy;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public EnemyList enemyList;
    Stack<GameObject> gameObjects = new Stack<GameObject>();
    Stack<GameObject> createdList = new Stack<GameObject>();
    bool isSpawning = false;
    GameObject holder;
    Vector3 start, end;
    int rounds;
    // Start is called before the first frame update
    void Start()
    {
        rounds = enemyList.waves;
        holder = GameObject.Find("EnemyHolder");
        foreach(var enemy in enemyList.enemies){
            Object loaded = Resources.Load(enemy.path, typeof(GameObject));
            var obj = loaded as GameObject;
            for (int i = 0; i < enemy.amount; i++)
            {
                gameObjects.Push(obj);
            }
        }
        start = transform.GetChild(0).position;
        end = transform.GetChild(1).position;
    }

    // Update is called once per frame
    void Update()
    {
        Spawning();
    }
    void Spawning()
    {
        if(gameObjects.Count != 0)
        {
            var popped = gameObjects.Pop();
            var created = Instantiate(popped, holder.transform.position, Quaternion.identity);
            created.SetActive(false);
            createdList.Push(created);
            print("spawn");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isSpawning && rounds > 0) StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        while(createdList.Count > 0)
        {
            var left = createdList.Pop();
            left.transform.position = start;
            left.SetActive(true);
            var right = createdList.Pop();
            if (right != null)
            {
                right.transform.position = end;
                right.SetActive(true);
            }
        }
        yield return new WaitForSeconds(enemyList.duration);
        rounds -= 1;
        isSpawning = false;
    }
}
