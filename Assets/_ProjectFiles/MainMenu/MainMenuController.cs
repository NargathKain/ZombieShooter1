using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

public class MenuController : MonoBehaviour
{
    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text ControllerSenTextValue = null;
    [SerializeField] private Slider ControllerSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainControllerSen = 4;

    [Header("Toggle Settings")]
    [SerializeField] private Toggle invertYToggle = null;

    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 1.0f;
    private int _qualityLevel;
    private bool _isFullscreen;
    private float _brightnessLevel;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown = null;
    [SerializeField] private Toggle fullscreenToggle = null;


    [Header("Confirmation Prompt")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Levels to Load")]
    public string _newGameLevel1;
    private string levelToLoad;
    [SerializeField] private GameObject NoSaveGamePanel_Dialog = null;

    [Header("Resolution Dropdown")]
    public TMP_Dropdown resolutionDropdown;
    public Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


    }


    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }




    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(1);
    }


    public void NewGameDialogNo()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void LoadGameDialogYes()
    {
        if (PlayerPrefs.HasKey("SavedLevel"))
        {
            levelToLoad = PlayerPrefs.GetString("SavedLevel");
            //PlayerPrefs.SetString("SavedLevel", levelToLoad);
            SceneManager.LoadScene(levelToLoad);
        }
        else
        {
            NoSaveGamePanel_Dialog.SetActive(true);
            //SceneManager.LoadScene("MainMenu");
        }
    }



    public void LoadGameDialogNo()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
        //volumeSlider.value.ToString();
    }

    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }

        if (MenuType == "Gameplay")
        {
            ControllerSenTextValue.text = defaultSen.ToString("0");
            ControllerSenSlider.value = defaultSen;
            mainControllerSen = defaultSen;
            invertYToggle.isOn = false;
            GameplayApply();
        }
        if (MenuType == "Graphics")
        {
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");
            qualityDropdown.value = 1;
            QualitySettings.SetQualityLevel(1);
            fullscreenToggle.isOn = true;
            Screen.fullScreen = false;
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

    }

    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);
    }

    public void SetControllerSen(float sensitivity)
    {
        mainControllerSen = Mathf.RoundToInt(sensitivity);
        ControllerSenTextValue.text = sensitivity.ToString("0");
        //ControllerSenSlider.value = sensitivity;
    }

    public void GameplayApply()
    {
        if (invertYToggle.isOn)
        {
            PlayerPrefs.SetInt("masterInvertY", 1);
        }
        else
        {
            PlayerPrefs.SetInt("masterInvertY", 0);
        }

        PlayerPrefs.SetFloat("masterControllerSen", mainControllerSen);
        StartCoroutine(ConfirmationBox());
    }


    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
        RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        _isFullscreen = isFullScreen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //RenderSettings.ambientLight = new Color(brightness, brightness, brightness, 1);
        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        //QualitySettings.SetQualityLevel(qualityIndex);

        PlayerPrefs.SetInt("masterFullscreen", _isFullscreen ? 1 : 0);
        Screen.fullScreen = _isFullscreen;

        StartCoroutine(ConfirmationBox());
    }


}