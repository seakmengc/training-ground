using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        var currRotation = transform.localRotation;
        currRotation.y = Input.GetKey(KeyCode.F) ? 40 / 180f : 0;

        transform.localRotation = currRotation;
    }
}
