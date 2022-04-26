using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SceneChanger : MonoBehaviour
{
	public void Update()
	{
        if(Input.GetKeyDown(KeyCode.Keypad1)||Input.GetKeyDown(KeyCode.Alpha1)){
            SceneManager.LoadScene("Detection_Menu");
        } else if(Input.GetKeyDown(KeyCode.Keypad2)||Input.GetKeyDown(KeyCode.Alpha2)){
            SceneManager.LoadScene("Detection_Hand_Interface");
        } else if(Input.GetKeyDown(KeyCode.Keypad3)||Input.GetKeyDown(KeyCode.Alpha3)){
            SceneManager.LoadScene("Detection_Virtual_Grasp");
        } else if(Input.GetKeyDown(KeyCode.Keypad4)||Input.GetKeyDown(KeyCode.Alpha4)){
            SceneManager.LoadScene("Interaction_Baseline");
        } else if(Input.GetKeyDown(KeyCode.Keypad5)||Input.GetKeyDown(KeyCode.Alpha5)){
            SceneManager.LoadScene("Interaction_Hand_Interface");
        } else if(Input.GetKeyDown(KeyCode.Keypad6)||Input.GetKeyDown(KeyCode.Alpha6)){
            SceneManager.LoadScene("Interaction_Virtual_Grasp");
        } else if(Input.GetKeyDown(KeyCode.Keypad7)||Input.GetKeyDown(KeyCode.Alpha7)){
            SceneManager.LoadScene("HandInterfaceDemo");
        }

		
	}
	// public void Exit()
	// {
	// 	Application.Quit ();
	// }
}