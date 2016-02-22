package com.tsv.buying;

import java.util.Date;

public class JsonUtility {
    public static Date jsonDateToDate(String jsonDate) {
        //  "/Date(1321867151710+0100)/"
        int i1 = jsonDate.indexOf("(");
        int i2 = jsonDate.indexOf(")") - 5;
        String s = jsonDate.substring(i1 + 1, i2);
        long l = Long.valueOf(s);
        return new Date(l);
    }
}
