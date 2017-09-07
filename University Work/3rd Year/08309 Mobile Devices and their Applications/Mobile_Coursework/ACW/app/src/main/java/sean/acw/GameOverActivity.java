package sean.acw;

import android.content.Context;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;


public class GameOverActivity extends AppCompatActivity {

    private Button m_Button_Continue;

    private TextView m_Textview_Score;
    private TextView m_Textview_Highscore;
    private TextView m_Textview_NewHighscore;

    private int m_Score = 0;
    private int m_Highscore = 0;

    private String HIGH_SCORE = "high_score";

    private SharedPreferences game_settings;
    private SharedPreferences.Editor game_settings_editor;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game_over);

        game_settings = getSharedPreferences("game_settings", Context.MODE_PRIVATE);

        game_settings_editor = game_settings.edit();

        m_Highscore = game_settings.getInt(HIGH_SCORE, 0);
        m_Score = getIntent().getIntExtra("SCORE", 0);;

        m_Button_Continue = (Button)findViewById(R.id.gameover_button_continue);

        m_Textview_Highscore = (TextView)findViewById(R.id.gameover_textview_highscore);
        m_Textview_Score = (TextView)findViewById(R.id.gameover_textview_score);
        m_Textview_NewHighscore = (TextView)findViewById(R.id.gameover_textview_newhighscore);

        m_Textview_NewHighscore.setText("");

        m_Textview_Score.setText(getString(R.string.score, m_Score));

        if (m_Score > m_Highscore) {
            game_settings_editor.putInt(HIGH_SCORE, m_Score);
            game_settings_editor.commit();

            m_Textview_Highscore.setText(getString(R.string.highscore, m_Score));
            m_Textview_NewHighscore.setText(R.string.new_highscore);
        }
        else {
            m_Textview_Highscore.setText(getString(R.string.highscore, m_Highscore));
        }

        m_Button_Continue.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }
}

