using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushFactory : Factory
{
    [SerializeField] private GameObject bushPrefab;

    public override GameObject CreateObject()
    {
        return Instantiate(bushPrefab);
    }
}
