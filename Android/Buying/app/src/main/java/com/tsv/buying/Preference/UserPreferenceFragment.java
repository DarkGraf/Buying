package com.tsv.buying.Preference;

import android.os.Bundle;
import android.preference.PreferenceFragment;

import com.tsv.buying.R;

public class UserPreferenceFragment extends PreferenceFragment {
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        addPreferencesFromResource(R.xml.userpreferences);
    }
}
