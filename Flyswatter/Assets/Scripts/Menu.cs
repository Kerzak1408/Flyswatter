using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    private GameObject[][] objectsToAppear;
    int currentIndex = 0;
    private float timer = 1;
    private const float AppearTime = 0.75f;

	// Update is called once per frame
	private void Update ()
    {
        if (objectsToAppear == null || currentIndex >= objectsToAppear.GetLength(0))
        {
            Debug.Log("ObjectsToAppear = " + objectsToAppear);
            return;
        }

        timer -= Time.deltaTime;
        foreach (var go in objectsToAppear[currentIndex])
        {

            go.transform.localScale += (Time.deltaTime / AppearTime) * Vector3.one;
        }
        
        if (timer < 0)
        {
            foreach (var go in objectsToAppear[currentIndex])
            {
                Debug.Log("ObjectToAppear[" + currentIndex + "] contains " + gameObject.name);
                go.transform.localScale = Vector3.one;
            }

            timer = AppearTime;
            currentIndex++;
        }
        
    }

    public void Appear(GameObject[][] objects)
    {
        currentIndex = 0;
        timer = AppearTime;
        objectsToAppear = objects;
        foreach (var array in objects)
        {
            foreach (var gameobject in array)
            {
                gameobject.SetActive(true);
                gameobject.transform.localScale = Vector3.zero;
            }
        }
    }
}
