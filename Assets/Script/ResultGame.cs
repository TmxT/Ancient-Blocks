using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class ResultGame : MonoBehaviour
{
    public GameObject[] stars;

    public GameObject panelButton;
    public GameObject panelResult;
    public GameObject panelPause;

    public TextMeshProUGUI precentage;
    public TextMeshProUGUI timeSpent;
    public TextMeshProUGUI enemyKilled;
    public AudioSource calculateSfx;
    public AudioSource starSfx;
    public AudioSource backsound;
    
    public int enemy;
    public int reqTime;
    public int thisLevel;

    [HideInInspector] public int killed;
    [HideInInspector] public int min;

    [HideInInspector] public float sec;

    public bool isPause;
    private bool isKeyboardDevice;

    [SerializeField] private int totalResult;

    private Player player;
    private AnimationButton animationButton;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        animationButton = gameObject.GetComponent<AnimationButton>();
        
        if (PlayerPrefs.GetString("Device") == "Keyboard")
        {
            isKeyboardDevice = true;
            panelButton.SetActive(false);
        }
        else
        {
            isKeyboardDevice = false;
            panelButton.SetActive(true);
        }

        player.isKeyboardDevice = isKeyboardDevice;

        isPause = false;
        killed = enemy;        
    }

    private void FixedUpdate()
    {
        if (!isPause)
        {
            sec += Time.deltaTime;

            if (sec >= 59)
            {
                sec = 0;
                min++;
            }

            timeSpent.text = min + ":" + sec.ToString("F0");
            enemyKilled.text = killed + "/" + enemy;
        }
    }

    public void Result()
    {
        backsound.Stop();

        int starsResult;
        float enemyResult, timeStarResult;
        float _kill, _time, _timeSpent, _timeBonuses = 50;

        _kill = enemy - killed;
        _timeSpent = ((min * 60) + (int)sec);
        _time = reqTime - _timeSpent + _timeBonuses;
        isPause = true;
        
        panelResult.SetActive(true);

        if (_kill >= 0 && _timeSpent > 0)
        {
            timeStarResult = (_time / reqTime) * 100f;
            enemyResult = (_kill / enemy) * 100f;

            totalResult = (int)(timeStarResult + enemyResult) / 2;
            
            if (totalResult >= 90)
            {
                starsResult = 3;
            }
            else if (totalResult >= 60)
            {
                starsResult = 2;
            }
            else if (totalResult >= 25)
            {
                starsResult = 1;
            }
            else
            {
                starsResult = 0;
            }
            
            StartCoroutine(PrecentageAnimation(starsResult));

            if (PlayerPrefs.GetInt("Precentage_" + thisLevel) < totalResult)
            {
                PlayerPrefs.SetInt("Stars_" + thisLevel, starsResult);
                PlayerPrefs.SetInt("Precentage_" + thisLevel, totalResult);
            }

            if (!LevelLarger() && totalResult > 25 && thisLevel <= 10)
            {
                PlayerPrefs.SetInt("Level", thisLevel + 1);
            }
        }
        else
        {
            StartCoroutine(PrecentageAnimation(0));
            totalResult = 0;
        }

    }

    IEnumerator PrecentageAnimation(int _starsResult)
    {
        calculateSfx.Play();

        for (int i = 0; i <= totalResult; i++)
        {
            yield return new WaitForSeconds(.005f);
            precentage.text = i.ToString() + "%";
        }

        calculateSfx.Stop();

        precentage.GetComponent<Animator>().SetTrigger("precentage");

        for (int i = 0; i < _starsResult; i++)
        {
            yield return new WaitForSeconds(.5f);
            stars[i].SetActive(true);
            starSfx.Play();
        }
    }

    public void BtnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public void BtnNext()
    {
        if (totalResult >= 25 && thisLevel <= 10)
        {
            SceneManager.LoadScene("Level_" + (thisLevel + 1));
        }
    }

    public void BtnPlayAgain()
    {
        SceneManager.LoadScene("Level_" + thisLevel);
    }

    public void BtnPause()
    {
        animationButton.BtnPause();

        if (panelPause.activeSelf)
        {
            panelPause.SetActive(false);
            isPause = false;
        }
        else
        {
            panelPause.SetActive(true);
            isPause = true;
        }
    }

    public void BtnAttack()
    {
        player.BtnAttack();
    }

    public void BtnJump()
    {
        player.BtnJump();
        animationButton.BtnJump();
    }

    public void BtnMoveLeftDown()
    {
        player.BtnMoveLeftDown();
        animationButton.BtnWalkLeft_Down();
    }
    public void BtnMoveRightDown()
    {
        player.BtnMoveRightDown();
        animationButton.BtnWalkRight_Down();
    }

    public void BtnMoveLeftUp()
    {
        player.BtnMoveLeftUp();
        animationButton.BtnWalkLeft_Up();
    }
    public void BtnMoveRightUp()
    {
        player.BtnMoveRightUp();
        animationButton.BtnWalkRight_Up();
    }

    private bool LevelLarger()
    {
        return thisLevel > PlayerPrefs.GetInt("Level");
    }
}
