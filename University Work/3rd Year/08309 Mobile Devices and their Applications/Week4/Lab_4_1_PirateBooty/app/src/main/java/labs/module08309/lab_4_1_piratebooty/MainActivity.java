package labs.module08309.lab_4_1_piratebooty;

import android.content.Context;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.os.AsyncTask;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.ImageView;
import android.widget.TextView;
import android.widget.Toast;

import org.w3c.dom.Text;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.net.URL;
import java.util.Random;

public class MainActivity extends AppCompatActivity {

    private int m_Score;
    private int m_NumberOfGrogs;
    private int m_HighScore = 0;
    private Random m_RNG = new Random();
    String urlString;
    private String HIGH_SCORE = "high_score";
    private String SCORE = "score";
    private String NUMBER_OF_GROGS = "number_of_grogs";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        m_Score = 0;
        m_NumberOfGrogs = 0;

        SharedPreferences sharedPreferences = getPreferences(MODE_PRIVATE);
        m_HighScore = sharedPreferences.getInt(HIGH_SCORE, 0);

        setHighScore(m_HighScore);
        addCurrentScore(0);;
    }

    @Override
    protected  void onPause() {
        super.onPause();

        SharedPreferences.Editor editor = getPreferences(MODE_PRIVATE).edit();
        editor.putInt(SCORE, m_Score);
        editor.putInt(NUMBER_OF_GROGS, m_NumberOfGrogs);
        editor.commit();
    }

    @Override
    protected void onStart() {
        super.onStart();

        SharedPreferences sharedPreferences = getPreferences(MODE_PRIVATE);
        m_Score = sharedPreferences.getInt(SCORE, 0);
        m_NumberOfGrogs = sharedPreferences.getInt(NUMBER_OF_GROGS, 0);

        addCurrentScore(0);
    }

    private void setHighScore(int pScore)
    {
        TextView textView = (TextView)findViewById(R.id.highScoreLabelTextView);
        String highScoreString = getString(R.string.high_score_label, pScore);
        textView.setText(highScoreString);

        SharedPreferences.Editor editor = getPreferences(MODE_PRIVATE).edit();
        editor.putInt(HIGH_SCORE, m_HighScore);
        editor.commit();
    }

    private void addCurrentScore(int pScore)
    {
        m_Score += pScore;
        TextView textView = (TextView)findViewById(R.id.currentScoreLabelTextView);
        String scoreString = getString(R.string.current_score_label, m_Score);
        textView.setText(scoreString);

        if (m_Score > m_HighScore) {
            m_HighScore = m_Score;
            setHighScore(m_HighScore);
        }
    }

    public void drinkGrogOnClick(View pView)
    {
        m_NumberOfGrogs++;
        String grogsString = getString(R.string.grogs_toast_label, m_NumberOfGrogs);
        Toast toast = Toast.makeText(this, grogsString, Toast.LENGTH_SHORT);
        toast.show();
    }

    public void plunderOnClick(View pView)
    {
        int randomNumber = m_RNG.nextInt(20);
        if (randomNumber > m_NumberOfGrogs) {
            addCurrentScore(randomNumber * m_NumberOfGrogs);
            urlString = "http://www.hull.ac.uk/php/349628/08027/labs/treasure.jpg";
        } else {
            TextView textView = (TextView)findViewById(R.id.currentScoreLabelTextView);
            textView.setText((R.string.fail_label));
            m_Score = 0;
            m_NumberOfGrogs = 0;
            urlString = "http://www.hull.ac.uk/php/349628/08027/labs/death.jpg";
        }

        new downloadImage().execute(urlString);

        /*
        ImageView imageView = (ImageView)findViewById(R.id.imageView);
        Bitmap bitmap = null;
        try {
            bitmap = BitmapFactory.decodeStream((InputStream) new URL(urlString).getContent());
        } catch (IOException e) {
            Log.i("My Exception", e.getMessage());
        }
        imageView.setImageBitmap(bitmap);
        */
    }



    private class downloadImage extends AsyncTask<String, String, Bitmap>   {

        protected  Bitmap doInBackground(String... args) {
            /*
            Bitmap bitmap = null;
            try {
                bitmap = BitmapFactory.decodeStream((InputStream)new URL(args[0]).getContent());
            } catch (Exception e) {
                e.printStackTrace();
            }
            return bitmap;
            */

            Bitmap bitmap = null;
            try {
                // Try to read from itnernal storage
                FileInputStream reader = getApplicationContext().openFileInput(args[0]);
                bitmap = BitmapFactory.decodeStream(reader);
            } catch (FileNotFoundException fileNotFound) {
                try {
                    // if file was not found try to download it and tore it for next time
                    String url = "http://www.hull.ac.uk/php/349628/08027/labs/" + args[0];
                    bitmap = BitmapFactory.decodeStream((InputStream) new URL(url).getContent());
                    FileOutputStream writer = null;
                    try {
                        writer = getApplicationContext().openFileOutput(args[0], Context.MODE_PRIVATE);
                        bitmap.compress(Bitmap.CompressFormat.JPEG, 100, writer);
                    } catch (Exception e) {
                        Log.i("My Error", e.getMessage());
                    } finally {
                        writer.close();
                    }
                } catch (Exception e) {
                    Log.i("My Error", e.getMessage());
                }
            }

            return bitmap;
        }

        protected void onPostExecute(Bitmap image) {
            if (image != null) {
                ImageView imageview = (ImageView)findViewById(R.id.imageView);
                imageview.setImageBitmap(image);
            } else {
                Toast.makeText(MainActivity.this, "Image Does Not Exist or Network Error", Toast.LENGTH_SHORT).show();
            }
        }
    }
}
