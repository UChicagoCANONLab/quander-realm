using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Wrapper;

public class DebugScreen : MonoBehaviour
{
    private enum Option
    {
        Add_Reward,
        Toggle_Loading_Scr,
        Open_Dialog,
        Close_Dialog_UI,
        Clear_Save_File,
        BB_Goto_Level,
        BB_Toggle_Debug,
        BB_Clear_Markers
    };

    public GameObject toggleContainer;
    public GameObject togglePrefab;
    public TMP_InputField inputField;
    public Button submitButton;
    public TextMeshProUGUI consoleLog;

    private Option currentOption;
    
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
        Application.logMessageReceived += UpdateLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= UpdateLog;
    }

    private void UpdateLog(string condition, string stackTrace, LogType type)
    {
        if (condition.Contains("[BeauRoutine]"))
            return;

        consoleLog.text = condition;

        Color color;
        switch (type)
        {
            case LogType.Error:
            case LogType.Exception:
                color = Color.red;
                break;
            case LogType.Warning:
                color = Color.yellow;
                break;
            default:
                color = Color.white;
                break;
        }
        consoleLog.color = color;
    }

    private void SetupOptions()
    {
        foreach (Option option in Enum.GetValues(typeof(Option)))
        {
            GameObject toggleGO = Instantiate(togglePrefab, toggleContainer.transform);
            toggleGO.GetComponent<Toggle>().onValueChanged.AddListener((isOn) => SetCurrentOption(option, isOn));
            toggleGO.GetComponent<Toggle>().group = toggleContainer.GetComponent<ToggleGroup>();
            toggleGO.GetComponentInChildren<TextMeshProUGUI>().text = option.ToString();
        }
    }

    private void SubmitCommand()
    {
        string fieldText = inputField.text.Trim();

        switch(currentOption)
        {
            case Option.Toggle_Loading_Scr: Events.ToggleLoadingScreen?.Invoke(); break;            
            case Option.Add_Reward:         Events.AddReward?.Invoke(fieldText); break;
            case Option.Open_Dialog:        Events.StartDialogueSequence?.Invoke(fieldText); break;
            case Option.Close_Dialog_UI:    Events.CloseDialogueView?.Invoke(); break;
            case Option.Clear_Save_File:    Events.ClearSaveFile?.Invoke(); break;
            case Option.BB_Goto_Level:      Events.BBGotoLevel?.Invoke(fieldText); break;
            case Option.BB_Toggle_Debug:    Events.BBToggleDebug?.Invoke(); break;
            case Option.BB_Clear_Markers:   Events.BBClearMarkers?.Invoke(); break;
        }

        Debug.LogFormat("Entered Command: <b>{0}</b> with parameter <b>{1}</b>", currentOption.ToString(), fieldText);
        //gameObject.SetActive(false);
    }

    private void SetCurrentOption(Option option, bool isOn)
    {
        if (!(isOn))
            return;

        currentOption = option;
    }
}
