using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapBuilderWpf.Helpers
{
	public static class EventHelper
	{
		public static event EventHandler<jsonMessageArgs> jsonMessageEvent;
		public static event EventHandler<dynamicMessagesArgs> dynamicMessageEvent;

		public static void jsonMessage(object sender, string message, string target)
		{
			jsonMessageArgs e = new jsonMessageArgs(message, target);
			jsonMessageEvent?.Invoke(sender, e);
		}

		public static void dynamicMessage(object sender, dynamic message, string target)
		{
			dynamicMessagesArgs e = new dynamicMessagesArgs(message, target);
			dynamicMessageEvent?.Invoke(sender, e);
		}
	}

	public class jsonMessageArgs : EventArgs
	{
		public jsonMessageArgs(string args, string target)
		{
			_args = args;
			_target = target;
		}

		private dynamic _args;
		public dynamic args { get { return _args; } }
		private string _target;
		public string target { get { return _target; } }
	}

	public class dynamicMessagesArgs : EventArgs
	{
		public dynamicMessagesArgs(dynamic args, string target)
		{
			_args = args;
			_target = target;
		}

		private dynamic _args;
		public dynamic args { get { return _args; } }
		private string _target;
		public string target { get { return _target; } }
	}
}
