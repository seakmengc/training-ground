using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Valve.VR;
using System.Threading.Tasks;

public class CarController : MonoBehaviour
{
	private float horizontalInput;
	private float verticalInput;

	public WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
	public WheelCollider backLeftWheelCollider, backRightWheelCollider;
	public Transform frontLeftTransform, frontRightTransform;
	public Transform backLeftTransform, backRightTransform;

	public Transform rollTransform;
	public Transform leftUpperHand, rightUpperHand;

	private float maxSteerAngle = 60;
	private float motorForce = 400f;
	private float brakeForce = 4000f;

	private float motorAccelerationForce = 1f;
	private bool isBraking = false;
	private bool normalInput = true;

	private GameManager gameManager;

	public SteamVR_Action_Boolean grabPinch;
	SteamVR_Input_Sources rightHand = SteamVR_Input_Sources.RightHand;
	SteamVR_Input_Sources leftHand = SteamVR_Input_Sources.LeftHand;

	Renderer cameraScreenRenderer;
	Material frontCamera;
	Material backCamera;

	public Vector3 com;
	private Rigidbody rb;

	private void Start()
    {
		gameManager = FindObjectOfType<GameManager>();

		rb = GetComponent<Rigidbody>();
		rb.centerOfMass = com;

		cameraScreenRenderer = GameObject.Find("CameraFrontScreen").GetComponent<Renderer>();
		frontCamera = Array.Find(cameraScreenRenderer.materials, material => material.name.Contains("CameraFront"));
		backCamera = Array.Find(cameraScreenRenderer.materials, material => material.name.Contains("CameraBack"));

		if (!normalInput)
        {
			grabPinch.AddOnChangeListener(OnTiggerPressedOrReleased, rightHand);
			grabPinch.AddOnChangeListener(OnTiggerPressedOrReleased, leftHand);
		}

	}

	private void OnTiggerPressedOrReleased(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource, bool newState)
	{
		if(fromSource == SteamVR_Input_Sources.RightHand)
        {
			verticalInput = newState ? 1.0f : 0f;
        }

		if (fromSource == SteamVR_Input_Sources.LeftHand)
		{
			isBraking = newState;
		}

		Debug.Log(newState + fromSource.ToString());
    }


    private void FixedUpdate()
	{
		if (gameManager.isNotRunning())
		{
			return;
		}

		//Get input
		if(normalInput)
        {
			horizontalInput = Input.GetAxis("Horizontal");
			verticalInput = Input.GetAxis("Vertical");
		} else
        {
			horizontalInput = InputTracking.GetLocalRotation(XRNode.RightHand).y * -3.0f;
		}

		Debug.Log(verticalInput);

		// Update Camera Screen to Front/Back
		cameraScreenRenderer.material = verticalInput < 0 ? backCamera : frontCamera;    

		Steer();

		HandleMotor();

		UpdateWheelPoses();

		UpdateRollPose(frontLeftWheelCollider);

		// avoid car going too fast when reversing
		if (isBraking || verticalInput < 0)
		{
			motorAccelerationForce = 1f;
		}
		else
        {
			//Increase acceleration overtime
			if (motorAccelerationForce < 5)
			{
				motorAccelerationForce += 0.5f;
			}
		}

	}

    private void OnCollisionEnter(Collision other)
    {
		if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Road Sign"))
        {
			gameManager.ReduceOneLive();
			Debug.Log(other.gameObject.tag);
        }
    }

    private void Steer()
	{
		float steeringAngle = maxSteerAngle * horizontalInput / motorAccelerationForce;
		//Debug.Log(maxSteerAngle + " " + horizontalInput + " " + motorAccelerationForce);
		frontLeftWheelCollider.steerAngle = steeringAngle;
		frontRightWheelCollider.steerAngle = steeringAngle;
	}

	private void UpdateWheelPoses()
	{
		//UpdateWheelPose(frontLeftWheelCollider, frontLeftTransform);
		UpdateWheelPose(frontLeftWheelCollider, frontLeftTransform, 0f);
		UpdateWheelPose(frontRightWheelCollider, frontRightTransform, 0f);
		//UpdateWheelPose(backLeftWheelCollider, backLeftTransform);
		UpdateWheelPose(backLeftWheelCollider, backLeftTransform, 0f);
		UpdateWheelPose(backRightWheelCollider, backRightTransform, 0f);
    }

	private void UpdateRollPose(WheelCollider collider)
    {
		rollTransform.localRotation = new Quaternion(
			rollTransform.localRotation.x,
			rollTransform.localRotation.y,
			collider.steerAngle / -maxSteerAngle,
			rollTransform.localRotation.w
		);

		var changesInZ = rollTransform.localRotation.z * 120f;

        leftUpperHand.localRotation = Quaternion.Euler(
			-1.277f - (changesInZ / 3f),
			-48.368f - changesInZ,
			58.172f - changesInZ
		);

		rightUpperHand.localRotation = Quaternion.Euler(
			0.882f - (changesInZ / 3f),
			35.766f - (changesInZ * 2),
			55.864f + changesInZ
		);
	}

	//Update car wheel position and rotation based on collider
	private void UpdateWheelPose(WheelCollider collider, Transform transform, float yRotation = 180f)
	{
		Vector3 pos;
        Quaternion quat;

        collider.GetWorldPose(out pos, out quat);
		//Debug.Log("Quat: " + quat);
        quat *= Quaternion.Euler(0, yRotation, 0);

        transform.position = pos;
        transform.rotation = quat;
    }

	private void HandleMotor()
    {
		if(normalInput)
        {
			isBraking = Input.GetKey(KeyCode.Space);
		}

		float currBrakeForce = isBraking ? brakeForce : 0f;
		backLeftWheelCollider.brakeTorque = backRightWheelCollider.brakeTorque = currBrakeForce;

		//Notify game manager to update brake UI
		gameManager.SetBraking(isBraking);

		float motorTorque = verticalInput * motorForce * motorAccelerationForce;
		backLeftWheelCollider.motorTorque = backRightWheelCollider.motorTorque = motorTorque;
	}
}
