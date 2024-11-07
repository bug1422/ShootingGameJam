using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public static BulletScript instance;
    public int damage = 0;

    GameObject blood;
    public void SetDamage(int damage) => this.damage = damage;
    private void Awake()
    {
        blood = Resources.Load("prefabs/Blood",typeof(GameObject)) as GameObject;
    }
    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(instance);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var flag = collision.gameObject.CompareTag("Hostile");
        if (flag)
        {
            var bloodPos = Vector2.zero;
            var script = collision.GetComponent<EnemyAI>();
            var headArea = script.GetHeadArea();
            var headHeight = (headArea + collision.transform.position.y);
            var output = 0;
            if (transform.position.y >= headHeight)
            {
                output = damage * 2;
                bloodPos = new Vector2(0, headArea);
            }
            else
            {
                output = damage;
            }
            script.DeduceHealth(output);
            print(bloodPos);
            StartCoroutine(AddBlood(collision.gameObject, bloodPos));
        }
        print(collision.gameObject);
        Destroy(this.gameObject);
    }

    IEnumerator AddBlood(GameObject parent, Vector2 offset)
    {
        if (blood != null)
        {
            blood.transform.position = offset;
            var obj = Instantiate(blood, parent.transform);
            yield return new WaitForSeconds(1f);
            GameObject.Destroy(obj);
        }
        yield return new WaitForSeconds(1f);
    }
}
