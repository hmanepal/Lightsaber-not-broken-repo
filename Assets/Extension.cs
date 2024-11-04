using UnityEngine;

public class LightsaberScript : MonoBehaviour
{
    // Instance Variables
    public float extendSpeed = 0.1f;  // Extend/collapse speed
    private bool weaponActive = true; // State: lightsaber on/off
    private float scaleMin = 0f;      // Minimum scale value
    private float scaleMax;           // Maximum scale value (initial y scale)
    private float extendDelta;        // Interpolation value for scaling
    private float scaleCurrent;       // Current y scale value of the blade
    private float localScaleX;        // Original x scale value
    private float localScaleZ;        // Original z scale value
    public GameObject blade;          // Reference to the blade GameObject

    // Audio
    public AudioClip toggleSound;     // Sound for extending/retracting
    private AudioSource audioSource;  // Audio source component

    void Start()
    {
        // Save the initial local x and z scale values
        localScaleX = transform.localScale.x;
        localScaleZ = transform.localScale.z;

        // Set the maximum scaling value to the initial y scale
        scaleMax = transform.localScale.y;

        // Assume the lightsaber starts maximally extended and active
        scaleCurrent = scaleMax;
        extendDelta = scaleMax / extendSpeed;

        // Set initial state to on
        weaponActive = true;

        // Initialize audio source
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Check for spacebar press to toggle the lightsaber state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Invert the interpolation value based on the current state
            extendDelta = weaponActive ? -Mathf.Abs(extendDelta) : Mathf.Abs(extendDelta);

            // Play the toggle sound only when spacebar is pressed
            if (toggleSound != null)
            {
                audioSource.PlayOneShot(toggleSound);
            }
            else
            {
                Debug.LogWarning("Toggle sound is not assigned!");
            }
        }

        // Adjust the current scale based on the interpolation value and time
        scaleCurrent += extendDelta * Time.deltaTime;

        // Clamp the scale to ensure it remains within bounds
        scaleCurrent = Mathf.Clamp(scaleCurrent, scaleMin, scaleMax);

        // Apply the scale to the lightsaber's local y-axis
        transform.localScale = new Vector3(localScaleX, scaleCurrent, localScaleZ);

        // Update the weapon state based on the current scale
        weaponActive = scaleCurrent > 0;

        // Activate/Deactivate the blade GameObject based on the lightsaber state
        if (weaponActive && !blade.activeSelf)
        {
            blade.SetActive(true);
        }
        else if (!weaponActive && blade.activeSelf)
        {
            blade.SetActive(false);
        }
    }
}
