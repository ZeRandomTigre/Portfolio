package sean.acw;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {

    private Button m_Button_Start;
    private Button m_Button_Reset_Highscore;
    private Button m_Button_Continue;
    private Button m_Button_Options;

    private TextView m_TextView_HighScore;

    private int m_Highscore = 0;

    private String HIGH_SCORE = "high_score";
    private String NEW_GAME = "new_game";

    private SharedPreferences game_settings;
    private SharedPreferences.Editor game_settings_editor;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        game_settings = getSharedPreferences("game_settings", 0);

        game_settings_editor = game_settings.edit();

        m_Highscore = game_settings.getInt(HIGH_SCORE, 0);

        m_TextView_HighScore = (TextView)findViewById(R.id.main_textview_highscore);
        m_TextView_HighScore.setText(getString(R.string.highscore, m_Highscore));

        m_Button_Start = (Button)findViewById(R.id.main_button_start_game);
        m_Button_Reset_Highscore = (Button)findViewById(R.id.main_button_reset_highscore);
        m_Button_Continue = (Button) findViewById(R.id.main_button_continuegame);
        m_Button_Options = (Button) findViewById(R.id.main_button_options);

        m_Button_Reset_Highscore.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                game_settings_editor.putInt(HIGH_SCORE, 0);
                game_settings_editor.commit();

                m_Highscore = game_settings.getInt(HIGH_SCORE, 0);
                m_TextView_HighScore.setText(getString(R.string.highscore, m_Highscore));
            }
        });

        m_Button_Start.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                game_settings_editor.putBoolean(NEW_GAME, true);
                game_settings_editor.commit();

                Intent intent = new Intent(getApplicationContext(), GameActivity.class);
                startActivity(intent);
            }
        });

        m_Button_Continue.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                game_settings_editor.putBoolean(NEW_GAME, false);
                game_settings_editor.commit();

                Intent intent = new Intent(getApplicationContext(), GameActivity.class);
                startActivity(intent);
            }
        });

        m_Button_Options.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Intent intent = new Intent(getApplicationContext(), OptionsActivity.class);
                startActivity(intent);
            }
        });
    }

    @Override
    protected void onResume() {
        super.onResume();

        m_Highscore = game_settings.getInt(HIGH_SCORE, 0);
        m_TextView_HighScore.setText(getString(R.string.highscore, m_Highscore));
    }
}
