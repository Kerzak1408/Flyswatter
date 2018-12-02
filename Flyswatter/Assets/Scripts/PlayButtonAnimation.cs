using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButtonAnimation : MonoBehaviour
{
    private float waitingTimer;
    private float animationTimer;

    private const float NoAnimationTime = 4;
    private const float AnimationTime = 0.4f;
    private const int AnimationDegrees = 360 * 5;

	// Use this for initialization
	void Start ()
    {
        waitingTimer = NoAnimationTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (waitingTimer > 0)
        {
            waitingTimer -= Time.deltaTime;
            if (waitingTimer <= 0)
            {
                animationTimer = AnimationTime;
            }
        }
        else
        {
            
            if (animationTimer < 0)
            {
                transform.eulerAngles = Vector3.zero;
                waitingTimer = NoAnimationTime;
            }
        }
	}
}
