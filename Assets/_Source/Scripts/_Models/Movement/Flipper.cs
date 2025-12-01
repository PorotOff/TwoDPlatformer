using UnityEngine;

public class Flipper
{
    Transform _transform;

    private Quaternion _rightFace;
    private Quaternion _leftFace;

    public Flipper (Transform transform, bool isInverted = false)
    {
        _transform = transform;

        if (isInverted == false)
        {
            _rightFace = Quaternion.identity;
            _leftFace = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            _rightFace = Quaternion.Euler(0, 180f, 0);
            _leftFace = Quaternion.identity;
        }
    }

    public void LookAt(Vector3 targetPosition)
    {
        if (_transform.position.x < targetPosition.x)
            FlipLeft(_transform);
        else
            FlipRight(_transform);
    }

    public void Flip(Vector2 input)
    {
        if (input.x > 0)
            FlipRight(_transform);
        else if (input.x < 0)
            FlipLeft(_transform);
    }

    private void FlipRight(Transform transform)
        => transform.rotation = _rightFace;

    private void FlipLeft(Transform transform)
        => transform.rotation = _leftFace;
}