using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Button nextButton;

        [SerializeField] Transform choiceRoot;
        [SerializeField] Transform choicesParent;

        [SerializeField] GameObject choicePrefarb;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next); // Actions but with different syntax
            UpdateUI();
        }

        private void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            AIText.text = playerConversant.GetText();
            nextButton.gameObject.SetActive(playerConversant.HasNext());
            foreach (Transform item in choicesParent)
            {
                Destroy(item.gameObject);
            }
            foreach (string choiceText in playerConversant.GetChoices())
            {
                GameObject option = Instantiate(choicePrefarb, choicesParent);
                option.GetComponentInChildren<TextMeshProUGUI>().text = choiceText;
            }
        }


    }
}
