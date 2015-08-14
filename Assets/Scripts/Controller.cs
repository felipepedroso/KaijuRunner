using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {
    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;

    public bool ShouldRun;
    public float RotationSpeed=60;
    public float MaxRotation;
    private float rotationY;

	// Use this for initialization
	void Start () {
        rotationY = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (ShouldRun)
        {
            CharacterController controller = GetComponent<CharacterController>();
            if (controller.isGrounded)
            {
                moveDirection = Vector3.forward * Time.fixedDeltaTime;
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection *= speed;


                float rotationAmount = Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime;
                rotationY += rotationAmount;

                rotationY = Mathf.Clamp(rotationY, -MaxRotation, MaxRotation);

                transform.rotation = Quaternion.Euler(0, rotationY, 0);
                

                if (Input.GetButton("Jump"))
                    moveDirection.y = jumpSpeed;

            }
            moveDirection.y -= gravity * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
        }

        if (Input.GetKey("s"))
        {
            ShouldRun = true;
        }
	}
}
