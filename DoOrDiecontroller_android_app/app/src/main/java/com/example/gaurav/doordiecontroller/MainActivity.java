package com.example.gaurav.doordiecontroller;


import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.Handler;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.util.Log;
import android.view.View;
import android.view.Menu;
import android.view.MenuItem;
import android.widget.Button;
import android.widget.TextView;

import java.io.IOException;
import java.io.OutputStream;
import java.net.Socket;

public class MainActivity extends AppCompatActivity {

    Socket socket_1, socket_2;
    String host_name = "10.42.0.1";
    String host_port_1 = "22112";
    String host_port_2 = "22113";
    OutputStream out_1, out_2;
    String player_name="Gaurav";

    String LOG_TAG = "dod";

    private SensorManager sensorManager;
    float ax,ay,az;   // these are the acceleration in x,y and z axis
    TextView acc;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        player_name = getIntent().getStringExtra("player_name");
        host_name = getIntent().getStringExtra("host_ip");
        Log.e(LOG_TAG, "name="+player_name);
        Log.e(LOG_TAG, "host="+host_name);

        sensorManager= (SensorManager) getSystemService(Context.SENSOR_SERVICE);

        new ConnectServer(this).execute();
    }

    @Override
    protected void onPause() {
        super.onPause();
        sensorManager.unregisterListener(sensorlistener);
    }

    @Override
    protected void onResume() {
        super.onResume();
        //sensorManager.registerListener(sensorlistener, sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER), SensorManager.SENSOR_DELAY_NORMAL);
    }

    @Override
    protected void onDestroy() {
        super.onDestroy();
        try {
            socket_1.close();
            socket_2.close();
        } catch (IOException e) {
            Log.e(LOG_TAG, "error in closing sockets");
        }
    }

    private SensorEventListener sensorlistener=new SensorEventListener() {
        public void onSensorChanged(SensorEvent event){
            if (event.sensor.getType()==Sensor.TYPE_ACCELEROMETER){
                ax=event.values[0];
                ay=event.values[1];
                az=event.values[2];
                String acc = "{\"x\":\""+Double.toString(ax)+"\",\"y\":\""+Double.toString(ay)+"\",\"z\":\""+Double.toString(az)+"\"}";
                try {
                    out_1.write(acc.getBytes());
                } catch (IOException e) {
                    Log.e(LOG_TAG, "error in accelerometer send");
                }
            }
        }

        public void onAccuracyChanged(Sensor sensor, int accuracy){

        }
    };

    public void onClick(View v) {
        switch (v.getId()) {
            case  R.id.fire: {
                try {
                    out_2.write(("{\"name\":\"fire\"}").getBytes());
                } catch (IOException e) {
                    Log.e(LOG_TAG, "error in button click");
                }
                break;
            }
            case  R.id.jump: {
                try {
                    out_2.write(("{\"name\":\"jump\"}").getBytes());
                } catch (IOException e) {
                    Log.e(LOG_TAG, "error in button click");
                }
                break;
            }
            case  R.id.bomb: {
                try {
                    out_2.write(("{\"name\":\"bomb\"}").getBytes());
                } catch (IOException e) {
                    Log.e(LOG_TAG, "error in button click");
                }
                break;
            }
        }
    }

    private class ConnectServer extends AsyncTask<Void , Void, Void> {

        private Context mContext;

        public ConnectServer(Context context) {
            mContext = context;
        }

        @Override
        protected Void doInBackground(Void... params) {
            try {
                Log.e(LOG_TAG, "connecting...");
                socket_1 = new Socket(host_name, Integer.parseInt(host_port_1));
                out_1 = socket_1.getOutputStream();
                socket_2 = new Socket(host_name, Integer.parseInt(host_port_2));
                out_2 = socket_2.getOutputStream();
                sensorManager.registerListener(sensorlistener, sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER), SensorManager.SENSOR_DELAY_NORMAL);

            } catch(IOException e) {
                Log.e(LOG_TAG, "error in connecting");
                Intent myIntent = new Intent(mContext,Activity1.class);
                myIntent.putExtra("error","error in establishing connection...");
                startActivity(myIntent);
            }
            return null;
        }
        @Override
        protected void onPostExecute(Void aVoid) {
            super.onPostExecute(aVoid);
            try {
                String pname="{\"name\":\""+player_name+"\"}";
                out_2.write(pname.getBytes());
            } catch (Exception e) {
                Log.e(LOG_TAG, "error in send");
            }
        }
    }

    /*
    private void start_client_1() {

        final Handler handler = new Handler();
        Thread thread = new Thread(new Runnable() {
            @Override
            public void run() {

                try {
                    socket_1 = new Socket(host_name, Integer.parseInt(host_port_1));
                    out_1 = socket_1.getOutputStream();
                    Socket s = new Socket("xxx.xxx.xxx.xxx", 9002);

                    OutputStream out = s.getOutputStream();

                    PrintWriter output = new PrintWriter(out);

                    output.println(msg);
                    output.flush();
                    BufferedReader input = new BufferedReader(new InputStreamReader(s.getInputStream()));
                    final String st = input.readLine();

                    handler.post(new Runnable() {
                        @Override
                        public void run() {

                            String s = mTextViewReplyFromServer.getText().toString();
                            if (st.trim().length() != 0)
                                mTextViewReplyFromServer.setText(s + "\nFrom Server : " + st);
                        }
                    });

                    output.close();
                    out.close();
                    s.close();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            }
        });

        thread.start();
    }*/
}

/*

import android.app.Activity;
import android.app.AlertDialog;
import android.app.IntentService;
import android.content.DialogInterface;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.util.Log;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.ScrollView;
import android.widget.TextView;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class MainActivity extends Activity {
    final static String TAG = "AndroidCheatSocket";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        final LinearLayout linearLayout = new LinearLayout(this);
        linearLayout.setOrientation(LinearLayout.VERTICAL);
        TextView textView;

        final String defaultHostname = "192.168.0.";
        textView = new TextView(this);
        textView.setText("hostname / IP:");
        linearLayout.addView(textView);
        final EditText hostnameEditText = new EditText(this);
        hostnameEditText.setText(defaultHostname);
        hostnameEditText.setSingleLine(true);
        linearLayout.addView(hostnameEditText);

        textView = new TextView(this);
        textView.setText("port:");
        linearLayout.addView(textView);
        final EditText portEditText = new EditText(this);
        portEditText.setText("12345");
        portEditText.setSingleLine(true);
        linearLayout.addView(portEditText);

        textView = new TextView(this);
        textView.setText("data to send:");
        linearLayout.addView(textView);
        final EditText dataEditText = new EditText(this);
        dataEditText.setText(String.format("GET / HTTP/1.1\r\nHost: %s\r\n\r\n", defaultHostname));
        linearLayout.addView(dataEditText);

        final TextView replyTextView = new TextView(this);
        final ScrollView replyTextScrollView = new ScrollView(this);
        replyTextScrollView.addView(replyTextView);

        final Button button = new Button(this);
        button.setText("contact server");
        button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                button.setEnabled(false);
                new MyAsyncTask(MainActivity.this, replyTextView, button).execute(
                        hostnameEditText.getText().toString(),
                        portEditText.getText().toString(),
                        dataEditText.getText().toString());

            }
        });
        linearLayout.addView(button);

        textView = new TextView(this);
        textView.setText("output:");
        linearLayout.addView(textView);
        linearLayout.addView(replyTextScrollView);

        this.setContentView(linearLayout);
    }

    private class MyAsyncTask extends AsyncTask<String, Void, String> {
        Activity activity;
        Button button;
        TextView textView;
        IOException ioException;
        MyAsyncTask(Activity activity, TextView textView, Button button) {
            super();
            this.activity = activity;
            this.textView = textView;
            this.button = button;
            this.ioException = null;
        }
        @Override
        protected String doInBackground(String... params) {
            StringBuilder sb = new StringBuilder();
            try {
                Socket socket = new Socket(
                        params[0],
                        Integer.parseInt(params[1]));
                OutputStream out = socket.getOutputStream();
                out.write(params[2].getBytes());
                InputStream in = socket.getInputStream();
                byte buf[] = new byte[1024];
                int nbytes;
                while ((nbytes = in.read(buf)) != -1) {
                    sb.append(new String(buf, 0, nbytes));
                }
                socket.close();
            } catch(IOException e) {
                this.ioException = e;
                return "error";
            }
            return sb.toString();
        }
        @Override
        protected void onPostExecute(String result) {
            if (this.ioException != null) {
                new AlertDialog.Builder(this.activity)
                        .setTitle("An error occurrsed")
                        .setMessage(this.ioException.toString())
                        .setIcon(android.R.drawable.ic_dialog_alert)
                        .show();
            } else {
                this.textView.setText(result);
            }
            this.button.setEnabled(true);
        }
    }
}
*/
