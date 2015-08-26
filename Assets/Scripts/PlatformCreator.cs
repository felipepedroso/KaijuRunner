using UnityEngine;
using System.Collections;

public class PlatformCreator : MonoBehaviour {
    public GameObject FloorPrefab;
    public Transform CurrentPlaneTransform;

    public static int PlataformNameCount = 1;
    void Start() 
    {
        FloorPrefab = Resources.Load<GameObject>("Prefabs/Floor");
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag.ToLower() == "player")
        {
            if (FloorPrefab != null)
            {
                float creatorZ = gameObject.transform.position.z;
                //Vector3 position = AttachPoint.position + Vector3.forward * FloorPrefab.transform.GetChild(0).transform.localScale.z * 0.5f;
                Vector3 position = new Vector3(0, 0, CurrentPlaneTransform.position.z + (CurrentPlaneTransform.localScale.z * 10));
                GameObject newFloor = (GameObject)Instantiate(FloorPrefab, position, Quaternion.identity);
                newFloor.name = "Floor" + PlataformNameCount++;

            }
        }
        Destroy(gameObject);
    }
}
