using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {
    public Transform ShootingPoint;
    public GameObject SmallShootPrefab;
    public GameObject BigShootPrefab;
    public float ShootingCooldown;
    private float timePressedShoot;
    private float shootingTimer;
    public float ChargeTime;
    private float lastShootTime;
    public bool HasRequestedPressed;
    public bool HasRequestedReleased;


	// Use this for initialization
	void Start () {
        timePressedShoot = 0;
        lastShootTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (ShootPressed())
        {
            timePressedShoot = Time.time;
            Debug.Log("Pressed shoot! " + timePressedShoot);
            HasRequestedPressed = false;
        }

        if (ShootReleased())
        {
            
            if (Time.time - timePressedShoot >= ChargeTime)
            {
                BigShoot();
                Debug.Log("Released Big Shoot!");
            }
            else
            {
                Debug.Log("Released Shoot!");
                Shoot();
            }
            timePressedShoot = 0;
            HasRequestedReleased = false;
        }
	}

    private void BigShoot()
    {
        if (BigShootPrefab != null)
        {
            Instantiate(BigShootPrefab, ShootingPoint.position, gameObject.transform.rotation);
        }
    }

    public bool ShootPressed() 
    {
        return Input.GetButtonDown("Fire1") || HasRequestedPressed;
    }

    public bool ShootReleased()
    {
        return Input.GetButtonUp("Fire1") || HasRequestedReleased;
    }

    public void RequestPressed() 
    {
        HasRequestedPressed = true;
    }

    public void RequestRelease()
    {
        HasRequestedReleased = true;
    }

    public void Shoot()
    {
        if (SmallShootPrefab != null)
        {
            float timeNow = Time.time;

            if (timeNow - lastShootTime >= ShootingCooldown)
            {
                Instantiate(SmallShootPrefab, ShootingPoint.position, gameObject.transform.rotation);
                lastShootTime = timeNow;
            }
        }
    }
}
