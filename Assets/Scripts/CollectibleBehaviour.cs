using UnityEngine;
using System.Collections;

public class CollectibleBehaviour : MonoBehaviour {
    public enum CollectibleType { COIN, LIFE, POWERUP }

    public CollectibleType Type;
    public int Value;
}
