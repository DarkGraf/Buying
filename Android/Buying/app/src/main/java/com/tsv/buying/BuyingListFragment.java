package com.tsv.buying;

import android.app.ListFragment;
import android.os.Bundle;
import android.os.Handler;
import android.util.Log;
import android.widget.ArrayAdapter;

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

public class BuyingListFragment extends ListFragment {
    private ArrayAdapter<Buying> adapter;
    private ArrayList<Buying> buyings = new ArrayList<>();

    private  static final String TAG = "BUYING";
    private Handler handler = new Handler();

    @Override
    public void onActivityCreated(Bundle savedInstanceState) {
        super.onActivityCreated(savedInstanceState);

        int layoutID = android.R.layout.simple_list_item_1;
        adapter = new ArrayAdapter<>(getActivity(), layoutID, buyings);
        setListAdapter(adapter);

        Thread t = new Thread(new Runnable() {
            @Override
            public void run() {
                refreshBuyings();
            }
        });
        t.start();
    }

    public void refreshBuyings() {
        String buyingsFeed = getString(R.string.buyings_feed);
        try {
            URL url = new URL(buyingsFeed);
            URLConnection connection = url.openConnection();
            InputStream stream = connection.getInputStream();
            InputStreamReader reader = new InputStreamReader(stream);
            BufferedReader bufferedReader = new BufferedReader(reader);
            String line = bufferedReader.readLine();

            JSONArray jsonArray = new JSONArray(line);
            for (int i = 0; i < jsonArray.length(); i++) {
                JSONObject jsonObject = jsonArray.getJSONObject(i);
                UUID id = UUID.fromString(jsonObject.getString("Id"));
                String goods = jsonObject.getString("Goods");
                int priority = jsonObject.getInt("Priority");
                Date inputDate = JsonUtility.jsonDateToDate(jsonObject.getString("InputDate"));
                String comment = jsonObject.getString("Comment");
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
        buyings.add(buying);
        adapter.notifyDataSetChanged();
    }
}
