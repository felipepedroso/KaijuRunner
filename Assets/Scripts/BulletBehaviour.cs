using UnityEngine;
using System.Collections;

public class BulletBehaviour : MonoBehaviour {
    public float Speed;
    public float Lifetime;

    void Start() {
        gameObject.GetComponent<MeshRenderer>().material.color = new Color(Random.value, Random.value, Random.value);
        Destroy(gameObject, Lifetime);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        gameObject.transform.position += (transform.forward * Speed * Time.fixedDeltaTime);
	}
}
