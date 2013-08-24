using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Logger : LazySingleton<Logger> {
	
	public enum LogChannel {
		GFX,
		PHX,
		SND,
		UI,
		ALL,
	}
	
	public enum LogSeverity {
		INFO,
		WARNING,
		ERROR,
	}
	
	public struct LogMessage {
		LogChannel channel;
		LogSeverity severity;
		object message;
		object context;
		
		public LogMessage(object _message, object _context, LogChannel _channel, LogSeverity _severity) {
			message = _message;
			context = _context;
			channel = _channel;
			severity = _severity;
		}
		
		public override string ToString() {
			if (string.IsNullOrEmpty(context.ToString()))
			{
				return string.Format("{0}: [{1}] {2}", severity.ToString(), channel.ToString(), message.ToString());
			}
			else {
				return string.Format("{0}: {1}: [{2}] {3}", context.ToString(), severity.ToString(), channel.ToString(), message.ToString());
			}
		}
		
	};
	
	public Dictionary<LogChannel, LogSeverity> channels = new Dictionary<LogChannel, LogSeverity>();
	public List<LogMessage> savedMessages = new List<LogMessage>();
	
	void Awake() {
		for (int i = 0; i < (int)LogChannel.ALL + 1; ++i) {
			channels.Add((LogChannel)i, LogSeverity.INFO);
		}
	}
	
	public static void Log(object message, LogChannel channel = LogChannel.ALL) {
		Debug.Log(message);
		LogSeverity severity = LogSeverity.INFO;
		if (Instance.channels[channel] <= severity) {
			string context = "";
#if DEBUG
			string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
			int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
			context = string.Format("{0}({1})", currentFile, currentLine);
#endif
			Instance.savedMessages.Add(new LogMessage(message, context, channel, severity));
		}
	}
	
	public static void LogWarning(object message, LogChannel channel = LogChannel.ALL) {
		Debug.LogWarning(message);
		LogSeverity severity = LogSeverity.WARNING;
		if (Instance.channels[channel] <= severity) {
			string context = "";
#if DEBUG
			string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
			int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
			context = string.Format("{0}({1})", currentFile, currentLine);
#endif
			Instance.savedMessages.Add(new LogMessage(message, context, channel, severity));
		}
	}
	
	public static void LogError(object message, LogChannel channel = LogChannel.ALL) {
		Debug.LogError(message);
		LogSeverity severity = LogSeverity.ERROR;
		if (Instance.channels[channel] <= severity) {
			string context = "";
#if DEBUG
			string currentFile = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();
			int currentLine = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileLineNumber();
			context = string.Format("{0}({1})", currentFile, currentLine);
#endif
			Instance.savedMessages.Add(new LogMessage(message, context, channel, severity));
		}
	}
	
}
