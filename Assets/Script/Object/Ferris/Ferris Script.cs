using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisScript : MonoBehaviour
{
    public float rotationSpeed = 5f;
    public Transform[] transforms;
    // Start is called before the first frame update
    void Start()
    {
        transforms = transform.GetComponentsInChildren<Transform>();
        var len = transform.childCount;
        print(len);
        for (int i = 0; i < len; i++)
        {
            transforms[i] = transform.GetChild(i).GetComponent<Transform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        var len = transform.childCount;
        transform.Rotate(Vector3.fwd, rotationSpeed * Time.deltaTime);
        for (int i = 0; i < len; i++)
        {
            transforms[i].Rotate(Vector3.fwd,-rotationSpeed * Time.deltaTime);
        }
    }
}
