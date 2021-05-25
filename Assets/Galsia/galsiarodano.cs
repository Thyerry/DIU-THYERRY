using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class galsiarodano : MonoBehaviour
{
    private float rotation;
    public float RotationSpeed;


    void Update()
    {
        rotation += Time.deltaTime * RotationSpeed;

        transform.rotation = Quaternion.Euler(0, rotation, 0);
    }
}
