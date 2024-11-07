using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerHands : MonoBehaviour
{
    public float RotationSpeed = 2f;

    Transform leftHand;
    Transform rightHand;
    PlayerAnimation _animation;

    bool isFacingRight = true;
    private bool isRightHand = false;
    public void Setup(Transform left, Transform right, PlayerAnimation animation)
    {
        this.leftHand = left;
        this.rightHand = right;
        _animation = animation;
    }
    public void SetWeapon(bool isRightHand) {
        if (isRightHand)
        {
            leftHand.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            rightHand.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        this.isRightHand = isRightHand;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerHealth.isAlive) RotatingHands();
    }
    public void Flip(bool value)
    {
        if(isFacingRight != value)
        {
            var dir = new Vector3(1, value ? 1 : -1, 1);
            if (isRightHand)
            {
                rightHand.localScale = dir;
            }
            else
            {
                leftHand.localScale = dir;
            }
            isFacingRight = value;
        }

    }

    void RotatingHands()
    {
        var mousePos = Input.mousePosition;
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (isRightHand)
        {
            rightHand.rotation = Quaternion.Slerp(rightHand.rotation, LookAt(rightHand.position, mouseWorldPos), RotationSpeed * Time.deltaTime);
        }
        else
        {
            leftHand.rotation = Quaternion.Slerp(leftHand.rotation, LookAt(leftHand.position, mouseWorldPos), RotationSpeed * Time.deltaTime);
        }
    }

    Quaternion LookAt(Vector2 start, Vector2 end)
    {
        float angle = Mathf.Atan2(end.y - start.y, end.x - start.x) * Mathf.Rad2Deg;
        FlipArmOtherDir(angle);
        var targetRotation = Quaternion.Euler(0, 0, angle);
        return targetRotation;
    }
    void FlipArmOtherDir(float angle)
    {
        var theChosenHand = isRightHand ? rightHand : leftHand;
        var dir = Vector3.zero;

        if (isFacingRight)
        {
            if (Mathf.Abs(angle) > 90)
            {
                dir = new Vector3(1, -1, 1);
                _animation.FlipBody(true);
                _animation.SetLeftArmOnTop(true);
                _animation.ReverseBodySpeed();
            }
            else
            {
                dir = new Vector3(1, 1, 1);
                _animation.FlipBody(false);
                _animation.SetLeftArmOnTop(false);
                _animation.NormalBodySpeed();
            }
        }
        if (!isFacingRight)
        {
            _animation.SetLeftArmOnTop(true);
            if (Mathf.Abs(angle) < 90)
            {
                dir = new Vector3(1, 1, 1);
                _animation.FlipBody(false);
                _animation.ReverseBodySpeed();
            }
            else
            {
                dir = new Vector3(1, -1, 1);
                _animation.FlipBody(true);
                _animation.NormalBodySpeed();
            }
        }
        theChosenHand.localScale = dir;
    }
}
