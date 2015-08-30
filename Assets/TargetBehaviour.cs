using UnityEngine;
using System.Collections;

public class TargetBehaviour : MonoBehaviour
{
    public float TrackWidth;
    public float Margin;
    public int NumberOfTracks;
    public int currentTrack;
    public Transform Player;
    public float DistanceFromPlayer;

    public float Speed;
    private float targetXBounds;
    private float[] tracksPosition;
    private InputManager inputManager;

    void Start()
    {
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
        float positionX = 0.0f;

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

        transform.position = new Vector3(Mathf.Clamp(positionX, -targetXBounds, targetXBounds), Player.position.y, Player.position.z + DistanceFromPlayer);
    }
}
