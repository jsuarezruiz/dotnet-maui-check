﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCheck.Models
{
	public class SharedState
	{
		Dictionary<string, string> envVars = new Dictionary<string, string>();
		Dictionary<string, Dictionary<string, object>> charts = new Dictionary<string, Dictionary<string, object>>();

		public void ContributeState(Checkup checkup, string key, object value)
		{
			var checkupId = checkup.Id;

			if (!charts.ContainsKey(checkupId))
				charts.Add(checkupId, new Dictionary<string, object>());

			charts[checkupId][key] = value;
		}

		public bool TryGetState<T>(string checkupId, string key, out T notes) where T : class
		{
			notes = default;

			if (charts.TryGetValue(checkupId, out var checkupNotes) && checkupNotes.TryGetValue(key, out var value))
				notes = value as T;

			return notes != default;
		}

		public void SetEnvironmentVariable(string name, string value)
		{
			envVars[name] = value;

			Util.EnvironmentVariables[name] = value;
		}

		public string GetEnvironmentVariable(string name)
			=> envVars.ContainsKey(name) ? envVars?[name] : null;

		public bool TryGetEnvironmentVariable(string name, out string value)
			=> envVars.TryGetValue(name, out value);

		public IEnumerable<KeyValuePair<string, string>> GetEnvironmentVariables()
			=> envVars;
	}
}
