using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private bool ShouldRun;

    private InputManager inputManager;

    public float Speed = 6.0F;
    public float JumpSpeed = 8.0F;
    public float Gravity = 20.0F;
    public Transform PlayerTarget;

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
            if (controller.isGrounded)
            {
                moveDirection = transform.TransformDirection(Vector3.forward);
                moveDirection *= Speed;

                if (PlayerTarget != null)
                {
                    transform.LookAt(PlayerTarget);
                }

                if (inputManager.GetButtonDown("Jump"))
                    moveDirection.y = JumpSpeed;

            }
            moveDirection.y -= Gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }
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
