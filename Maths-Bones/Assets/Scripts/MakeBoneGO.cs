using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBoneGO : MonoBehaviour
{
    public GameObject prefabBary;
    public List<GameObject> goPoints = new List<GameObject>();
    public GameObject bary;
    void Start()
    {
        calculBarycentre();
        recenter();
    }

    public void calculBarycentre()
    {
        Vector3 barypos = Vector3.zero;
        foreach (GameObject go in goPoints)
        {
            barypos += go.transform.position;
        }

        barypos /= goPoints.Count;
        bary = Instantiate(prefabBary, barypos, Quaternion.Euler(Vector3.zero));
    }

    public void recenter()
    {
        Vector3 OG = Vector3.zero + bary.transform.position;
        bary.transform.position = Vector3.zero;
        foreach (GameObject go in goPoints)
        {
            go.transform.position -= OG;
        }
    }
    
}
