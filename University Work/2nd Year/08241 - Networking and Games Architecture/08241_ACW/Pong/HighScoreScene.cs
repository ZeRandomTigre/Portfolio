using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using lb = Sce.PlayStation.HighLevel.UI;
using Sce.PlayStation.HighLevel.UI;

namespace Pong
{
	public class HighScoreScene : Sce.PlayStation.HighLevel.GameEngine2D.Scene
	{
		private Sce.PlayStation.HighLevel.UI.Scene m_uiScene;
		private EditableText editTextBox = new EditableText();
		private Client m_client;
		private int m_highscore;
		
		public HighScoreScene (int highscore)
		{
			m_highscore = highscore;
			
			this.Camera.SetViewFromViewport();
			Sce.PlayStation.HighLevel.UI.Panel dialog = new Panel();
			dialog.Width = Director.Instance.GL.Context.GetViewport().Width;
			dialog.Height = Director.Instance.GL.Context.GetViewport().Height;
			
			ImageBox ib = new ImageBox();
			ib.Width = dialog.Width;
			ib.Image = new ImageAsset("/Application/images/title.png", false);
			ib.Height = dialog.Height;
			ib.SetPosition(0.0f, 0.0f);
			
			// editable text box parameters
			editTextBox.Name = "editText";
			editTextBox.Text = "You got a new high score, Enter your name (do not enter ',' or spaces)";
			editTextBox.Width = 400;
			editTextBox.Height = 100;
			editTextBox.SetPosition(dialog.Width/2.0f - editTextBox.Width/2.0f, 220.0f);
			
			// retrieve highscores
			// retrieve highscore from server
			Client m_client = new Client(new string[1] {"Highscorers1234567890"});
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
			
			// when text box changed - fire event ( change player name and replace scene )
			editTextBox.TextChanged += OnTextEdit;			
						
			dialog.AddChildLast(ib);
			dialog.AddChildLast(editTextBox);
			dialog.AddChildLast(highscores);
			
			m_uiScene = new Sce.PlayStation.HighLevel.UI.Scene();
			m_uiScene.RootWidget.AddChildLast(dialog);
			m_uiScene.RootWidget.AddChildLast(highscores);
			
			UISystem.SetScene(m_uiScene);
			Scheduler.Instance.ScheduleUpdateForTarget(this, 0, false);
		}
		
		public void OnTextEdit(object sender, TextChangedEventArgs e)
		{
			
			// retrieve highscore from server
			m_client = new Client(new string[1] {"Highscorers1234567890"});
			string[] highScorers = new string[10];
			string[] sectionsMessage = m_client.responseMessage.Split(' ');			
			highScorers = sectionsMessage[sectionsMessage.Length - 1].Split(',');
			
			bool invalidEdit = false;
			
			for (int i = 1; i < highScorers.Length; i = i + 2)
			{
				int serverHighscore = int.Parse(highScorers[i]);
				if (m_highscore > serverHighscore)
				{
					highScorers[i] = m_highscore.ToString();
					//if (editTextBox.Text.Contains(",") && editTextBox.Text.Contains(" "))
					//{
						highScorers[i-1] = editTextBox.Text;
						invalidEdit = false;
					//}
					break;
				}				
			}
			
			string clientHighscoreData = "";
			// add player name and highscore to the server
			for (int i = 0; i < highScorers.Length; i++)
			{
				clientHighscoreData += highScorers[i] + ',';
			}
			
			m_client = new Client(new string[2] {"Highscorers1234567890", clientHighscoreData});
			
			if (invalidEdit == false)
				Director.Instance.ReplaceScene(new TitleScene());
			else
				Director.Instance.ReplaceScene(new HighScoreScene(m_highscore));
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
		
		~HighScoreScene()
		{
			
		}
	}
}

