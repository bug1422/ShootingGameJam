using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Sprite leftDefault;
    [SerializeField]
    Sprite rightDefault;

    [SerializeField]
    AnimationClip die;

    [SerializeField]
    AnimatorOverrideController overrideBodyController;
    RuntimeAnimatorController defaultBodyController;

    int defaultLeftSortingOrder = 2;
    bool isFacingRight = true;
    
    SpriteRenderer _bodyRenderer;
    Animator _bodyAnimator;
    SpriteRenderer _leftRenderer;
    SpriteRenderer _rightRenderer;

    public void Setup(SpriteRenderer bodyPart, SpriteRenderer leftArm, SpriteRenderer rightArm, Animator bodyAnim)
    {
       
        _bodyRenderer = bodyPart;
        _leftRenderer = leftArm;
        _rightRenderer = rightArm;
        _bodyAnimator = bodyAnim;
        defaultBodyController = _bodyAnimator.runtimeAnimatorController;
        defaultLeftSortingOrder = _leftRenderer.sortingOrder;
    }

    public void SetColor(Color color)
    {
        _bodyRenderer.color = color;
        _leftRenderer.color = color;
        _rightRenderer.color = color;
    }
    
    public float GetDieClipDuration()
    {
        return die.length;
    }
    
    public void FlipBody(bool value)
    {
        _bodyRenderer.flipX = value;
    }
    public void Flip(bool value) 
    {
        if(isFacingRight != value)
        {
            /*var dir = new Vector3(value ? 1 : -1, 1, 1);
            transform.localScale = dir;*/
            _leftRenderer.sortingOrder = _rightRenderer.sortingOrder;
            _rightRenderer.sortingOrder = defaultLeftSortingOrder;
            _bodyRenderer.flipX = !value;
            isFacingRight = value;
        }
    }
    public void SetWeapon(WeaponInfo weapon)
    {
        if (weapon.GunData.IsRightHand)
        {
            _leftRenderer.sprite = leftDefault;
            _rightRenderer.sprite = weapon.ArmSprite;
        }
        else
        {
            _leftRenderer.sprite = weapon.ArmSprite;
            _rightRenderer.sprite = rightDefault;
        }
    }
    void SetArmEnabled(bool enabled)
    {
        _leftRenderer.enabled = enabled;
        _rightRenderer.enabled = enabled;
    }
    public void Run() { _bodyAnimator.SetTrigger("Run"); }
    public void Stop() { _bodyAnimator.SetTrigger("Stop"); }
    public void Respawn() {
        SetArmEnabled(true);
        _bodyAnimator.SetBool("IsAlive",true);
    }
    
    public void Death() {
        _bodyAnimator.SetBool("IsAlive",false);
        SetArmEnabled(false);
        
    }
    public void Jump() { _bodyAnimator.SetTrigger("Jump"); }
    public void Land() { _bodyAnimator.SetTrigger("Land"); }
    public void SetLeftArmOnTop(bool value)
    {
        _leftRenderer.sortingOrder = value ? 5 : 0;
    }
    public void ReverseBodySpeed() { _bodyAnimator.runtimeAnimatorController = overrideBodyController; }
    public void NormalBodySpeed() { _bodyAnimator.runtimeAnimatorController = defaultBodyController; }

}
