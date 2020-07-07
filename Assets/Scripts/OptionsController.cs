using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public SphereCollider soccerBallCollider;
    public GameObject ResetHighScoresPanel;

    public Toggle musicToggle;
    public Slider musicSlider;
    public AudioSource musicAudio;
    public Toggle SFXToggle;
    public Slider SFXSlider;
    public AudioSource SFXAudio_Hit;
    public AudioSource SFXAudio_Target;
    public Slider kickForceSlider;
    public Text kickForceValueText;
    public Slider kickForceRadiusSlider;
    public Text kickForceRadiusValueText;
    public Slider ballHitBoxSizeSlider;
    public Text ballHitBoxSizeValueText;
    public Slider gravitySlider;
    public Text gravityValueText;

    public static bool isMusicOn;
    public static float musicVolume;
    public static bool isSFXOn;
    public static float SFXVolume;
    public static float kickForce;
    public static float kickForceRadius;
    public static float ballHitBoxSize;
    public static float gravity;

    // Defaults
    private const bool isMusicOn_default = true;
    private const float musicVolume_default = 0.75f;
    private const bool isSFXOn_default = true;
    private const float SFXVolume_default = 0.5f;
    private const float kickForce_default = 1.0f;
    private const float kickForceRadius_default = 2.0f;
    private const float ballHitBoxSize_default = 0.11f;
    private const float gravity_default = 5.0f;

    private void Awake()
    {
        // Game Music
        isMusicOn = PlayerPrefs.GetInt("IsMusicOn", isMusicOn_default ? 1 : 0) == 1 ? true : false;
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", musicVolume_default);
        musicToggle.isOn = isMusicOn;
        musicAudio.volume = musicToggle.isOn ? musicVolume : 0;
        musicSlider.value = musicVolume;

        // Sound Effects
        isSFXOn = PlayerPrefs.GetInt("IsSFXOn", isSFXOn_default ? 1 : 0) == 1 ? true : false;
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", SFXVolume_default);
        SFXToggle.isOn = isSFXOn;
        SFXAudio_Hit.volume = isSFXOn ? SFXVolume : 0;
        SFXAudio_Target.volume = isSFXOn ? SFXVolume : 0;
        SFXSlider.value = SFXVolume;

        // Kick Force
        kickForce = PlayerPrefs.GetFloat("KickForce", kickForce_default);
        kickForceSlider.value = kickForce;
        kickForceValueText.text = kickForce.ToString("0.00");

        // Kick Force Radius
        kickForceRadius = PlayerPrefs.GetFloat("KickForceRadius", kickForceRadius_default);
        kickForceRadiusSlider.value = kickForceRadius;
        kickForceRadiusValueText.text = kickForceRadius.ToString("0.00");

        // Ball Hit Box Size
        ballHitBoxSize = PlayerPrefs.GetFloat("BallHitBoxSize", ballHitBoxSize_default);
        soccerBallCollider.radius = ballHitBoxSize;
        ballHitBoxSizeSlider.value = ballHitBoxSize * 10.0f;
        ballHitBoxSizeValueText.text = (ballHitBoxSize * 10.0f).ToString("0.00");

        // Gravity
        gravity = PlayerPrefs.GetFloat("Gravity", gravity_default);
        Physics.gravity = new Vector3(0, -gravity, 0);
        gravitySlider.value = gravity;
        gravityValueText.text = gravity.ToString("0.00");
    }

    public void RestoreDefaultSettings()
    {
        // Game Music
        isMusicOn = isMusicOn_default;
        musicVolume = musicVolume_default;
        PlayerPrefs.SetInt("IsMusicOn", isMusicOn ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        musicToggle.isOn = isMusicOn;
        musicAudio.volume = isMusicOn ? musicVolume : 0;
        musicSlider.value = musicVolume;

        // Sound Effects
        isSFXOn = isSFXOn_default;
        SFXVolume = SFXVolume_default;
        PlayerPrefs.SetInt("IsSFXOn", isSFXOn ? 1 : 0);
        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
        SFXToggle.isOn = isSFXOn;
        SFXAudio_Hit.volume = isSFXOn ? SFXVolume : 0;
        SFXAudio_Target.volume = isSFXOn ? SFXVolume : 0;
        SFXSlider.value = SFXVolume;

        // Kick Force
        kickForce = kickForce_default;
        PlayerPrefs.SetFloat("KickForce", kickForce);
        kickForceSlider.value = kickForce;
        kickForceValueText.text = kickForce.ToString("0.00");

        // Kick Force Radius
        kickForceRadius = kickForceRadius_default;
        PlayerPrefs.SetFloat("KickForceRadius", kickForceRadius);
        kickForceRadiusSlider.value = kickForceRadius;
        kickForceRadiusValueText.text = kickForceRadius.ToString("0.00");

        // Ball Hit Box Size
        ballHitBoxSize = ballHitBoxSize_default;
        PlayerPrefs.SetFloat("BallHitBoxSize", ballHitBoxSize);
        soccerBallCollider.radius = ballHitBoxSize;
        ballHitBoxSizeSlider.value = ballHitBoxSize * 10.0f;
        ballHitBoxSizeValueText.text = (ballHitBoxSize * 10.0f).ToString("0.00");

        // Gravity
        gravity = gravity_default;
        PlayerPrefs.SetFloat("Gravity", gravity);
        Physics.gravity = new Vector3(0, -gravity, 0);
        gravitySlider.value = gravity;
        gravityValueText.text = gravity.ToString("0.00");
    }

    public void ToggleMusic()
    {
        isMusicOn = musicToggle.isOn;
        PlayerPrefs.SetInt("IsMusicOn", isMusicOn ? 1 : 0);
        musicToggle.isOn = isMusicOn;
        musicAudio.volume = isMusicOn ? musicVolume : 0;
    }

    public void AdjustMusicVolume()
    {
        musicVolume = musicSlider.value;

        if (isMusicOn)
            musicAudio.volume = musicVolume;

        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void ToggleSFX()
    {
        isSFXOn = SFXToggle.isOn;
        PlayerPrefs.SetInt("IsSFXOn", isSFXOn ? 1 : 0);
        SFXAudio_Hit.volume = isSFXOn ? SFXVolume : 0;
        SFXAudio_Target.volume = isSFXOn ? SFXVolume : 0;
    }

    public void AdjustSFXVolume()
    {
        SFXVolume = SFXSlider.value;

        if (isSFXOn)
        {
            SFXAudio_Hit.volume = SFXVolume;
            SFXAudio_Target.volume = SFXVolume;
        }

        PlayerPrefs.SetFloat("SFXVolume", SFXVolume);
    }

    public void AdjustKickForce()
    {
        kickForce = kickForceSlider.value;
        kickForceValueText.text = kickForce.ToString("0.00");
        PlayerPrefs.SetFloat("KickForce", kickForce);
    }

    public void AdjustKickForceRadius()
    {
        kickForceRadius = kickForceRadiusSlider.value;
        kickForceRadiusValueText.text = kickForceRadius.ToString("0.00");
        PlayerPrefs.SetFloat("KickForceRadius", kickForceRadius);
    }

    public void AdjustBallHitBoxSize()
    {
        ballHitBoxSize = ballHitBoxSizeSlider.value / 10.0f;
        soccerBallCollider.radius = ballHitBoxSize;
        ballHitBoxSizeValueText.text = (ballHitBoxSize * 10.0f).ToString("0.00");
        PlayerPrefs.SetFloat("BallHitBoxSize", ballHitBoxSize);
    }

    public void AdjustGravity()
    {
        gravity = gravitySlider.value;
        Physics.gravity = new Vector3(0, -gravity, 0);
        gravityValueText.text = gravity.ToString("0.00");
        PlayerPrefs.SetFloat("Gravity", gravity);
    }

    public void OpenResetHighScoresPrompt()
    {
        ResetHighScoresPanel.SetActive(true);
    }

    public void CloseResetHighScoresPrompt()
    {
        ResetHighScoresPanel.SetActive(false);
    }
}
