using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeFactory : Factory
{
    [SerializeField] private GameObject treePrefab;

    public override GameObject CreateObject()
    {
        return Instantiate(treePrefab);

    }
}
