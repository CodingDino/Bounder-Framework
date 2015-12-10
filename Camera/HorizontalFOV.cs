using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class HorizontalFOV : MonoBehaviour
{
	public float orthoSize = 5;
	public float defaultAspectRatio = 1.765f;
    public float positionScale = 5;
    public Camera FOVCamera;

    void Update()
    {
        float aspectRatio = ((float)FOVCamera.pixelWidth) / ((float)FOVCamera.pixelHeight);

        FOVCamera.orthographicSize = orthoSize / aspectRatio;

        float positionAdjust = (defaultAspectRatio - aspectRatio) * positionScale;
        FOVCamera.transform.position = new Vector3(FOVCamera.transform.position.x,
                                                     positionAdjust,
                                                     FOVCamera.transform.position.z);
    }
}
