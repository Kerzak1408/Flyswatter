using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderController : CharacterBase
{
    public float speed = 2f;

    private Simulation simulation;

    private void Start()
    {
        simulation = Camera.main.GetComponent<Simulation>();
    }

    private void Update()
    {
        if (target == null)
        {
            SelectNewTarget(simulation.GetRandomBee());

            return;
        }
        
        foreach (var bee in simulation.bees)
        {
            if (((Vector2)transform.position - (Vector2)bee.transform.position).sqrMagnitude < 0.1f)
            {
                AudioSource.PlayClipAtPoint(Resources.Load<AudioClip>("Sounds/Eat"), Camera.main.transform.position);
                simulation.BeeDied(bee);
                return;
            }
        }


        if (target != null)
        {
            var direction = (Vector2)target.transform.position - (Vector2)transform.position;
            transform.position += speed * Time.deltaTime * (Vector3)direction.normalized;
        }

	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected");
    }
}
