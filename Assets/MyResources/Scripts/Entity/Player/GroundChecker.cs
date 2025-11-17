using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public event Action<bool> Grounded;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
            Grounded?.Invoke(true);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
            Grounded?.Invoke(false);
    }
}