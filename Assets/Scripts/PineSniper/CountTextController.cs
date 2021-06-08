using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountTextController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.LookAt(Camera.main.transform);
        Quaternion quaternion = this.transform.rotation;
        quaternion.y = 0;
        this.transform.rotation = quaternion;
    }
}
