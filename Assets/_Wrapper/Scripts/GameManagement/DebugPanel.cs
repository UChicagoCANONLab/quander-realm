using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Wrapper;

public class DebugPanel : MonoBehaviour
{
    private enum Option
    {
        Add_Reward,
        ToggleLoadingScreen,
        Open_Dialog_UI,
        Close_Dialog_UI,
        BB_Goto_Level,
        BB_Toggle_Debug,
        BB_Clear_Markers
    };

    public TMP_Dropdown dropDown;
    public TMP_InputField inputField;
    public Button submitButton;
    
    private void Awake()
    {
#if PRODUCTION_FB
        gameObject.SetActive(false);
#endif
        SetupOptions();
        submitButton.onClick.AddListener(SubmitCommand);
    }

    private void OnEnable()
    {
        inputField.text = "";
    }

    private void SetupOptions()
    {
        List<TMP_Dropdown.OptionData> optionsList = new List<TMP_Dropdown.OptionData>();
        foreach (Option option in Enum.GetValues(typeof(Option)))
            optionsList.Add(new TMP_Dropdown.OptionData(option.ToString()));

        dropDown.AddOptions(optionsList);
    }

    private void SubmitCommand()
    {
        string fieldText = inputField.text.Trim();
        string dropDownText = dropDown.captionText.text;
        Option commandType = Enum.Parse<Option>(dropDownText);

        switch(commandType)
        {
            case Option.ToggleLoadingScreen:
                ToggleLoadingScreen();
                break;            
            case Option.Add_Reward:
                AddReward(fieldText);
                break;
            case Option.Open_Dialog_UI:
                OpenDialogueSequence(fieldText);
                break;
            case Option.Close_Dialog_UI:
                CloseDialogueUI();
                break;
            case Option.BB_Goto_Level:
                Events.BBGotoLevel?.Invoke(fieldText);
                break;
            case Option.BB_Toggle_Debug:
                Events.BBToggleDebug?.Invoke();
                break;
            case Option.BB_Clear_Markers:
                Events.BBClearMarkers?.Invoke();
                break;
        }


        Debug.LogFormat("Entered Command {0} {1}", dropDownText, fieldText);
        gameObject.SetActive(false);
    }

    static private void OpenDialogueSequence(string sequenceID)
    {
        Events.StartDialogueSequence?.Invoke(sequenceID);
    }

    static private void CloseDialogueUI()
    {
        Events.CloseDialogueView?.Invoke();
    }

    static private void AddReward(string rewardID)
    {
        Events.AddReward?.Invoke(rewardID);
    }

    static private void ToggleLoadingScreen()
    {
        Events.ToggleLoadingScreen?.Invoke();
    }
}
