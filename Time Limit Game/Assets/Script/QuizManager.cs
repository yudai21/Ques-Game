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
    public GameObject correctObject; //〇のゲームオブジェクト
    public GameObject incorrectObject; //×のゲームオブジェクト
    public GameObject cursorObject;

    public Vector3 correctPosition; //〇のカーソル位置
    public Vector3 incorrectPosition; //×のカーソル位置

    private string[] questions = 
    {
        "ハリセンボンはその名前の通り\n実際に約1000本の針を持っている。〇か×か",
        "月には空気が無い。〇か×か",
        "日本に一番近い国は、韓国である。〇か×か。",
        "マグロは1時間だけ、止まって睡眠をとる。〇か×か。"
    };

    private bool[] answers = { false, true, };

    private string[] explanationScenes =
    {
        // "ExplanationScene1",
        // "ExplanationScene2",
        // "ExplanationScene3",
        // "ExplanationScene4"
    };
    private int currentQuestionIndex;
    private bool isSelectedTrue = true; //〇が選択されているかどうか

    void Start()
    {
        currentQuestionIndex = Random.Range(0, questions.Length);
        ShowQuestion();
        UpdateSelectionImage();
    }

    void Update()
    {
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

    void ShowQuestion()
    {
        questionText.text = questions[currentQuestionIndex];
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
            Debug.Log("aaa");
            GotoExplanationScene();
        }
        else
        {
            Debug.Log("bbb");
        }
    }

    void GotoExplanationScene()
    {
        SceneManager.LoadScene(explanationScenes[currentQuestionIndex]);
    }
}
