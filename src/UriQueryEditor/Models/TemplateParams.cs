using System.Text;
using System.Text.RegularExpressions;

namespace UriQueryEditor.Models
{
	public record TemplateParams
	{
		public static readonly IReadOnlyList<TemplateRegex> Templates = new TemplateRegex[]
		{
			new("Grafana | ${}", @"\$\{[^\}]*\}")
		};

		public static (string output, List<Segment> parameters) Extract(string input, string pattern)
		{
			if (string.IsNullOrWhiteSpace(pattern))
				return (input, new List<Segment>());

			var sb = new StringBuilder();
			var pos = 0;
			List<Segment> parameters = new();
			foreach (Match match in Regex.Matches(input, pattern))
			{
				if (match.Index > pos)
					sb.Append(input.AsSpan(pos, match.Index - pos));
				var segment = new Segment(GetKeyForIndex(parameters.Count), match.Value);
				sb.Append(segment.Key);
				parameters.Add(segment);
				pos = match.Index + match.Length;
			}

			if (pos < input.Length)
				sb.Append(input.AsSpan(pos));
			return (sb.ToString(), parameters);
		}

		public static string Apply(string input, IReadOnlyList<Segment> parameters)
		{
			foreach (var t in parameters)
				input = input.Replace(t.Key, t.Value);
			return input;
		}

		public static string GetKeyForIndex(int index) => $"_$${index}_";
	}

	public record TemplateRegex(string Name, string Pattern);
}
