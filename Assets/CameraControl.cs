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
    GameObject player;

    [SerializeField]
    float maxX, minX, maxY, minY;

    Vector2 limits = new Vector2(-6.748f, -1.864248f);

    private void Awake()
    {
        maxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;
        minX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        maxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
        minY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
    }
    private void Update()
    {
        if(player != null)
            cam.transform.position = new Vector3(player.transform.position.x, cam.transform.position.y, cam.transform.position.z);
        else
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        cam.transform.position = new Vector3(Mathf.Clamp(transform.position.x, limits.x, limits.y), cam.transform.position.y, cam.transform.position.z);
    }
}

// max -0.9216854
// min -7.7