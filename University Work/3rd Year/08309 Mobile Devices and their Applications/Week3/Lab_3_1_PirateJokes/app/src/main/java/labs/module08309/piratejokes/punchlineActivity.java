package labs.module08309.piratejokes;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.TextView;

public class punchlineActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_punchline);

        int position = getIntent().getIntExtra("position", 0);
        TextView punchLineTextView = (TextView)findViewById(R.id.punchlineTextView);
        String[] punchLines = getResources().getStringArray(R.array.punchlines);
        punchLineTextView.setText(punchLines[position]);
    }
}
