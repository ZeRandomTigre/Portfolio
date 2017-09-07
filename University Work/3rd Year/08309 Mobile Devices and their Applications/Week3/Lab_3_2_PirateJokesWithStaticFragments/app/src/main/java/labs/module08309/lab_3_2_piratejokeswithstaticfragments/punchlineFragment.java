package labs.module08309.lab_3_2_piratejokeswithstaticfragments;

import android.app.Fragment;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;


public class punchlineFragment extends Fragment {

    private TextView m_PunchlineTextView = null;


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_punchline, container, false);
        m_PunchlineTextView = (TextView)view.findViewById(R.id.punchlineTextView);
        return view;
    }

    public void showPunchLineForIndex(int pIndex) {
        String[] punchLines = getResources().getStringArray(R.array.punchlines);
        m_PunchlineTextView.setText(punchLines[pIndex]);
    }

}
