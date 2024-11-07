using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winning : MonoBehaviour
{
    // Start is called before the first frame update
    Animator animator;
    SpriteRenderer sprite;

    public float speed = 2f;
    bool isWin = false;

    public delegate void OnWinning();
    public static event OnWinning OnWin;
    void Awake()
    {
        OnWin = null;    
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnWin.Invoke();
        print("win");
        sprite.flipX = false;
        animator.SetTrigger("Walk");
        isWin = true;
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (isWin)
        {
            transform.position = new Vector2(transform.position.x + speed,transform.position.y);
        }
    }
}
