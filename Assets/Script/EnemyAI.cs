using System.Collections;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    SpriteHandler sprite;
    Transform transform;
    Transform head;
    [SerializeField]
    int health = 500;

    [SerializeField]
    int biteDamage = 20;

    float angerMeter = 0;
    float boringMeter = 0;
    [SerializeField]
    float minRange = 50f, maxRange = 100f;
    [SerializeField]
    float alertDist = 8f;
    [SerializeField]
    float biteDist = 1f;

    bool isAlive = true;
    bool isFacingLeft = true;
    bool isWalking = false;
    bool isTracking = false;
    bool touchPlayer = false;
    //Velocity
    [SerializeField]
    float velocity = 1f;

    Rigidbody2D rb;
    Transform player;
    //
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player")?.transform;
        transform = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponentInChildren<SpriteHandler>();
        head = transform.Find("Head");
        StartCoroutine(Idle());
    }
    public void DeduceHealth(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            isAlive = false;
            rb.bodyType = RigidbodyType2D.Static;
            for (int i = 0; i < transform.childCount; i++)
            {
                var entity = transform.GetChild(i);
                if(entity.CompareTag("Decal")) GameObject.Destroy(entity.gameObject);
            }
            StartCoroutine(Die());
        }
    }
    public float GetHeadArea()
    {
        return head.transform.localPosition.y;
    }
    #region Trigger Collision
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchPlayer = true;
            if(PlayerHealth.isAlive) StartCoroutine(Bite());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            touchPlayer = false;
        }
    }
    #endregion
    #region Collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.collider.name);
    }
    #endregion

    private void Update()
    {
        if(isAlive) sprite.Flip(isFacingLeft);
        if (rb.velocity.x != 0)
        {
            sprite.Walk();
        }
        else sprite.Idle();
    }
    IEnumerator Die()
    {
        sprite.Die();
        var duration = sprite.GetDieDuration();
        yield return new WaitForSeconds(duration + 2);
        sprite.TurnCorpse();
    }
    IEnumerator Walk()
    {
        isWalking = true;
        isFacingLeft = Random.Range(-10, 10) % 2 == 0 ? true : false;
        var dirVec = (isFacingLeft ? 1 : -1) * velocity;
        var duration = Random.Range(2, 4);
        int i = 0;
        while (i <= duration)
        {
            rb.velocity = new Vector2(dirVec, 0);
            yield return new WaitForSeconds(1);
            i++;
        }
        boringMeter = 0;
        isWalking = false;
        yield return new WaitForSeconds(2);
        if(isAlive) StartCoroutine(Idle());
    }
    IEnumerator Chase()
    {
        isWalking = true;
        float dist = 0;
        while (dist < alertDist)
        {
            isFacingLeft = transform.position.x < player.position.x ? true : false;
            var dirVec = (isFacingLeft ? 1 : -1) * velocity;
            rb.velocity = new Vector2(dirVec, 0);
            yield return new WaitForSeconds(1);
            dist = Vector2.Distance(transform.position, player.position);
        }
        boringMeter = 0;
        isWalking = false;
        yield return new WaitForSeconds(1);
        if (isAlive) StartCoroutine(Idle());
    }
    IEnumerator Idle()
    {
        boringMeter += 10;
        print(boringMeter);
        if (boringMeter > Random.Range(minRange, maxRange))
        {
            if (!isWalking && isAlive)
            {
                CheckForPlayer();
            }
            else boringMeter = 0;
        }
        else
        {
            yield return new WaitForSeconds(2f);
            if (isAlive) StartCoroutine(Idle());
        }
    }
    IEnumerator Bite()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
        sprite.Bite();
        yield return new WaitForSeconds(sprite.GetBiteDuration());
        if (touchPlayer)
        {
            PlayerHealth.DeduceHealth(biteDamage);
            Heart.UpdateHeart(biteDamage);
        }
    }

    void CheckForPlayer()
    {
        if (player)
        {
            var dist = Vector2.Distance(transform.position, player.position);
            if (dist <= alertDist)
            {
                StartCoroutine(Chase());
            }
            else StartCoroutine(Walk());
        }
        else StartCoroutine(Walk());
    }
}
