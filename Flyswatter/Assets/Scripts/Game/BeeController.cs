using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeController : CharacterBase
{
    private GameObject BeeHive;

    private GameObject[] flowers;

    private enum State { FlyingToFlower, FlyingFromFlower }

	private void Update ()
    {
		if (flowers == null)
        {
            return;
        }

        // Flought to beehive and reached it.
        if (target == BeeHive && ((Vector2)transform.position - (Vector2)target.transform.position).sqrMagnitude < 0.01f)
        {
            int flowerIndex = Random.Range(0, flowers.Length);
            SelectNewTarget(flowers[flowerIndex]);
            return;
        }

        // Flought to a flower and reached it.
        if (((Vector2) transform.position - (Vector2) target.transform.position).sqrMagnitude < 0.01f)
        {
            SelectNewTarget(BeeHive);
            return;
        }

        // Continue flying.
        var direction = (Vector2) target.transform.position - (Vector2) transform.position;
        transform.position += Time.deltaTime * (Vector3) direction.normalized;
	}



    public void Initialize(List<GameObject> flowers, GameObject beehive)
    {
        this.flowers = flowers.ToArray();
        int flowerIndex = Random.Range(0, flowers.Count);
        SelectNewTarget(flowers[flowerIndex]);
        BeeHive = beehive;
    }
}
