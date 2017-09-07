package sean.acw;

import android.content.Context;
import android.graphics.drawable.Drawable;
import android.support.v7.widget.AppCompatDrawableManager;
import android.widget.GridLayout;

public class CardButton extends android.support.v7.widget.AppCompatButton {

    private int m_Row;
    private int m_Column;
    private int m_FrontDrawableId;

    private float m_ButtonWidth = getResources().getDisplayMetrics().density * 75;
    private float m_ButtonHeight = getResources().getDisplayMetrics().density * 75;

    private boolean isMatched;
    private boolean isSelected;

    private Drawable m_Front;
    private Drawable m_Back;
    private Drawable m_Selected;

    public CardButton(Context pContext, int pRow, int pColumn, int pFrontImageDrawableId) {
        super(pContext);

        m_Row = pRow;
        m_Column = pColumn;
        m_FrontDrawableId = pFrontImageDrawableId;

        m_Front = AppCompatDrawableManager.get().getDrawable(pContext, pFrontImageDrawableId);
        m_Back = AppCompatDrawableManager.get().getDrawable(pContext, R.drawable.back);
        m_Selected = AppCompatDrawableManager.get().getDrawable(pContext, R.drawable.selected);

        setBackground(m_Back);

        GridLayout.LayoutParams layoutParams = new GridLayout.LayoutParams(GridLayout.spec(pRow), GridLayout.spec(pColumn));

        layoutParams.width = (int) m_ButtonWidth;
        layoutParams.height = (int) m_ButtonHeight;
        setLayoutParams(layoutParams);
    }

    public boolean isMatched() {
        return isMatched;
    }

    public void setMatched(boolean matched) {
        isMatched = matched;
    }

    public void selectCard() {
        setBackground(m_Selected);
        isSelected = true;
    }

    public void deSelectCard() {
        setBackground(m_Back);
        isSelected = false;
    }

    public void setCardToFront() {
        setBackground(m_Front);
    }

    public void setCardToBack() {
        setBackground(m_Back);
    }

    public void resetDrawableId() { m_Front = AppCompatDrawableManager.get().getDrawable(getContext(), m_FrontDrawableId); }

    public int getRow() {
        return m_Row;
    }

    public int getColumn() {
        return m_Column;
    }

    public int getFrontDrawableId() {
        return m_FrontDrawableId;
    }

    public void setFrontDrawableId(int pFrontDrawableId) {
        m_FrontDrawableId = pFrontDrawableId;
    }

    public float getButtonWidth() {
        return m_ButtonWidth;
    }

    public float getButtonHeight() {
        return m_ButtonHeight;
    }
}
