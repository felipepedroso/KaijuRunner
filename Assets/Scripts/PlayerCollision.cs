using UnityEngine;
using System.Collections;
using System;

public class PlayerCollision : MonoBehaviour {
    public float Strenght;

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag.ToLower())
        {
            case "obstacle":
                ObstacleCollision(hit);

                break;
            case "collectible":
                CollectibleCollision(hit);
                break;
            default:
                break;
        }
    }

    private void CollectibleCollision(ControllerColliderHit hit)
    {
        GameObject collectibleGameObj = hit.gameObject;
        CollectibleBehaviour cb = collectibleGameObj.GetComponent<CollectibleBehaviour>();

        if (cb != null)
        {
            switch (cb.Type)
            {
                case CollectibleBehaviour.CollectibleType.COIN:
                    gameObject.SendMessage("CoinCollected", cb.Value);
                    break;
                case CollectibleBehaviour.CollectibleType.LIFE:
                    gameObject.SendMessage("LifeCollected", cb.Value);
                    break;
                case CollectibleBehaviour.CollectibleType.SPEED:
                    gameObject.SendMessage("SpeedCollected", cb.Value);
                    break;
                default:
                    break;
            }
            Destroy(collectibleGameObj);
        }
    }

    private void ObstacleCollision(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
        body.velocity = pushDir * Strenght;

        hit.gameObject.SendMessage("TakeDamage", 100);

        ObstacleBehaviour ob = hit.gameObject.GetComponent<ObstacleBehaviour>();

        if (ob != null)
        {
            gameObject.SendMessage("TakeDamage", ob.DamageAmount);
        }
    }
}
