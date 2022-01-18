using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	private float horizontalInput;
	private float verticalInput;

	public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
	public WheelCollider backLeftWheelCollider, backRightWheelCollider;
	public Transform frontLeftTransform, frontRightTransform;
	public Transform backLeftTransform, backRightTransform;

	public Transform rollTransform;

	private float maxSteerAngle = 60;
	private float motorForce = 400f;
	private float brakeForce = 1000f;

	private float motorAccelerationForce = 1f;
	private bool isBraking = false;

	private GameManager gameManager;

    private void Start()
    {
		gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
	{
		if (gameManager.isNotRunning())
		{
			return;
		}

		//Get input
		horizontalInput = Input.GetAxis("Horizontal");
		verticalInput = Input.GetAxis("Vertical");

		Steer();

		HandleMotor();

		UpdateWheelPoses();

		UpdateRollPose(frontLeftWheelCollider);

		if (isBraking)
		{
			motorAccelerationForce = 1f;
		}

		//Increase acceleration overtime
		if (motorAccelerationForce < 5)
		{
			motorAccelerationForce += 0.5f;
		}
	}

    private void OnCollisionEnter(Collision other)
    {
		if (other.gameObject.CompareTag("Ground"))
        {
			gameManager.ReduceOneLive();
			Debug.Log(other.gameObject.tag);
        }
    }

    private void Steer()
	{
		float steeringAngle = maxSteerAngle * horizontalInput / motorAccelerationForce;
		frontLeftWheelCollider.steerAngle = steeringAngle;
		frontRightWheelCollider.steerAngle = steeringAngle;
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontLeftWheelCollider, frontLeftTransform);
		UpdateWheelPose(frontRightWheelCollider, frontRightTransform, 0f);
		UpdateWheelPose(backLeftWheelCollider, backLeftTransform);
		UpdateWheelPose(backRightWheelCollider, backRightTransform, 0f);
    }

	private void UpdateRollPose(WheelCollider collider)
    {
		rollTransform.localRotation = new Quaternion(0, 0, collider.steerAngle / -maxSteerAngle, 1);
    }

	//Update car wheel position and rotation based on collider
	private void UpdateWheelPose(WheelCollider collider, Transform transform, float yRotation = 180f)
	{
		Vector3 pos;
		Quaternion quat;

		collider.GetWorldPose(out pos, out quat);
		quat *= Quaternion.Euler(0, yRotation, 0);

		transform.position = pos;
		transform.rotation = quat;
	}

	private void HandleMotor()
    {
		isBraking = Input.GetKey(KeyCode.Space);
		float currBrakeForce = isBraking ? brakeForce : 0f;

		frontLeftWheelCollider.brakeTorque = currBrakeForce;
		frontRightWheelCollider.brakeTorque = currBrakeForce;
		//Notify game manager to update brake UI
		gameManager.SetBraking(isBraking);

		if(isBraking)
        {
			return;
        }

		float motorTorque = verticalInput * motorForce * motorAccelerationForce;

		frontLeftWheelCollider.motorTorque = frontRightWheelCollider.motorTorque = motorTorque;
	}
}
