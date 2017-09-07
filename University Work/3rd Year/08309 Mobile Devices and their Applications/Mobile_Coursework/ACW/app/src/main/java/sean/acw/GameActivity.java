package sean.acw;

import android.content.Intent;
import android.content.SharedPreferences;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.MotionEvent;
import android.view.View;
import android.view.animation.Animation;
import android.view.animation.TranslateAnimation;
import android.widget.GridLayout;

import java.util.Random;
import android.os.Handler;

public class GameActivity extends AppCompatActivity {

    private GridLayout m_GridLayout;

    private int[] m_CardImageLocations;
    private int[] m_CardImage;

    private CardButton m_SelectedButton1;
    private CardButton m_SelectedButton2;

    private CardButton[][] m_CardButtons;

    private boolean isBusy = false;
    private boolean isNewGame = true;
    private boolean isClickMode = true;

    private int m_TotalClicks = 0;
    private int m_TotalMatchedCards = 0;
    private int m_Score = 0;
    private int m_NumElements = 0;
    private int m_NumColumns = 0;
    private int m_NumRows = 0;

    private String NUM_ELEMENTS = "num_elements";
    private String SCORE = "score";
    private String TOTAL_CLICKS = "total_clicks";
    private String TOTAL_MATCHED_CARDS = "total_matched_cards";
    private String NEW_GAME = "new_game";
    private String CLICK_MODE ="click_mode";

    private SharedPreferences game_settings;
    private SharedPreferences.Editor game_settings_editor;

    private Handler handler;

    private float oldX, newX, oldY, newY, deltaX, deltaY;
    private String direction;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_game);

        game_settings = getSharedPreferences("game_settings", 0);
        game_settings_editor = game_settings.edit();

        isNewGame = game_settings.getBoolean(NEW_GAME, true);
        isClickMode = game_settings.getBoolean(CLICK_MODE, false);

        m_GridLayout = (GridLayout) findViewById(R.id.game_gridlayout);

        m_NumColumns = m_GridLayout.getColumnCount();
        m_NumRows = m_GridLayout.getRowCount();

        m_NumElements = m_NumRows * m_NumColumns;

        m_CardButtons = new CardButton[m_NumRows][m_NumColumns];

        m_CardImage = new int[m_NumElements / 2];

        m_CardImage[0] = R.drawable.apple;
        m_CardImage[1] = R.drawable.banana;
        m_CardImage[2] = R.drawable.cherry;
        m_CardImage[3] = R.drawable.kiwi;
        m_CardImage[4] = R.drawable.mango;
        m_CardImage[5] = R.drawable.orange;
        m_CardImage[6] = R.drawable.pear;
        m_CardImage[7] = R.drawable.pineapple;

        m_CardImageLocations = new int[m_NumElements];

        ShuffleCardImages();

        // populate grid
        int m_currentButton = 0;
        for (int row = 0; row < m_NumRows; row++) {
            for(int column = 0; column < m_NumColumns; column++) {
                CardButton tempCardButton = new CardButton(this, row, column, m_CardImage[ m_CardImageLocations[m_currentButton] ]);
                tempCardButton.setId(m_currentButton);

                m_CardButtons[row][column] = tempCardButton;

                m_CardButtons[row][column].setOnTouchListener(new View.OnTouchListener() {
                        @Override
                        public boolean onTouch(View v, MotionEvent event) {
                            return onTouchListener(v, event);
                        }
                    });


                m_GridLayout.addView(m_CardButtons[row][column]);

                m_currentButton++;
            }
        }
    }

    private void ShuffleCardImages() {
        Random rand = new Random();

        // double up cards
        for (int i = 0; i < m_NumElements; i++) {
            m_CardImageLocations[i] = i % m_NumElements / 2;
        }

        // randomise card locations
        for (int i = 0; i < m_NumElements; i++) {
            int temp = m_CardImageLocations[i];

            int swapIndex = rand.nextInt(m_NumElements - 1);

            m_CardImageLocations[i] = m_CardImageLocations[swapIndex];

            m_CardImageLocations[swapIndex] = temp;
        }

    }

    private void onClickListener(View pView) {
        m_TotalClicks++;

        // wait until flipping has finished
        if (isBusy)
            return;

        final CardButton button = (CardButton) pView;
        handler = new Handler();

        // can't selected already matched cards
        if (button.isMatched())
            return;

        // select first card
        if (m_SelectedButton1 == null) {
            m_SelectedButton1 = button;
            m_SelectedButton1.selectCard();
            return;
        }

        // deselect first card
        if (m_SelectedButton1.getId() == button.getId()) {
            m_SelectedButton1.deSelectCard();
            m_SelectedButton1 = null;
            return;
        }

        // if drag mode enabled then disallow cards that aren't adjacent to be selected
        if (!isClickMode && m_SelectedButton1 != null) {
            if (button.getRow() > m_SelectedButton1.getRow() + 1)
                return;
            if (button.getRow() < m_SelectedButton1.getRow() - 1)
                return;
            if (button.getColumn() > m_SelectedButton1.getColumn() + 1)
                return;
            if (button.getColumn() < m_SelectedButton1.getColumn() - 1)
                return;
        }

        // cards have been matched
        if (m_SelectedButton1.getFrontDrawableId() == button.getFrontDrawableId()) {
            // select second card
            button.selectCard();

            // set to matched and disable cards
            m_SelectedButton1.setMatched(true);
            button.setMatched(true);
            m_SelectedButton1.setEnabled(false);
            button.setEnabled(false);

            m_TotalMatchedCards++;

            // game over
            if (m_TotalMatchedCards == m_NumElements / 2) {
                handler.postDelayed(new Runnable() {
                    @Override
                    public void run() {
                        gameOver();
                    }
                }, 1000);
            }

            isBusy = true;
            // wait for 250ms - then deselect and show front of cards
            handler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    m_SelectedButton1.deSelectCard();
                    button.deSelectCard();

                    m_SelectedButton1.setCardToFront();
                    button.setCardToFront();

                    m_SelectedButton1 = null;
                    isBusy = false;
                }
            }, 250);
        } else {
            // select second card;
            m_SelectedButton2 = button;
            m_SelectedButton2.selectCard();

            // wait 250 ms:
            // deselect card and show front briefly for 1 second
            // before flipping back over
            isBusy = true;
            handler.postDelayed(new Runnable() {
                @Override
                public void run() {
                    m_SelectedButton1.deSelectCard();
                    m_SelectedButton2.deSelectCard();

                    m_SelectedButton1.setCardToFront();
                    m_SelectedButton2.setCardToFront();

                    handler.postDelayed(new Runnable() {
                        @Override
                        public void run() {
                            m_SelectedButton1.setCardToBack();
                            m_SelectedButton2.setCardToBack();

                            m_SelectedButton1 = null;
                            m_SelectedButton2 = null;
                            isBusy = false;
                        }
                    }, 1000);
                }
            }, 250);
        }
    }

    private boolean onTouchListener(View pView, MotionEvent pEvent) {
        CardButton button = (CardButton)pView;

        if (isBusy) {
            return false;
        }

        switch(pEvent.getAction()) {
            case(MotionEvent.ACTION_DOWN):
                oldX = pEvent.getX();
                oldY = pEvent.getY();
                break;

            case(MotionEvent.ACTION_UP):
                newX = pEvent.getX();
                newY = pEvent.getY();
                deltaX = newX - oldX;
                deltaY = newY - oldY;

                if (isClickMode) {
                    onClickListener(pView);
                    return false;
                }

                // Use deltaX and deltaY to determine the direction
                if(Math.abs(deltaX) > Math.abs(deltaY)) {
                    if(deltaX > 0){
                        if (deltaX < 100) {
                            onClickListener(pView);
                            return false;
                        }

                        if (deltaY > 100) {
                            direction = "RIGHTDOWN";
                            switchCardButtons(button, direction);
                            break;
                        }

                        if (deltaY < -100) {
                            direction = "RIGHTUP";
                            switchCardButtons(button, direction);
                            break;
                        }

                        direction = "RIGHT";
                        switchCardButtons(button, direction);
                    }
                    else {
                        if (deltaX > -100) {
                            onClickListener(pView);
                            return false;
                        }

                        if (deltaY > 100) {
                            direction = "LEFTDOWN";
                            switchCardButtons(button, direction);
                            break;
                        }

                        if (deltaY < -100) {
                            direction = "LEFTUP";
                            switchCardButtons(button, direction);
                            break;
                        }

                        direction = "LEFT";
                        switchCardButtons(button, direction);
                    }
                } else {
                    if( deltaY > 0) {
                        if (deltaY < 100) {
                            onClickListener(pView);
                            break;
                        }

                        if (deltaX > 100) {
                            direction = "RIGHTDOWN";
                            switchCardButtons(button, direction);
                            break;
                        }

                        if (deltaX < -100) {
                            direction = "LEFTDOWN";
                            switchCardButtons(button, direction);
                            break;
                        }

                        direction = "DOWN";
                        switchCardButtons(button, direction);
                    }
                    else {
                        if (deltaY > -100) {
                            onClickListener(pView);
                            break;
                        }

                        if (deltaX > 100) {
                            direction = "RIGHTUP";
                            switchCardButtons(button, direction);
                            break;
                        }

                        if (deltaX < -100) {
                            direction = "LEFTUP";
                            switchCardButtons(button, direction);
                            break;
                        }

                        direction = "UP";
                        switchCardButtons(button, direction);
                    }
                }
        }

        Log.d("OnTouchListener", "Direction: " + direction);
        Log.d("OnTouchListener", "DeltaX: " + deltaX + " , DeltaY: " + deltaY);

        return true;
    }

    private void switchCardButtons(CardButton button, String direction) {
        float buttonWidth = button.getButtonWidth();

        switch (direction) {
            case ("RIGHT"):
                if (button.getColumn() == m_NumColumns - 1)
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow()][button.getColumn() + 1])) {
                    doAnimation(button, 1.0f, 0.0f);
                    doAnimation(m_CardButtons[button.getRow()][button.getColumn() + 1], -1.0f, 0);
                }

                break;
            case ("LEFT"):
                if (button.getColumn() == 0)
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow()][button.getColumn() - 1])) {
                    doAnimation(button, -1.0f, 0.0f);
                    doAnimation(m_CardButtons[button.getRow()][button.getColumn() - 1], 1.0f, 0.0f);
                }


                break;
            case ("DOWN"):
                if (button.getRow() == m_NumRows - 1)
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow() + 1][button.getColumn()])) {
                    doAnimation(button, 0.0f, 1.0f);
                    doAnimation(m_CardButtons[button.getRow() + 1][button.getColumn()], 0.0f, -1.0f);
                }

                break;
            case ("UP"):
                if (button.getRow() == 0)
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow() - 1][button.getColumn()])) {
                    doAnimation(button, 0.0f, -1.0f);
                    doAnimation(m_CardButtons[button.getRow() - 1][button.getColumn()], 0.0f, 1.0f);
                }

                break;
            case ("RIGHTDOWN"):
                if ((button.getColumn() == m_NumColumns - 1) || (button.getRow() == m_NumRows - 1))
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow() + 1][button.getColumn() + 1])) {
                    doAnimation(button, 1.0f, 1.0f);
                    doAnimation(m_CardButtons[button.getRow() + 1][button.getColumn() + 1], -1.0f, -1.0f);
                }

                break;
            case ("RIGHTUP"):
                if ((button.getColumn() == m_NumColumns - 1) || (button.getRow() == 0))
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow() - 1][button.getColumn() + 1])) {
                    doAnimation(button, 1.0f, -1.0f);
                    doAnimation(m_CardButtons[button.getRow() - 1][button.getColumn() + 1], -1.0f, 1.0f);
                }


                break;
            case("LEFTDOWN"):
                if ((button.getColumn() == 0) || (button.getRow() == m_NumRows - 1))
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow() + 1][button.getColumn() - 1])) {
                    doAnimation(button, -1.0f, 1.0f);
                    doAnimation(m_CardButtons[button.getRow() + 1][button.getColumn() - 1], 1.0f, -1.0f);
                }

                break;
            case("LEFTUP"):
                if ((button.getColumn() == 0) || (button.getRow() == 0))
                    break;

                if (switchButtonDrawableId(button, m_CardButtons[button.getRow() - 1][button.getColumn() - 1])) {
                    doAnimation(button, -1.0f, -1.0f);
                    doAnimation(m_CardButtons[button.getRow() - 1][button.getColumn() - 1], 1.0f, 1.0f);
                }

                break;
        }
    }

    private void doAnimation(CardButton button, float x, float y) {
        isBusy = true;

        TranslateAnimation animation1 = new TranslateAnimation(
                Animation.RELATIVE_TO_SELF, 0.0f, Animation.RELATIVE_TO_SELF, x,
                Animation.RELATIVE_TO_SELF, 0.0f, Animation.RELATIVE_TO_SELF, y);
        animation1.setDuration(1000);

        button.startAnimation(animation1);

        handler = new Handler();

        handler.postDelayed(new Runnable() {
            @Override
            public void run() {
                isBusy = false;
            }
        }, 1000);
    }

    private boolean switchButtonDrawableId(CardButton button1, CardButton button2) {
        int tempId = 0;

        if (button1.isMatched() || button2.isMatched())
            return false;

        button1.deSelectCard();
        button2.deSelectCard();

        m_SelectedButton1 = null;
        m_SelectedButton2 = null;

        tempId = button1.getFrontDrawableId();
        button1.setFrontDrawableId(button2.getFrontDrawableId());
        button2.setFrontDrawableId(tempId);

        button1.resetDrawableId();
        button2.resetDrawableId();

        return true;
    }

    private void gameOver() {
        m_Score = 1000000 / m_TotalClicks;
        Intent intent = new Intent(getApplicationContext(), GameOverActivity.class);
        intent.putExtra("SCORE", m_Score);
        startActivity(intent);
        finish();
    }
}
