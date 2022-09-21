using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTriggerer : MonoBehaviour
{
    [SerializeField] private ScoreManager _scoreManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Scoreable scoreable))
        {
            _scoreManager.ChangeScore(scoreable.type);
            Destroy(other.transform.parent.gameObject);
        }
    }
}
