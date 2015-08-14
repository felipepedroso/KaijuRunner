using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour {

    void OnBecameInvisible() 
    {
        Destroy(transform.parent.gameObject);
    }
}
