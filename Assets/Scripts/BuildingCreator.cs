using UnityEngine;
using System.Collections;

public class BuildingCreator : MonoBehaviour {
    public GameObject FloorGameObject;

    void Start()
    {
        StartCoroutine("CreateBuildings");

    }

    IEnumerator CreateBuildings()
    {
        Vector3 floorLocalScale = FloorGameObject.transform.localScale;

        for (int i = 0; i < 2; i++)
        {
            int side = (i % 2 == 0 ? 1 : -1);

            float nextZ = 0;

            while (nextZ < floorLocalScale.z * 10)
            {
                GameObject randomBuilding = GenerateRandomBuilding();
                Vector3 buildingScale = new Vector3(Random.Range(1, 3), Random.Range(5, 10), 1);
                randomBuilding.transform.localScale = buildingScale;

                float xPosition = ((floorLocalScale.x * 5) + (buildingScale.x * 0.5f)) * side;
                float yPosition = buildingScale.y * 0.5f;
                float zPosition = nextZ + transform.parent.transform.position.z - floorLocalScale.z * 5f +(buildingScale.z * 0.5f);

                Vector3 buildingPosition = new Vector3(xPosition, yPosition, zPosition);
                randomBuilding.transform.position = buildingPosition;

                nextZ += (buildingScale.z);
            }
                

                yield return null;
        }
        
    }

    private GameObject GenerateRandomBuilding() 
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube.transform.parent = gameObject.transform;

        cube.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);

        return cube;
    }
}
