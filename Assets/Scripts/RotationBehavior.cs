using UnityEngine;
using System.Collections;

public class RotationBehavior : MonoBehaviour {
    public Vector3 RotationSpeed;

	void FixedUpdate () {
        transform.Rotate(RotationSpeed);
	}
}
