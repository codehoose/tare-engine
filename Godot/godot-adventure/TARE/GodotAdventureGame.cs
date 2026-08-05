using System.Diagnostics;
using System.Linq;
using Godot;
using GodotAdventure.TARE;
using TareEngine;
using TareEngine.Models;
using TareEngine.Parser;
using TareEngine.States;
using AdventureGameEngine = TareEngine.Engine;

public partial class GodotAdventureGame : Node, IAdventureGame
{
	private AdventureGameEngine _engine;
	private string _currentGraphic = "";
	private StateMachine StateMachine { get; set; }

	[Export]
	private RichTextLabel roomDescription;
	[Export]
	private TextureRect roomPicture;
	[Export]
	private LineEdit commandText;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		commandText.TextSubmitted += Text_Entered;

		StateMachine = new StateMachine(this);
		_engine = new AdventureGameEngine();
		var dataProvider = new GameDataDeserializer();
		_engine.Init(dataProvider);
		StateMachine.EnterState(InitGameState.Instance);
	}

	private void Text_Entered(string newText)
	{
		if (string.IsNullOrEmpty(newText)) return;

		WriteLine("");
		// PARSE INTO WORDS ....
		var result = _engine.Parse(newText);
		if (HasGraphicChanged())
		{
			ShowGraphic();
		}
		switch (result)
		{
			case ParserResult.ChangeRoom:
				ClearGraphic();
				StateMachine.EnterState(DescribeRoomState.Instance);
				break;
			case ParserResult.Error:
				string error = GetLastError();
				WriteLine(error);
				break;
			case ParserResult.CannotSeeItem:
				WriteLine(_engine.LastError);
				break;
			case ParserResult.ShowLastMessage:
				WriteLine(_engine.LastMessage);
				break;
			case ParserResult.DescribeRoom:
				DescribeRoom(true);
				break;
		}

		commandText.Text = ""; // clear the buffer for n+1 actions in same location
	}

	private void Write(string msg) => roomDescription.Text += msg;
	private void WriteLine(string msg) => Write(msg + "\n");

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Init()
	{
		
	}

	public void ClearGraphic()
	{
		
	}

	public void DescribeRoom(bool isLook = false)
	{
		Debug.WriteLine("Hello from Describing Room");
		ShowGraphic();
		roomDescription.Text = _engine.CurrentRoom.Description + "\n\n";
		DescribeItems();
		DescribeExits();
		commandText.GrabFocus();
	}

	private void DescribeItems()
	{
		if (_engine.CurrentRoom.Items.Count == 0) return;
		Write("You can see: ");
		var items = _engine.CurrentRoom.Items.Where(i => (i.Flags & ObjectFlags.Hidden) != ObjectFlags.Hidden).Select(i => i.Name).ToArray();
		WriteLine(GetJoined(items));
	}

	private void DescribeExits()
	{
		Write("Exits: ");
		var exits = _engine.GetExits();
		WriteLine(GetJoined(exits));
	}

	public void WhatNext()
	{
		
	}

	public void ClearTerminal()
	{
		
	}

	public void ClearKeyboard()
	{
		
	}

	public void ToggleKeyboard(bool enableKeyboard)
	{
		
	}
	
	private string GetJoined(string[] items, string multiple = ", ", string twoItems = " and ")
	{
		var joiner = items.Length > 2 ? ", " : " and ";
		return string.Join(joiner, items);
	}
	
	
	private string GetLastError()
	{
		var lastError = _engine.LastError;
		if (string.IsNullOrEmpty(lastError)) return "I didn't understand that";
		return lastError;
	}
	
	private bool ShowGraphic()
	{
		var graphicName = "res://" + _engine.CurrentRoom.Graphic + ".png";
		if (!string.IsNullOrEmpty(_engine.CurrentRoom.GraphicFlag))
		{
			var index = _engine.Flags.GetValue(_engine.CurrentRoom.GraphicFlag);
			graphicName = "res://" + _engine.CurrentRoom.GetGraphic(index) + ".png";
		}

		if (!_engine.CurrentRoom.HasGraphic) return false;

		if (_currentGraphic == graphicName) return true;

		ClearGraphic();

		var tex = GD.Load<Texture2D>(graphicName);
		roomPicture.Texture = tex;

		return true;
	}	
	
	private bool HasGraphicChanged()
	{
		string graphicName = _engine.CurrentRoom.Graphic;
		if (!string.IsNullOrEmpty(_engine.CurrentRoom.GraphicFlag))
		{
			int index = _engine.Flags.GetValue(_engine.CurrentRoom.GraphicFlag);
			graphicName = _engine.CurrentRoom.GetGraphic(index);
		}
		graphicName = "res://" + graphicName + ".png";
		
		return graphicName != _currentGraphic;
	}
}
