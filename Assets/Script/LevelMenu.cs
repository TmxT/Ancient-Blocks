using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelMenu : MonoBehaviour
{
    public Image[] btnLevel;

    public GameObject[] stars;

    public GameObject panelInfoLevel;

    public TextMeshProUGUI precentage;

    public Sprite levelLocked;

    public Animator infoLevelAnim;

    public AudioSource clickAudioSource;
    public AudioSource calculateSfx;
    public AudioSource starSfx;

    private int levelUnlocked;
    private int levelClicked;

    private void Awake()
    {
        levelUnlocked = PlayerPrefs.GetInt("Level");

        for (int i = 0; i < 10; i++)
        {
            if (i > levelUnlocked - 1)
            {
                btnLevel[i].sprite = levelLocked;
                btnLevel[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "";
                btnLevel[i].GetComponent<Button>().enabled = false;
            }
        }
    }

    public void BtnLevel(int _level)
    {
        panelInfoLevel.SetActive(true);
        clickAudioSource.Play();

        if (_level <= levelUnlocked)
        {
            levelClicked = _level;
            StartCoroutine(InfoLevel());
        }
    }

    IEnumerator InfoLevel()
    {
        stars[0].SetActive(false);
        stars[1].SetActive(false);
        stars[2].SetActive(false);

        precentage.text = "0%";

        calculateSfx.Play();
        for (int i = 0; i < PlayerPrefs.GetInt("Precentage_" + levelClicked); i++)
        {
            yield return new WaitForSeconds(.01f);
            precentage.text = i.ToString() + "%";
        }
        calculateSfx.Stop();

        precentage.GetComponent<Animator>().SetTrigger("precentage");

        for (int i = 0; i < PlayerPrefs.GetInt("Stars_" + levelClicked); i++)
        {
            yield return new WaitForSeconds(.5f);
            stars[i].SetActive(true);
            starSfx.Play();
        }
    }

    public void BtnPlay()
    {
        clickAudioSource.Play();
        Destroy(GameObject.FindGameObjectWithTag("Respawn"));
        SceneManager.LoadScene("Level_" + levelClicked);
    }

    public void BtnClose()
    {
        StartCoroutine(WaitForInfoLevelClose());
    }

    private IEnumerator WaitForInfoLevelClose()
    {
        clickAudioSource.Play();
        infoLevelAnim.SetTrigger("out");
        yield return new WaitForSeconds(infoLevelAnim.runtimeAnimatorController.animationClips.Length * .5f);
        panelInfoLevel.SetActive(false);
    }

    public void BtnMenu()
    {
        clickAudioSource.Play();
        SceneManager.LoadScene("MainMenu");
    }
}
