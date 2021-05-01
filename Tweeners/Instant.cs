using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instant : MonoBehaviour, IObjectTweener
{
    public void MoveTo(Transform transform, Vector3 targetPosition) {
        transform.position = targetPosition;
    }
}
