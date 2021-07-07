using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicRuneRock : MonoBehaviour
{
    public MeshRenderer _meshRenderer;
    public Material _material;
    public float _timeSum = 0;
    Vector3 _posStart;

    private void Awake()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponentInChildren<MeshRenderer>();

        _material = _meshRenderer.material;
        _posStart = transform.position;
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x, 1.3f + 0.2f * Mathf.Sin(_timeSum * 1.2f), transform.position.z);

        byte c_ = (byte)(175 + 80 * Mathf.Sin(_timeSum * 1.2f)).ToInt();
        _material.SetColor("_EmissionColor", new Color32(c_, c_, c_, 255));

        _timeSum += Globals.timeDeltaFixed * 1f;
    }
}
