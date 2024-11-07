using System.Collections;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    Color invincibleColor;

    [SerializeField]
    int health = 100;

    [SerializeField]
    Vector3 groundRayOffset = Vector3.zero;

    [SerializeField]
    float groundRayDistance;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    int startingGun = 0;

    [SerializeField]
    float jumpForce = 0.1f;

    [SerializeField]
    float velocity = 2.2f;

    PlayerShoot _shoot;
    PlayerHands _hands;
    PlayerAnimation _animation;
    Rigidbody2D _body;

    public static int gunIndex = 0;
    WeaponInfo weapon;

    float inputX = 0f;
    float inputY = 0f;

    bool isFacingRight = true;
    bool isRunning = false;
    bool isJumping = false;
    bool isOnGround = false;
    bool isDying = false;
    bool isWin = false;

    bool shoot = false;
    bool shootCooldown = false;
    bool addJump = false;

    float gravity = 3f;
    float speed = 0f;
    float dieDuration = 1f;
    float respawnDuration = 2f;

    public delegate void OnWinning();
    public static event OnWinning OnWin;

    private void Awake()
    {
    }

    private void Start()
    {
        //Setup PlayerControl
        transform.position = GameObject.Find("Spawnpoint").transform.position;
        _body = GetComponent<Rigidbody2D>();
        _body.bodyType = RigidbodyType2D.Dynamic;

        _hands = GetComponentInChildren<PlayerHands>();
        _animation = GetComponentInChildren<PlayerAnimation>();
        _shoot = GetComponentInChildren<PlayerShoot>();

        //Setup PlayerShoot
        var spawner = GameObject.Find("BulletSpawner");
        var reloadObj = GameObject.Find("Reload");
        reloadObj.SetActive(false);
        var bulletHolder = GameObject.Find("BulletHolder");
        var animator = spawner.GetComponent<Animator>();
        var bullet = Resources.Load("prefabs/Bullet", typeof(GameObject)) as GameObject;
        _shoot.Setup(spawner,reloadObj,bulletHolder, bullet,animator);

        //Setup PlayerHands
        var left = GameObject.Find("Left").transform;
        var right = GameObject.Find("Right").transform;
        _hands.Setup(left, right, _animation);

        //Setup PlayerAnimation
        var leftArm = GameObject.Find("Left").GetComponent<SpriteRenderer>();
        var rightArm = GameObject.Find("Right").GetComponent<SpriteRenderer>();
        var bodyPart = GameObject.Find("Body").GetComponent<SpriteRenderer>();
        var bodyAnim = GameObject.Find("Body").GetComponent<Animator>();
        _animation.Setup(bodyPart, leftArm, rightArm, bodyAnim);

        //Setup HUD
        var healthHUD = GameObject.FindAnyObjectByType<Heart>();
        healthHUD.Setup();
        var bulletHUD = GameObject.FindAnyObjectByType<Bullet>();
        bulletHUD.Setup();

        //Event
        Winning.OnWin += Win;

        gunIndex = startingGun;
        weapon = GunList.getInfo(startingGun);
        _shoot.SetWeapon(weapon);
        _hands.SetWeapon(weapon.GunData.IsRightHand);
        _animation.SetWeapon(weapon);

        PlayerHealth.maxHP = health;
        PlayerHealth.Reset();
    }
    void Win()
    {
        StartCoroutine(WinWalk());
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        if (isWin)
        {
            _body.velocity = new Vector2(speed * 2, 0);
            HandleAnimation();
        }
        else {
            if (PlayerHealth.isAlive)
            {
                CheckJump();
                HandleMovement();
                HandleAnimation();
                GetPlayerMovement();
            }
            else
            {
                if (!isDying) StartCoroutine(Die());
            }
        }
    }
    void CheckJump()
    {
        if(_body.velocity.y <= 0f)
        {
            isJumping = false;
        }
        var hit = Physics2D.Raycast(transform.position + groundRayOffset, Vector3.down, groundRayDistance, groundLayer);
        if (hit && !isJumping)
        {
            print("on ground");
            isOnGround = true;
        }
    }
    private void HandleAnimation()
    {
        if (inputX != 0)
        {
            _animation.Flip(isFacingRight);
            _hands.Flip(isFacingRight);
            _animation.Run();
        }
        else _animation.Stop();
        if (isJumping)
        {
            _animation.Jump();
        }
        if (isOnGround) _animation.Land();
        
    }

    private void HandleMovement()
    {
        speed = inputX * velocity;
        var jump = inputY * jumpForce;
        print(isJumping);
        _body.velocity = new Vector2(speed, _body.velocity.y);
        if(!isJumping)
        {
            if (addJump && isOnGround)
            {
                isOnGround = false;
                addJump = false;
                isJumping = true;
                print("jump" + addJump);
                _body.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
            }
        }
    
    }
    private void GetPlayerMovement()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = 0;
        if (!addJump && Input.GetKeyDown(KeyCode.Space))
        {
            inputY = 1;
            addJump = true;
        }
        if (speed != 0)
        {
            isFacingRight = speed > 0;
            isRunning = true;
        }
        else isRunning = false;

    }

    #region Enumerator
    IEnumerator WinWalk()
    {
        yield return new WaitUntil(() => _body.velocity.y == 0);
        isWin = true;
        yield return new WaitForSeconds(4f);
        OnWin.Invoke();
        _body.bodyType = RigidbodyType2D.Static;
    }
    IEnumerator Die()
    {
        isDying = true;
        _animation.Death();
        yield return new WaitForSeconds(_animation.GetDieClipDuration() + dieDuration);
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {

        PlayerHealth.invincible = true;
        _animation.Respawn();
        PlayerHealth.Reset();
        isDying = false;
        _animation.SetColor(invincibleColor);
        yield return new WaitForSeconds(respawnDuration);
        _animation.SetColor(Color.white);
        PlayerHealth.invincible = false;
    }
    #endregion


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position + groundRayOffset, transform.position + groundRayOffset + (Vector3.down * groundRayDistance));
    }
}