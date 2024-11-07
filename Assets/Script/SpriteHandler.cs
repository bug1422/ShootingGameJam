using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteHandler : MonoBehaviour
{
    SpriteRenderer _renderer;
    Animator _animator;
    [SerializeField]
    AnimationClip biteClip;
    [SerializeField]
    AnimationClip dieClip;
    GameObject sprite;
    // Start is called before the first frame update
    void Awake()
    {
        sprite = transform.GetChild(0).gameObject;
        _renderer = sprite.GetComponent<SpriteRenderer>();
        _animator = sprite.GetComponent<Animator>();
    }
    public void Flip(bool value)
    {
        _renderer.flipX = value;
    }
    public void Die()
    {
        _animator.SetBool("IsAlive",false);
    }
    public void Walk()
    {
        _animator.SetTrigger("Walk");
    }
    public void Idle()
    {
        _animator.SetTrigger("Idle");
    }
    public void Bite()
    {
        _animator.SetTrigger("Bite");
    }
    public float GetBiteDuration()
    {
        return biteClip.length;
    }
    public float GetDieDuration()
    {
        return dieClip.length;
    }
    public void TurnCorpse()
    {
        Destroy(_animator);
        _renderer.sortingLayerName = "DecoFront";
        sprite.transform.SetParent(null, true);
        print(sprite.transform.position);
        Destroy(this.gameObject);
    }
}
