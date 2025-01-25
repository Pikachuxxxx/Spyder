using UnityEngine;

public class Pit : MonoBehaviour
{
    [Header("Color change mats")]
    public Material defaultMaterial;
    public Material collisionMaterial;

    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
    }

    void OnTriggerEnter(Collider collision)
    {
                    objectRenderer.material = collisionMaterial;
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player"))
        {
            objectRenderer.material = collisionMaterial;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (defaultMaterial != null)
        {
            Invoke("RevertMaterial", 0.1f);
        }
    }

    private void RevertMaterial()
    {
        objectRenderer.material = defaultMaterial;
    }
}
