using UnityEngine;
using System.Collections;
using System;

public class PlayerMove : MonoBehaviour {
    private InputManager inputManager;
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    private Vector3 reference;

    private CharacterController controller;
    public bool ShouldRun;
    private Animator animator;

    public GameObject Target;

    private bool boostSpeed;
    private float speedMultiplier;
    private float boostBegin;
    [Range(1.0f, 10.0f)]
    public float MaximumBoostTime;

    void Start () {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        controller = GetComponent<CharacterController>();
        animator = gameObject.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!ShouldRun)
        {
            ShouldRun = inputManager.GetButtonUp("Jump");
            animator.SetBool("ShouldRun", ShouldRun);
            return;
        }

        CheckBoostSpeed();

        if (controller.isGrounded)
        {
            animator.SetBool("IsJumping", false);
            moveDirection = transform.TransformDirection(Vector3.forward);
            moveDirection *= boostSpeed ? speed * speedMultiplier : speed; ;

            if (Target != null)
            {
                gameObject.transform.LookAt(Target.transform);
            }

            if (Input.GetButtonUp("Jump"))
            {
                moveDirection.y = jumpSpeed;
                animator.SetBool("IsJumping", true);
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        if (Target != null)
        {
            PlayerTargetMovement targetMovement = Target.GetComponent<PlayerTargetMovement>();
            Vector3 playerPosition = gameObject.transform.position;
            gameObject.transform.position = new Vector3(Mathf.Clamp(playerPosition.x, -targetMovement.targetXBounds,targetMovement.targetXBounds), playerPosition.y, playerPosition.z);
        }
    }

    private void CheckBoostSpeed()
    {
        if (boostSpeed)
        {
            if (Time.time - boostBegin >= MaximumBoostTime)
            {
                boostSpeed = false;
                gameObject.SendMessage("SetPowerUpInvencibility", false);
            }
        }
    }

    public void SpeedCollected(float value)
    {
        boostSpeed = true;
        speedMultiplier = value;
        boostBegin = Time.time;
        gameObject.SendMessage("SetPowerUpInvencibility", true);
    }
}
