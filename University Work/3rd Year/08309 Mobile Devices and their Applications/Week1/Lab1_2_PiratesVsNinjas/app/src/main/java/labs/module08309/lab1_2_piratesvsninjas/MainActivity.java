package labs.module08309.lab1_2_piratesvsninjas;

import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.widget.ImageView;
import android.widget.TextView;

public class MainActivity extends AppCompatActivity {

    private final int mPirates = 3;
    private final int mNinjas = 2;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        TextView punchlineView = (TextView) findViewById(R.id.punchlineView);

        String punchline = GetPunchline();

        punchlineView.setText(punchline);
    }

    private String GetPunchline()
    {
        String result = "";
        if(mPirates < mNinjas)
        {
            result = getString(R.string.punchline2);
            ImageView imageView = (ImageView) findViewById(R.id.imageView);
            imageView.setImageResource(R.drawable.ninja);
        }
        else
        {
            result = getString(R.string.punchline1);
            ImageView imageView = (ImageView) findViewById(R.id.imageView);
            imageView.setImageResource(R.drawable.pirate);
        }
        return result;
    }
}
