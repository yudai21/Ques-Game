using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TalkController : MonoBehaviour
{
    public TextMeshProUGUI firstText; // TextMeshProのUIコンポーネントをアタッチ
    public Image subtitleImage; // Imageコンポーネントをアタッチ
    public GameObject playerFash;
    public string firstMessage = ""; // 表示したい字幕を指定
    public PlayerMovement playerMovement; // PlayerMovementスクリプトをアタッチ
    public float typingSpeed = 0.1f; // 文字を表示する速度
    void Start()
    {
        // シーンが開始されたときに字幕を表示
        StartCoroutine(TypeFirst());
        playerFash.gameObject.SetActive(true);
    }

    void Update()
    {
        // Enterキーが押されたときに字幕を非表示にし、プレイヤーを動かす
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HideFirst();
            playerMovement.StartMoving(); // プレイヤーの動きを開始
            StopAllCoroutines(); // コールチンを停止
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
    }

    void HideFirst()
    {
        firstText.text = ""; // 字幕を非表示
        subtitleImage.enabled = false; // 画像を非表示
        playerFash.gameObject.SetActive(false);
    }
}