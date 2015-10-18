using UnityEngine;
using System.Collections;

public class ElementsCreator : MonoBehaviour {
    public GameObject PlatformGameObject;
    public int Rows;
    public int Columns;
    [Range (0, 3)]
    public int RowsToSkip;

    public GameObject[] ObstaclesPrefabs;
    public GameObject[] CollectiblesPrefabs;
    public bool DebugPositions;

    public void Update()
    {
        if (DebugPositions)
        {
            Vector3 platformPosition = gameObject.transform.parent.position;
            Vector3 platformDimensions = PlatformGameObject.GetComponent<MeshCollider>().bounds.size;

            float minZ = platformPosition.z - platformDimensions.z / 2;
            float minX = platformPosition.x - platformDimensions.x * 0.125f;

            float maxZ = platformPosition.z + platformDimensions.z / 2;
            float maxX = platformPosition.x + platformDimensions.x * 0.125f;

            float zOffset = platformDimensions.z / (Rows + 1);
            float xOffset = (platformDimensions.x * 0.25f) / (Columns + 1);

            float heigth = 1.0f;

            for (int i = 0; i < Rows; i++)
            {
                Vector3 p1 = new Vector3(minX, heigth, minZ + (i + 1) * zOffset);
                Vector3 p2 = new Vector3(maxX, heigth, minZ + (i + 1) * zOffset);

                Debug.DrawLine(p1, p2);
            }

            for (int j = 0; j < Columns; j++)
            {
                Vector3 p1 = new Vector3(minX + (j + 1) * xOffset, heigth, minZ);
                Vector3 p2 = new Vector3(minX + (j + 1) * xOffset, heigth, maxZ);
                Debug.DrawLine(p1, p2);
            }
        }
    }

	// Use this for initialization
	void Start () {
        ObstaclesPrefabs = Resources.LoadAll<GameObject>("Obstacles");
        CollectiblesPrefabs = Resources.LoadAll<GameObject>("Items");

        if (ObstaclesPrefabs == null || ObstaclesPrefabs.Length < 1 || CollectiblesPrefabs == null || CollectiblesPrefabs.Length < 1 || PlatformGameObject == null)
        {
            Debug.LogError("Please check if the prefabs or the Platform GameObject were set.");
            return;
        }

        Vector3 platformPosition = gameObject.transform.parent.position;
        Vector3 platformDimensions = PlatformGameObject.GetComponent<MeshCollider>().bounds.size;

        float minZ = platformPosition.z - platformDimensions.z/2;
        float minX = platformPosition.x - platformDimensions.x * 0.125f;

        float zOffset = platformDimensions.z / (Rows + 1);
        float xOffset = (platformDimensions.x * 0.25f) / (Columns + 1);

        for (int i = 0; i < Rows; i++)
        {
            if (i < RowsToSkip)
            {
                continue;
            }

            for (int j = 0; j < Columns; j++)
            {
                int randomNumber = Random.Range(0, 100);
                GameObject[] gameObjectSource = null;

                if (randomNumber < 25)
                {
                    continue;
                }
                else if (randomNumber < 60)
                {
                    gameObjectSource = CollectiblesPrefabs;
                }
                else
                {
                    gameObjectSource = ObstaclesPrefabs;
                }

                if (gameObjectSource != null && gameObjectSource.Length > 0)
                {
                    GameObject obstaclePrefab = gameObjectSource[Random.Range(0, gameObjectSource.Length)];

                    GameObject go = (GameObject)Instantiate(obstaclePrefab, new Vector3(minX + ((j + 1) * xOffset), 1.5f, minZ + ((i + 1) * zOffset)), obstaclePrefab.transform.rotation);
                    go.transform.parent = gameObject.transform;
                }
            }
        }
	}
}
