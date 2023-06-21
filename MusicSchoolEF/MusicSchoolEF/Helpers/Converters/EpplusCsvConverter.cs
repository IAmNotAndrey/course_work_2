using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MusicSchoolEF.Helpers.Converters
{
	public static class EpplusCsvConverter
	{
		public static byte[] ConvertToCsv(this ExcelPackage package)
		{
			var worksheet = package.Workbook.Worksheets[0];

			var maxColumnNumber = worksheet.Dimension.End.Column;
			var currentRow = new List<string>(maxColumnNumber);
			var totalRowCount = worksheet.Dimension.End.Row;
			var currentRowNum = 1;

			var memory = new MemoryStream();

			using (var writer = new StreamWriter(memory, Encoding.UTF8))
			{
				while (currentRowNum <= totalRowCount)
				{
					BuildRow(worksheet, currentRow, currentRowNum, maxColumnNumber);
					WriteRecordToFile(currentRow, writer, currentRowNum, totalRowCount);
					currentRow.Clear();
					currentRowNum++;
				}
			}

			return memory.ToArray();
		}

		private static void WriteRecordToFile(List<string> record, StreamWriter sw, int rowNumber, int totalRowCount)
		{
			var commaDelimitedRecord = record.ToDelimitedString(",");

			if (rowNumber == totalRowCount)
			{
				sw.Write(commaDelimitedRecord);
			}
			else
			{
				sw.WriteLine(commaDelimitedRecord);
			}
		}

		private static void BuildRow(ExcelWorksheet worksheet, List<string> currentRow, int currentRowNum, int maxColumnNumber)
		{
			for (int i = 1; i <= maxColumnNumber; i++)
			{
				var cell = worksheet.Cells[currentRowNum, i];
				if (cell == null)
				{
					// add a cell value for empty cells to keep data aligned.
					AddCellValue(string.Empty, currentRow);
				}
				else
				{
					AddCellValue(GetCellText(cell), currentRow);
				}
			}
		}

		private static string GetCellText(ExcelRangeBase cell)
		{
			return cell.Value == null ? string.Empty : cell.Value.ToString();
		}

		private static void AddCellValue(string s, List<string> record)
		{
			record.Add(string.Format("{0}{1}{0}", '"', s));
		}
	}
}
