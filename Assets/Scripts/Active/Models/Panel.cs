using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Panel : MonoBehaviour {

    public Side up;
    public Side right;
    public Side down;
    public Side left;

    public int heightLevel;
    
    public void ApplyRotation(int rotation) {

        while(rotation > 0) {
            this.transform.Rotate(new Vector3(0, 90f, 0));

            Side _left = left;
            Side _up = up;
            Side _right = right;
            Side _down = down;

            up = _left;
            right = _up;
            down = _right;
            left = _down;

            Debug.Log("rotation complete");
            rotation -= 90;
        }
    }

}

[System.Serializable]
public class Side : IEquatable<Side> {

    public enum SideShape { Straight, Left, Right }
    public enum SideAngle { Flat = 0, Fifteen = 1, Thirty = 2 }
    public enum SideHeight { Zero = 0, One = 1, Two = 2 }

    public SideShape sideShape;
    public SideAngle sideAngle;
    public SideHeight sideHeight;

    public bool Equals(Side otherPanel) {

        // need to fix this... left must match right and vice versa
        if(sideShape == otherPanel.sideShape && sideShape == SideShape.Straight
            || sideShape == SideShape.Left && otherPanel.sideShape == SideShape.Right
            || sideShape == SideShape.Right && otherPanel.sideShape == SideShape.Left) {
            if(sideAngle == otherPanel.sideAngle) {
                return true;
            }
        }

        return false;
    }
}
