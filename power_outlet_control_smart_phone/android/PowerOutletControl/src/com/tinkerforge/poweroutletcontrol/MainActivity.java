package com.tinkerforge.poweroutletcontrol;

import android.os.Bundle;
import android.app.Activity;
import android.app.AlertDialog;
import android.app.ProgressDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.os.AsyncTask;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;
import android.widget.EditText;

import com.tinkerforge.BrickletIndustrialQuadRelay;
import com.tinkerforge.IPConnection;
import com.tinkerforge.TinkerforgeException;
import com.tinkerforge.NotConnectedException;
import com.tinkerforge.AlreadyConnectedException;

public class MainActivity extends Activity {
	final Context context = this;
	private IPConnection ipcon = null;
	private BrickletIndustrialQuadRelay relay = null;
	private EditText host;
	private EditText port;
	private EditText uid;
	private Button connect;
	private Button a_on;
	private Button a_off;
	private Button b_on;
	private Button b_off;

	enum ConnectResult {
		SUCCESS,
		NO_CONNECTION,
		NO_DEVICE
	}

	class ConnectAsyncTask extends AsyncTask<Void, Void, ConnectResult> {
		private ProgressDialog progressDialog;
		private String currentHost;
		private String currentPort;
		private String currentUID;

		@Override
		protected void onPreExecute() {
			currentHost = host.getText().toString();
			currentPort = port.getText().toString();
			currentUID = uid.getText().toString();

			if (currentHost.length() == 0 || currentPort.length() == 0 || currentUID.length() == 0) {
				AlertDialog.Builder builder = new AlertDialog.Builder(context);
				builder.setMessage("Host/Port/UID cannot be empty");
				builder.create().show();
				cancel(true);
				return;
			}

			host.setEnabled(false);
			port.setEnabled(false);
			uid.setEnabled(false);
			connect.setEnabled(false);
			a_on.setEnabled(false);
			a_off.setEnabled(false);
			b_on.setEnabled(false);
			b_off.setEnabled(false);

			progressDialog = new ProgressDialog(context);
			progressDialog.setMessage("Connecting to " + currentHost + ":" + currentPort);
			progressDialog.setCancelable(false);
			progressDialog.show();
		}

		protected ConnectResult doInBackground(Void... params) {
			ipcon = new IPConnection();
			relay = new BrickletIndustrialQuadRelay(currentUID, ipcon);

			try {
				ipcon.connect(currentHost, Integer.parseInt(currentPort));
			} catch(java.net.UnknownHostException e) {
				return ConnectResult.NO_CONNECTION;
			} catch(java.io.IOException e) {
				return ConnectResult.NO_CONNECTION;
			} catch(AlreadyConnectedException e) {
				return ConnectResult.NO_CONNECTION;
			}

			try {
				if (relay.getIdentity().deviceIdentifier != BrickletIndustrialQuadRelay.DEVICE_IDENTIFIER) {
					ipcon.disconnect();
					return ConnectResult.NO_DEVICE;
				}
			} catch (TinkerforgeException e1) {
				try {
					ipcon.disconnect();
				} catch (NotConnectedException e2) {
				}

				return ConnectResult.NO_DEVICE;
			}

			return ConnectResult.SUCCESS;
		}

		@Override
		protected void onPostExecute(ConnectResult result) {
			progressDialog.dismiss();

			if (result == ConnectResult.SUCCESS) {
				connect.setText("Disconnect");
				connect.setOnClickListener(new DisconnectClickListener());
				connect.setEnabled(true);
				a_on.setEnabled(true);
				a_off.setEnabled(true);
				b_on.setEnabled(true);
				b_off.setEnabled(true);
			} else {
				AlertDialog.Builder builder = new AlertDialog.Builder(context);

				if (result == ConnectResult.NO_CONNECTION) {
					builder.setMessage("Could not connect to " + currentHost + ":" + currentPort);
				} else { // ConnectResult.NO_DEVICE
					builder.setMessage("Could not find Industrial Quad Relay Bricklet [" + currentUID + "]");
				}

				builder.setCancelable(false);
				builder.setPositiveButton("Retry", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						dialog.dismiss();
						new ConnectAsyncTask().execute();
					}
				});
				builder.setNegativeButton("Cancel", new DialogInterface.OnClickListener() {
					@Override
					public void onClick(DialogInterface dialog, int which) {
						host.setEnabled(true);
						port.setEnabled(true);
						uid.setEnabled(true);
						connect.setText("Connect");
						connect.setOnClickListener(new ConnectClickListener());
						connect.setEnabled(true);
						dialog.dismiss();
					}
				});
				builder.create().show();
			}
		}
	}

	class DisconnectAsyncTask extends AsyncTask<Void, Void, Boolean> {
		@Override
		protected void onPreExecute() {
			connect.setEnabled(false);
			a_on.setEnabled(false);
			a_off.setEnabled(false);
			b_on.setEnabled(false);
			b_off.setEnabled(false);
		}

		protected Boolean doInBackground(Void... params) {
			try {
				ipcon.disconnect();
				return true;
			} catch(TinkerforgeException e) {
				return false;
			}
		}

		@Override
		protected void onPostExecute(Boolean result) {
			if (result) {
				host.setEnabled(true);
				port.setEnabled(true);
				uid.setEnabled(true);
				connect.setText("Connect");
				connect.setOnClickListener(new ConnectClickListener());
			}

			connect.setEnabled(true);
		}
	}

	class TriggerAsyncTask extends AsyncTask<Void, Void, Void> {
		private int selectionMask;
		
		TriggerAsyncTask(int selectionMask) {
			this.selectionMask = selectionMask;
		}
		
		protected Void doInBackground(Void... params) {
			try {
				relay.setMonoflop(selectionMask, 15, 1500);
			} catch (TinkerforgeException e) {
			}

			return null;
		}
	}

	class ConnectClickListener implements OnClickListener {
		public void onClick(View v) {
			new ConnectAsyncTask().execute();
		}
	}

	class DisconnectClickListener implements OnClickListener {
		public void onClick(View v) {
			new DisconnectAsyncTask().execute();
		}
	}

	class TriggerClickListener implements OnClickListener {
		private int selectionMask;
		
		TriggerClickListener(int selectionMask) {
			this.selectionMask = selectionMask;
		}
		
		public void onClick(View v) {
			new TriggerAsyncTask(selectionMask).execute();
		}
	}

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_main);

		host = (EditText)findViewById(R.id.host);
		port = (EditText)findViewById(R.id.port);
		uid = (EditText)findViewById(R.id.uid);
		connect = (Button)findViewById(R.id.connect);
		a_on = (Button)findViewById(R.id.a_on);
		a_off = (Button)findViewById(R.id.a_off);
		b_on = (Button)findViewById(R.id.b_on);
		b_off = (Button)findViewById(R.id.b_off);

		SharedPreferences settings = getPreferences(0);
		host.setText(settings.getString("host", host.getText().toString()));
		port.setText(settings.getString("port", port.getText().toString()));
		uid.setText(settings.getString("uid", uid.getText().toString()));

		connect.setOnClickListener(new ConnectClickListener());
		a_on.setOnClickListener(new TriggerClickListener((1 << 0) | (1 << 2)));
		a_off.setOnClickListener(new TriggerClickListener((1 << 0) | (1 << 3)));
		b_on.setOnClickListener(new TriggerClickListener((1 << 1) | (1 << 2)));
		b_off.setOnClickListener(new TriggerClickListener((1 << 1) | (1 << 3)));
		
		a_on.setEnabled(false);
		a_off.setEnabled(false);
		b_on.setEnabled(false);
		b_off.setEnabled(false);

		if (savedInstanceState != null && savedInstanceState.getBoolean("connected", false)) {
			new ConnectAsyncTask().execute();
		}
	}

	@Override
	protected void onSaveInstanceState(Bundle outState) {
		super.onSaveInstanceState(outState);

		boolean connected = false;

		if (ipcon != null) {
			connected = ipcon.getConnectionState() == IPConnection.CONNECTION_STATE_CONNECTED;
		}

		outState.putBoolean("connected", connected);
	}

	@Override
	protected void onStop() {
		super.onStop();

		SharedPreferences settings = getPreferences(0);
		SharedPreferences.Editor editor = settings.edit();

		editor.putString("host", host.getText().toString());
		editor.putString("port", port.getText().toString());
		editor.putString("uid", uid.getText().toString());
		editor.commit();
	}
}
