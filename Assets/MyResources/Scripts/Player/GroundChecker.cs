using System;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    private int _groundEntriesCount = 0;

    public bool IsGrounded => _groundEntriesCount > 0;
    
    public event Action<bool> Grounded;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            _groundEntriesCount++;

            if (_groundEntriesCount == 1)
                Grounded?.Invoke(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out _))
        {
            _groundEntriesCount--;

            if (_groundEntriesCount == 0)
                Grounded?.Invoke(false);
        }
    }
}