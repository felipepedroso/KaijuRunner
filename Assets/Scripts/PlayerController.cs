using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Vector3 moveDirection = Vector3.zero;
    private bool ShouldRun;

    private InputManager inputManager;

    public float Speed = 6.0F;
    public float JumpSpeed = 8.0F;
    public float Gravity = 20.0F;
    public Transform PlayerTarget;
    public float Strenght;

    private float zStart;
    public Text DistanceValueText;

    // Use this for initialization
    void Start()
    {
        inputManager = GameObject.FindObjectOfType<InputManager>();
        zStart = gameObject.transform.position.z;
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

    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
            return;

        if (hit.moveDirection.y < -0.3F)
            return;

        Vector3 pushDir = new Vector3(hit.moveDirection.x, hit.moveDirection.y, hit.moveDirection.z);
        body.velocity = pushDir * Strenght;
    }

    void Update()
    {
        if (!ShouldRun)
        {
            ShouldRun = inputManager.GetButton("Jump") || inputManager.GetButton("Fire1");
            return;
        }

        DistanceValueText.text = string.Format("{0:00000000}", (int)(gameObject.transform.position.z - zStart)/10); 
    }
}
