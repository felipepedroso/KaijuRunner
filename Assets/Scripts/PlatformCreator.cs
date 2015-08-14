using UnityEngine;
using System.Collections;

public class PlatformCreator : MonoBehaviour {
    public GameObject FloorPrefab;
    public Transform AttachPoint;

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag.ToLower() == "player")
        {
            if (FloorPrefab != null)
            {
                Vector3 position = AttachPoint.position + Vector3.forward * FloorPrefab.transform.GetChild(0).transform.localScale.z * 0.5f;
                GameObject newFloor = (GameObject)Instantiate(FloorPrefab, position, Quaternion.identity);
                newFloor.name = "Floor";

            }
        }
    }
}
