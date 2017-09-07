package labs.module08309.lab_3_2_piratejokeswithstaticfragments;


import android.content.Context;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import java.util.List;


public class setupFragment extends Fragment {

    private ListSelectionListener m_Listener = null;

    @Override
    public void onAttach(Context context) {
        super.onAttach(context);
        try {
            m_Listener = (ListSelectionListener)context;
        } catch (ClassCastException e) {
            throw new ClassCastException(context.toString() + " must implement ListSelectionListener");
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        View view = inflater.inflate(R.layout.fragment_setup, container, false);

        ListView listView = (ListView)view.findViewById(R.id.listView);

        String[] setups = getResources().getStringArray(R.array.setups);

        ArrayAdapter<String> arrayAdapter = new ArrayAdapter<String>(getContext(), android.R.layout.simple_list_item_1, android.R.id.text1, setups);

        listView.setAdapter((arrayAdapter));

        listView.setOnItemClickListener(new AdapterView.OnItemClickListener() {
            @Override
            public void onItemClick(AdapterView<?> parent, View view, int position, long id) {
                m_Listener.onListSelection(position);
            }
        });

        return view;
    }

    public interface ListSelectionListener {
        public void onListSelection(int index);
    }
}
