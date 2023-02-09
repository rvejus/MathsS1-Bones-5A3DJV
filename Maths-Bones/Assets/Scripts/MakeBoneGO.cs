using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBoneGO : MonoBehaviour
{
    public GameObject prefabBary;
    public List<GameObject> goPoints = new List<GameObject>();
    public GameObject bary;
    public Vector3 OG=Vector3.zero;
    public List<List<float>> covarMat = new List<List<float>>();
    void Start()
    {
        calculBarycentre();
        recenter();
        initCovarMat();
        covarMatCalcul();
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
        OG = Vector3.zero + bary.transform.position;
        bary.transform.position = Vector3.zero;
        foreach (GameObject go in goPoints)
        {
            go.transform.position -= OG;
        }
    }
    public void initCovarMat()
    {
        covarMat.Add(new List<float>(3));
        covarMat.Add(new List<float>(3));
        covarMat.Add(new List<float>(3));
    }
    public float variance(string angle)
    {
        float Var = 0;
        float moyenne = 0;
        if (angle == "x")
        {
            foreach (GameObject go in goPoints)
            {
                moyenne += go.transform.position.x;
                Var += Mathf.Pow(go.transform.position.x, 2);
            }
            
        }else if (angle == "y")
        {
            foreach (GameObject go in goPoints)
            {
                moyenne += go.transform.position.y;
                Var += Mathf.Pow(go.transform.position.y, 2);
            }
            
        }else if (angle == "z")
        {
            foreach (GameObject go in goPoints)
            {
                moyenne += go.transform.position.z;
                Var += Mathf.Pow(go.transform.position.z, 2);
            }
            
        }
        moyenne /= goPoints.Count;
        Var /= goPoints.Count;
        Var -= Mathf.Pow(moyenne, 2);
        return Var;
    }
    public void covarMatCalcul()
    {
        
    }
}
