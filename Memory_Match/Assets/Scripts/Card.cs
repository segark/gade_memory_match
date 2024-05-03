using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class Card : MonoBehaviour
{
    [HideInInspector] public int id;
    public Sprite cardback;
    [HideInInspector] public Sprite cardfront;
    private Image image;
    private Button button;

    private bool isflippingOpen;
    private bool isflippingClose;
    private bool flipped;
    private float flipAmount = 1f;

    public float flipSpeed = 4f;
    // Start is called before the first frame update

   

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        
    }

    // onclick method to flip the cards 
    public void FlipCard()
    {
        if(CardManager.instance.choice1 == 0)
        {
            CardManager.instance.choice1 = id;
            CardManager.instance.AddChosenCard(this.gameObject);

            isflippingOpen=true;
            StartCoroutine(FlipOpen());
            button.interactable = false; // make sure you dont click twice
        }
        else if (CardManager.instance.choice2 == 0)
        {
            CardManager.instance.choice2 = id;
            CardManager.instance.AddChosenCard(this.gameObject);

            isflippingOpen = true;
            StartCoroutine(FlipOpen());
            button.interactable = false; // make sure you dont click twice

            // compare the cards
            StartCoroutine(CardManager.instance.CompareCards());
        }
    }
    IEnumerator FlipOpen()
    {
        while (isflippingOpen && flipAmount > 0)
        {
            flipAmount -= Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount <= 0)
            {
                image.sprite = cardfront;
                isflippingOpen = false;
                isflippingClose = true;
            }
            yield return null;
        }

        while (isflippingClose && flipAmount < 1)
        {
            flipAmount += Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount >= 1)
            {

                isflippingClose = false;
            }

            yield return null;
        }
    }
    IEnumerator FlipClose()
    {
        while (isflippingOpen && flipAmount > 0f)
        {
            flipAmount -= Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount <= 0)
            {
                image.sprite = cardback;
                isflippingOpen = false;
                isflippingClose = true;
            }
            yield return null;
        }

        while (isflippingClose && flipAmount < 1f)
        {
            flipAmount += Time.deltaTime * flipSpeed;
            flipAmount = Mathf.Clamp01(flipAmount);
            transform.localScale = new Vector3(flipAmount, transform.localScale.y, transform.localScale.z);

            if (flipAmount >= 1)
            {

                isflippingClose = false;
            }

            yield return null;
        }
        button.interactable = true;
    }
    public void CloseCard()
    {
        isflippingOpen = true;
        StartCoroutine(FlipClose());
    }
    // flipping card speed 
    // close card 
}
