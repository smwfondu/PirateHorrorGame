using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Material baseSkyMaterial; // Base sky material
    public Material morningSkybox;   // Skybox material for morning
    public Material daySkybox;       // Skybox material for day
    public Material sunsetSkybox;    // Skybox material for sunset
    public Material nightSkybox;     // Skybox material for night

    public Flare sunFlare;

    public float morningToDayDuration = 90f;
    public float dayToSunsetDuration = 60f;
    public float sunsetToNightDuration = 60f;

    private float transitionTimer = 0f;
    private int currentPhase = 0;

    private Color startSunDiscColor, startSunHaloColor, startHorizonLineColor, startSkyGradientTopColor, startSkyGradientBottomColor;
    private Color targetSunDiscColor, targetSunHaloColor, targetHorizonLineColor, targetSkyGradientTopColor, targetSkyGradientBottomColor;

    private float startSunDiscMultiplier, startSunDiscExponent, startSunHaloExponent, startSunHaloContribution;
    private float targetSunDiscMultiplier, targetSunDiscExponent, targetSunHaloExponent, targetSunHaloContribution;

    private float startHorizonLineExponent, startHorizonLineContribution;
    private float targetHorizonLineExponent, targetHorizonLineContribution;
    private float startSkyGradientExponent, targetSkyGradientExponent;

    // Sun Shit
    // Duration to complete the sun's full rotation (combined durations of all transitions)
    private float fullDayDuration; // Duration of a full day in seconds (can be adjusted based on your need)
    private float currentRotation = 0f;

    // Reference to the directional light (the sun)
    public Light directionalLight;

    // Start and end rotation (rotation from horizon to overhead to horizon)
    private float startRotation = 0f; // Starting at the horizon (east)
    private float endRotation = 190f;    // Ending at the horizon (west)

    private float timeElapsed = 0f;
    private bool nightTimeStarted = false;

    private float moonTargetRotation = 60f; // Target rotation for the moon (angle on x-axis)
    private bool moonMode = false; // Indicates whether the moon phase is active

    private void Start()
    {
        fullDayDuration = morningToDayDuration + dayToSunsetDuration;

        RenderSettings.skybox = baseSkyMaterial;

        // Initialize with morning colors directly onto the base sky material
        SetStartProperties(morningSkybox);
        SetTargetProperties(daySkybox);
        ApplyProperties(baseSkyMaterial, startSunDiscColor, startSunHaloColor, startHorizonLineColor, startSkyGradientTopColor, startSkyGradientBottomColor,
                        startSunDiscMultiplier, startSunDiscExponent, startSunHaloExponent, startSunHaloContribution,
                        startHorizonLineExponent, startHorizonLineContribution, startSkyGradientExponent);

        directionalLight.intensity = 1.5f;
        RenderSettings.reflectionIntensity = 1;
        RenderSettings.ambientIntensity = 1;

        currentPhase = 1;
    }

    private void OnApplicationQuit()
    {
        ApplyProperties(baseSkyMaterial, startSunDiscColor, startSunHaloColor, startHorizonLineColor, startSkyGradientTopColor, startSkyGradientBottomColor,
                        startSunDiscMultiplier, startSunDiscExponent, startSunHaloExponent, startSunHaloContribution,
                        startHorizonLineExponent, startHorizonLineContribution, startSkyGradientExponent);
    }

    private void Update()
    {
        // Update the transition timer
        float transitionDuration = GetCurrentTransitionDuration();
        transitionTimer += Time.deltaTime;
        float progress = transitionTimer / transitionDuration;

        if (progress <= 1f)
        {
            // Lerp between start and target properties
            baseSkyMaterial.SetColor("_SunDiscColor", Color.Lerp(startSunDiscColor, targetSunDiscColor, progress));
            baseSkyMaterial.SetColor("_SunHaloColor", Color.Lerp(startSunHaloColor, targetSunHaloColor, progress));
            baseSkyMaterial.SetColor("_HorizonLineColor", Color.Lerp(startHorizonLineColor, targetHorizonLineColor, progress));
            baseSkyMaterial.SetColor("_SkyGradientTop", Color.Lerp(startSkyGradientTopColor, targetSkyGradientTopColor, progress));
            baseSkyMaterial.SetColor("_SkyGradientBottom", Color.Lerp(startSkyGradientBottomColor, targetSkyGradientBottomColor, progress));

            // Lerp for other sun-related and horizon-related properties
            baseSkyMaterial.SetFloat("_SunDiscMultiplier", Mathf.Lerp(startSunDiscMultiplier, targetSunDiscMultiplier, progress));
            baseSkyMaterial.SetFloat("_SunDiscExponent", Mathf.Lerp(startSunDiscExponent, targetSunDiscExponent, progress));
            baseSkyMaterial.SetFloat("_SunHaloExponent", Mathf.Lerp(startSunHaloExponent, targetSunHaloExponent, progress));
            baseSkyMaterial.SetFloat("_SunHaloContribution", Mathf.Lerp(startSunHaloContribution, targetSunHaloContribution, progress));

            baseSkyMaterial.SetFloat("_HorizonLineExponent", Mathf.Lerp(startHorizonLineExponent, targetHorizonLineExponent, progress));
            baseSkyMaterial.SetFloat("_HorizonLineContribution", Mathf.Lerp(startHorizonLineContribution, targetHorizonLineContribution, progress));

            baseSkyMaterial.SetFloat("_SkyGradientExponent", Mathf.Lerp(startSkyGradientExponent, targetSkyGradientExponent, progress));

            // Update the Directional Light color
            if (directionalLight != null)
            {
                directionalLight.color = Color.Lerp(startSunDiscColor, targetSunDiscColor, progress);
            }
        }
        else
        {
            // Move to the next phase when the transition is complete
            transitionTimer = 0f;
            MoveToNextPhase();
        }

        // Update the time elapsed
        timeElapsed += Time.deltaTime;

        if (timeElapsed < fullDayDuration && nightTimeStarted != true)
        {
            currentRotation = Mathf.Lerp(startRotation, endRotation, timeElapsed / fullDayDuration);
        }
        else if (nightTimeStarted == false && timeElapsed >= fullDayDuration)
        {
            timeElapsed = 0f;
            nightTimeStarted = true;
            moonMode = true;
        }

        if (nightTimeStarted)
        {
            float intensity = Mathf.Lerp(1f, 0.1f, timeElapsed / sunsetToNightDuration);
            directionalLight.intensity = intensity;
            RenderSettings.reflectionIntensity = intensity;
            RenderSettings.ambientIntensity = intensity;

            if(intensity < 0.01f)
            {
                directionalLight.gameObject.SetActive(false);
            }
        }

        if (moonMode)
        {
            currentRotation = Mathf.Lerp(0f, moonTargetRotation, timeElapsed / sunsetToNightDuration);
            directionalLight.intensity = 0.5f; // Lower intensity for the moon
            directionalLight.color = new Color(0.5f, 0.5f, 0.7f); // Soft moonlight color
        }

        // Apply the calculated rotation to the sun light (around the x-axis)
        if (directionalLight != null)
        {
            directionalLight.transform.rotation = Quaternion.Euler(currentRotation, -154f, 0f);
        }
    }

    public bool GetNightStatus()
    {
        return nightTimeStarted;
    }
    private void MoveToNextPhase()
    {
        currentPhase++;
        switch (currentPhase)
        {
            case 1:
                SetStartProperties(morningSkybox);
                SetTargetProperties(daySkybox);
                break;
            case 2:
                SetStartProperties(daySkybox);
                SetTargetProperties(sunsetSkybox);
                break;
            case 3:
                SetStartProperties(sunsetSkybox);
                SetTargetProperties(nightSkybox);
                break;
            case 4:
                // Night phase - stop transitions
                SetStartProperties(nightSkybox);
                SetTargetProperties(nightSkybox); // No transition, stays at night
                break;
        }
    }

    private float GetCurrentTransitionDuration()
    {
        switch (currentPhase)
        {
            case 1: return morningToDayDuration;
            case 2: return dayToSunsetDuration;
            case 3: return sunsetToNightDuration;
            default: return Mathf.Infinity; // Night has no transition
        }
    }

    private void SetStartProperties(Material skyboxMaterial)
    {
        startSunDiscColor = skyboxMaterial.GetColor("_SunDiscColor");
        startSunHaloColor = skyboxMaterial.GetColor("_SunHaloColor");
        startHorizonLineColor = skyboxMaterial.GetColor("_HorizonLineColor");
        startSkyGradientTopColor = skyboxMaterial.GetColor("_SkyGradientTop");
        startSkyGradientBottomColor = skyboxMaterial.GetColor("_SkyGradientBottom");

        startSunDiscMultiplier = skyboxMaterial.GetFloat("_SunDiscMultiplier");
        startSunDiscExponent = skyboxMaterial.GetFloat("_SunDiscExponent");
        startSunHaloExponent = skyboxMaterial.GetFloat("_SunHaloExponent");
        startSunHaloContribution = skyboxMaterial.GetFloat("_SunHaloContribution");

        startHorizonLineExponent = skyboxMaterial.GetFloat("_HorizonLineExponent");
        startHorizonLineContribution = skyboxMaterial.GetFloat("_HorizonLineContribution");

        startSkyGradientExponent = skyboxMaterial.GetFloat("_SkyGradientExponent");
    }

    private void SetTargetProperties(Material skyboxMaterial)
    {
        targetSunDiscColor = skyboxMaterial.GetColor("_SunDiscColor");
        targetSunHaloColor = skyboxMaterial.GetColor("_SunHaloColor");
        targetHorizonLineColor = skyboxMaterial.GetColor("_HorizonLineColor");
        targetSkyGradientTopColor = skyboxMaterial.GetColor("_SkyGradientTop");
        targetSkyGradientBottomColor = skyboxMaterial.GetColor("_SkyGradientBottom");

        targetSunDiscMultiplier = skyboxMaterial.GetFloat("_SunDiscMultiplier");
        targetSunDiscExponent = skyboxMaterial.GetFloat("_SunDiscExponent");
        targetSunHaloExponent = skyboxMaterial.GetFloat("_SunHaloExponent");
        targetSunHaloContribution = skyboxMaterial.GetFloat("_SunHaloContribution");

        targetHorizonLineExponent = skyboxMaterial.GetFloat("_HorizonLineExponent");
        targetHorizonLineContribution = skyboxMaterial.GetFloat("_HorizonLineContribution");

        targetSkyGradientExponent = skyboxMaterial.GetFloat("_SkyGradientExponent");
    }

    private void ApplyProperties(Material skyMaterial, Color sunDisc, Color sunHalo, Color horizon, Color gradientTop, Color gradientBottom,
                                 float sunDiscMultiplier, float sunDiscExponent, float sunHaloExponent, float sunHaloContribution,
                                 float horizonLineExponent, float horizonLineContribution, float skyGradientExponent)
    {
        skyMaterial.SetColor("_SunDiscColor", sunDisc);
        skyMaterial.SetColor("_SunHaloColor", sunHalo);
        skyMaterial.SetColor("_HorizonLineColor", horizon);
        skyMaterial.SetColor("_SkyGradientTop", gradientTop);
        skyMaterial.SetColor("_SkyGradientBottom", gradientBottom);

        skyMaterial.SetFloat("_SunDiscMultiplier", sunDiscMultiplier);
        skyMaterial.SetFloat("_SunDiscExponent", sunDiscExponent);
        skyMaterial.SetFloat("_SunHaloExponent", sunHaloExponent);
        skyMaterial.SetFloat("_SunHaloContribution", sunHaloContribution);

        skyMaterial.SetFloat("_HorizonLineExponent", horizonLineExponent);
        skyMaterial.SetFloat("_HorizonLineContribution", horizonLineContribution);

        skyMaterial.SetFloat("_SkyGradientExponent", skyGradientExponent);
    }
}
