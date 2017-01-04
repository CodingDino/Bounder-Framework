using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HorizontalFOV : MonoBehaviour
{
	public float orthoSize = 5;
    public Camera FOVCamera;

    void Update()
    {
		if (FOVCamera.pixelHeight == 0 || FOVCamera.pixelWidth == 0)
			return;
		
        float aspectRatio = ((float)FOVCamera.pixelWidth) / ((float)FOVCamera.pixelHeight);

        FOVCamera.orthographicSize = orthoSize / aspectRatio;
    }
}
