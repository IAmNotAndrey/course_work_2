using MusicSchoolEF.Models.Db;
using MusicSchoolEF.Models.Defaults;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace MusicSchoolEF.Helpers.ReportBuilders
{
    public static class ReportBuilder
    {
        public static ExcelPackage GetStudentTaskExcelReport(TreeNode<StudentNodeConnection> tree)
        {
            var package = new ExcelPackage();
            var worksheet = package.Workbook.Worksheets.Add("Отчёт");

            // Заголовки столбцов
            worksheet.Cells[1, 1].Value = "Название занятия";
            worksheet.Cells[1, 2].Value = "Описание занятия";
            worksheet.Cells[1, 3].Value = "Оценка";
            worksheet.Cells[1, 4].Value = "Комментарий";
            worksheet.Cells[1, 5].Value = "Преподаватель";

            // Установка стилей заголовков
            var headerCells = worksheet.Cells[1, 1, 1, 5];
            headerCells.Style.Font.Bold = true;
            headerCells.Style.Fill.PatternType = ExcelFillStyle.Solid;
            headerCells.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

            // Заполнение данными
            int rowIndex = 2;
            bool isFirstItem = true;
            foreach (var item in tree)
            {
                if (isFirstItem)
                {
                    isFirstItem = false;
                    continue;
                }

                var snc = item.TreeNode.Value!;
                var level = item.Level;

                string space = "  ";
				space = String.Concat(Enumerable.Repeat(space, level));

				worksheet.Cells[rowIndex, 1].Value = space + snc.NodeNavigation.Name;
                worksheet.Cells[rowIndex, 2].Value = snc.NodeNavigation.Description;
                worksheet.Cells[rowIndex, 3].Value = snc.Mark;
                worksheet.Cells[rowIndex, 4].Value = snc.Comment;
                worksheet.Cells[rowIndex, 5].Value = snc.NodeNavigation.OwnerNavigation.FullName;

                rowIndex++;
            }

            // Автоматическое изменение ширины столбцов для лучшей читаемости
            worksheet.Cells.AutoFitColumns();

            return package;
        }
    }
}
