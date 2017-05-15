using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantScale : MonoBehaviour {

    float initialSize;

    SizeScaler scaleField;

    public SizeScaler SetReference {
        set {
            if (scaleField == null) {
                initialSize = transform.localScale.z;
                scaleField = value;
            }
        }
    }

    public SizeScaler UnsetReference {
        set {
            if (scaleField = value) scaleField = null;
        }
    }

    private void FixedUpdate() {

        if (scaleField != null) {

            switch(scaleField.type) {
                case ScaleType.Static:
                    if (transform.localScale.z != scaleField.targetSize) transform.localScale = Vector3.one * scaleField.targetSize;
                    break;
                case ScaleType.Linear:
                    float scalar = (transform.position.z - (scaleField.transform.position.z - (0.5f * transform.localScale.z))) / scaleField.transform.localScale.z;
                    scalar = ScaleModifier(scalar);
                    transform.localScale = Vector3.one * scalar;
                    break;
                case ScaleType.Radial:
                    float radialScalar = (transform.position - scaleField.transform.position).sqrMagnitude / scaleField.transform.localScale.sqrMagnitude;
                    radialScalar = ScaleModifier(radialScalar);
                    transform.localScale = Vector3.one * radialScalar;
                    break;
            }
        }
    }


    float ScaleModifier(float _scalar) {
        return initialSize <= scaleField.targetSize ? (_scalar * (scaleField.targetSize - initialSize)) + initialSize : ((1 - _scalar) * (initialSize - scaleField.targetSize)) + scaleField.targetSize;
    }
}
