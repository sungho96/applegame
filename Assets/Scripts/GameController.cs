using UnityEngine;
using TMPro;
using System.Collections;
//0)장면이동
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameController : MonoBehaviour
{
    [SerializeField]
    private GameObject panelMainMenu;

    [SerializeField]
    private GameObject panelInGame;

    [SerializeField]
    private GameObject panelGameOver;

    [SerializeField]
    private TextMeshProUGUI textInGameScore;
    
    [SerializeField]
    private TextMeshProUGUI textInGameOverScore;

    [SerializeField]
    private Image timeGauge;

    [SerializeField]
    private float maxTime = 120.0f;

    private int currentScore = 0;
    private AudioSource audioSource;

    public bool IsGameStart {private set; get;} = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void GameStart()
    {
        IsGameStart = true;

        panelMainMenu.SetActive(false);
        panelInGame.SetActive(true);
        audioSource.Play();

        StartCoroutine(nameof(TimeCounter));

    }
    public void IncreaseScore(int addscore)
    {
        currentScore += addscore;
        textInGameScore.text = currentScore.ToString();
    }
    public void ButtonRestart()
    {
        SceneManager.LoadScene(0);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void GameOver()
    {
        textInGameOverScore.text = $"SCORE\n{currentScore}";

        panelInGame.SetActive(false);
        panelGameOver.SetActive(true);
        audioSource.Stop();
    }
    private IEnumerator TimeCounter()
    {
        float currentTime = maxTime;

        while( currentTime >0)
        {
            currentTime -= Time.deltaTime;
            timeGauge.fillAmount = currentTime / maxTime;
            Debug.Log(currentTime);

            yield return null;
        }
        Debug.Log("GameOver");
        
    }
}
