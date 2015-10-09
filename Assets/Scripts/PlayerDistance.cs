using UnityEngine;
using UnityEngine.UI;

public class PlayerDistance : MonoBehaviour {
    private float zStart;
    public Text DistanceValueText;
    public float Multiplier;

    public float CurrentDistance
    {
        get
        {
            return (gameObject.transform.position.z - zStart) * (Multiplier > 0 ? Multiplier : 1);
        }
    }

    // Use this for initialization
    void Start () {
        zStart = gameObject.transform.position.z;
    }
	
	// Update is called once per frame
	void Update () {
        if (DistanceValueText != null)
        {
            DistanceValueText.text = string.Format("{0:00000000}", (int)(CurrentDistance));
        }
    }
}
