using UnityEngine;

public class FortData : MonoBehaviour
{

    public FortType fortType;
    //Type is assigned in the prefab in the inspector
    public enum FortType { Cannon, Mortar, Wall, Lighthouse}
    
}
