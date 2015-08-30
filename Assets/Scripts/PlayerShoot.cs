using UnityEngine;
using System.Collections;

public class PlayerShoot : MonoBehaviour {
    private float timePressedShoot;
    private float shootingTimer;
    private float lastShootTime;
    private InputManager inputManager;

    public float ChargeTime;
    public Transform ShootingPoint;
    public GameObject SmallShootPrefab;
    public GameObject BigShootPrefab;
    public float ShootingCooldown;

	// Use this for initialization
	void Start () {
        timePressedShoot = 0;
        lastShootTime = 0;

        inputManager = GameObject.FindObjectOfType<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (inputManager.GetButtonDown("Fire1"))
        {
            timePressedShoot = Time.time;
            //Debug.Log("Pressed shoot! " + timePressedShoot);
        }

        if (inputManager.GetButtonUp("Fire1"))
        {
            if (Time.time - timePressedShoot >= ChargeTime)
            {
                BigShoot();
                //Debug.Log("Released Big Shoot!");
            }
            else
            {
                //Debug.Log("Released Shoot!");
                Shoot();
            }
            timePressedShoot = 0;
        }
	}

    private void BigShoot()
    {
        if (BigShootPrefab != null)
        {
            GameObject bulletGameObject = (GameObject)Instantiate(BigShootPrefab, ShootingPoint.position, gameObject.transform.rotation);
            SetInertiaSpeed(bulletGameObject);
        }
    }

    private void SetInertiaSpeed(GameObject bulletGameObject)
    {
        PlayerController playerController = gameObject.GetComponent<PlayerController>();
        BulletBehaviour bulletBehaviour = bulletGameObject.GetComponent<BulletBehaviour>();

        if (playerController != null && bulletBehaviour != null)
        {
            bulletBehaviour.Speed += playerController.Speed;
        }
    }

    public void Shoot()
    {
        if (SmallShootPrefab != null)
        {
            float timeNow = Time.time;

            if (timeNow - lastShootTime >= ShootingCooldown)
            {
                GameObject bulletGameObject = (GameObject)Instantiate(SmallShootPrefab, ShootingPoint.position, gameObject.transform.rotation);
                SetInertiaSpeed(bulletGameObject);
                lastShootTime = timeNow;
            }
        }
    }
}
