using System.Text.Encodings.Web;

namespace UriQueryEditor.Models;

public class Segment
{
	public string Key { get; set; }
	public string Value { get; set; }

	public Segment(string key = "", string value = "")
	{
		Key = key;
		Value = value;
	}

	public string Encode() => $"{UrlEncoder.Default.Encode(Key)}={UrlEncoder.Default.Encode(Value)}";
}