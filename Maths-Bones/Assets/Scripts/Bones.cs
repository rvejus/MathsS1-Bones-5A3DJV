using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bones : MonoBehaviour
{
    public GameObject rightArm ;
    public GameObject leftArm ;
    public GameObject rightLegs;
    public GameObject leftLegs ;
    public GameObject head;
    public List<Vector3> listeVerticesRA = new List<Vector3>();
    public List<Vector3> listeVerticesLA = new List<Vector3>();
    public List<Vector3> listeVerticesHE = new List<Vector3>();
    public List<Vector3> listeVerticesRL = new List<Vector3>();
    public List<Vector3> listeVerticesLL = new List<Vector3>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log(rightArm.GetComponent<MeshFilter>().mesh.vertices.Length);
            foreach (var vertice in rightArm.GetComponent<MeshFilter>().mesh.vertices)
            {
                listeVerticesRA.Add(vertice);  
            }
           print(listeVerticesRA.Count);
           
           Debug.Log(leftArm.GetComponent<MeshFilter>().mesh.vertices.Length);
           foreach (var vertice in leftArm.GetComponent<MeshFilter>().mesh.vertices)
           {
               listeVerticesLA.Add(vertice);  
           }
           print(listeVerticesLA.Count);
           
           Debug.Log(rightLegs.GetComponent<MeshFilter>().mesh.vertices.Length);
           foreach (var vertice in rightArm.GetComponent<MeshFilter>().mesh.vertices)
           {
               listeVerticesRL.Add(vertice);  
           }
           print(listeVerticesRL.Count);
           
           Debug.Log(leftLegs.GetComponent<MeshFilter>().mesh.vertices.Length);
           foreach (var vertice in rightArm.GetComponent<MeshFilter>().mesh.vertices)
           {
               listeVerticesLL.Add(vertice);  
           }
           print(listeVerticesLL.Count);
           
           Debug.Log(head.GetComponent<MeshFilter>().mesh.vertices.Length);
           foreach (var vertice in rightArm.GetComponent<MeshFilter>().mesh.vertices)
           {
               listeVerticesHE.Add(vertice);  
           }
           print(listeVerticesHE.Count);
        }
    }
}
