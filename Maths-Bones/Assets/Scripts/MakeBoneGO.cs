using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MakeBoneGO : MonoBehaviour
{
    public GameObject prefabBary;
    public List<GameObject> goPoints = new List<GameObject>();
    public GameObject bary;
    public GameObject qL;
    public GameObject qK;

    public GameObject prefabExtrmum;
    public GameObject PrefabGameObject;
    public Mesh MeshGameObject;
    public Vector3[] Meshvertices;
    public Vector3[] vertexWorldPosition;
    Transform PrefabTransform;
    
    public Vector3 OG=Vector3.zero;
    public List<List<float>> covarMat = new List<List<float>>();
    
    public float valeurPropre = 0;
    public Vector3 vecteurPropre = new Vector3();
    

    public Vector3[] worldPt;
    
    void Start()
    {
        MeshGameObject = PrefabGameObject.GetComponent<MeshFilter>().mesh;
        Meshvertices = MeshGameObject.vertices;
        vertexWorldPosition = Meshvertices;
        PrefabTransform = PrefabGameObject.transform;
        
        for( int i=0 ; i<Meshvertices.Length ; i++ )
        {
            vertexWorldPosition[i] = PrefabTransform.TransformPoint( Meshvertices[i]);
        }

        calculBarycentre();
        recenter();
        initCovarMat();
        covarMatCalcul();
        CalculValeurPropre();
        CalculExtremum();
        decenter();
    }
    public void calculBarycentre()
    {
        Vector3 barypos = Vector3.zero;
        /*foreach (Vector3 pos in vertexWorldPosition)
        {
            barypos += pos;
        }*/
        foreach (GameObject go in goPoints)
        {
            barypos += go.transform.position;
        }
        //barypos /= vertexWorldPosition.Length;
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
        //PrefabTransform.position -= OG;
    }
    public void decenter()
    {
       // OG = Vector3.zero + bary.transform.position;
        bary.transform.position = Vector3.zero;
        foreach (GameObject go in goPoints)
        {
            go.transform.position += OG;
        }

        bary.transform.position += OG;
        qL.transform.position += OG;
        qK.transform.position += OG;
        //PrefabTransform.position -= OG;
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
                
            }
            covar /= goPoints.Count;
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
                
            }
            covar /= goPoints.Count;
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
                
            }
            covar /= goPoints.Count;
        }
        return covar;
    }
    public void covarMatCalcul()
    {
        covarMat[0][0] = variance("x");covarMat[0][1] = covariance("xy"); covarMat[0][2] = covariance("xz");
        covarMat[1][0] = covariance("xy"); covarMat[1][1] = variance("y"); covarMat[1][2] = covariance("yz");
        covarMat[2][0] = covariance("xz"); covarMat[2][1] = covariance("yz"); covarMat[2][2] = variance("z");
        Debug.Log(covarMat[0][0]+" "+covarMat[0][1]+" "+covarMat[0][2]+"\n"+covarMat[1][0]+" "+covarMat[1][1]+" "+covarMat[1][2]+"\n"+covarMat[2][0]+" "+covarMat[2][1]+" "+covarMat[2][2]+"\n");
    }

    public void CalculValeurPropre()
    {
        Vector3 V0 = new Vector3(Random.value, Random.value, 1);
        float tolerance = 0.01f;
        float erreur = tolerance + 1;
        int iteration = 0;
        while (erreur> tolerance && iteration < 100)
        {
            Vector3 Vk = new Vector3();

            // M.Vk
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Vk[i] += covarMat[i][j] * V0[j];
                }
            }

            float precedentValeurPropre = valeurPropre;
            valeurPropre = Vk.magnitude / V0.magnitude;
            erreur = Mathf.Abs(valeurPropre - precedentValeurPropre) / Mathf.Abs(valeurPropre);

            vecteurPropre = Vk.normalized;

            
           /* if (iteration > 0) // Vérification seulement à partir de la deuxième itération
            {
                float lambda = Vector3.Dot(Vk, vecteurPropre) / vecteurPropre.magnitude; // Calcul de lambda
                Vector3 Av = new Vector3();
                for (int i = 0; i < 3; i++) // Calcul de A.v
                {
                    for (int j = 0; j < 3; j++)
                    {
                        Av[i] += covarMat[i][j] * vecteurPropre[j];
                    }
                }
                if ((Av - lambda * vecteurPropre).magnitude > tolerance) // Vérification de la cohérence
                {
                    Debug.LogWarning("La valeur propre et le vecteur propre ne sont pas cohérents ! a l'iteration : " + iteration);
                }
            } */
            V0 = vecteurPropre;
            iteration++;
           // Debug.Log("Valeur propre a l'iteration " + iteration + " = "+ valeurPropre);
            //Debug.Log("Vecteur propre a l'iteration " + iteration + " = "+ vecteurPropre);
        }
    }

    public List<Vector3> ProjectionDonneCentre( )
    {
        List<Vector3> PointProjete = new List<Vector3>() ;
        for (int i = 0 ; i <goPoints.Count  ; i++)
        {
            PointProjete.Add((Vector3.Dot(goPoints[i].transform.position, vecteurPropre) /
                                 Mathf.Pow(vecteurPropre.magnitude, 2)) *
                                vecteurPropre);
        }
        
        Debug.Log(PointProjete.Count);
        return PointProjete;
    }
    public Vector3 ProjectionCentre( Vector3 point)
    {
        return (Vector3.Dot(point, vecteurPropre) / Mathf.Pow(vecteurPropre.magnitude, 2)) * vecteurPropre;
    }
    
    public void CalculExtremum()
    {
        
        List<Vector3> points = ProjectionDonneCentre();
        Debug.Log(points.Count);
        Vector3 minusExtremus = Vector3.zero;
        float minusDistancius = 0;
        Vector3 bigusExtremus = Vector3.zero;
        float bigusDistancius = 0;
        int i = 0;
        int m = 0;
        int b = 0;
        foreach (var point in points)
        {
            i++;
            float distance = Vector3.Distance(bary.transform.position , point);
            float angle = Vector3.SignedAngle(bary.transform.position-point,bary.transform.forward, Vector3.up);
            if (distance >= minusDistancius && angle < 0)
            {
                minusDistancius = distance;
                minusExtremus = point;
                m++;
            }
            else if (distance >= bigusDistancius && angle > 0)
            {
                bigusDistancius = distance;
                bigusExtremus = point;
                b++;
            }

        }
        
        Debug.Log("Total Iter :" + i + "\n Total Minus : "+ m + " Total Bigus : " + b + " = " + (m+b) );
        
        Debug.Log(minusExtremus);
        Debug.Log(bigusExtremus);
        
        qL = Instantiate(prefabExtrmum, minusExtremus, Quaternion.Euler(Vector3.zero));
        qK = Instantiate(prefabExtrmum, bigusExtremus, Quaternion.Euler(Vector3.zero));
    }

}
