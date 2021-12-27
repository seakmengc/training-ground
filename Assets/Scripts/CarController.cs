using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject wheelFrontLeft;
    public GameObject wheelFrontRight;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            wheelFrontLeft.transform.rotation = new Quaternion(0, 180, 0, 0);
            wheelFrontRight.transform.rotation = new Quaternion(0, 0, 0, 0);
        } else
        {
            var horizontalInput = Input.GetAxis("Horizontal");

            wheelFrontLeft.transform.Rotate(Vector3.up * horizontalInput * Time.deltaTime * 15f);
            wheelFrontRight.transform.Rotate(Vector3.up * horizontalInput * Time.deltaTime * 15f);
        }

        var verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * verticalInput * Time.deltaTime * 5f);

        //make car turns based on wheel rotation
        var wheelRotation = wheelFrontLeft.transform.rotation.eulerAngles;
        wheelRotation.y -= 180;
        transform.Rotate(wheelRotation * verticalInput * Time.deltaTime * 5f);

        Debug.Log(wheelRotation);
    }

}
