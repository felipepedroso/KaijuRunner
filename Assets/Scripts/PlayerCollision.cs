using UnityEngine;
using System.Collections;

public class PlayerCollision : MonoBehaviour {
    public float Strenght;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
        body.velocity = pushDir * Strenght;

        if (hit.gameObject.tag.ToLower().Equals("obstacle"))
        {
            ObstacleBehaviour ob = hit.gameObject.GetComponent<ObstacleBehaviour>();

            if (ob != null)
            {
                gameObject.SendMessage("TakeDamage", ob.DamageAmount);
            }
        }
    }
}
