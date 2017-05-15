using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantScale : MonoBehaviour {

    Dictionary<SizeScaler, float> initialSize = new Dictionary<SizeScaler, float>();
    List<SizeScaler> scaleField = new List<SizeScaler>();

    public SizeScaler SetReference {
        set {
            scaleField.Add(value);
            initialSize.Add(value, transform.localScale.z);
        }
    }

    public SizeScaler UnsetReference {
        set {
            initialSize.Remove(value);
            scaleField.Remove(value);
        }
    }

    private void FixedUpdate() {

        if (scaleField[0] != null) {

            switch(scaleField[0].type) {
                case ScaleType.Static:
                    if (transform.localScale.z != scaleField[0].targetSize) transform.localScale = Vector3.one * scaleField[0].targetSize;
                    break;
                case ScaleType.Linear:
                    float scalar = (transform.position.z - (scaleField[0].transform.position.z - (0.5f * transform.localScale.z))) / scaleField[0].transform.localScale.z;
                    scalar = ScaleModifier(scalar);
                    transform.localScale = Vector3.one * scalar;
                    break;
                case ScaleType.Radial:
                    float radialScalar = (transform.position - scaleField[0].transform.position).sqrMagnitude / scaleField[0].transform.localScale.sqrMagnitude;
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
