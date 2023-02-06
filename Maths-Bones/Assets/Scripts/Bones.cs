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
    public List<Vector3> listeVertices = new List<Vector3>();

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
              listeVertices.Add(vertice);  
            }
           print(listeVertices.Count);
        }
    }
}
