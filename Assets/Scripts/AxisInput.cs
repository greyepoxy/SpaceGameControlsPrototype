using UnityEngine;

[System.Serializable]
public class AxisInput {

	public string verticalAxis;
	public string horizontalAxis;
	public AxisInput(string verticalAxis, string horizontalAxis)
	{
		this.verticalAxis = verticalAxis;
		this.horizontalAxis = horizontalAxis;
	}

	Vector2 bufferedInput = new Vector2(0, 0);
	public Vector2 GetInput(bool buffered = true)
	{
		float xDir = Input.GetAxis(this.horizontalAxis);
        float yDir = Input.GetAxis(this.verticalAxis);

		if (xDir == 0 && yDir == 0)
		{
			if (buffered)
				return bufferedInput;
			else
				return Vector2.zero;
		}
		
		bufferedInput.x = xDir;
		bufferedInput.y = yDir;

		return bufferedInput;
	}
	public bool HasInput()
	{
		return this.GetInput(false /* buffered */).SqrMagnitude() > 0;
	}
}
