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

    BounceController bc;

    private void Start() {
        bc = GetComponent<BounceController>();
    }


    private void FixedUpdate() {

        if (scaleField.Count != 0) {

            switch(scaleField[0].type) {
                case ScaleType.Static:
                    if (transform.localScale.z != scaleField[0].targetSize) transform.localScale = Vector3.one * scaleField[0].targetSize;
                    break;
                case ScaleType.Linear:
                    float scalar = (transform.position.z - (scaleField[0].transform.position.z - (0.5f * scaleField[0].transform.localScale.z))) / scaleField[0].transform.localScale.z;
                    ScaleModifier(scalar);
                    break;
                case ScaleType.Radial:
                    float radialScalar = 1 - ((transform.position - scaleField[0].transform.position).sqrMagnitude / (scaleField[0].transform.localScale * 0.25f).sqrMagnitude);
                    ScaleModifier(radialScalar);
                    break;
            }
        }
    }


    void ScaleModifier(float _scalar) {
        // ensure that scalar is indeed a scalar (between zero and one).
        if (_scalar > 1 || _scalar < 0) {

            Mathf.Clamp(_scalar, 0, 1);
        }

        float _initial = initialSize[scaleField[0]];
        float _target = scaleField[0].targetSize;

        // damn this is one dirty fix but it seems to work!
        // bc.ScaleConstantForce = Vector3.up * 9 * _scalar;

        // use left function if target scale is greater than  initial scale, right if vice versa
        _scalar = _initial <= _target ? (_scalar * (_target - _initial)) + _initial : ((1 - _scalar) * (_initial - _target)) + _target;

        transform.localScale = Vector3.one * _scalar;

    }
}
