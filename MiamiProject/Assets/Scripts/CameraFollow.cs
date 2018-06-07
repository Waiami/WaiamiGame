using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothSpeed = 0.125f;

    [SerializeField]
    private Vector3 cameraOffset = new Vector3(0,0,-10);

    [SerializeField]
    private float shakeMagnitude;
    [SerializeField]
    private float shakeDuration;
    [SerializeField]
    private float shakeRotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        MoveCameraSmooth();
	}

    private void MoveCameraSmooth()
    {
        Vector3 desiredPosition = target.position + cameraOffset;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
    }

    public void ResetCamera()
    {
        transform.position = target.position;
    }

    public void StartCameraShake()
    {
        Debug.Log("In");
        StartCoroutine(Shake());
    }

    public void StartLittleCameraShake()
    {
        StartCoroutine(Shake(shakeMagnitude/2, shakeRotation/2));
    }

    IEnumerator Shake(float shake = 0, float rotation = 0)
    {
        if(shake == 0)
        {
            shake = shakeMagnitude;
        }
        if(rotation == 0)
        {
            rotation = shakeRotation;
        }
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < shakeDuration)
        {
            
            float x = Random.Range(-1f, 1f) * shake + originalPos.x;
            float y = Random.Range(-1f, 1f) * shake + originalPos.y;
            
            transform.localPosition = new Vector3(x , y, originalPos.z);
            transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-rotation, rotation));

            elapsed += Time.deltaTime;
            shake /= 1.1f;

            yield return null;
        }
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }
}


