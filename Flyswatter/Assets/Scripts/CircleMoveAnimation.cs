using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMoveAnimation : MonoBehaviour
{
    public bool RightDirection;

    private float currentAngle;
    private float radius;
    

	// Use this for initialization
	void Start ()
    {
        radius = Vector3.Distance(transform.position, transform.parent.TransformPoint(Vector3.zero));
        if (!RightDirection)
        {
            currentAngle = Mathf.PI;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (RightDirection)
        {
            currentAngle += Time.deltaTime;
        }
        else
        {
            currentAngle -= Time.deltaTime;
        }

        Debug.Log(name + " --- " + currentAngle);
        currentAngle %= 2 * Mathf.PI;

        var offset = new Vector3(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * radius;
        transform.position = transform.parent.TransformPoint(Vector3.zero) + offset;
    }
}
