using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        for (int i = 0; i < 3; i++)
        {
            covarMat.Add(new List<float>(3));
            for (int j = 0; j < 3; j++)
            {
                covarMat[i].Add(0.0f);
            }
        }
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
    public float covariance(string angle)
    {
        float covar = 0;
        if (angle == "xy")
        {
            Debug.Log("xy");
            float moyX = 0;
            float moyY = 0;
            foreach (GameObject go in goPoints)
            {
                Vector3 pos = go.transform.position;
                moyX += pos.x;
                moyY += pos.y;
            }
            moyX /= goPoints.Count;
            moyY /= goPoints.Count;
            foreach (GameObject go in goPoints)
            {
                Vector3 pos = go.transform.position;
                covar += (pos.x - moyX) * (pos.y - moyY);
                covar /= goPoints.Count;
            }
        }
        else if (angle == "xz")
        {
            Debug.Log("xz");
            float moyX = 0;
            float moyZ = 0;
            foreach (GameObject go in goPoints)
            {
                Vector3 pos = go.transform.position;
                moyX += pos.x;
                moyZ += pos.z;
            }
            moyX /= goPoints.Count;
            moyZ /= goPoints.Count;
            foreach (GameObject go in goPoints)
            {
                Vector3 pos = go.transform.position;
                covar += (pos.x - moyX) * (pos.z - moyZ);
                covar /= goPoints.Count;
            }
        }
        else if (angle == "yz")
        {
            Debug.Log("yz");
            float moyZ = 0;
            float moyY = 0;
            foreach (GameObject go in goPoints)
            {
                Vector3 pos = go.transform.position;
                moyZ += pos.z;
                moyY += pos.y;
            }
            moyZ /= goPoints.Count;
            moyY /= goPoints.Count;
            foreach (GameObject go in goPoints)
            {
                Vector3 pos = go.transform.position;
                covar += (pos.z - moyZ) * (pos.y - moyY);
                covar /= goPoints.Count;
            }
        }
        return covar;
    }

    public void covarMatCalcul()
    {
        covarMat[0][0] = variance("x");
        covarMat[0][1] = covariance("xy");
        covarMat[0][2] = covariance("xz");
        covarMat[1][0] = covariance("xy");
        covarMat[1][1] = variance("y");
        covarMat[1][2] = covariance("yz");
        covarMat[2][0] = covariance("xz");
        covarMat[2][1] = covariance("xy");
        covarMat[2][2] = variance("z");
        Debug.Log(covarMat[0][0]+" "+covarMat[0][1]+" "+covarMat[0][2]+"\n"+covarMat[1][0]+" "+covarMat[1][1]+" "+covarMat[1][2]+"\n"+covarMat[2][0]+" "+covarMat[2][1]+" "+covarMat[2][2]+"\n");
    }
}
