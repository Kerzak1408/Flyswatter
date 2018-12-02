using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeKiller : MonoBehaviour
{
    private float timer;
    private bool down;

    private const float SmashTime = 0.1f;
    private Vector2 killPosition;
    Simulation simulation;

    private void Start()
    {
        // Change center of the rotation.
        var onlyChild = transform.GetChild(0);
        //onlyChild.transform.position += Vector3.up * onlyChild.GetComponent<Renderer>().bounds.size.y / 2;

        simulation = Camera.main.GetComponent<Simulation>();
    }

    // Update is called once per frame
    void Update ()
    {
		if (timer > 0)
        {
            timer -= Time.deltaTime;

            if (down)
            {
                transform.Rotate(Vector3.right * (Time.deltaTime / SmashTime) * 90);
                if (timer < 0)
                {
                    timer = SmashTime;
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    down = false;
                    simulation.Kill(killPosition);
                }
            }
            else
            {
                transform.Rotate(Vector3.left * (Time.deltaTime / SmashTime) * 90);
                if (timer < 0)
                {
                    transform.eulerAngles = new Vector3(0, 0, 0);
                    gameObject.SetActive(false);
                }
            }

        }
	}

    public void Kill(Vector2 position)
    {
        AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/Snatch"), Camera.main.transform.position);
        killPosition = position;
        gameObject.SetActive(true);
        var bounds = transform.GetChild(0).GetComponent<Renderer>().bounds.size;
        transform.position = new Vector3(position.x, position.y - bounds.y + bounds.x / 2, transform.position.z);
        timer = SmashTime;
        transform.eulerAngles = new Vector3(-90, 0, 0);
        down = true;
    }
}
