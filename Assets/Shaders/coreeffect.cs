using UnityEngine;

public class coreeffect : MonoBehaviour
{
    public GameObject laserBeamPrefab; // The laser beam prefab (LineRenderer)
    public Transform[] laserBeamPoints; // Points from where the laser beams will shoot (could be on the reactor core)
    public float beamSpeed = 10f; // Speed of laser beam movement
    public Color overloadColor = Color.red; // Color of the overload glow
    public float overloadIntensity = 10f; // Intensity of the glow

    private Material coreMaterial; // Material of the reactor core
    public bool isOverloading = false; // Check if the reactor is overloaded
    private float overloadTimer = 0f;

    private void Start()
    {
        // Get the material of the reactor core
        Renderer coreRenderer = GetComponent<Renderer>();
        if (coreRenderer != null)
        {
            coreMaterial = coreRenderer.material;
        }

        // If you want to have a starting glow, you can modify the material's emission here
        if (coreMaterial != null)
        {
            coreMaterial.SetColor("_EmissionColor", overloadColor * overloadIntensity);
        }
    }

    private void Update()
    {
        // Check if the reactor is in overload state
        if (isOverloading)
        {
            overloadTimer += Time.deltaTime;

            // Simulate the flickering or pulsing effect of the overload with time
            float pulse = Mathf.Sin(overloadTimer * 10f) * 0.5f + 0.5f; // Creates a smooth pulse effect
            coreMaterial.SetColor("_EmissionColor", overloadColor * overloadIntensity * pulse);

            // Update laser beam positions
            foreach (Transform point in laserBeamPoints)
            {
                CreateLaserBeam(point.position);
            }
        }
    }

    public void StartOverload()
    {
        isOverloading = true;
    }

    public void StopOverload()
    {
        isOverloading = false;
        // Reset the core glow
        if (coreMaterial != null)
        {
            coreMaterial.SetColor("_EmissionColor", Color.black);
        }
    }

    private void CreateLaserBeam(Vector3 startPosition)
    {
        // Instantiate a laser beam at the given start position
        GameObject laserBeam = Instantiate(laserBeamPrefab, startPosition, Quaternion.identity);
        LineRenderer lineRenderer = laserBeam.GetComponent<LineRenderer>();

        if (lineRenderer != null)
        {
            // Set the laser beam parameters (you can adjust this to your liking)
            lineRenderer.startColor = overloadColor;
            lineRenderer.endColor = overloadColor;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.SetPosition(0, startPosition); // Set start point

            // Set a random direction for the laser beams
            Vector3 direction = (transform.position - startPosition).normalized;
            lineRenderer.SetPosition(1, startPosition + direction * beamSpeed); // End point
        }
    }
}
