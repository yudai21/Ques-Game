using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    public string fileName = "Dialogue";
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
            while (reader.Peek() > -1)
            {
                string line = reader.ReadLine();
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
