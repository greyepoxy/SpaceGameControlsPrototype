using UnityEngine;

public class PlayerController : MonoBehaviour
{

	public float maxAccelWithoutBoost;
	public float maxVelocityWithoutBoost;

	MousePositionInput mouseInput;
	public AxisInput mainJoystickInput;
	public AxisInput secondaryJoystickInput;

	PlayerController()
	{
		mouseInput = new MousePositionInput();
		mainJoystickInput = new AxisInput("MainVertical", "MainHorizontal");
		secondaryJoystickInput = new AxisInput("SecondaryVertical", "SecondaryHorizontal");
	}
	
	void FixedUpdate()
	{
		Vector2 directionToFace;
		if (ShouldUseMouseInput())
			directionToFace = mouseInput.GetMouseDirectionRelativeFrom(this.transform.position);
		else
			directionToFace = mainJoystickInput.GetInput();

		Vector2 moveDirection = secondaryJoystickInput.GetInput(false /*buffered*/);

		Rigidbody2D rigidbody = this.GetComponent<Rigidbody2D>();
		rigidbody.MoveRotation(GetRotationFrom(directionToFace));

		rigidbody.AddForce(moveDirection * this.maxAccelWithoutBoost * rigidbody.mass);

		if (rigidbody.velocity.SqrMagnitude() > maxVelocityWithoutBoost * maxVelocityWithoutBoost)
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxVelocityWithoutBoost;
		}
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
