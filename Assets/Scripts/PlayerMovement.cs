using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private bool ShouldRun;

    private InputManager inputManager;

    public float Speed = 6.0F;
    public float JumpSpeed = 8.0F;
    public float Gravity = 20.0F;
    public Transform PlayerTarget;
    private bool boostSpeed;
    private float speedMultiplier;
    private float boostBegin;
    [Range (1.0f, 10.0f)]
    public float MaximumBoostTime;

    // Use this for initialization
    void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (ShouldRun)
        {
            CharacterController controller = GetComponent<CharacterController>();

            if (boostSpeed)
            {
                if (Time.time - boostBegin >= MaximumBoostTime)
                {
                    boostSpeed = false;
                    gameObject.SendMessage("SetPowerUpInvencibility", false);
                }
            }

            if (controller.isGrounded)
            {
                moveDirection = transform.TransformDirection(Vector3.forward);
                moveDirection *= boostSpeed ? Speed * speedMultiplier : Speed;

                if (PlayerTarget != null)
                {
                    transform.LookAt(PlayerTarget);
                }
            }

            if (inputManager.GetButtonDown("Jump") && controller.isGrounded)
            {
                moveDirection.y = JumpSpeed;
            }

            moveDirection.y -= Gravity * Time.fixedDeltaTime;
            controller.Move(moveDirection * Time.fixedDeltaTime);
        }
    }

    public void SpeedCollected(float value)
    {
        boostSpeed = true;
        speedMultiplier = value;
        boostBegin = Time.time;
        gameObject.SendMessage("SetPowerUpInvencibility", true);
    }

    void Update()
    {
        if (!ShouldRun)
        {
            ShouldRun = inputManager.GetButton("Jump") || inputManager.GetButton("Fire1");
            return;
        }
    }
}
