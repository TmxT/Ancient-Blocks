using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandomizedBlock : MonoBehaviour
{
    public Sprite[] blocks;

    public GameObject[] prefabBlocks;

    public GameObject spawnWarning;

    public Image preview;

    public TextMeshProUGUI previewTimerText;
    public TextMeshProUGUI previewBlocksBankText;
    public TextMeshProUGUI previewCooldownText;

    public Transform instantiatePos;

    public AudioSource spawnSfx;

    private Vector3 fixedPos;

    private int random;
    private int blocksBank;

    private float timer = 10;
    private float cooldownBlocks = 5;

    private bool randomizing = false;
    [HideInInspector] public bool rotateLeft;
    [HideInInspector] public bool rotateRight;

    private ResultGame resultGame;
    private BlocksManager blocksManager;

    private void Start()
    {
        preview.preserveAspect = true;
        resultGame = GetComponent<ResultGame>();
    }

    private void Update()
    {
        if (!resultGame.isPause)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                BtnSpawn();
            }
            
            if (timer <= 0)
            {
                timer = 11;
                blocksBank++;
            }
            else
            {
                timer -= Time.deltaTime;
                previewTimerText.text = timer.ToString("F0");
            }

            fixedPos = new Vector3(instantiatePos.position.x, instantiatePos.position.y, 0);

            if (cooldownBlocks > 0)
            {
                cooldownBlocks -= Time.deltaTime;
                previewCooldownText.text = cooldownBlocks.ToString("F0");
            }

            previewBlocksBankText.text = blocksBank.ToString();
        }
    }

    IEnumerator RandomingBlock()
    {
        spawnWarning.SetActive(true);
        spawnSfx.Play();

        for (int i = 0; i < 10; i++)
        {
            random = Random.Range(0, blocks.Length);
            preview.sprite = blocks[random];
            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(2f);

        InstantiateBlock();
        spawnSfx.Stop();
        spawnWarning.SetActive(false);
    }

    private void InstantiateBlock()
    {
        Instantiate(prefabBlocks[random], fixedPos, Quaternion.identity);
        blocksManager = prefabBlocks[random].GetComponent<BlocksManager>();

        randomizing = false;
        cooldownBlocks = 5;
        preview.sprite = null;
        blocksBank--;
    }

    public void BtnSpawn()
    {
        if (blocksBank > 0 && cooldownBlocks <= 0 && randomizing == false)
        {
            randomizing = true;
            previewCooldownText.text = "";
            StartCoroutine(RandomingBlock());
        }
    }

    public void BtnRotateLeft()
    {
        rotateLeft = true;
    }
    public void BtnRotateRight()
    {
        rotateRight = true;
    }
}
