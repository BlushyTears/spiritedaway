using Godot;
using System;

public partial class IntroMenu : CanvasLayer
{
	[Export]
	public Button _btnPlay;
	[Export]
	public Button _btnQuit;

	private void OnBtnStartPressed()
	{
		GD.Print("play");
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Visible = false;
		GameController._introMenuOpen = false;
	}

	private void OnBtnQuitPressed()
	{
		//GD.Print("quit");
		//GetTree().Free();
	}
}
