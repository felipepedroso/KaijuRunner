using UnityEngine;
using System.Collections;

public class PlatformCreator : MonoBehaviour {
    public GameObject FloorPrefab;
    public GameObject CurrentPlane;
    public static int PlataformNameCount = 1;


    public GameObject DebugPoint;

    void Start() 
    {
        GameObject[] FloorPrefabs = Resources.LoadAll<GameObject>("StreetBlocks");

        if (FloorPrefabs != null && FloorPrefabs.Length > 0)
        {
            FloorPrefab = FloorPrefabs[Random.Range(0, FloorPrefabs.Length)];
        }
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.tag.ToLower() == "player")
        {
            if (FloorPrefab != null)
            {
				Vector3 currentSize = CurrentPlane.GetComponent<MeshCollider>().bounds.size;
                Vector3 currentPosition = gameObject.transform.parent.position;

                Vector3 zeroPosition = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z + currentSize.z / 2);

                if (DebugPoint)
                {
                    DebugPoint.transform.position = zeroPosition;
                }

                GameObject newFloor = (GameObject)Instantiate(FloorPrefab, zeroPosition, Quaternion.identity);
                newFloor.SendMessage("FixPosition", zeroPosition);
                newFloor.name = "Floor" + PlataformNameCount++;
                //Destroy(gameObject);
            }
        }
        
    }
}
