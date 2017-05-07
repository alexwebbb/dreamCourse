using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

    public Side up;
    public Side right;
    public Side down;
    public Side left;

    int heightLevel;

}

[System.Serializable]
public struct Side {

    public enum SideShape { Straight, Left, Right }
    public enum SideAngle { Flat = 0, Fifteen = 1, Thirty = 2 }
    public enum SideHeight { Zero = 0, One = 1, Two = 2 }

    public SideShape sideShape;
    public SideAngle sideAngle;
    public SideHeight sideHeight;
}
