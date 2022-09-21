using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreable : MonoBehaviour
{
    public ScoreableType type;

    public enum ScoreableType
    {
        None,
        Sphere,
        Capsule,
    }
}
