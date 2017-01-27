using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed;
    public float tilt;

	void FixedUpdate()
	{
		Vector2 direction;
		if (ShouldUseMouseInput())
			direction = GetMouseDirection();
		else
			direction = GetLeftJoystickInput();


		Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
		rigidbody.MoveRotation(GetRotationFrom(direction));

		
		// float moveHorizontal = mouseDirection.x;
        // float moveVertical = mouseDirection.y;

        // Vector3 movement = new Vector3 (moveHorizontal, moveVertical, 0.0f);
		// Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
        // rigidbody.velocity = movement * speed;

        //rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}

	bool useMouseInput = true;
	Vector3 oldMousePos = Vector3.zero;
	bool ShouldUseMouseInput()
	{
		if (!useMouseInput) {
			Vector3 mousePos = GetMousePosOn2DPlane();
			if (mousePos != oldMousePos)
			{
				useMouseInput = true;
			}
		} else {
			if (GetLeftJoystickInput(false /* buffered */).SqrMagnitude() > 0)
			{
				oldMousePos = GetMousePosOn2DPlane();
				useMouseInput = false;
			}
		}
		return useMouseInput;
	}

	Vector2 bufferedJoystickInput = new Vector2(0, 0);
	Vector2 GetLeftJoystickInput(bool buffered = true)
	{
		float xDir = Input.GetAxis("Horizontal");
        float yDir = Input.GetAxis("Vertical");

		if (xDir == 0 && yDir == 0)
		{
			if (buffered)
				return bufferedJoystickInput;
			else
				return Vector2.zero;
		}
		
		bufferedJoystickInput.x = xDir;
		bufferedJoystickInput.y = yDir;

		return bufferedJoystickInput;
	}


	Vector3 GetMousePosOn2DPlane()
	{
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos2D);
	}

	Vector3 GetMouseDirection()
	{
		Vector3 mousePos3D = GetMousePosOn2DPlane();

		Vector3 directionToMouse = mousePos3D - this.transform.position;
		directionToMouse.z = 0;

		float sqrMagnitude = directionToMouse.sqrMagnitude;

		directionToMouse.Normalize();
		return directionToMouse;
	}

	float GetRotationFrom(Vector2 lookDir)
	{
		float zAngle = -90;
		
		// Determine the target rotation.  This is the rotation if the transform looks at the target point.
		float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
		Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle + zAngle));

		return Quaternion.Slerp(this.transform.rotation, targetRotation, 1).eulerAngles.z;
	}
}
