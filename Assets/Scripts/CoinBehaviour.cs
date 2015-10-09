using UnityEngine;
using System.Collections;

public class CoinBehaviour : MonoBehaviour {
    public float RotationSpeed;

	void FixedUpdate () {
        transform.Rotate(RotationSpeed, 0, 0);
	}
}
