using UnityEngine;
using System.Collections;

public class TargetMovement : MonoBehaviour
{
    public float TrackWidth;
    public float Margin;
    public int NumberOfTracks;
    public int currentTrack;
    private Transform PlayerTransform;
    public float DistanceFromPlayer;

    public float Speed;
    private float targetXBounds;
    private float[] tracksPosition;
    private InputManager inputManager;
    private CharacterController playerController;

    void Start()
    {
        GameObject playerGameObject = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = playerGameObject.transform;
        playerController = playerGameObject.GetComponent<CharacterController>();
        inputManager = GameObject.FindObjectOfType<InputManager>();

        targetXBounds = TrackWidth / 2 - Margin;

        if (NumberOfTracks > 1)
        {
            tracksPosition = new float[NumberOfTracks];

            float widthTrack = 2 * targetXBounds / NumberOfTracks;

            for (int i = 0; i < NumberOfTracks; i++)
            {
                tracksPosition[i] = -targetXBounds + widthTrack * (i + 0.5f);
            }

            currentTrack = tracksPosition.Length / 2;
        }
    }

    void Update()
    {
        float positionX = gameObject.transform.position.x;

        if (playerController.isGrounded)
        {
            if (NumberOfTracks > 1)
            {
                if (inputManager.HorizontalAxis > 0)
                {
                    currentTrack++;
                }
                else if (inputManager.HorizontalAxis < 0)
                {
                    currentTrack--;
                }

                if (currentTrack < 0)
                {
                    currentTrack = 0;
                }

                if (currentTrack > NumberOfTracks - 1)
                {
                    currentTrack = NumberOfTracks - 1;
                }

                positionX = tracksPosition[currentTrack];
            }
            else
            {
                positionX = transform.position.x + inputManager.HorizontalAxis * Speed * Time.deltaTime;
            }
        }
       
        transform.position = new Vector3(Mathf.Clamp(positionX, -targetXBounds, targetXBounds), PlayerTransform.position.y, PlayerTransform.position.z + DistanceFromPlayer);
    }
}
