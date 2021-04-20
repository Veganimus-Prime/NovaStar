using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
namespace Veganimus.NovaStar
{
    ///<summary>
    ///@author
    ///Aaron Grincewicz
    ///</summary>
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private PlayableDirector _director;
        [Header("Main Menu UI")]
        [SerializeField] private GameObject _optionsMenu;
        [SerializeField] private TMP_Dropdown _resolutionDropDown;
        [SerializeField] private Toggle _fullScreenToggle;
        [SerializeField] private Resolution[] _availableRes;
        [SerializeField] private Slider _brightnessSlider;
        [Header("Listening To")]
        [SerializeField] private LoadSceneEventSO _loadSceneEventSO;
        private void OnEnable()=> _loadSceneEventSO.OnEventRaised += SceneLoader; 
        
        private void OnDisable()=> _loadSceneEventSO.OnEventRaised -= SceneLoader;

        void Start()=> _director.Play(); //GetScreenResolution();

        void SceneLoader(string value)
        {
            switch(value)
            {
                case "Main_Menu":
                    _director.Play();
                    break;
                case "Loading":
                    SceneManager.LoadScene("Loading");
                    break;
            }
        }
        public void QuitGame() => Application.Quit();
#if UNITY_STANDALONE
        public void GetScreenResolution()
        {
            _availableRes = Screen.resolutions;
            List<string> reso = new List<string>();
            foreach (var res in _availableRes)
            {
                reso.Add(res.ToString());
            }
            Debug.Log(message: "Resolution:" + _availableRes.ToString());
            _resolutionDropDown.AddOptions(reso);
        }
        public void ChangeResolution()
        {
            var height = _availableRes[_resolutionDropDown.value - 1].height;
            var width = _availableRes[_resolutionDropDown.value - 1].width;
            Screen.SetResolution(width, height, false);
            Debug.Log(message: "Selected Res:" + width + height);
        }
        public void FullScreenToggle()
        {
            if (_fullScreenToggle.isOn == true)
            {
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            }
            else
            {
                Screen.fullScreenMode = FullScreenMode.Windowed;
            }
            Debug.Log(message: "Screeen mode: " + Screen.fullScreen + "Selected mode" + _fullScreenToggle.isOn); ;
        }

        public void ChangeBrightness() => Screen.brightness = _brightnessSlider.value;
#endif
    }
}