using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockFactory : Factory
{

    [SerializeField] private GameObject rockPrefab;

    public override GameObject CreateObject()
    {
        return Instantiate(rockPrefab);
    }
}
