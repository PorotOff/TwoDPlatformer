using UnityEngine;

public class SimpleFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;

    private void OnValidate()
    {
        if (_target != null)
            transform.position = new Vector3(_target.position.x + _xOffset, _target.position.y + _yOffset, transform.position.z);
    }

    private void LateUpdate()
        => transform.position = new Vector3(_target.position.x + _xOffset, _target.position.y + _yOffset, transform.position.z);
}