using UnityEngine;
using System.Collections;

public class ProjectileDamage : MonoBehaviour {
    public int DamageAmount;

    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Collided on " + collision.gameObject.name);
        GameObject collisionGameObject = collision.gameObject;

        if (collisionGameObject.tag.ToLower() == "obstacle")
        {
            collisionGameObject.SendMessage("TakeDamage", DamageAmount);
            
        }

        Destroy(gameObject);
    }
}
