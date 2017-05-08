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

        if(rotation == 90) {

            Side _left = left;
            Side _up = up;
            Side _right = right;
            Side _down = down;

            up = _left;
            right = _up;
            down = _right;
            left = _down;

            this.transform.Rotate(new Vector3(0, 90f, 0));
            // rotation -= 90;

        } else if (rotation == 180) {

            Side _left = left;
            Side _up = up;
            Side _right = right;
            Side _down = down;

            right = _left;
            down = _up;
            left = _right;
            up = _down;

            this.transform.Rotate(new Vector3(0, 180f, 0));

        } else if (rotation == 270) {

            Side _left = left;
            Side _up = up;
            Side _right = right;
            Side _down = down;

            down = _left;
            left = _up;
            up = _right;
            right = _down;

            this.transform.Rotate(new Vector3(0, 270f, 0));
        }

    }

}

[System.Serializable]
public struct Side : IEquatable<Side> {

    public enum SideShape { Straight, Left, Right }
    public enum SideAngle { Flat = 0, Fifteen = 1, Thirty = 2 }
    // public enum SideHeight { Zero = 0, One = 1, Two = 2 }

    public SideShape sideShape;
    public SideAngle sideAngle;
    public int sideHeight;

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
