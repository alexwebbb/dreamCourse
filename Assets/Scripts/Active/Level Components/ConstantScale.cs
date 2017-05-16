using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantScale : MonoBehaviour {


    // need to test negative scaling

    // need to create reset for returning to origin.
    // ie it should be a value that records the scale at time of launch
    // or possibly it resets on disable
    // or it can subscribe to the turn over event
    // do I need to implement velocity scaling?
    // maybe have velocity scaling
    // maybe launch controller needs to reference scale here
    // need to clamp the scalar on the radial scaler 



    // these are public so they can be copied
    public Dictionary<SizeScaler, float> initialSize = new Dictionary<SizeScaler, float>();
    public List<SizeScaler> scaleField = new List<SizeScaler>();


    public SizeScaler SetReference {
        set {
            if (!scaleField.Contains(value)) {
                initialSize.Add(value, transform.localScale.z);
                scaleField.Insert(0, value);
            }
        }
    }

    public SizeScaler UnsetReference {
        set {
            scaleField.Remove(value);
            initialSize.Remove(value);
        }
    }

    // this method is used to copy the stats of the player object when instantiating tracer objects
    public void CopyScale(ConstantScale other) {

        initialSize = other.initialSize;
        scaleField = other.scaleField;

    }

    private void FixedUpdate() {

        if (scaleField.Count != 0) {

            switch(scaleField[0].type) {
                case ScaleType.Static:
                    if (transform.localScale.z != scaleField[0].targetSize) transform.localScale = Vector3.one * scaleField[0].targetSize;
                    break;
                case ScaleType.Linear:
                    float scalar = (transform.position.z - (scaleField[0].transform.position.z - (0.5f * scaleField[0].transform.localScale.z))) / scaleField[0].transform.localScale.z;
                    scalar = ScaleModifier(scalar);
                    transform.localScale = Vector3.one * scalar;
                    break;
                case ScaleType.Radial:
                    float radialScalar =(transform.position - scaleField[0].transform.position).sqrMagnitude / (scaleField[0].transform.localScale * 0.25f).sqrMagnitude;
                    radialScalar = 1 - radialScalar;
                    radialScalar = ScaleModifier(radialScalar);
                    transform.localScale = Vector3.one * radialScalar;
                    break;
            }
        }
    }


    float ScaleModifier(float _scalar) {
        float _initial = initialSize[scaleField[0]];
        float _target = scaleField[0].targetSize;
        return _initial <= _target ? (_scalar * (_target - _initial)) + _initial : ((1 - _scalar) * (_initial - _target)) + _target;
    }
}
