using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.HighLevel.UI;
using lb = Sce.PlayStation.HighLevel.UI;
namespace Pong
{
	public class MenuScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene m_uiScene;
		bool localMultiplayerBool;
		
		public MenuScene ()
		{
			this.Camera.SetViewFromViewport();
			Panel dialog = new Panel();
			dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
			dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			ImageBox ib = new ImageBox();
			ib.Width = dialog.Width;
			ib.Image = new ImageAsset("/Application/images/title.png", false);
			ib.Height = dialog.Height;
			ib.SetPosition(0.0f, 0.0f);
			
			Button buttonPlay = new Button();
			buttonPlay.Name = "buttonPlay";
			buttonPlay.Text = "Play Game";
			buttonPlay.Width = 300;
			buttonPlay.Height = 50;
			buttonPlay.Alpha = 0.8f;
			buttonPlay.SetPosition(dialog.Width/2.0f - buttonPlay.Width/2.0f, 220.0f); 
			buttonPlay.TouchEventReceived += OnButtonPlay;
			
			Button localMultiplayer = new Button();
			localMultiplayer.Name = "localMultiplayer";
			localMultiplayer.Text = "Local Multiplayer";
			localMultiplayer.Width = 300;
			localMultiplayer.Height = 50;
			localMultiplayer.Alpha = 0.8f;
			localMultiplayer.SetPosition(dialog.Width/2.0f - buttonPlay.Width/2.0f, 300.0f); 
			localMultiplayer.TouchEventReceived += OnMultiplayerPlay;
			
			// retrieve highscores
			// retrieve highscore from server
			Client m_client = new Client(new string[1] {"Highscorers1234567890"});
			
			// check if server init
			if (m_client.responseMessage == "ERROR: no entries found")
			{
				m_client = new Client(new string[2] {"Highscorers1234567890", "Sean,10,Bob,5,Fred,4,Will,3,Jim,1"});
			}
			
			string[] highScorers = new string[10];
			string[] sectionsMessage = m_client.responseMessage.Split(' ');			
			highScorers = sectionsMessage[sectionsMessage.Length - 1].Split(',');
			string labelText = 	"Highscores" + Environment.NewLine +
								highScorers[0] + ": " + highScorers[1] + Environment.NewLine + 
								highScorers[2] + ": " + highScorers[3] + Environment.NewLine +
								highScorers[4] + ": " + highScorers[5] + Environment.NewLine +
								highScorers[6] + ": " + highScorers[7] + Environment.NewLine +
								highScorers[8] + ": " + highScorers[9] + Environment.NewLine;
			
			lb.Label highscores = new lb.Label();
			highscores.Name = "highscores";
			highscores.Text = labelText;
			highscores.Width = 300;
			highscores.Height = 400;
			highscores.Alpha = 0.8f;
			highscores.SetPosition(50, 220.0f);			
						
			dialog.AddChildLast(ib);
			dialog.AddChildLast(buttonPlay);
			dialog.AddChildLast(highscores);
			dialog.AddChildLast(localMultiplayer);
			m_uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			m_uiScene.RootWidget.AddChildLast(dialog);
			m_uiScene.RootWidget.AddChildLast(highscores);
			m_uiScene.RootWidget.AddChildLast(localMultiplayer);
			UISystem.SetScene(m_uiScene);
			Scheduler.Instance.ScheduleUpdateForTarget(this, 0, false);
		}
		
		public void OnButtonPlay(object sender, TouchEventArgs e)
		{
			Director.Instance.ReplaceScene(new GameScene(localMultiplayerBool));
		}
		
		public void OnMultiplayerPlay(object sender, TouchEventArgs e)
		{
			localMultiplayerBool = true;
			Director.Instance.ReplaceScene(new GameScene(localMultiplayerBool));
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			UISystem.Update(Touch.GetData(0));
		}
		
		public override void Draw ()
		{
			base.Draw();
			UISystem.Render ();
		}
		
		~MenuScene()
		{
			
		}
	}
}

