using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

public class ThirdTalk : MonoBehaviour
{
    public TextMeshProUGUI firstText;
    public TextMeshProUGUI dialogueText;
    public ThirdDialogue thirdDialogue;
    public GameObject playerObject;
    public GameObject enemyObject;
    public Image subtitleImage;
    public GameObject playerFash;
    public string firstMessage = "";
    public ThirdMove thirdMove;
    public float typingSpeed = 0.1f;

    private bool isDialogueActive = false;
    private bool isFirstTextCompleted = false;
    private bool isTyping = false;
    private int currentDialogueIndex = 0;
    private List<string[]> dialogues;
    private Coroutine dialogueCoroutine;

    void Start()
    {
        if (thirdDialogue == null || dialogueText == null || playerObject == null || enemyObject == null)
        {
            Debug.LogError("Missing required component references.");
        }
        dialogues = thirdDialogue.GetDialogues();
        StartCoroutine(TypeFirst());
        playerFash.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isFirstTextCompleted)
            {
                if (!isDialogueActive)
                {
                    HideFirst();
                    thirdMove.StartMoving();
                }
                else if (!isTyping)
                {
                    if (currentDialogueIndex >= dialogues.Count)
                    {
                        // ダイアログが全て終了したのでシーンを切り替える
                        SceneManager.LoadScene("3rd Game"); // "NextSceneName" を次のシーン名に変更
                    }
                    else
                    {
                        NextDialogue();
                    }
                }
            }
        }
    }

    IEnumerator TypeFirst()
    {
        firstText.text = "";
        subtitleImage.enabled = true;

        foreach (char letter in firstMessage.ToCharArray())
        {
            firstText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isFirstTextCompleted = true;
    }

    void HideFirst()
    {
        firstText.text = "";
        subtitleImage.enabled = false;
        playerFash.SetActive(false);
    }

    public void TriggerDialogue()
    {
        isDialogueActive = true;
        currentDialogueIndex = 0;

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        NextDialogue();
    }

    void NextDialogue()
    {
        if (isTyping || currentDialogueIndex >= dialogues.Count) return;

        string[] dialogue = dialogues[currentDialogueIndex];
        if (dialogue.Length < 2)
        {
            Debug.LogWarning("Invalid dialogue format: " + string.Join(",", dialogue));
            currentDialogueIndex++;
            return;
        }

        string character = dialogue[0];
        string message = dialogue[1];

        if (dialogueCoroutine != null)
        {
            StopCoroutine(dialogueCoroutine);
        }
        dialogueCoroutine = StartCoroutine(TypeText(character, message));
        currentDialogueIndex++;
    }

    IEnumerator TypeText(string character, string message)
    {
        isTyping = true;
        subtitleImage.enabled = true;
        dialogueText.text = "";

        playerObject.SetActive(character == "Player");
        enemyObject.SetActive(character == "Enemy");

        foreach (char letter in message.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
