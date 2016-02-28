package com.tsv.buying;

import android.app.ListFragment;
import android.app.LoaderManager;
import android.content.ContentResolver;
import android.content.ContentValues;
import android.content.CursorLoader;
import android.content.Loader;
import android.database.Cursor;
import android.os.Bundle;
import android.os.Handler;
import android.text.TextUtils;
import android.util.Log;
import android.widget.ArrayAdapter;
import android.widget.SimpleCursorAdapter;
import android.widget.Toast;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import java.net.URLConnection;
import java.util.ArrayList;
import java.util.Date;
import java.util.UUID;

public class BuyingListFragment extends ListFragment implements LoaderManager.LoaderCallbacks<Cursor> {
    SimpleCursorAdapter adapter;

    private  static final String TAG = "BUYING";
    private Handler handler = new Handler();

    @Override
    public void onActivityCreated(Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);

        int layoutID = android.R.layout.simple_list_item_2;
        adapter = new SimpleCursorAdapter(getActivity(), layoutID, null,
                new String[] { BuyingProvider.KEY_GOODS, BuyingProvider.KEY_COMMENT },
                new int[] { android.R.id.text1, android.R.id.text2 }, 0);
        setListAdapter(adapter);
        getLoaderManager().initLoader(0, null, this);

        refreshBuyingsAsync();
    }

    public void refreshBuyingsAsync() {
        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                refreshBuyingsInternal();
            }
        });
        t.start();
    }

    private void refreshBuyingsInternal() {
        handler.post(new Runnable() {
            @Override
            public void run() {
                getLoaderManager().restartLoader(0, null, BuyingListFragment.this);
            }
        });

        String buyingsFeed = getString(R.string.buyings_feed);
        try {
            URL url = new URL(buyingsFeed);
            URLConnection connection = url.openConnection();
            InputStream stream = connection.getInputStream();
            InputStreamReader reader = new InputStreamReader(stream);
            BufferedReader bufferedReader = new BufferedReader(reader);
            String line = bufferedReader.readLine();

            // Если получили данные, то очистим таблицу.
            handler.post(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(getActivity(), "Refreshing buyings list...", Toast.LENGTH_SHORT).show();
                    ContentResolver cr = getActivity().getContentResolver();
                    cr.delete(BuyingProvider.CONTENT_URI, null, null);
                }
            });

            JSONArray jsonArray = new JSONArray(line);
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject jsonObject = jsonArray.getJSONObject(i);
                UUID id = UUID.fromString(jsonObject.getString("Id"));
                String goods = jsonObject.getString("Goods");
                int priority = jsonObject.getInt("Priority");
                Date inputDate = JsonUtility.jsonDateToDate(jsonObject.getString("InputDate"));
                String comment = jsonObject.isNull("Comment") ? "" : jsonObject.getString("Comment");
                final Buying buying = new Buying(id, goods, priority, inputDate, comment);

                handler.post(new Runnable() {
                    @Override
                    public void run() {
                        addNewBuying(buying);
                    }
                });
            }
        }
        catch(MalformedURLException e) {
            Log.d(TAG, "MalformedURLException");
        } catch (IOException e) {
            Log.d(TAG, "IOException");
        } catch (JSONException e) {
            Log.d(TAG, "JSONException");
        }
    }

    private void addNewBuying(Buying buying) {
        ContentValues values = new ContentValues();
        values.put(BuyingProvider.KEY_ID, buying.getId().toString());
        values.put(BuyingProvider.KEY_GOODS, buying.getGoods());
        values.put(BuyingProvider.KEY_PRIORITY, buying.getPriority());
        values.put(BuyingProvider.KEY_INPUTDATE, buying.getInputDate().getTime());
        values.put(BuyingProvider.KEY_COMMENT,  buying.getComment());

        ContentResolver cr = getActivity().getContentResolver();
        cr.insert(BuyingProvider.CONTENT_URI, values);
    }

    // *** LoaderManager.LoaderCallbacks<Cursor> ***

    @Override
    public Loader<Cursor> onCreateLoader(int id, Bundle args) {
        String[] projection = new String[] {
                BuyingProvider.KEY_ID,
                BuyingProvider.KEY_GOODS,
                BuyingProvider.KEY_COMMENT
        };

        CursorLoader loader = new CursorLoader(getActivity(), BuyingProvider.CONTENT_URI, projection,
                null, null, null);
        return loader;
    }

    @Override
    public void onLoadFinished(Loader<Cursor> loader, Cursor data) {
        adapter.swapCursor(data);
    }

    @Override
    public void onLoaderReset(Loader<Cursor> loader) {
        adapter.swapCursor(null);
    }
}
