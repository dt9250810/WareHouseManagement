using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WareHouseManagement
{
    class PrintHelper
    {

        //匯出temp EXCEL方法
        public static void GetTempXlsx(DataGridView dataGridView, string title, ToolStripProgressBar progressBar)
        {
            string path = "C:\\ct_temp\\";
            string saveFileName = path + "printTemp.xlsx";
            string titleFull = "";
            progressBar.Value = 0;

            if (CheckFolder(path) == false)
                return;

            titleFull = "【秋田機械股份有限公司】(" + title + ") (製表日期：" + DateTime.Now.ToString("yyyy-MM-dd ddd HH:mm:ss") + ")";

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
            if (xlApp == null)
            {
                MessageBox.Show("無法建立Excel物件，可能您的電腦未安裝Excel", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1

            worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[1, dataGridView.ColumnCount]].Merge();
            worksheet.Cells[1, 1] = titleFull;
            worksheet.Name = "Report";



            // 創造單元格
            Microsoft.Office.Interop.Excel.Range range;
            // 起始單元格(titleFull)
            range = worksheet.Cells[1, 1];
            range.Font.Bold = true;
            range.Font.Size = 12;
            range.Font.Name = "Consolas";
            // range.Interior.ColorIndex = 16;

            //水平居中
            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            //垂直居中
            range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

            // 調整欄位最適寬度
            if (title == "貨料記錄表")
            {
                range = worksheet.Cells[2, 1];  // 編號
                range.ColumnWidth = 6.75;
                range = worksheet.Cells[2, 2];  // 型號
                range.ColumnWidth = 18;
                range = worksheet.Cells[2, 3];  // 種類名稱
                range.ColumnWidth = 16;
                range = worksheet.Cells[2, 4];  // 種類代號
                range.ColumnWidth = 11;
                range = worksheet.Cells[2, 5];  // 規格
                range.ColumnWidth = 25;
                range = worksheet.Cells[2, 6];  // 廠牌
                range.ColumnWidth = 13.5;
                range = worksheet.Cells[2, 7];  // 條碼號
                range.ColumnWidth = 13;
                range = worksheet.Cells[2, 8];  // 數量
                range.ColumnWidth = 7;
                range = worksheet.Cells[2, 9];  // 備註
                range.ColumnWidth = 18;
            }

            //寫入標題
            for (int i = 0; i < dataGridView.ColumnCount; i++)
            {
                worksheet.Cells[2, i + 1] = dataGridView.Columns[i].HeaderText;
                range = worksheet.Cells[2, i + 1];
                range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
                range.Font.Bold = true;
                range.Font.Size = 14;
                range.Font.Name = "Consolas";
                range.Interior.Color = Color.LightGray;
                range.Borders.LineStyle = 1;
            }

            progressBar.Visible = true;
            progressBar.Maximum = dataGridView.Rows.Count * dataGridView.ColumnCount;

            //寫入數值
            for (int r = 0; r < dataGridView.Rows.Count; r++)
            {
                for (int i = 0; i < dataGridView.ColumnCount; i++)
                {
                    range = worksheet.Cells[r + 3, i + 1];
                    range.Font.Name = "Consolas";
                    range.Borders.LineStyle = 1; // 表格框線
                    range.WrapText = true; // 自動換行

                    if ((r + 1) % 3 == 0)
                        range.Interior.Color = Color.FromArgb(224, 255, 255);

                    worksheet.Cells[r + 3, i + 1] = dataGridView.Rows[r].Cells[i].Value;

                    // 進度條
                    progressBar.Value += 1;
                }
                System.Windows.Forms.Application.DoEvents();
            }

            worksheet.Columns.EntireColumn.AutoFit();//列寬自適應
                                                     //worksheet.Columns.NumberFormatLocal = "@";
            progressBar.Visible = false;
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("匯出文件時出錯,文件可能正被開啟！\n" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            xlApp.Quit();
            worksheet = null;
            GC.Collect();//強行銷燬
        }

        public static Boolean CheckFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
    }
}