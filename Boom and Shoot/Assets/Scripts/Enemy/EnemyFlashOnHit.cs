using UnityEngine;

public class EnemyFlashOnHit : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;
    private Material targetMaterial;
    private float flashTimer = 0f;
    private bool isFlashing = false;

    private void Start()
    {
        if (targetRenderer == null)
        {
            targetRenderer = GetComponentInChildren<Renderer>();
        }

        if (targetRenderer != null)
        {
            targetMaterial = targetRenderer.material;
            originalColor = targetMaterial.color;
        }
        else
        {
            Debug.LogWarning("EnemyFlashOnHit: No renderer found to flash.");
        }
    }

    private void Update()
    {
        if (isFlashing)
        {
            flashTimer -= Time.deltaTime;

            if (flashTimer <= 0f)
            {
                if (targetMaterial != null)
                {
                    targetMaterial.color = originalColor;
                }
                isFlashing = false;
            }
        }
    }

    public void Flash()
    {
        if (targetMaterial != null && gameObject.activeInHierarchy)
        {
            targetMaterial.color = flashColor;
            flashTimer = flashDuration;
            isFlashing = true;
        }
    }
}
