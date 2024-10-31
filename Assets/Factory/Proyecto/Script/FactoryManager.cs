using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : MonoBehaviour
{
    public BushFactory bushFactory;
    public RockFactory rockFactory;
    public TreeFactory treeFactory;

    public void CreateBush()
    {
        bushFactory.CreateObject();
    }

    public void CreateRock()
    {
        rockFactory.CreateObject();
    }

    public void CreateTree()
    {
        treeFactory.CreateObject();
    }
}
