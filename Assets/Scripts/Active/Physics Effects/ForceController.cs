using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceController : MonoBehaviour, IBallComponent {

    Rigidbody rb;
    LaunchController lc;
    ConstantForce cf;
    Dictionary<MonoBehaviour, float> cfDict = new Dictionary<MonoBehaviour, float>();
    Dictionary<MonoBehaviour, float> drDict = new Dictionary<MonoBehaviour, float>();


    public void Initialize(Rigidbody _rb, LaunchController _lc) {
        rb = _rb;
        lc = _lc;

        rb.drag = lc.drag;
        rb.angularDrag = lc.angleDrag;
    }

    public void SetDrag (MonoBehaviour _source, float _drag)
    {
        if (!drDict.ContainsKey(_source)) drDict.Add(_source, _drag);
        //{ set { rb.drag = lc.drag + value; } }

        DragPoll();
    }

    public void ReleaseDrag(MonoBehaviour _source)  {
        // { rb.drag = lc.drag; }
        if (drDict.ContainsKey(_source)) drDict.Remove(_source);

        DragPoll();
    }

    public void SetConstantForce(MonoBehaviour _source, float _force) {

        if (!cfDict.ContainsKey(_source)) cfDict.Add(_source, _force);

        ForcePoll();
    }

    public void ReleaseConstantForce(MonoBehaviour _source) {

        if (cfDict.ContainsKey(_source)) cfDict.Remove(_source);

        ForcePoll();
    }

    void Awake() {
        cf = GetComponent<ConstantForce>();
    }

    void ForcePoll() {

        float _total = 0;

        foreach (float value in cfDict.Values) {
            _total += value;
        }

        cf.force = Vector3.up * _total;

    }

    void DragPoll()
    {

        float _total = 0;

        foreach (float value in drDict.Values) {
            _total += value;
        }

        rb.drag = lc.drag + _total;

    }
}

