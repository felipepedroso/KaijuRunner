using UnityEngine;
using System.Collections;

public class PlayerTargetMovement : MonoBehaviour
{
    public float TrackWidth;
    public float Margin;
    public float DistanceFromPlayer;

    public float Speed;
    private float targetXBounds;
    private InputManager inputManager;
    private CharacterController playerController;
    private Transform PlayerTransform;
    private PlayerMove playerMovement;

    void Start()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");

        PlayerTransform = playerGameObject.transform;
        playerController = playerGameObject.GetComponent<CharacterController>();
        playerMovement = playerGameObject.GetComponent<PlayerMove>();

        inputManager = GameObject.FindObjectOfType<InputManager>();

        targetXBounds = TrackWidth / 2 - Margin;
    }

    void Update()
    {
        float positionX = gameObject.transform.position.x;

        if (playerMovement.ShouldRun)
        {
            positionX = transform.position.x + inputManager.HorizontalAxis * Speed * Time.deltaTime;
        }

        transform.position = new Vector3(Mathf.Clamp(positionX, -targetXBounds, targetXBounds), PlayerTransform.position.y, PlayerTransform.position.z + DistanceFromPlayer);
    }
}
