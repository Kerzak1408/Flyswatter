using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    protected GameObject target;

    protected void SelectNewTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}

