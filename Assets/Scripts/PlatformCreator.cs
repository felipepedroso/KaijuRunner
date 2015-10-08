using UnityEngine;
using System.Collections;

public class PlatformCreator : MonoBehaviour {
    public GameObject FloorPrefab;
    public GameObject CurrentPlane;

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
				Vector3 currentSize = CurrentPlane.GetComponent<Renderer>().bounds.size;
				Vector3 currentPosition = CurrentPlane.transform.position;

                //Vector3 position = AttachPoint.position + Vector3.forward * FloorPrefab.transform.GetChild(0).transform.localScale.z * 0.5f;
				Vector3 position = new Vector3(0, 0, currentPosition.z + currentSize.z);
                GameObject newFloor = (GameObject)Instantiate(FloorPrefab, position, Quaternion.identity);
                newFloor.name = "Floor" + PlataformNameCount++;
                Destroy(gameObject);
            }
        }
        
    }
}
