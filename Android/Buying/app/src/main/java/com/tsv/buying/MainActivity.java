package com.tsv.buying;

import android.app.FragmentManager;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;
import android.support.v7.app.AppCompatActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Toast;

import com.tsv.buying.Preference.FragmentPreferenceActivity;

// Стр. 324
public class MainActivity extends AppCompatActivity {
    // Признак изменения настроек.
    boolean preferenceWasChanged;
    SharedPreferences.OnSharedPreferenceChangeListener preferenceListener;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        updateFromPreferences();

        // Используется в SharedPreferences коллекция WeakHashMap, поэтому сохраняем ссылку.
        preferenceListener = new SharedPreferences.OnSharedPreferenceChangeListener() {
            @Override
            public void onSharedPreferenceChanged(SharedPreferences sharedPreferences, String key) {
                preferenceWasChanged = true;
            }
        };
        Context context = getApplicationContext();
        PreferenceManager.getDefaultSharedPreferences(context)
                    .registerOnSharedPreferenceChangeListener(preferenceListener);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    private static final int SHOW_PREFERENCES = 1;
    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        switch (item.getItemId()) {
            case R.id.action_settings: {
                Intent i = new Intent(this, FragmentPreferenceActivity.class);
                startActivityForResult(i, SHOW_PREFERENCES);
                return true;
            }
        }
        return super.onOptionsItemSelected(item);
    }

    public boolean autoUpdate = false;
    public int updateFreq = 0;

    private void updateFromPreferences() {
        Context context = getApplicationContext();
        SharedPreferences preferences = PreferenceManager.getDefaultSharedPreferences(context);

        autoUpdate = preferences.getBoolean(FragmentPreferenceActivity.PREF_AUTO_UPDATE, false);
        updateFreq = Integer.parseInt(preferences.getString(FragmentPreferenceActivity.PREF_UPDATE_FREQ, "60"));
        preferenceWasChanged = false;
    }

    @Override
    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        if (requestCode == SHOW_PREFERENCES && preferenceWasChanged) {
            updateFromPreferences();

            FragmentManager fm = getFragmentManager();
            final BuyingListFragment buyingList = (BuyingListFragment) fm.findFragmentById(R.id.BuyingListFragment);
            buyingList.refreshBuyingsAsync();
        }
    }
}
