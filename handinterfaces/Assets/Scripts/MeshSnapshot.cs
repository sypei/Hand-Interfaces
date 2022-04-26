using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSnapshot : MonoBehaviour
{
    private OVRHand ovrHand;
    private bool IsDataValid;
    private bool IsDataHighConfidence;
    private Mesh meshSnapshot;
    [SerializeField]
    private float scaleFactor = 1.2f;
    //private MeshCollider meshCollider;
    // Start is called before the first frame update
    void Start()
    {
        ovrHand = GetComponent<OVRHand>();
        IsDataValid = ovrHand.IsDataValid;
        IsDataHighConfidence = ovrHand.IsDataHighConfidence;
        //Method1
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<SkinnedMeshRenderer>().enabled = true;
        meshSnapshot = new Mesh();

        //Method2
        //GetComponent<OVRSkeleton>().enabled = true;
        //Method2 will interfere gesture detection and object manipulation, thus stop using it.
    }

    // Update is called once per frame
    void Update()
    {
        IsDataValid = ovrHand.IsDataValid;
        IsDataHighConfidence = ovrHand.IsDataHighConfidence;
        if (IsDataHighConfidence && IsDataValid)//if current frame is good, cache it
        {
            //Method2
            //GetComponent<OVRSkeleton>().enabled = true;
            //Method1
            GetComponent<SkinnedMeshRenderer>().BakeMesh(meshSnapshot);//save current pose into meshsnapshot
            meshSnapshot = ScaleMesh(meshSnapshot);
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<SkinnedMeshRenderer>().enabled = true;
        } else {//if current frame is not good, retrieve the latest good frame
            //Method2
            //GetComponent<OVRSkeleton>().enabled = false;
            GetComponent<MeshFilter>().sharedMesh = meshSnapshot;//load the latest good frame
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        
    }

    private Mesh ScaleMesh(Mesh mesh)
    {
        bool RecalculateNormals = false;
        Vector3[] _baseVertices = mesh.vertices;
        
        if (_baseVertices == null)
            _baseVertices = mesh.vertices;
        var vertices = new Vector3[_baseVertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var vertex = _baseVertices[i];
            vertex.x = vertex.x * scaleFactor;
            vertex.y = vertex.y * scaleFactor;
            vertex.z = vertex.z * scaleFactor;
            vertices[i] = vertex;
        }
        mesh.vertices = vertices;
        if (RecalculateNormals)
            mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        
        return mesh;

    }
}
