namespace MusicSchoolAsp.Helpers.HtmlStrings
{
	public static class ListGetter<T>
	{
		public static string GetList(ICollection<T> items, string? class_ = null)
		{
			string outS = "";

			string html_class = "";
			if (class_ != null)
				html_class = $"class={class_}";

			foreach (var item in items) 
			{
				outS += $"<li {html_class}>{item}</li>";
			}

			return outS;
		}

		/// <summary>
		/// Возвращает html-строку списка <li> с возможнотью добавления своих атриюутов для каждого элемента
		/// </summary>
		public static string GetList<TDataValue>(
			IDictionary<TDataValue, T> keyValuePairs,
			string? class_ = null,
			List<string>? otherAttributesValues = null
			)
		{
			string outS = "";
			string classHtml = class_ != null ? $"class={class_}" : "";
			string otherAttributesHtml = "";
			if (otherAttributesValues != null)
			{
				foreach (var attrValue in otherAttributesValues)
				{
					otherAttributesHtml += attrValue + " ";
				}
			}

			foreach (var pair in keyValuePairs)
			{
				outS += $"<li {classHtml} {otherAttributesHtml}</li>";
			}

			return outS;
		}
	}
}
