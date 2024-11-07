using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShoot: MonoBehaviour
{
    public GameObject bullet;
    GameObject spawner;
    GameObject bulletHolder;
    GameObject reloadObj;
    Animator animator;
    WeaponInfo weapon;

    Stack<GameObject> spawned;
    public static int shotLeft = 0;
    public static int magazine = 0;
    bool isShooting = false;
    bool isReloading = false;
    bool win = false;
    public delegate void OnShoot();
    public static event OnShoot OnAmmoUpdateHUD;

    public delegate void OnGunChange();
    public static event OnGunChange OnGunUpdateHUD;
    void Awake()
    {
        OnAmmoUpdateHUD = null;
        OnGunUpdateHUD = null;
    }
    public void Setup(GameObject spawner, GameObject reloadObj, GameObject bulletHolder, GameObject bullet, Animator animator)
    {
        this.spawner = spawner;
        this.reloadObj = reloadObj;
        this.bulletHolder = bulletHolder;
        this.animator = animator;
        this.bullet = bullet;
    }
    public void SetWeapon(WeaponInfo weapon)
    {
        this.weapon = weapon;
        spawner.transform.localPosition = weapon.BulletPosition;
        animator.runtimeAnimatorController = weapon.GunData.BulletAnimation;
        bullet.GetComponent<SpriteRenderer>().sprite = weapon.BulletSprite;
        var script = bullet.GetComponent<BulletScript>();
        script.SetDamage(weapon.GunData.Damage);
        OnGunUpdateHUD.Invoke();
        magazine = weapon.GunData.Magazine;
        shotLeft = magazine;
        OnAmmoUpdateHUD.Invoke();
        PlayerControl.OnWin += Win;
    }
    void Win() => win = true;
    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            StartCoroutine(Reload());
        }
        if (!isShooting && !isReloading)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Fire();
                StartCoroutine(Shoot());
            }
        }

    }
    IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(weapon.GunData.CooldownPerShot);
        isShooting = false;
    }
    IEnumerator Reload()
    {
        isReloading = true;
        reloadObj.SetActive(true);
        shotLeft = magazine;
        yield return new WaitForSeconds(weapon.GunData.ReloadTime);
        reloadObj.SetActive(false);
        OnAmmoUpdateHUD.Invoke();
        isReloading = false;
    }
    public void Fire()
    {
        if(shotLeft != 0)
        {
            var type = weapon.GunData.GetType();

            bullet.transform.position = spawner.transform.position;
            var start = spawner.transform.position;
            var end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
            bullet.transform.localRotation = Quaternion.Euler(0, 0, angle);

            if (type == typeof(ShotgunData))
            {
                ShootShotgun();
            }
            else if (type == typeof(PistolData))
            {
                ShootPistol();
            }
            else if (type == typeof(RifleData))
            {
                ShootPistol();
            }
            animator.SetTrigger("Shoot");
            shotLeft -= 1;
            OnAmmoUpdateHUD.Invoke();
        }
    }
    void ShootShotgun()
    {

        var shotgunInfo = (weapon.GunData as ShotgunData);
        var startAngle = -1 * shotgunInfo.SpreadAngle * Mathf.FloorToInt(shotgunInfo.SpreadLine / 2);
        var zAngle = bullet.transform.localRotation.z - startAngle;
        for (int i = 0; i < shotgunInfo.SpreadLine; i++)
        {
            print(startAngle);
            var instance = Instantiate(bullet);
            instance.transform.position = spawner.transform.position;
            instance.transform.localRotation = Quaternion.Euler(0, 0, startAngle);
            var dir = GetBulletDirection();
            var body = instance.GetComponent<Rigidbody2D>();
            var rotatedDir = Quaternion.AngleAxis(startAngle, Vector3.forward) * dir;
            body.AddForce(rotatedDir * weapon.GunData.BulletSpeed, ForceMode2D.Impulse);
            startAngle += shotgunInfo.SpreadAngle;
            zAngle += shotgunInfo.SpreadAngle;
        }

    }
    void ShootPistol()
    {
        var instance = Instantiate(bullet);
        instance.transform.position = spawner.transform.position;
        var dir = GetBulletDirection();
        var body = instance.GetComponent<Rigidbody2D>();
        body.AddForce(dir * weapon.GunData.BulletSpeed, ForceMode2D.Impulse);
    }
    Vector3 GetBulletDirection()
    {
        var edge = spawner.GetComponent<EdgeCollider2D>();
        var points = edge.points;
        var spawnPos = spawner.transform.position;
        var start = new Vector2(spawnPos.x, spawnPos.y) + points[0];
        var end = new Vector2(spawnPos.x, spawnPos.y) + points[1];
        var result = edge.transform.TransformDirection(end - start);
        return result;
    }
}