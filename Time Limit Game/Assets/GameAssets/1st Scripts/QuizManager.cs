using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
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
        "ハリセンボンはその名前の通り\n実際に約1000本の針を持っている。〇か×か",
        "月には空気が無い。〇か×か",
        "日本に一番近い国は、韓国である。\n〇か×か。",
        "マグロは1時間だけ止まって睡眠をとる。\n〇か×か。"
    };

    private bool[] answers = { false, true, false, false};

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

        if (correctCount >= 1)
        {
            GoToClearScene();
        }
        else
        {
            foreach (GameObject obj in objectsToHide)
            {
                obj.SetActive(true);
            }

            SelectNewQuestion();
            ShowQuestion();
        }       
    }

    void GoToClearScene()
    {
        SceneManager.LoadScene("1st Clear");
    }
}
