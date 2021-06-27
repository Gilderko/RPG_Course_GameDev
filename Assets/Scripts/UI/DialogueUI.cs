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
        [SerializeField] GameObject AIResponse;
        [SerializeField] GameObject choiceHide;
        [SerializeField] Transform choicesParent;
        [SerializeField] GameObject choicePrefarb;
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI conversantName;

        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => { playerConversant.Next(); }); // Actions but with different syntax           
            quitButton.onClick.AddListener(() => { playerConversant.Quit(); });

            UpdateUI();
        }       

        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsDialogueActive());
            if (!playerConversant.IsDialogueActive())
            {
                return;
            }
            conversantName.text = playerConversant.GetCurrentConversantName();
            AIResponse.SetActive(!playerConversant.IsChoosing());
            choiceHide.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            foreach (Transform item in choicesParent)
            {
                Destroy(item.gameObject);
            }

            foreach (DialogueNode choiceNode in playerConversant.GetChoices())
            {
                GameObject option = Instantiate(choicePrefarb, choicesParent);
                option.GetComponentInChildren<TextMeshProUGUI>().text = choiceNode.GetText();
                Button button = option.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => 
                {
                    playerConversant.SelectChoice(choiceNode); // Lambda function
                });
            }
        }
    }
}
