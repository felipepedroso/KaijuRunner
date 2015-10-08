using UnityEngine;
using System.Collections;

public class ObstaclesCreator : MonoBehaviour {
    public GameObject[] ObstaclesPrefabs;
    public GameObject PlatformGameObject;

    public int MaxObstacles;

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

        int obstaclesCount = Random.Range(1, MaxObstacles);

        float diffBetweenObstacles = platformDimensions.z / (obstaclesCount + 1);

        for (int i = 0; i < obstaclesCount; i++)
        {
            GameObject obstaclePrefab = ObstaclesPrefabs[Random.Range(0, ObstaclesPrefabs.Length)];
       

            GameObject go = (GameObject)Instantiate(obstaclePrefab, new Vector3(0.0f, 1.5f, minZ + ((i+1) * diffBetweenObstacles)), Quaternion.identity);
            go.transform.parent = gameObject.transform;

            float obstacleWidth = GetObstacleDimensions(go).x;
            float x = Random.Range(minX + obstacleWidth / 2, maxX - obstacleWidth/2);

            go.transform.position = new Vector3(x, go.transform.position.y, go.transform.position.z);
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
