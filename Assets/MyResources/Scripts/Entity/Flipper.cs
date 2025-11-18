using UnityEngine;

public class Flipper
{
    private Quaternion _rightFace = Quaternion.identity;
    private Quaternion _leftFace = Quaternion.Euler(0, 180f, 0);

    public void Flip(Transform currentTransform, Vector3 targetPosition)
    {
        if (currentTransform.position.x < targetPosition.x)
            FlipLeft(currentTransform);
        else
            FlipRight(currentTransform);
    }

    public void Flip(Transform currentTransform, Vector2 input)
    {
        if (input.x > 0)
            FlipRight(currentTransform);
        else if (input.x < 0)
            FlipLeft(currentTransform);
    }

    public void FlipRight(Transform transform)
        => transform.rotation = _rightFace;

    public void FlipLeft(Transform transform)
        => transform.rotation = _leftFace;
}