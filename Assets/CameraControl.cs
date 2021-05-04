using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    SpriteRenderer mapRenderer;

    [SerializeField]
    float maxX, minX, maxY, minY;

    private void Awake()
    {
        maxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        minX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        maxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
        minY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
    }
}
