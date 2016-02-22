package com.tsv.buying;

import java.util.Date;
import java.util.UUID;

public class Buying {
    private UUID id;
    private String goods;
    private int priority;
    private Date inputDate;
    private String comment;

    public UUID getId() { return id; }
    public String getGoods() { return goods; }
    public int getPriority() { return priority; }
    public Date getInputDate() { return inputDate; }
    public String getComment() { return comment; }

    public Buying(UUID id, String goods, int priority, Date inputDate, String comment) {
        this.id = id;
        this.goods = goods;
        this.priority = priority;
        this.inputDate = inputDate;
        this.comment = comment;
    }

    @Override
    public String toString() {
        return goods;
    }
}
