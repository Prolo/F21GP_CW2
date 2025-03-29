using UnityEngine;

public class LightDetector : MonoBehaviour
{
    public LayerMask occlusionMask;
    private Light[] sceneLights;
    public float illuminationLevel { get; private set; }

    [Header("Visual Indicator")]
    [SerializeField] private GameObject illuminationSpherePrefab;
    private GameObject illuminationSphereInstance;
    private Material sphereMaterial;

    private float previousIlluminationLevel = -1f;

    void Start()
    {
        if (illuminationSpherePrefab != null)
        {
            illuminationSphereInstance = Instantiate(illuminationSpherePrefab, transform.position + Vector3.up * 1f, Quaternion.identity, transform);
            sphereMaterial = illuminationSphereInstance.GetComponent<Renderer>().material;
        }
    }

    void Update()
    {
        sceneLights = FindObjectsByType<Light>(FindObjectsSortMode.None) ?? new Light[0];

        float newIllumination = CalculateIllumination();

        if (!Mathf.Approximately(newIllumination, previousIlluminationLevel))
        {
            Debug.Log("Enemy Illumination Level: " + newIllumination);
            previousIlluminationLevel = newIllumination;
        }

        illuminationLevel = newIllumination;

        UpdateSphereColor();
    }

    void UpdateSphereColor()
    {
        if (sphereMaterial != null)
        {
            sphereMaterial.color = Color.Lerp(Color.black, Color.yellow, illuminationLevel);
        }
    }

    float CalculateIllumination()
    {
        if (sceneLights == null) return 0f;
        Vector3 samplePoint = transform.position + Vector3.up * 1.0f;
        float totalBrightness = 0f;

        foreach (Light light in sceneLights)
        {
            if (!light.isActiveAndEnabled) continue;

            bool lit = false;
            float brightness = 0f;

            if (light.type == LightType.Point || light.type == LightType.Spot)
            {
                Vector3 toLight = light.transform.position - samplePoint;
                float distSq = toLight.sqrMagnitude;
                float distance = Mathf.Sqrt(distSq);
                if (distance > light.range) continue;

                if (!Physics.Raycast(samplePoint, toLight.normalized, distance, occlusionMask))
                {
                    lit = true;
                    brightness = light.intensity / distSq;
                }

                if (light.type == LightType.Spot)
                {
                    float angleToLight = Vector3.Angle(light.transform.forward, -toLight);
                    if (angleToLight > light.spotAngle * 0.5f)
                    {
                        continue;
                    }
                }
            }

            if (lit)
            {
                totalBrightness += brightness;
            }
        }
        return Mathf.Clamp01(totalBrightness);
    }
}
