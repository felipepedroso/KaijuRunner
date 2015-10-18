using UnityEngine;
using System.Collections;

public class CollectibleBehaviour : MonoBehaviour {
    public enum CollectibleType { COIN, LIFE, SPEED }

    public CollectibleType Type;
    public float Value;

}
