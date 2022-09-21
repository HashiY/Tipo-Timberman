using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class PauseMenuController : MonoBehaviour
{
	public Slider volumeSlider;
	public AudioMixer mixer;
	private float value;
	Resolution[] resolutions;
	public Dropdown resolutionDropdown;

	void Awake()
	{
		GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;

		OnGameStateChanged(GameStateManager.Instance.CurrentGameState);
	}

	void OnDestroy()
	{
		GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newGameState)
	{
		gameObject.SetActive(newGameState == GameState.Paused);
	}


    private void Start()
    {
		mixer.GetFloat("Volume",out value);
		volumeSlider.value = value;

		resolutions = Screen.resolutions;
		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
        {
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if(resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
            {
				currentResolutionIndex = i;
            }
		}

		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = currentResolutionIndex;
		resolutionDropdown.RefreshShownValue();
	}

    public void SetVolume()
    {
		mixer.SetFloat("Volume", volumeSlider.value);

	}

	public void SetQuality(int qualityIndex)
    {
		QualitySettings.SetQualityLevel(qualityIndex);
    }

	public void SetFullscreen(bool isFullscreen)
    {
		Screen.fullScreen = isFullscreen;
    }

	public void SetResolution(int resolutionIndex)
    {
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
}
