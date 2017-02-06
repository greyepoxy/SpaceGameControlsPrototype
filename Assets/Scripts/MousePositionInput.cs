using UnityEngine;

public class MousePositionInput
{
    Vector3 oldMousePos = Vector3.zero;
    public bool IsActive()
    {
        Vector3 mousePos = GetMousePosOn2DMainCameraPlane();
        if (mousePos != oldMousePos)
        {
            oldMousePos = mousePos;
            return true;
        }
        return false;
    }

    Vector3 GetMousePosOn2DMainCameraPlane()
	{
		Vector3 mousePos2D = Input.mousePosition;
		mousePos2D.z = -Camera.main.transform.position.z;
		return Camera.main.ScreenToWorldPoint(mousePos2D);
	}

	public Vector3 GetMouseDirectionRelativeFrom(Vector3 relativePos)
	{
		Vector3 mousePos3D = GetMousePosOn2DMainCameraPlane();

		Vector3 directionToMouse = mousePos3D - relativePos;
		directionToMouse.z = 0;

		float sqrMagnitude = directionToMouse.sqrMagnitude;

		directionToMouse.Normalize();
		return directionToMouse;
	}
}