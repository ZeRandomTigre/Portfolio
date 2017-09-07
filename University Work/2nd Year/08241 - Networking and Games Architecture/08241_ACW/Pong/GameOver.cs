using System;
using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Imaging;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Audio;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;
using Sce.PlayStation.Core.Input;

namespace Pong
{
	public class GameOverScene : Scene
	{
		private TextureInfo m_ti;
		private int m_playerScore;
		private Client m_client;
		
		public GameOverScene (bool win, int playerScore, Client client)
		{
			m_client = client;
			m_playerScore = playerScore;
			
			this.Camera.SetViewFromViewport();
			// add label with highscore
			Font font = new Font(FontAlias.System,32,FontStyle.Bold);
			FontMap fontMap = new FontMap(font, 512);
			Label label1 = new Label("Score: " + m_playerScore.ToString(),fontMap);
			label1.Position = new Vector2(400,20);
			
			string gameOverMessage;
			if(win)
				gameOverMessage = "Player 1 wins";
			else
				gameOverMessage = "Player 2 wins";
			
			Label label2 = new Label(gameOverMessage, fontMap);
			label2.Position = new Vector2(400,300);		
			
			this.AddChild(label1);	
			this.AddChild(label2);	
			
			
			Scheduler.Instance.ScheduleUpdateForTarget(this,0,false);
			
			Touch.GetData(0).Clear();
		}
		
		public override void Update (float dt)
		{
			base.Update (dt);
			int touchCount = Touch.GetData(0).ToArray().Length;
			if(touchCount > 0 || Input2.GamePad0.Cross.Press)
			{
				// check if new highscore
				// if true  - enter highscore screen
								
				// retrieve highscore from server
				m_client = new Client(new string[1] {"Highscorers1234567890"});
				string[] highScorers = new string[10];
				string[] sectionsMessage = m_client.responseMessage.Split(' ');			
				highScorers = sectionsMessage[sectionsMessage.Length - 1].Split(',');
				bool newHighscore = false;
				for (int i = 1; i < highScorers.Length; i = i + 2)
				{					
					int serverHighscore = int.Parse(highScorers[i]);
					if (m_playerScore > serverHighscore )
					{
						Director.Instance.ReplaceScene( new HighScoreScene(m_playerScore));
						newHighscore = true;
						break;
					}					
				}
				
				if (newHighscore == false)
				{
					Director.Instance.ReplaceScene( new TitleScene());
				}
			}
		}
		
		~GameOverScene()
		{
		}
	}
}

