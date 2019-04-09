using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour // utils class for the enemy damage
{
    static public Material[] GetAllMaterials(GameObject go) // gets all the materials of a game object
    {
        Renderer[] rends = go.GetComponentsInChildren<Renderer>(); // includes children
        List<Material> mats = new List<Material>(); // gets the materials
        foreach (Renderer rend in rends) // for all the objects get the renderers
        {
            mats.Add(rend.material);
        }
        return mats.ToArray();
    }
}
