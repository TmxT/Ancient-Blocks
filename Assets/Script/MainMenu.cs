using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public GameObject panelMenu;
    public GameObject panelDevice;
    public GameObject panelSetting;
    public GameObject panelTutorial;
    public GameObject panelConfirm;

    public Toggle musicToggle;

    public AudioSource musicAudioSource;
    public AudioSource clickAudioSource;

    public AudioMixer mixerMusic;

    public Animator btnSettingAnim;
    private Animator panelMenuAnim;
    private Animator panelDeviceAnim;
    private Animator panelSettingAnim;
    private Animator panelTutorialAnim;

    private float sliderValue;

    private void Start()
    {
        panelMenuAnim = panelMenu.GetComponent<Animator>();
        panelDeviceAnim = panelDevice.GetComponent<Animator>();
        panelSettingAnim = panelSetting.GetComponent<Animator>();
        panelTutorialAnim = panelTutorial.GetComponent<Animator>();
        
        PlayerPrefs.SetInt("Level", 10);
    }

    private void Update()
    {
        if (!musicToggle.isOn)
        {
            musicAudioSource.mute = true;
        }
        else
        {
            musicAudioSource.mute = false;
        }
    }

    public void BtnStart()
    {
        StartCoroutine(WaitForMenuOut());
    }

    private IEnumerator WaitForMenuOut()
    {
        clickAudioSource.Play();
        panelMenuAnim.SetTrigger("out");
        yield return new WaitForSeconds(panelMenuAnim.runtimeAnimatorController.animationClips.Length * .5f);
        panelMenu.SetActive(false);
        panelDevice.SetActive(true);
    }

    public void BtnDevice(int _device)
    {
        clickAudioSource.Play();

        if (_device == 1)
        {
            PlayerPrefs.SetString("Device", "Android");
        } else if (_device == 2)
        {
            PlayerPrefs.SetString("Device", "Keyboard");
        }
        
        if (PlayerPrefs.GetInt("Level") < 1)
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        SceneManager.LoadScene("LevelMenu");
    }

    public void BtnDeviceClose()
    {
        StartCoroutine(WaitForDeviceOut());
    }

    private IEnumerator WaitForDeviceOut()
    {
        clickAudioSource.Play();
        panelDeviceAnim.SetTrigger("out");
        yield return new WaitForSeconds(panelMenuAnim.runtimeAnimatorController.animationClips.Length * .5f);
        panelDevice.SetActive(false);
        panelMenu.SetActive(true);
    }

    public void BtnTutorial()
    {
        StartCoroutine(WaitForTutorial());
    }

    private IEnumerator WaitForTutorial()
    {
        clickAudioSource.Play();

        if (panelTutorial.activeSelf)
        {
            panelTutorialAnim.SetTrigger("out");
            yield return new WaitForSeconds(panelTutorialAnim.runtimeAnimatorController.animationClips.Length * .5f);
            panelTutorial.SetActive(false);
        }
        else
        {
            panelTutorial.SetActive(true);
        }
    }

    public void BtnQuit()
    {
        clickAudioSource.Play();

        Application.Quit();
    }

    public void BtnSetting()
    {
        StartCoroutine(WaitForBtnSetting());
    }

    public void BtnConfirm()
    {
        if (!panelConfirm.activeSelf)
        {
            panelConfirm.SetActive(true);
        }
        else
        {
            panelConfirm.SetActive(false);
        }
    }

    public void BtnDeleteData()
    {
        PlayerPrefs.DeleteAll();
        BtnConfirm();
    }

    private IEnumerator WaitForBtnSetting()
    {
        clickAudioSource.Play();

        if (panelSetting.activeSelf)
        {
            btnSettingAnim.SetBool("open", false);
            panelSettingAnim.SetTrigger("out");
            yield return new WaitForSeconds(btnSettingAnim.runtimeAnimatorController.animationClips.Length);
            panelSetting.SetActive(false);
        }
        else
        {
            btnSettingAnim.SetBool("open", true);
            yield return new WaitForSeconds(btnSettingAnim.runtimeAnimatorController.animationClips.Length * .2f);
            panelSetting.SetActive(true);
        }
    }

    public void MusicVolume(float _sliderValue)
    {
        mixerMusic.SetFloat("Music", _sliderValue);
    }
}
