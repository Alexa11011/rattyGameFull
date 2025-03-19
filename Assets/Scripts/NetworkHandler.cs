using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;


public static class NetworkHandler
{

	



	public interface MessageHandler
	{
		void ProcessMessage(string messageContents);

		void ProcessMessage(MessageContents messageContents);
	}


	public class ThreadParams
	{
		private bool terminate;
		private TcpListener listener;
		private Queue<MessageContents> messages = new Queue<MessageContents>();

		public ThreadParams()
		{
			terminate = false;
		}

		public void StartListener(TcpListener tcpListener)
		{
			listener = tcpListener;
			listener.Start();
		}

		public void AddMessageToQueue(MessageContents message)
		{
			lock (messages)
			{
				messages.Enqueue(message);
			}
		}

		public bool HasMessage()
		{
			return messages.Count > 0;
		}

		public MessageContents GetNextMessage()
		{
			MessageContents nextMessage = new MessageContents();
			lock (messages)
			{
				if (messages.Count > 0)
					nextMessage = messages.Dequeue();
			}
			return nextMessage;
		}

		public TcpListener GetTcpListener()
		{
			return listener;
		}

		public void Terminate()
		{
			terminate = true;
			if (listener != null) listener.Stop();
		}

		public bool Terminating()
		{
			return terminate;
		}

		public void Reset()
		{
			terminate = false;
			listener = null;
		}
	}

	[Serializable()]
	public class MessageContents
	{
		public enum MessageType { ERROR, NOTIFICATION, EMPTY, SINGLE, MULTI };

		private MessageType messageType;

		//dictonaries hold pairs of strings
		private Dictionary<string, string> values = new Dictionary<string, string>();

		public MessageContents(Dictionary<string, string> values)
		{
			this.messageType = MessageType.MULTI;
			this.values = values;
		}

		public MessageContents(MessageType messageType, string message)
		{
			if (messageType == MessageType.ERROR)
			{
				this.messageType = MessageType.ERROR;
				this.values.Add("Error", message);
			}
			else if (messageType == MessageType.EMPTY)
			{
				this.messageType = MessageType.EMPTY;
			}
			else
			{
				this.messageType = messageType;
				this.values.Add(message, string.Empty);
			}
		}

		public MessageContents(string key, string value)
		{
			this.messageType = MessageType.ERROR;
			this.values.Add(key, value);
		}

		public MessageContents(string message)
		{
			this.messageType = MessageType.SINGLE;
			this.values.Add(message, null);
		}

		public MessageContents()
		{
			this.messageType = MessageType.EMPTY;
		}

		public MessageType GetMessageType()
		{
			return messageType;
		}

		public bool HasError()
		{
			return messageType == MessageType.ERROR;
		}

		public string Message()
		{
			switch (messageType)
			{
				case MessageType.EMPTY:
				case MessageType.MULTI:
					return string.Empty;
				default:
					return values.Keys.First();
			}
		}

		public string GetKey(int index)
		{
			return values != null && values.Keys.Count > index ? values.Keys.ElementAt(index) : string.Empty;
		}

		public string GetValue(string key)
		{
			return values != null && values.ContainsKey(key) ? values[key] : string.Empty;
		}

		public byte[] Serialize()
		{
			string completeMessage = messageType.ToString();
			foreach (string key in values.Keys)
			{
				completeMessage += "~" + key.Replace('~', ' ');
				if (values[key] != null && values[key] != string.Empty) completeMessage += "|" + values[key].Replace('|', ' ');
			}
			return Encoding.UTF8.GetBytes(completeMessage);
		}

		public static MessageContents Deserialize(byte[] data)
		{
			try
			{
				MessageType? dataMessageType = null;

				string[] messageLines = Encoding.UTF8.GetString(data).TrimEnd('\0').Split('~');
				if (messageLines.Length == 0)
				{
					return new MessageContents();   // empty message
				}

				foreach (MessageType messageType in Enum.GetValues(typeof(MessageType)))
				{
					if (messageLines[0].Equals(messageType.ToString()))
					{
						dataMessageType = messageType;
						break;
					}
				}

				if (dataMessageType == null)
				{
					if (messageLines.Length == 1)
					{
						// if only a single line message was received, return as a single
						return new MessageContents(MessageType.SINGLE, messageLines[0]);
					}

					// otherwise assume malformed message
					throw new Exception("Unknown message type '" + messageLines[0] + "'");
				}
				else if (messageLines.Length == 1)
				{
					// if a message type was sent with no contents, return an empty message
					return new MessageContents();
				}

				if ((MessageType)dataMessageType == MessageType.MULTI)
				{
					Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
					for (int i = 1; i < messageLines.Length; i++)
					{
						string[] keyValuePair = messageLines[i].Split(new char[] { '|' });
						if (keyValuePair.Length == 1)
						{
							keyValuePairs.Add(keyValuePair[0], null);
						}
						else
						{
							keyValuePairs.Add(keyValuePair[0], keyValuePair[1]);
						}
					}
					return new MessageContents(keyValuePairs);
				}
				else if ((MessageType)dataMessageType == MessageType.ERROR)
				{
					string[] keyValuePair = messageLines[1].Split(new char[] { '|' });
					return new MessageContents(keyValuePair[0], keyValuePair[1]);
				}
				else
				{
					return new MessageContents((MessageType)dataMessageType, messageLines[1]);
				}
			}
			catch (Exception ex)
			{
				return new MessageContents("Deserializer", ex.Message);
			}
		}
	}

	// to be used when actions involving return messages need to be performed on the main unity thread
	public static Thread SendTcpMessage(string ipAddress, int port, ThreadParams threadParams, MessageContents messageContents)
	{
		return SendTcpMessage(ipAddress, port, threadParams, null,  messageContents);
	}

	// to be used when actions involving return messages can be run from another thread
	public static Thread SendTcpMessage(string ipAddress, int port, MessageHandler messageHandler, MessageContents messageContents)
	{
		return SendTcpMessage(ipAddress, port, null, messageHandler,  messageContents);
	}

	private static Thread SendTcpMessage(string ipAddress, int port, ThreadParams threadParams, MessageHandler messageHandler, MessageContents messageContents)
	{
		ReturnMessage(threadParams, messageHandler, new MessageContents(MessageContents.MessageType.NOTIFICATION, "Creating message thread"));

		Thread thread = new Thread(() =>
		{
			try
			{
				ReturnMessage(threadParams, messageHandler, new MessageContents(MessageContents.MessageType.NOTIFICATION, "Sending Tcp message"));

				TcpClient client = new TcpClient(ipAddress, port);
				NetworkStream stream = client.GetStream();
				byte[] data = messageContents.Serialize();
				stream.Write(data, 0, data.Length);
				stream.Close();
				client.Close();
			}
			catch (Exception ex)
			{
				ReturnMessage(threadParams, messageHandler, new MessageContents("Server", ex.Message));
			}
		});
		thread.Start();
		return thread;
	}

	private static void ReturnMessage(ThreadParams threadParams, MessageHandler messageHandler, MessageContents messageContents)
	{
		if (messageHandler != null)
		{
			messageHandler.ProcessMessage(messageContents);
		}
		else if (threadParams != null)
		{
			threadParams.AddMessageToQueue(messageContents);
		}
	}

	public static Thread StartTcpListening(int port, ThreadParams threadParams)
	{
		return StartTcpListening(port, null, threadParams);
	}

	public static Thread StartTcpListening(int port, MessageHandler messageHandler, ThreadParams threadParams)
	{
		threadParams.StartListener(new TcpListener(IPAddress.Any, port));
		Thread thread = new Thread(() => Listener(messageHandler, threadParams));
		thread.Start();
		return thread;
	}

	private static void Listener(MessageHandler messageHandler, ThreadParams threadParams)
	{
		ReturnMessage(threadParams, messageHandler, new MessageContents(MessageContents.MessageType.NOTIFICATION, "Listener started"));

		while (!threadParams.Terminating())
		{
			try
			{
				TcpClient tcpClient = threadParams.GetTcpListener().AcceptTcpClient();
				NetworkStream stream = tcpClient.GetStream();
				byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
				stream.Read(bytes, 0, tcpClient.ReceiveBufferSize);
				ReturnMessage(threadParams, messageHandler, MessageContents.Deserialize(bytes));

				// messageHandler.ProcessMessage(Encoding.UTF8.GetString(bytes).TrimEnd('\0'));

				tcpClient.Close();
			}
			catch (Exception e)
			{
				if (!threadParams.Terminating())
					ReturnMessage(threadParams, messageHandler, new MessageContents("Listener", e.Message));
			}
		}
		ReturnMessage(threadParams, messageHandler, new MessageContents(MessageContents.MessageType.NOTIFICATION, "Listener started"));
	}

	/*
	public static void SendUdpMessage()
	{
		// can implement later if necessary
	}

	public static void StartUdpListening()
	{
		// can implement later if necessary
	}
	*/
}
