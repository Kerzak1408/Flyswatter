using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAnimation : MonoBehaviour
{
    private float fallConst = 2;
    private float timer = 0.3f;
    private SpriteRenderer spriteRenderer;

	// Use this for initialization
	private void Start ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	private void Update ()
    {
		if (timer < 0)
        {
            Destroy(gameObject);
            return;
        }

        timer -= Time.deltaTime;
        var color = spriteRenderer.color;
        color.a -= Time.deltaTime / 0.3f;
        spriteRenderer.color = color;
	}
}
