using UnityEngine;
using System.Collections;

public class RandomBuild : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Vector3 scale = new Vector3(1, Random.Range(5, 10), 1);
        gameObject.transform.localScale = scale;

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
	}
}
