using System;
using TareEngine;
using TareEngine.Parser;
using TareEngine.States;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdventureGame : MonoBehaviour, IAdventureGame
{
    
    private Engine _engine;
    public StateMachine StateMachine { get; private set; }


    [SerializeField] private RoomImages roomImages;
    [SerializeField] private Image roomImage;
    [SerializeField] private TMPro.TMP_Text descriptionText;
    [SerializeField] private TMPro.TMP_InputField commandInput;
    
    
    private void Awake()
    {
        StateMachine = new StateMachine(this);
        _engine = new Engine();
        _engine.Init(GetComponent<GameDataProvider>());
        StateMachine.EnterState(InitGameState.Instance);

        commandInput.onSubmit.AddListener(Command_Entered);
    }

    private void Command_Entered(string command)
    {
        commandInput.text = "";
        var result = _engine.Parse(command);
        
        if (HasGraphicChanged())
        {
            ShowGraphic();
        }
        
        switch (result)
        {
            case ParserResult.ChangeRoom:
                StateMachine.EnterState(DescribeRoomState.Instance);
                break;
            case ParserResult.Error:
                string error = GetLastError();
                descriptionText.text += "\n" + error;
                break;
            case ParserResult.CannotSeeItem:
                descriptionText.text += "\n" + _engine.LastError;
                break;
            case ParserResult.ShowLastMessage:
                descriptionText.text += "\n" + _engine.LastMessage;
                break;
            case ParserResult.DescribeRoom:
                DescribeRoom(true);
                break;
        }        

        if (commandInput.enabled)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(commandInput.gameObject);
        }
    }
    
    private string GetLastError()
    {
        var lastError = _engine.LastError;
        if (string.IsNullOrEmpty(lastError)) return "I didn't understand that";
        return lastError;
    }    
    
    public void Init()
    {
        // Was used for keyboard setup
    }

    public void ClearGraphic()
    {
        roomImage.sprite = null;
        descriptionText.text = "";
    }

    private string GetRoomGraphic()
    {
        var slug = _engine.CurrentRoom.Graphic;
        
        if (!_engine.CurrentRoom.HasGraphic) return "";

        if (slug.StartsWith("backgrounds/"))
        {
            return slug.Substring("backgrounds/".Length);
        }

        return slug;
    }

    private void ShowGraphic()
    {
        var tex = roomImages.GetImage(GetRoomGraphic());
        roomImage.sprite = tex ? Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero) : null;
    }
    
    public void DescribeRoom(bool isLook = false)
    {
        descriptionText.text = _engine.CurrentRoom.Description;
        if (_engine.CurrentRoom.HasGraphic)
        {
            ShowGraphic();
        }
        else
        {
            roomImage.sprite = null;
        }
        
        // Describe items and exits
    }

    public void WhatNext()
    {
        
    }

    public void ClearTerminal()
    {
        descriptionText.text = "";
    }

    public void ClearKeyboard()
    {
        
    }

    public void ToggleKeyboard(bool enableKeyboard)
    {
        commandInput.enabled = enableKeyboard;
        if (enableKeyboard)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(commandInput.gameObject);
        }
    }
    
    private bool HasGraphicChanged()
    {
        string graphicName = _engine.CurrentRoom.Graphic;
        if (!string.IsNullOrEmpty(_engine.CurrentRoom.GraphicFlag))
        {
            int index = _engine.Flags.GetValue(_engine.CurrentRoom.GraphicFlag);
            graphicName = _engine.CurrentRoom.GetGraphic(index);
        }

        string currentGraphicName = roomImage?.sprite?.name ?? string.Empty;
        return graphicName != currentGraphicName;
    }
}
