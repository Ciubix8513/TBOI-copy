using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{

    public void SetSeed( InputField field) 
    {
        var s = field.text;
        try
        {
            var i = int.Parse(s);
            SeedSetting.seed = i;
        }
        catch (System.Exception)
        {
            field.text = "";            
        }        
    }

    public void LoadRecords()=>UnityEngine.SceneManagement.SceneManager.LoadScene("Records",LoadSceneMode.Single);
    public static void LoadGame(int depth) 
    {
        SeedSetting.Depth = depth;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game",LoadSceneMode.Single);
    }
    public  void QuitToMainMenu() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main menu", LoadSceneMode.Single);
    }
    public void QuitGame() => Application.Quit();
    
}
