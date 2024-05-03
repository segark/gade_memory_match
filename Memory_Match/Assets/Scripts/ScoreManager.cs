using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    public int currentScoreOne;
    public int currentComboOne;
    private int currentTurnOne;

    public int currentScoreTwo;
    public int currentComboTwo;
    private int currentTurnTwo;
    public int playTime;
    private int mins;
    private int secs;

    private int currentPlayer;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scorePlayerOneText;
    public TextMeshProUGUI comboPlayerOneText;
    public TextMeshProUGUI turnsPlayerOneText;

    public TextMeshProUGUI scorePlayerTwoText;
    public TextMeshProUGUI comboPlayerTwoText;
    public TextMeshProUGUI turnsPlayerTwoText;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        UpdateScoreText();
        StartCoroutine("PlayTime");
        currentPlayer = 1; // Start with Player 1

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator PlayTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            playTime++;
            secs = (playTime % 60);
            mins = (playTime/60) %60;
            UpdateTime();
        }
    }

    void UpdateTime()
    {
        timeText.text = "Time:" + mins.ToString("D2")+ ":" + secs.ToString("D2");
    }

    public void AddScore(int scoreAmount)
    {
       
        if (currentPlayer == 1)
        {
            currentComboOne++;
            currentTurnOne++;
            currentScoreOne += scoreAmount * currentComboOne;
            UpdateScoreText();
        }

        else if (currentPlayer == 2)
        {
            currentComboTwo++;
            currentTurnTwo++;
            currentScoreTwo += scoreAmount * currentComboTwo;
            UpdateScoreText();
        }
           
       
    }

   void UpdateScoreText()
    {
        scorePlayerOneText.text = "Score;" + currentScoreOne.ToString("N");
        comboPlayerOneText.text = "Combo:" + currentComboOne.ToString();
        turnsPlayerOneText.text = currentTurnOne.ToString();

        scorePlayerTwoText.text = "Score;" + currentScoreTwo.ToString("N");
        comboPlayerTwoText.text = "Combo:" + currentComboTwo.ToString();
        turnsPlayerTwoText.text = currentTurnTwo.ToString();
    }

    public void UpdateCurrentTurn()
    {
        if (currentPlayer == 1)
        {
            currentTurnOne++;
            
        }
        else if (currentPlayer == 2)
        {
            currentTurnTwo++;
            
        }
        UpdateScoreText();
    }

    public void ResetCombo()
    {
        if(currentPlayer == 1)
        {
            currentComboOne = 0;
            currentTurnOne++;
        }
        else if (currentPlayer == 2)
        {
            currentComboTwo=0;
            currentTurnTwo++;
        }
        
        UpdateScoreText() ;
    }

    public void StopTimer()
    {
        StopCoroutine("PlayTime");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SwitchPlayerTurn()
    {
        if (currentPlayer == 1)
        {
            currentPlayer = 2;
            
        }
        else if (currentPlayer == 2)
        {
            currentPlayer = 1;
            
        }
        
    }
}
