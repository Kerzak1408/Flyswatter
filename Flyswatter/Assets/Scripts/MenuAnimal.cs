using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimal : MonoBehaviour
{
    public bool RightDirection;

    private float behindPositionY = 3.790829f;
    private float frontPositionY = 3.032729f;
    private float worldWidth;
    
    private Vector3 currentDirection;

    // Use this for initialization
    void Start ()
    {
        Vector3 topLeftPoint = Camera.main.ViewportToWorldPoint(new Vector2(0, 1));
        var worldHeight = topLeftPoint.y * 2;
        Vector3 bottomRightPoint = Camera.main.ViewportToWorldPoint(new Vector2(1, 0));
        worldWidth = bottomRightPoint.x * 2;
        Debug.Log("Width = " + worldWidth + " Height = " + worldHeight);
        currentDirection = RightDirection ? Vector3.right : Vector3.left;
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.position += Time.deltaTime * currentDirection;
        if (currentDirection == Vector3.right && transform.position.x > 2.9f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0.01f);
            currentDirection = Vector3.up;
        }
        else if (currentDirection == Vector3.left && transform.position.x < -2.9f)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.01f);
            currentDirection = Vector3.down;
        }
        else if (currentDirection == Vector3.up && transform.position.y > behindPositionY)
        {
            transform.position = new Vector3(transform.position.x, behindPositionY, transform.position.z);
            currentDirection = Vector3.left;
        }
        else if (currentDirection == Vector3.down && transform.position.y < frontPositionY)
        {
            transform.position = new Vector3(transform.position.x, frontPositionY, transform.position.z);
            currentDirection = Vector3.right;
        }
    }
}
