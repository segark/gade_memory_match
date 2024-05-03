using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class CardManager : MonoBehaviour
{
    public static CardManager instance;
    public List<Sprite> SpriteList = new List<Sprite>();
    private List <GameObject> buttonList = new List<GameObject>();
    private List<GameObject> hiddenButtonList = new List<GameObject>();

    private List<GameObject> chosenCards = new List<GameObject>();
    private int lastmatchID;
    public bool chosen;

    [Header("How many pairs you wants to play")]
    public int pairs;

    [Header("Card prefab button")]
    public GameObject cardPrefab;

    [Header("The parent spacer to hold cards in")]
    public Transform spacer;

    [Header("Score per match")]
    public int match = 100;


    public int choice1;
    public int choice2;

    public int totalMatches=0;

    public int matchesBeforeShuffle = 2;
    private int matchesMade = 0; // Keep track of the number of matches made

    public GameObject winPanel;
    public TextMeshProUGUI winText; 

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        winPanel.SetActive(false);
    }
    void Start()
    {
        FillPlayField();
       
    }


    ////void Update()
    ////{
    ////    // Check if the condition for shuffling is met
    ////    if (matchesMade % matchesBeforeShuffle == 0 && matchesMade > 0)
    ////    {
    ////        // Trigger the shuffle action
    ////        ShuffleGrid();
    ////    }
    ////}
    ////void ShuffleGrid()
    ////{
    ////    for (int i = 0; i < buttonList.Count; i++)
    ////    {
    ////        int randomIndex = Random.Range(i, buttonList.Count);
    ////        GameObject temp = buttonList[randomIndex];
    ////        buttonList[randomIndex] = buttonList[i];
    ////        buttonList[i] = temp;
    ////        // Update the positions of the shuffled cards
    ////        buttonList[i].transform.position = CalculatePosition(i); // Implement this method to calculate card positions
    ////    }
    ////}
    public void OnMatchMade()
    {
        matchesMade++;
    }
    Vector3 CalculatePosition(int index)
    {
        // Determine the number of columns in the grid
        int numColumns = 5; // Adjust this based on your grid layout

        // Determine the row and column of the card based on its index
        int row = index / numColumns;
        int col = index % numColumns;

        // Determine the position of the card based on row and column
        float offsetX = 2.0f; // Adjust this value based on your card spacing
        float offsetY = 2.0f; // Adjust this value based on your card spacing
        Vector3 position = new Vector3(col * offsetX, 0, row * offsetY);

        return position;
    }

    void FillPlayField()
    {
        for (int i = 0; i <(pairs *2); i++)
        {
            GameObject newCard = Instantiate(cardPrefab, spacer);
            buttonList.Add(newCard);
            hiddenButtonList.Add(newCard);
        }
        ShuffleCards();
    }

    void ShuffleCards()
    {
        int num = 0; // number of cards 
        int cardPairs = buttonList.Count / 2; // halve the cards for pairs

        for(int i =0 ; i <cardPairs;i++) // count card amount per match 
        {
            num++;
            for(int j =0; j<2; j++)
            {
                int cardIndex = Random.Range(0,buttonList.Count);
                Card tempCard = buttonList[cardIndex].GetComponent<Card>();
                tempCard.id = num;
                tempCard.cardfront = SpriteList[num-1]; // sprite list must = amount of pairs

                buttonList.Remove(buttonList[cardIndex]);
            }
        }
    }

    public void AddChosenCard(GameObject card)
    {
        chosenCards.Add(card);
    }

    public IEnumerator CompareCards()
    {
        ScoreManager.instance.SwitchPlayerTurn();
        if (choice2==0 || chosen)
        {
            yield break;
        }
        chosen = true;
        yield return new WaitForSeconds(1.5f);

        // no match 
        if((choice1 !=0 && choice2 != 0) &&(choice1 !=choice2) )
        {
            
            // flip back cards
            FlipAllBack();
            // reset combo
            ScoreManager.instance.ResetCombo();
            
        }
        else if ((choice1 != 0 && choice2 != 0) && (choice1 == choice2))
        {
           

            //if(totalMatches == 2) {

            //    lastmatchID = choice1;
            //    //add to score
               
            //    ScoreManager.instance.AddScore(match);
            //    // remove match
            //    RemoveMatch();
            //    //clear chosen cards
            //    chosenCards.Clear();
            //    Debug.Log("the cardshave been shuffled");
            //    //ReshuffleCards();
            //}
          

                lastmatchID = choice1;
                //add to score
                totalMatches++;
                ScoreManager.instance.AddScore(match);
                // remove match
                RemoveMatch();
                //clear chosen cards
                chosenCards.Clear();
            
            OnMatchMade();
        }

        // reset all choices

        choice1 = 0;
        choice2 = 0;
        chosen = false;

        // check if won 
        CheckWin();
        

    }

    void FlipAllBack()
    {
        foreach(GameObject card in chosenCards)
        {
            card.GetComponent<Card>().CloseCard();
        }
        chosenCards.Clear();
    }
    void ReshuffleCards()
    {
        int num = 0; // number of cards 
        int cardPairs = hiddenButtonList.Count / 2; // halve the cards for pairs

        for (int i = 0; i < cardPairs; i++) // count card amount per match 
        {
            num++;
            for (int j = 0; j < 2; j++)
            {
                int cardIndex = Random.Range(0, hiddenButtonList.Count);
                Card tempCard = hiddenButtonList[cardIndex].GetComponent<Card>();
                tempCard.id = num;
                tempCard.cardfront = SpriteList[num - 1]; // sprite list must = amount of pairs

                hiddenButtonList.Remove(hiddenButtonList[cardIndex]);
            }
        }
    }
    void RemoveMatch()
    {
        for(int i = hiddenButtonList.Count -1; i >= 0; i--)
        {
            Card tempCard = hiddenButtonList[i].GetComponent<Card>();
            if(tempCard.id == lastmatchID)
            {
                // add effect 

                // remove the card
                hiddenButtonList.RemoveAt(i);
            }
        }
    }

    void CheckWin()
    {
        if (hiddenButtonList.Count < 1)
        {
            // stop timer 
            ScoreManager.instance.StopTimer();

            if (ScoreManager.instance.currentScoreOne> ScoreManager.instance.currentScoreTwo)
            {
                //show ui
                winPanel.SetActive(true);
                winText.text = "Player One Won";
                Debug.Log("play one won ");
            }
            else if(ScoreManager.instance.currentScoreOne < ScoreManager.instance.currentScoreTwo)
            {
                //show ui
                winPanel.SetActive(true);
                winText.text = "Player Two Won";
                Debug.Log("play two won ");
            }
            else if (ScoreManager.instance.currentScoreOne == ScoreManager.instance.currentScoreTwo)
            {
                //show ui
                winPanel.SetActive(true);
                winText.text = "Its a tie !!";
                Debug.Log("tie ");
            }

        }
    }

    
}
