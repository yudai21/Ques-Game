using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ThirdDialogue : MonoBehaviour
{
    public string fileName = "";
    private List<string[]> dialogues = new List<string[]>();

    void Start()
    {
        LoadDialogue();
    }

    void LoadDialogue()
    {
        TextAsset csvFile = Resources.Load<TextAsset>(fileName);

        if (csvFile != null)
        {
            StringReader reader = new StringReader(csvFile.text);
            bool isFirstLine = true;  // 一行目をスキップするためのフラグ

            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();

                if (isFirstLine)
                {
                    isFirstLine = false;  // 一行目をスキップ
                    continue;
                }

                string[] entries = line.Split(',');
                dialogues.Add(entries);
            }
        }
        else
        {
            Debug.LogError("CSV file not found");
        }
    }

    public List<string[]> GetDialogues()
    {
        return dialogues;
    }
}
