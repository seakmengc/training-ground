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

	private float maxSteerAngle = 45;
	private float motorForce = 300f;
	private float brakeForce = 600f;

	private float motorAccelerationForce = 1f;
	private bool isBraking = false;

	private void Steer()
	{
		float steeringAngle = maxSteerAngle * horizontalInput * 0.5f;
		frontLeftWheelCollider.steerAngle = steeringAngle;
		frontRightWheelCollider.steerAngle = steeringAngle;
	}

	private void UpdateWheelPoses()
	{
		UpdateWheelPose(frontLeftWheelCollider, frontLeftTransform);
		UpdateWheelPose(frontRightWheelCollider, frontRightTransform);
		UpdateWheelPose(backLeftWheelCollider, backLeftTransform);
		UpdateWheelPose(backRightWheelCollider, backRightTransform);
	}

	private void UpdateRollPose(WheelCollider collider)
    {
		rollTransform.localRotation = new Quaternion(0, 0, collider.steerAngle / -maxSteerAngle, 1);
    }

	//Update car wheel position and rotation based on collider
	private void UpdateWheelPose(WheelCollider collider, Transform transform)
	{
		Vector3 pos;
		Quaternion quat;

		collider.GetWorldPose(out pos, out quat);

		transform.position = pos;
		transform.rotation = quat;
	}

	private void HandleMotor()
    {
		isBraking = Input.GetKey(KeyCode.Space);
		float currBrakeForce = isBraking ? brakeForce : 0f;

		frontLeftWheelCollider.brakeTorque = currBrakeForce;
		frontRightWheelCollider.brakeTorque = currBrakeForce;
		if(isBraking)
        {
			return;
        }

		frontLeftWheelCollider.motorTorque = verticalInput * motorForce * motorAccelerationForce;
		frontRightWheelCollider.motorTorque = verticalInput * motorForce * motorAccelerationForce;
	}

	private void FixedUpdate()
	{
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
		if (motorAccelerationForce < 2)
		{
			motorAccelerationForce += 0.1f;
		}
	}

}
