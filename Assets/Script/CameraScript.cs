using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    // Start is called before the first frame update
    CinemachineVirtualCamera cam;
    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        Winning.OnWin += Win;
    }
    void Win()
    {
        cam.Follow = null;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
