using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float scrollSpeed = 0.1f;

    private Renderer rend;
    private Vector2 offset;

    void Start()
    {
        rend = GetComponent<Renderer>();

        if (rend == null || rend.material == null)
        {
            Debug.LogWarning("BackgroundScroller: Renderer ya da Material eksik.");
            enabled = false;
            return;
        }

        offset = rend.material.mainTextureOffset;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;
        rend.material.mainTextureOffset = offset;
    }
}