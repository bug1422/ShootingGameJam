using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    TextMeshProUGUI _text;
    Image _image;
    int ammo = PlayerShoot.magazine;
    public void Setup()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _image = GetComponentInChildren<Image>();
        PlayerShoot.OnAmmoUpdateHUD += UpdateAmmo;
        PlayerShoot.OnGunUpdateHUD += UpdateGun;
    }
    private void UpdateGun()
    {
        _image.sprite = GunList.getSprite();
    }
    private void UpdateAmmo()
    {
        if (_text != null) _text.text = $"X {PlayerShoot.shotLeft}";
    }
}
