using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float speed;
    public float tilt;

	MousePositionInput mouseInput;
	public AxisInput mainJoystickInput;

	PlayerController()
	{
		mouseInput = new MousePositionInput();
		mainJoystickInput = new AxisInput("Vertical", "Horizontal");
	}
	
	void FixedUpdate()
	{
		Vector2 direction;
		if (ShouldUseMouseInput())
			direction = mouseInput.GetMouseDirectionRelativeFrom(this.transform.position);
		else
			direction = mainJoystickInput.GetInput();


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
	bool ShouldUseMouseInput()
	{
		if (!useMouseInput) {
			if (mouseInput.IsActive())
			{
				useMouseInput = true;
			}
		} else {
			if (mainJoystickInput.HasInput())
			{
				useMouseInput = false;
			}
		}
		return useMouseInput;
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
