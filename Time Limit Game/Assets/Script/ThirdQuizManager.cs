using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ThirdQuizManager : MonoBehaviour
{
    public TMP_InputField questionText;
    public GameObject correctObject; // 〇のゲームオブジェクト
    public GameObject incorrectObject; // ×のゲームオブジェクト
    public GameObject cursorObject;
    public TMP_Text timerText; // 制限時間表示用のText

    public Vector3 correctPosition; // 〇のカーソル位置
    public Vector3 incorrectPosition; // ×のカーソル位置
    public GameObject[] objectsToHide; // 非表示にしたいオブジェクトの配列
    public TMP_InputField explanationPanel;
    public TMP_Text[] explanationTexts;

    private string[] questions = 
    {
        "日本で一番収穫されている果物は\nりんごである。〇か×か",
        "地球の自転速度は変わらない。〇か×か",
        "お寿司屋さんでは、醤油のことを「くろ」と呼ぶ。〇か×か",
        "オムライスは日本で生まれた。〇か×か",
        "シロクマの毛は白い。〇か×か",
        "ライターとマッチ、先に発明されたのは\nマッチである。〇か×か",
        "東京ディズニーランドは東京にある。〇か×か",
        "パイロットと副操縦士の機内食は違う。〇か×か",
        "ロダン作の彫刻「考える人」は\n自信の罪について考えている。〇か×か"
    };

    private bool[] answers = { false, false, false, true, false, false, false, true, false};

    private int currentQuestionIndex;
    private bool isSelectedTrue = true; // 〇が選択されているかどうか
    private int correctCount = 0;
    private List<int> usedQuestions = new List<int>(); // 出題済みの問題インデックスを保持するリスト

    private float timeLimit = 15f; // 15秒の制限時間
    private float currentTime;
    private bool isTimerRunning = true; // タイマーが動作中かどうかを示すフラグ

    void Start()
    {
        // すべての解説Textを非表示にしておく
        foreach (TMP_Text text in explanationTexts)
        {
            text.gameObject.SetActive(false);
        }
        explanationPanel.gameObject.SetActive(false);

        SelectNewQuestion();
        ShowQuestion();
        UpdateSelectionImage();
        ResetTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            UpdateTimer();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            isSelectedTrue = true;
            UpdateSelectionImage();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            isSelectedTrue = false;
            UpdateSelectionImage();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer(isSelectedTrue);
        }
    }

    void ResetTimer()
    {
        currentTime = timeLimit;
        isTimerRunning = true;
        UpdateTimerDisplay();
    }

    void UpdateTimer()
    {
        currentTime -= Time.deltaTime;
        UpdateTimerDisplay();

        if (currentTime < 0)
        {
            TimeUp();
        }
    }

    void UpdateTimerDisplay()
    {
        timerText.text = "" + Mathf.Ceil(currentTime).ToString();
    }

    void TimeUp()
    {
        Debug.Log("時間切れ");
    }

    void SelectNewQuestion()
    {
        List<int> availableQuestions = new List<int>();

        // まだ出題されていない問題をリストに追加
        for (int i = 0; i < questions.Length; i++)
        {
            if (!usedQuestions.Contains(i))
            {
                availableQuestions.Add(i);
            }
        }

        if (availableQuestions.Count > 0)
        {
            // ランダムに新しい問題を選択
            currentQuestionIndex = availableQuestions[Random.Range(0, availableQuestions.Count)];
            usedQuestions.Add(currentQuestionIndex); // 選択した問題を出題済みリストに追加
        }
        else
        {
            Debug.Log("すべての問題が出題されました。");
        }
    }

    void ShowQuestion()
    {
        questionText.text = questions[currentQuestionIndex];
        ResetTimer();
    }

    void UpdateSelectionImage()
    {
        if (isSelectedTrue)
        {
            correctObject.GetComponent<SpriteRenderer>().color = Color.white; //選択されている〇の画像を強調表示
            incorrectObject.GetComponent<SpriteRenderer>().color = Color.gray; //選択されていない×の画像を薄く表示

            cursorObject.transform.position = correctPosition;
        }
        else
        {
            correctObject.GetComponent<SpriteRenderer>().color = Color.gray; //選択されていない〇の画像を薄く表示
            incorrectObject.GetComponent<SpriteRenderer>().color = Color.white; //選択されている×の画像を強調表示

            cursorObject.transform.position = incorrectPosition;
        }
    }

    void CheckAnswer(bool playerAnswer)
    {
        if (playerAnswer == answers[currentQuestionIndex])
        {
            correctCount++;
            ShowExplanation();
            isTimerRunning = false; // 正解時にタイマーをストップ
        }
        else
        {
            Debug.Log("不正解です。");
        }
    }  

    void ShowExplanation()
    {
        foreach (GameObject obj in objectsToHide)
        {
            obj.SetActive(false);
        }

        explanationTexts[currentQuestionIndex].gameObject.SetActive(true); // 正解した問題に対応する解説を表示
        explanationPanel.gameObject.SetActive(true);
        StartCoroutine(HideExplanationAfterDelay(5f)); // 5秒後に非表示にする
    }

    IEnumerator HideExplanationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        explanationTexts[currentQuestionIndex].gameObject.SetActive(false); // 解説テキストを非表示にする
        explanationPanel.gameObject.SetActive(false);

        if (correctCount < 3)
        {
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(true);
            }

            // 新しい問題を出題
            SelectNewQuestion();
            ShowQuestion();
        }
        else
        {
            GoToClearScene();
        }
    }

    void GoToClearScene()
    {
        SceneManager.LoadScene("3rd Clear");
    }
}