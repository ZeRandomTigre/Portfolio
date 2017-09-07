package sean.acw;

import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

public class OptionsActivity extends AppCompatActivity {

    private Button m_button_dragMode;
    private Button m_button_clickmode;

    private String CLICK_MODE = "click_mode";

    SharedPreferences game_settings;
    SharedPreferences.Editor game_settings_editor;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_options);

        game_settings = getSharedPreferences("game_settings", 0);

        game_settings_editor = game_settings.edit();

        m_button_dragMode = (Button) findViewById(R.id.options_button_dragmode);
        m_button_clickmode = (Button) findViewById(R.id.options_button_clickmode);

        m_button_dragMode.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                game_settings_editor.putBoolean(CLICK_MODE, false);
                game_settings_editor.commit();

                Toast toast = Toast.makeText(getApplicationContext(), R.string.drag_mode_enabled, Toast.LENGTH_SHORT);
                toast.show();
            }
        });

        m_button_clickmode.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                game_settings_editor.putBoolean(CLICK_MODE, true);
                game_settings_editor.commit();

                Toast toast = Toast.makeText(getApplicationContext(), R.string.click_mode_enabled, Toast.LENGTH_SHORT);
                toast.show();
            }
        });

    }
}
