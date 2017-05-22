using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour, IBallComponent {

    Rigidbody rb;
    LaunchController lc;
    ConstantForce cf;
    Dictionary<MonoBehaviour, float> cfDict = new Dictionary<MonoBehaviour, float>();

    public void Initialize(Rigidbody _rb, LaunchController _lc) {
        rb = _rb;
        lc = _lc;
    }

    public float AddDrag { set { rb.drag = lc.drag + value; } }
    public void ResetDrag() { rb.drag = lc.drag; }

    public void SetConstantForce(MonoBehaviour _source, float _force) {

        if (!cfDict.ContainsKey(_source)) cfDict.Add(_source, _force);

        Poll();
    }

    public void ReleaseConstantForce(MonoBehaviour _source) {

        if (cfDict.ContainsKey(_source)) cfDict.Remove(_source);

        Poll();
    }

    void Awake() {
        cf = GetComponent<ConstantForce>();
    }

    void Poll() {

        float _total = 0;

        foreach (float value in cfDict.Values) {
            _total += value;
        }

        cf.force = Vector3.up * _total;

    }
}

