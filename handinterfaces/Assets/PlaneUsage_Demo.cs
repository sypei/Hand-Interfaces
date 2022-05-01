using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.Events;
using UnityEngine.UI;

/**
 * This class is an example of how to setup a cutting Plane from a GameObject
 * and how to work with coordinate systems.
 * 
 * When a Place slices a Mesh, the Mesh is in local coordinates whilst the Plane
 * is in world coordinates. The first step is to bring the Plane into the coordinate system
 * of the mesh we want to slice. This script shows how to do that.
 */
public class PlaneUsage_Demo : MonoBehaviour {
	
	[SerializeField]
	private VirtualScissors_Demo VS;
	public GameObject objToCut;
	public Material crossMat;
	public Vector3 normalVector;
	public GameObject slicePlane;
	public bool recursiveSlice;
	public PlaneUsage_Demo plane;
	public float sliceDistance = 0.1f;

	/**
	 * This function will slice the provided object by the plane defined in this
	 * GameObject. We use the GameObject this script is attached to define the position
	 * and direction of our cutting Plane. Results are then returned to the user.
	 */
	public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null) {
        // slice the provided object using the transforms of this object
        return obj.Slice(transform.position, transform.up, crossSectionMaterial);
	}

	void Update(){
		normalVector = slicePlane.transform.up;
		//if (Input.GetKeyDown(KeyCode.C)) {
		if (VS.IsCutting){
			// only slice the parent object
			if (!recursiveSlice) {
                SlicedHull hull = plane.SliceObject(objToCut, crossMat);

				if (hull != null) {
					GameObject newObject1 = hull.CreateLowerHull(objToCut, crossMat);
					GameObject newObject2 = hull.CreateUpperHull(objToCut, crossMat);
					newObject1.transform.localPosition -= sliceDistance*normalVector;
					newObject2.transform.localPosition += sliceDistance*normalVector;
					newObject1.AddComponent<BoxCollider>();
					newObject2.AddComponent<BoxCollider>();
					newObject1.layer = LayerMask.NameToLayer("Cuttable");
					newObject2.layer = LayerMask.NameToLayer("Cuttable");
					Destroy(objToCut);
					//objToCut.SetActive(false);
				}
			}
			else {
				// in here we slice both the parent and all child objects
                SliceObjectRecursive(plane, objToCut, crossMat);

				Destroy(objToCut);
				//objToCut.SetActive(false);
			}
		}
	}
	private void OnTriggerStay(Collider collision){ 
		if (collision.gameObject.layer == 3){//Cuttable Layer
			objToCut = collision.gameObject;
		}
	
	}
    public GameObject[] SliceObjectRecursive(PlaneUsage_Demo plane, GameObject obj, Material crossSectionMaterial) {

		// finally slice the requested object and return
        SlicedHull finalHull = plane.SliceObject(obj, crossSectionMaterial);

		if (finalHull != null) {
			GameObject lowerParent = finalHull.CreateLowerHull(obj, crossMat);
			GameObject upperParent = finalHull.CreateUpperHull(obj, crossMat);

			if (obj.transform.childCount > 0) {
				foreach (Transform child in obj.transform) {
					if (child != null && child.gameObject != null) {

						// if the child has chilren, we need to recurse deeper
						if (child.childCount > 0) {
                            GameObject[] children = SliceObjectRecursive(plane, child.gameObject, crossSectionMaterial);

							if (children != null) {
								// add the lower hull of the child if available
								if (children[0] != null && lowerParent != null) {
									children[0].transform.SetParent(lowerParent.transform, false);
								}

								// add the upper hull of this child if available
								if (children[1] != null && upperParent != null) {
									children[1].transform.SetParent(upperParent.transform, false);
								}
							}
						}
						else {
							// otherwise, just slice the child object
                            SlicedHull hull = plane.SliceObject(child.gameObject, crossSectionMaterial);

							if (hull != null) {
								GameObject childLowerHull = hull.CreateLowerHull(child.gameObject, crossMat);
								GameObject childUpperHull = hull.CreateUpperHull(child.gameObject, crossMat);

								// add the lower hull of the child if available
								if (childLowerHull != null && lowerParent != null) {
									childLowerHull.transform.SetParent(lowerParent.transform, false);
								}

								// add the upper hull of the child if available
								if (childUpperHull != null && upperParent != null) {
									childUpperHull.transform.SetParent(upperParent.transform, false);
								}
							}
						}
					}
				}
			}

			return new GameObject[] {lowerParent, upperParent};
		}

		return null;
	}
	#if UNITY_EDITOR
	/**
	 * This is for Visual debugging purposes in the editor 
	 */
	public void OnDrawGizmos() {
		EzySlice.Plane cuttingPlane = new EzySlice.Plane();

		// the plane will be set to the same coordinates as the object that this
		// script is attached to
		// NOTE -> Debug Gizmo drawing only works if we pass the transform
		cuttingPlane.Compute(transform);

		// draw gizmos for the plane
		// NOTE -> Debug Gizmo drawing is ONLY available in editor mode. Do NOT try
		// to run this in the final build or you'll get crashes (most likey)
		cuttingPlane.OnDebugDraw();
	}

	#endif
}
