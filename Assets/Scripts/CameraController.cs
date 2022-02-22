using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(GameManager.gameState == GameState.Won)
        {
            TransitionCameraToBackAndHigher();
        }

        var currRotation = transform.localRotation;
        currRotation.y = Input.GetKey(KeyCode.F) ? 40 / 180f : 0;

        transform.localRotation = currRotation;
    }

    private void TransitionCameraToBackAndHigher()
    {
        if(transform.localPosition.y > 10)
        {
            return;
        }

        transform.Translate(new Vector3(0f, 0.5f, -2f) * Time.deltaTime, Space.Self);
    }
}
