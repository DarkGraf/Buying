package com.tsv.buying;

import android.content.ContentProvider;
import android.content.ContentUris;
import android.content.ContentValues;
import android.content.Context;
import android.content.UriMatcher;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.database.sqlite.SQLiteQueryBuilder;
import android.net.Uri;
import android.support.annotation.Nullable;
import android.text.TextUtils;
import android.util.Log;

public class BuyingProvider extends ContentProvider {
    private static class BuyingDatabaseHelper extends SQLiteOpenHelper {
        private static final String TAG = "BuyingProvider";

        private static final String DATABASE_NAME = "buyings.db";
        private static final int DATABASE_VERSION = 1;
        private static final String BUYING_TABLE = "buyings";

        private static final String DATABASE_CREATE =
                "create table " + BUYING_TABLE + " (" +
                KEY_ID + " TEXT primary key, " +
                KEY_GOODS + " TEXT, " +
                KEY_PRIORITY + " INTEGER, " +
                KEY_INPUTDATE + " INTEGER, " +
                KEY_COMMENT + " TEXT);";

        private SQLiteDatabase buyingDB;

        public BuyingDatabaseHelper(Context context, String name, SQLiteDatabase.CursorFactory factory, int version) {
            super(context, name, factory, version);
        }

        @Override
        public void onCreate(SQLiteDatabase db) {
            db.execSQL(DATABASE_CREATE);
        }

        @Override
        public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
            Log.w(TAG, "Upgrading database from version " + oldVersion + " to " +
              newVersion + ", which will destroy all old data.");
            db.execSQL("DROP TABLE IF EXISTS " + BUYING_TABLE);
            onCreate(db);
        }
    }

    public static final Uri CONTENT_URI = Uri.parse("content://com.tsv.buyingprovider/buyings");

    public static final String KEY_ID = "_id";
    public static final String KEY_GOODS = "goods";
    public static final String KEY_PRIORITY = "priority";
    public static final String KEY_INPUTDATE = "input_date";
    public static final String KEY_COMMENT = "comment";

    private BuyingDatabaseHelper dbHelper;

    private static final UriMatcher uriMatcher;

    // Константы для различных запросов в URI.
    private static final int BUYINGS = 1;
    private static final int BUYING_ID = 2;

    static {
        uriMatcher = new UriMatcher(UriMatcher.NO_MATCH);
        uriMatcher.addURI("com.tsv.buyingprovider", "buyings", BUYINGS);
        uriMatcher.addURI("com.tsv.buyingprovider", "buyings/#", BUYING_ID);
    }

    @Override
    public boolean onCreate() {
        Context context = getContext();
        dbHelper = new BuyingDatabaseHelper(context, BuyingDatabaseHelper.DATABASE_NAME, null,
                BuyingDatabaseHelper.DATABASE_VERSION);
        return true;
    }

    @Nullable
    @Override
    public Cursor query(Uri uri, String[] projection, String selection, String[] selectionArgs, String sortOrder) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();

        SQLiteQueryBuilder builder = new SQLiteQueryBuilder();
        builder.setTables(BuyingDatabaseHelper.BUYING_TABLE);

        switch (uriMatcher.match(uri)) {
            case BUYING_ID:
                builder.appendWhere(KEY_ID + "=" + uri.getPathSegments().get(1));
                break;
            default: break;
        }

        String orderBy;
        if (TextUtils.isEmpty(sortOrder)) {
            orderBy = KEY_GOODS;
        }
        else {
            orderBy = sortOrder;
        }

        Cursor cursor = builder.query(database, projection, selection, selectionArgs,
                null, null, orderBy);
        cursor.setNotificationUri(getContext().getContentResolver(), uri);
        return cursor;
    }

    @Nullable
    @Override
    public String getType(Uri uri) {
        switch (uriMatcher.match(uri)) {
            case BUYINGS: return "vnd.android.cursor.dir/vnd.tsv.buying";
            case BUYING_ID: return "vnd.android.cursor.item/vnd.tsv.buying";
            default: throw new IllegalArgumentException("Unsupported URI: " + uri);
        }
    }

    @Nullable
    @Override
    public Uri insert(Uri uri, ContentValues values) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();
        long rowID = database.insert(BuyingDatabaseHelper.BUYING_TABLE, "buying", values);

        if (rowID > 0) {
            Uri resultUri = ContentUris.withAppendedId(CONTENT_URI, rowID);
            getContext().getContentResolver().notifyChange(resultUri, null);
            return resultUri;
        }

        throw new SQLException("Failed to insert row into " + uri);
    }

    @Override
    public int delete(Uri uri, String selection, String[] selectionArgs) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();

        int count;
        switch (uriMatcher.match(uri)) {
            case BUYINGS:
                count = database.delete(BuyingDatabaseHelper.BUYING_TABLE, selection, selectionArgs);
                break;
            case BUYING_ID:
                String segment = uri.getPathSegments().get(1);
                count = database.delete(BuyingDatabaseHelper.BUYING_TABLE,
                        KEY_ID + "=" + segment +
                        (!TextUtils.isEmpty(selection) ? " AND (" + selection + ")" : ""), selectionArgs);
                break;
            default: throw new IllegalArgumentException("Unsupported URI: " + uri);

        }
        getContext().getContentResolver().notifyChange(uri, null);
        return count;
    }

    @Override
    public int update(Uri uri, ContentValues values, String selection, String[] selectionArgs) {
        SQLiteDatabase database = dbHelper.getWritableDatabase();

        int count;
        switch (uriMatcher.match(uri)) {
            case BUYINGS:
                count = database.update(BuyingDatabaseHelper.BUYING_TABLE, values, selection, selectionArgs);
                break;
            case BUYING_ID:
                String segment = uri.getPathSegments().get(1);
                count = database.update(BuyingDatabaseHelper.BUYING_TABLE, values,
                        KEY_ID + "=" + segment +
                        (!TextUtils.isEmpty(selection) ? " AND (" + selection + ")" : ""), selectionArgs);
                break;
            default: throw new IllegalArgumentException("Unsupported URI: " + uri);

        }
        getContext().getContentResolver().notifyChange(uri, null);
        return count;
    }
}
