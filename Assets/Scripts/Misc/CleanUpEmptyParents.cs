using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanUpEmptyParents : MonoBehaviour
{
    void Update()
    {
        if (transform.childCount == 0)
        {
            Destroy(gameObject);
        }
    }
}
