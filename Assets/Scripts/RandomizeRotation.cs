using UnityEngine;
using System.Collections;

public class RandomizeRotation : MonoBehaviour {
    public int[] AvailableRotations;

    public GameObject[] ObjectsToKeepPosition;

	void Start () {
        if (AvailableRotations != null && AvailableRotations.Length > 0)
        {
            int rotationIndex = Random.Range(0, AvailableRotations.Length);
            int rotationValue = AvailableRotations[rotationIndex];

            Vector3[] previousRotations = null;

            if (ObjectsToKeepPosition != null && ObjectsToKeepPosition.Length > 0)
            {
                previousRotations = new Vector3[ObjectsToKeepPosition.Length];

                for (int i = 0; i < ObjectsToKeepPosition.Length; i++)
                {
                    previousRotations[i] = ObjectsToKeepPosition[i].transform.position;
                }
            }

            transform.Rotate(new Vector3(0, rotationValue, 0));

            if (ObjectsToKeepPosition != null && ObjectsToKeepPosition.Length > 0 && previousRotations != null)
            {
                for (int i = 0; i < ObjectsToKeepPosition.Length; i++)
                {
                    ObjectsToKeepPosition[i].transform.position = previousRotations[i];
                }
            }
        }
	}
}
