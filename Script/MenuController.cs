using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{

    [Header("Volume Setting")]
    [SerializeField] private TMP_Text volumeTextValue = null;



    [Header("Levels To Load")]
    public string _newGameLevel;
    private string leveltoload;
    
    //Gets the scene that needs to be loaded//

    public void NewGameDialogYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }

    //Quits The Game//
    public void ExitButton()
    {
        Application.Quit();
    }

    //volume setter//

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }

   
}
