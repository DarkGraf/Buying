package com.tsv.buying.Preference;

import android.preference.PreferenceActivity;

import com.tsv.buying.R;

import java.util.List;

public class FragmentPreferenceActivity extends PreferenceActivity {
    public static final String PREF_AUTO_UPDATE = "PREF_AUTO_UPDATE";
    public static final String PREF_UPDATE_FREQ = "PREF_UPDATE_FREQ";

    @Override
    public void onBuildHeaders(List<Header> target) {
        loadHeadersFromResource(R.xml.preference_headers, target);
    }
}
