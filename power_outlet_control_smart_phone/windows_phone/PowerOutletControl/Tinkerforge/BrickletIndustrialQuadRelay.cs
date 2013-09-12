/* ***********************************************************
 * This file was automatically generated on 2013-09-12.      *
 *                                                           *
 * Bindings Version 2.0.11                                    *
 *                                                           *
 * If you have a bugfix for this file and want to commit it, *
 * please fix the bug in the generator. You can find a link  *
 * to the generator git on tinkerforge.com                   *
 *************************************************************/

using System;

namespace Tinkerforge
{
	/// <summary>
	///  Device for controlling up to 4 Solid State Relays
	/// </summary>
	public class BrickletIndustrialQuadRelay : Device
	{
		/// <summary>
		///  Used to identify this device type in
		///  <see cref="Tinkerforge.IPConnection.EnumerateCallback"/>
		/// </summary>
		public static int DEVICE_IDENTIFIER = 225;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_SET_VALUE = 1;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_GET_VALUE = 2;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_SET_MONOFLOP = 3;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_GET_MONOFLOP = 4;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_SET_GROUP = 5;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_GET_GROUP = 6;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_GET_AVAILABLE_FOR_GROUP = 7;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte CALLBACK_MONOFLOP_DONE = 8;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_SET_SELECTED_VALUES = 9;

		/// <summary>
		///  Function ID to be used with
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetResponseExpected"/>,
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpected"/> and
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetResponseExpectedAll"/>.
		/// </summary>
		public static byte FUNCTION_GET_IDENTITY = 255;



		/// <summary>
		///  This callback is triggered whenever a monoflop timer reaches 0. The
		///  parameters contain the involved pins and the current value of the pins
		///  (the value after the monoflop).
		/// </summary>
		public event MonoflopDoneEventHandler MonoflopDone;
		/// <summary>
		/// </summary>
		public delegate void MonoflopDoneEventHandler(BrickletIndustrialQuadRelay sender, int selectionMask, int valueMask);

		/// <summary>
		///  Creates an object with the unique device ID <c>uid</c> and adds  it to
		///  the IPConnection <c>ipcon</c>.
		/// </summary>
		public BrickletIndustrialQuadRelay(string uid, IPConnection ipcon) : base(uid, ipcon)
		{
			this.apiVersion[0] = 2;
			this.apiVersion[1] = 0;
			this.apiVersion[2] = 0;
			callbackWrappers[CALLBACK_MONOFLOP_DONE] = new CallbackWrapper(OnMonoflopDone);

			responseExpected[FUNCTION_SET_VALUE] = ResponseExpectedFlag.FALSE;
			responseExpected[FUNCTION_GET_VALUE] = ResponseExpectedFlag.ALWAYS_TRUE;
			responseExpected[FUNCTION_SET_MONOFLOP] = ResponseExpectedFlag.FALSE;
			responseExpected[FUNCTION_GET_MONOFLOP] = ResponseExpectedFlag.ALWAYS_TRUE;
			responseExpected[FUNCTION_SET_GROUP] = ResponseExpectedFlag.FALSE;
			responseExpected[FUNCTION_GET_GROUP] = ResponseExpectedFlag.ALWAYS_TRUE;
			responseExpected[FUNCTION_GET_AVAILABLE_FOR_GROUP] = ResponseExpectedFlag.ALWAYS_TRUE;
			responseExpected[FUNCTION_SET_SELECTED_VALUES] = ResponseExpectedFlag.FALSE;
			responseExpected[FUNCTION_GET_IDENTITY] = ResponseExpectedFlag.ALWAYS_TRUE;
			responseExpected[CALLBACK_MONOFLOP_DONE] = ResponseExpectedFlag.ALWAYS_FALSE;
		}

		/// <summary>
		///  Sets the output value with a bitmask. The bitmask
		///  is 16 bit long, *true* refers to a closed relay and *false* refers to 
		///  an open relay.
		///  
		///  For example: The value 0b0000000000000011 will close the relay 
		///  of pins 0-1 and open the other pins.
		///  
		///  If no groups are used (see <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetGroup"/>), the pins correspond to the
		///  markings on the Quad Relay Bricklet.
		///  
		///  If groups are used, the pins correspond to the element in the group.
		///  Element 1 in the group will get pins 0-3, element 2 pins 4-7, element 3
		///  pins 8-11 and element 4 pins 12-15.
		/// </summary>
		public void SetValue(int valueMask)
		{
			byte[] request = CreateRequestPacket(10, FUNCTION_SET_VALUE);
			LEConverter.To((short)valueMask, 8, request);

			SendRequest(request);

		}

		/// <summary>
		///  Returns the bitmask as set by <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetValue"/>.
		/// </summary>
		public int GetValue()
		{
			byte[] request = CreateRequestPacket(8, FUNCTION_GET_VALUE);

			byte[] response = SendRequest(request);

			return LEConverter.UShortFrom(8, response);
		}

		/// <summary>
		///  Configures a monoflop of the pins specified by the first parameter
		///  bitmask.
		///  
		///  The second parameter is a bitmask with the desired value of the specified
		///  pins (*true* means relay closed and *false* means relay open).
		///  
		///  The third parameter indicates the time (in ms) that the pins should hold
		///  the value.
		///  
		///  If this function is called with the parameters 
		///  ((1 &lt;&lt; 0) | (1 &lt;&lt; 3), (1 &lt;&lt; 0), 1500):
		///  Pin 0 will close and pin 3 will open. In 1.5s pin 0 will open and pin
		///  3 will close again.
		///  
		///  A monoflop can be used as a fail-safe mechanism. For example: Lets assume you
		///  have a RS485 bus and a Quad Relay Bricklet connected to one of the slave
		///  stacks. You can now call this function every second, with a time parameter
		///  of two seconds and pin 0 closed. Pin 0 will be closed all the time. If now
		///  the RS485 connection is lost, then pin 0 will be opened in at most two seconds.
		/// </summary>
		public void SetMonoflop(int selectionMask, int valueMask, long time)
		{
			byte[] request = CreateRequestPacket(16, FUNCTION_SET_MONOFLOP);
			LEConverter.To((short)selectionMask, 8, request);
			LEConverter.To((short)valueMask, 10, request);
			LEConverter.To((int)time, 12, request);

			SendRequest(request);

		}

		/// <summary>
		///  Returns (for the given pin) the current value and the time as set by
		///  <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetMonoflop"/> as well as the remaining time until the value flips.
		///  
		///  If the timer is not running currently, the remaining time will be returned
		///  as 0.
		/// </summary>
		public void GetMonoflop(byte pin, out int value, out long time, out long timeRemaining)
		{
			byte[] request = CreateRequestPacket(9, FUNCTION_GET_MONOFLOP);
			LEConverter.To((byte)pin, 8, request);

			byte[] response = SendRequest(request);

			value = LEConverter.UShortFrom(8, response);
			time = LEConverter.UIntFrom(10, response);
			timeRemaining = LEConverter.UIntFrom(14, response);
		}

		/// <summary>
		///  Sets a group of Quad Relay Bricklets that should work together. You can
		///  find Bricklets that can be grouped together with <see cref="Tinkerforge.BrickletIndustrialQuadRelay.GetAvailableForGroup"/>.
		///  
		///  The group consists of 4 elements. Element 1 in the group will get pins 0-3,
		///  element 2 pins 4-7, element 3 pins 8-11 and element 4 pins 12-15.
		///  
		///  Each element can either be one of the ports ('a' to 'd') or 'n' if it should
		///  not be used.
		///  
		///  For example: If you have two Quad Relay Bricklets connected to port A and
		///  port B respectively, you could call with "['a', 'b', 'n', 'n']".
		///  
		///  Now the pins on the Quad Relay on port A are assigned to 0-3 and the
		///  pins on the Quad Relay on port B are assigned to 4-7. It is now possible
		///  to call <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetValue"/> and control two Bricklets at the same time.
		/// </summary>
		public void SetGroup(char[] group)
		{
			byte[] request = CreateRequestPacket(12, FUNCTION_SET_GROUP);
			LEConverter.To((char[])group, 8, 4, request);

			SendRequest(request);

		}

		/// <summary>
		///  Returns the group as set by <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetGroup"/>
		/// </summary>
		public char[] GetGroup()
		{
			byte[] request = CreateRequestPacket(8, FUNCTION_GET_GROUP);

			byte[] response = SendRequest(request);

			return LEConverter.CharArrayFrom(8, response, 4);
		}

		/// <summary>
		///  Returns a bitmask of ports that are available for grouping. For example the
		///  value 0b0101 means: Port *A* and Port *C* are connected to Bricklets that
		///  can be grouped together.
		/// </summary>
		public byte GetAvailableForGroup()
		{
			byte[] request = CreateRequestPacket(8, FUNCTION_GET_AVAILABLE_FOR_GROUP);

			byte[] response = SendRequest(request);

			return LEConverter.ByteFrom(8, response);
		}

		/// <summary>
		///  Sets the output value with a bitmask, according to the selection mask. 
		///  The bitmask is 16 bit long, *true* refers to a closed relay and 
		///  *false* refers to an open relay.
		///  
		///  For example: The values 00b0000000000000011, b0000000000000001 will close 
		///  the relay of pin 0, open the relay of pin 1 and leave the others untouched.
		///  
		///  If no groups are used (see <see cref="Tinkerforge.BrickletIndustrialQuadRelay.SetGroup"/>), the pins correspond to the
		///  markings on the Quad Relay Bricklet.
		///  
		///  If groups are used, the pins correspond to the element in the group.
		///  Element 1 in the group will get pins 0-3, element 2 pins 4-7, element 3
		///  pins 8-11 and element 4 pins 12-15.
		///  
		///  .. versionadded:: 2.0.0~(Plugin)
		/// </summary>
		public void SetSelectedValues(int selectionMask, int valueMask)
		{
			byte[] request = CreateRequestPacket(12, FUNCTION_SET_SELECTED_VALUES);
			LEConverter.To((short)selectionMask, 8, request);
			LEConverter.To((short)valueMask, 10, request);

			SendRequest(request);

		}

		/// <summary>
		///  Returns the UID, the UID where the Bricklet is connected to, 
		///  the position, the hardware and firmware version as well as the
		///  device identifier.
		///  
		///  The position can be 'a', 'b', 'c' or 'd'.
		///  
		///  The device identifiers can be found :ref:`here &lt;device_identifier&gt;`.
		///  
		///  .. versionadded:: 2.0.0~(Plugin)
		/// </summary>
		public override void GetIdentity(out string uid, out string connectedUid, out char position, out byte[] hardwareVersion, out byte[] firmwareVersion, out int deviceIdentifier)
		{
			byte[] request = CreateRequestPacket(8, FUNCTION_GET_IDENTITY);

			byte[] response = SendRequest(request);

			uid = LEConverter.StringFrom(8, response, 8);
			connectedUid = LEConverter.StringFrom(16, response, 8);
			position = LEConverter.CharFrom(24, response);
			hardwareVersion = LEConverter.ByteArrayFrom(25, response, 3);
			firmwareVersion = LEConverter.ByteArrayFrom(28, response, 3);
			deviceIdentifier = LEConverter.UShortFrom(31, response);
		}

		/// <summary>
		/// </summary>
		protected void OnMonoflopDone(byte[] response)
		{
			int selectionMask = LEConverter.UShortFrom(8, response);
			int valueMask = LEConverter.UShortFrom(10, response);

			var handler = MonoflopDone;
			if(handler != null)
			{
				handler(this, selectionMask, valueMask);
			}
		}
	}
}
