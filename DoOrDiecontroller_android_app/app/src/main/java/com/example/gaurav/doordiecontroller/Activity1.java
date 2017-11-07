package com.example.gaurav.doordiecontroller;

import android.content.Intent;
import android.os.Bundle;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import java.io.IOException;

public class Activity1 extends AppCompatActivity {

    Button done;
    EditText player_name, host_ip;
    String LOG_TAG = "dod";
    String error;
    TextView err;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_1);

        error = getIntent().getStringExtra("error");

        done = (Button) findViewById(R.id.done);
        err = (TextView) findViewById(R.id.error_view);
        player_name = (EditText) findViewById(R.id.player_name);
        host_ip = (EditText) findViewById(R.id.host_ip);

        if (error!=null) {
            Log.e(LOG_TAG, error);
            err.setText(error);
        } else {
            err.setText("");
        }
    }

    public void onDoneClick(View v) {
        switch (v.getId()) {
            case  R.id.done: {

                Intent myIntent = new Intent(v.getContext(),MainActivity.class);
                Log.e(LOG_TAG, player_name.getText().toString());
                myIntent.putExtra("player_name",player_name.getText().toString());
                myIntent.putExtra("host_ip", host_ip.getText().toString());
                startActivity(myIntent);

                break;
            }
        }
    }
}
