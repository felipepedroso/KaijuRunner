using UnityEngine;
using System.Collections;

public class ObstacleHealth : MonoBehaviour {
    public int StartingHealth;
    public int CurrentHealth;

    public GameObject ExplosionPrefab;
    public float ExplosionRadius;
    public float ExplosionPower;

    void Start()
    {
        CurrentHealth = StartingHealth;
    }


    public void TakeDamage(int amount)
    {
        CurrentHealth -= amount;

        if (CurrentHealth <= 0)
        {
            if (ExplosionPrefab != null)
            {
                GameObject go = (GameObject)Instantiate(ExplosionPrefab, gameObject.transform.position, gameObject.transform.rotation);
                go.transform.parent = gameObject.transform;

                var explosionPos = gameObject.transform.position;

                float damageMultiplier = amount / StartingHealth > 0.8 ? 2.0f : 1.0f;
                float explosionRadius = ExplosionRadius * damageMultiplier;
                float explosionPower = ExplosionPower * damageMultiplier;

                var colliders = Physics.OverlapSphere(explosionPos, explosionRadius);

                foreach (var hit in colliders)
                {
                    if (hit == null)
                    {
                        continue;
                    }

                    Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
                    if (rigidbody != null)
                    {

                        rigidbody.AddExplosionForce(explosionPower, explosionPos, explosionRadius, 3.0f);
                    }
                }
            }
            gameObject.layer = 13;
            Destroy(gameObject, 1.0f);
        }
    }

}
