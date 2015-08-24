using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
    
    private Vector3 moveDirection = Vector3.zero;
    private float rotationY;
    private bool ShouldRun;
    
    public float RotationSpeed=60;
    public float MaxRotation;
    public float Speed = 6.0F;
    public float JumpSpeed = 8.0F;
    public float Gravity = 20.0F;
    private InputManager inputManager;
    
	// Use this for initialization
	void Start () {
        rotationY = 0;
        inputManager = GameObject.FindObjectOfType<InputManager>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (ShouldRun)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                moveDirection = transform.TransformDirection(Vector3.forward);
                moveDirection *= Speed;

                float rotationAmount = inputManager.HorizontalAxis * RotationSpeed * Time.deltaTime;
                rotationY += rotationAmount;
                rotationY = Mathf.Clamp(rotationY, -MaxRotation, MaxRotation);
                transform.rotation = Quaternion.Euler(0, rotationY, 0);
                

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
