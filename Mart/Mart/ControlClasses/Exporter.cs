using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Mart
{
    public class Exporter
    {
        public static void DataGridViewToExel(DataGridView dgv)
        {
            if (dgv.Rows.Count > 0)
            {
                 SaveFileDialog sfd = new SaveFileDialog();
                 sfd.Filter = "Excel Workbook |*.xlsx | Excel Workbook | *.xls";
                 sfd.ValidateNames = true;
                
                 if (sfd.ShowDialog() == DialogResult.OK)
                 {
                     /* creating Excel Application  */
                      Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
                     
                    /* creating new WorkBook within Excel application  */
                     Microsoft.Office.Interop.Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);
                     
                     /* creating new Excelsheet in workbook */
                     Microsoft.Office.Interop.Excel.Worksheet worksheet = null;                        
                     worksheet = workbook.ActiveSheet;
                       
                     /* storing header part in Excel */
                     for (int i = 1; i < dgv.Columns.Count + 1; i++)                 
                        worksheet.Cells[1, i] = dgv.Columns[i - 1].HeaderText;                 
                     
                     /* storing Each row and column value to excel sheet */
                     for (int i = 0; i < dgv.Rows.Count; i++)                 
                        for (int j = 0; j < dgv.Columns.Count; j++)
                            worksheet.Cells[i + 2, j + 1] = dgv.Rows[i].Cells[j].Value.ToString();                                     
                     
                     /* save file to a specific location depend on SaveFileDialog */
                     workbook.SaveAs(sfd.FileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);                        
                     app.Quit();
                }
            }
        }
    }
}
