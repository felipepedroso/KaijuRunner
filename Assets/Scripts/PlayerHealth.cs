using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PlayerHealth : MonoBehaviour {
    public int StartingHealth;
    public int CurrentHealth;

    public bool IsInvencible;

    public Slider HealthSlider;
    public Image DamageOverlay;
    public float DamageFadingSpeed;
    public Color DamageColor = new Color(1f, 0f, 0f, 0.1f);
    private bool receivedDamage;
    private PlayerMovement controller;
    private PlayerMovement playerShooter;
    private bool powerUpInvencible;

    // Use this for initialization
    void Start () {
        CurrentHealth = StartingHealth;
        controller = gameObject.GetComponent<PlayerMovement>();
        playerShooter = gameObject.GetComponent<PlayerMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        if (CurrentHealth <= 0)
        {
            Death();
        }

        if (DamageOverlay != null)
        {
            if (receivedDamage)
            {
                DamageOverlay.color = DamageColor;
            }
            else {
                DamageOverlay.color = Color.Lerp(DamageOverlay.color, Color.clear, DamageFadingSpeed * Time.deltaTime);
            }
        }

        receivedDamage = false;
	}

    public void TakeDamage(int amount)
    {
        receivedDamage = true;
        if (!IsInvencible)
        {
            CurrentHealth -= powerUpInvencible ? 0 : amount;
        }
        

        if (HealthSlider != null)
        {
            HealthSlider.value = (float)CurrentHealth / (float)StartingHealth;
        }
    }

    public void LifeCollected(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, StartingHealth);
    }

    public void SetPowerUpInvencibility(bool enabled)
    {
        powerUpInvencible = enabled;
    }

    private void Death()
    {

        if (controller != null)
        {
            controller.enabled = false;
        }

        if (playerShooter != null)
        {
            playerShooter.enabled = false;
        }

        this.enabled = false;

        gameObject.GetComponent<Animator>().SetTrigger("Die");
    }
}
