using UnityEngine;
using System.Collections;

public class PositionFixBehaviour : MonoBehaviour {
    public GameObject Plane;

    public void FixPosition(Vector3 zeroPosition)
    {
        if (Plane != null)
        {
            Vector3 planeSize = Plane.GetComponent<MeshCollider>().bounds.size;

            gameObject.transform.position = zeroPosition + new Vector3(0.0f, 0.0f, planeSize.z / 2);
        }
    }
}
