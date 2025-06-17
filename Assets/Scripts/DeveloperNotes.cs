using UnityEngine;
using System.IO;

public class DeveloperNotes : MonoBehaviour
{

    [TextArea(3, 10)]
    public string TODO_Notes;
    
    [TextArea(3, 10)]
    public string developerNotes;

    private string filePath1;
    private string filePath2;

    //Called when the script is loaded or when the value of any inspector field is changed
    private void OnValidate()
    {
        //Defines the path to the file to save the notes to
        filePath1 = Application.dataPath + "/TODO_Notes.txt";
        filePath2 = Application.dataPath + "/DeveloperNotes.txt";

        //Write the notes to the file
        SaveNotesToFile();
        
    }

    //method to write the notes to the text file
    private void SaveNotesToFile()
    {
        try
        {
            //write the content to file, overwriting its current content
            File.WriteAllText(filePath1, TODO_Notes);
            File.WriteAllText(filePath2, developerNotes);
            Debug.Log("TODO_notes saved to: " + filePath1);
            Debug.Log("DeveloperNotes saved to: " + filePath2);
        } catch (System.Exception e)
        {
            Debug.LogError("Error writing to file: " + e.Message);
        }
    }
}
