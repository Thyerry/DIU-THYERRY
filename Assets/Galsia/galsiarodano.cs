using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class galsiarodano : MonoBehaviour
{
    void Update()
    {
        this.transform.rotation.SetEulerRotation(this.transform.rotation.x + .01f, this.transform.rotation.y - .01f, this.transform.rotation.z);
    }
}
