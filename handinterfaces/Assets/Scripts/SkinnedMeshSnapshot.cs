using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshSnapshot : MonoBehaviour
{
    [SerializeField]
    private SkinnedMeshRenderer meshRenderer;
    [SerializeField]
    private MeshCollider meshCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Mesh meshSnapshot = new Mesh();
        meshRenderer.BakeMesh(meshSnapshot);
        meshCollider.sharedMesh = null;
        meshCollider.sharedMesh = meshSnapshot;
    }
}
