using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{ 
    public static void ResetLocal(this Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
