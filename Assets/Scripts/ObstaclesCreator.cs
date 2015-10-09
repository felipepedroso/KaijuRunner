using UnityEngine;
using System.Collections;

public class ObstaclesCreator : MonoBehaviour {
    public GameObject[] ObstaclesPrefabs;
    public GameObject PlatformGameObject;

    public int Rows;
    public int Columns;

	// Use this for initialization
	void Start () {
        if (ObstaclesPrefabs == null || ObstaclesPrefabs.Length < 1 || PlatformGameObject == null)
        {
            Debug.LogError("Please check if the obstacles prefabs or the Platform GameObject where set.");
            return;
        }
        
        Vector3 platformPosition = PlatformGameObject.transform.position;
        Vector3 platformDimensions = PlatformGameObject.GetComponent<Renderer>().bounds.size;

        float minZ = platformPosition.z - platformDimensions.z/2;
        float maxZ = platformPosition.z + platformDimensions.z/2;

        float minX = platformPosition.x - platformDimensions.x * 0.125f;
        float maxX = platformPosition.x + platformDimensions.x * 0.125f;

        float zOffset = platformDimensions.z / (Rows + 1);
        float xOffset = (platformDimensions.x * 0.25f) / (Columns + 1);

        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Columns; j++)
            {
                if (Random.Range(0, 100) % 2 == 0)
                {
                    continue;
                }
                GameObject obstaclePrefab = ObstaclesPrefabs[Random.Range(0, ObstaclesPrefabs.Length)];

                GameObject go = (GameObject)Instantiate(obstaclePrefab, new Vector3(minX + ((j+1) * xOffset), 1.5f, minZ + ((i + 1) * zOffset)), Quaternion.identity);
                go.transform.parent = gameObject.transform;
            }
        }
	}

    private Vector3 GetObstacleDimensions(GameObject go)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);

        if (go.GetComponent<Collider>() != null)
        {
            bounds.Encapsulate(go.GetComponent<Collider>().bounds);
        }
        else
        {
            var childrenColliders = go.GetComponentsInChildren<Collider>();

            foreach (var collider in childrenColliders)
            {
                bounds.Encapsulate(bounds);
            }
        }

        return bounds.size;
    }
}
