using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    public Transform car;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        var carPos = car.position;
        carPos.y = transform.position.y;

        transform.position = carPos;

        transform.rotation = Quaternion.Euler(90f, car.eulerAngles.y, 0f);
    }
}
