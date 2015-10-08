using UnityEngine;
using System.Collections;

public class FloorScript : MonoBehaviour {

    void OnBecameInvisible() 
    {
        if (transform.parent != null)
        {
            Destroy(transform.parent.gameObject);
        }

        Destroy(gameObject);
    }
}
