// Developer Express Code Central Example:
// How to merge cells horizontally in GridView via the CustomDrawCell event
// 
// This example illustrates an approach that is similar to that one described in
// the following thread: http://www.devexpress.com/scid=E2472.
// The mentioned
// functionality is implemented via the GridView.CustomDrawCell event handling.
// In
// addition, this example illustrates how to implement merged text scrolling.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E4039

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System.Drawing;
using DevExpress.XtraGrid.Views.Printing;
using DevExpress.XtraPrinting;
using System.Reflection;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;

namespace HorizontalMerging {
    public partial class Form1 : Form {
        MyGridViewHandler ViewHandler = null;
        public Form1() {
            InitializeComponent();
            ViewHandler = new MyGridViewHandler(gridView1);
        }

        private DataTable CreateTable(int RowCount) {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Name1", typeof(string));
            tbl.Columns.Add("Name2", typeof(string));
            tbl.Columns.Add("Name3", typeof(string));
            tbl.Columns.Add("Name4", typeof(string));
            tbl.Columns.Add("Name5", typeof(string));
            tbl.Columns.Add("Name6", typeof(bool));
            for (int i = 0; i < RowCount; i++) {
                if (i == 1)
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "", "", true });

                else
                    tbl.Rows.Add(new object[] { String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), "", false });
            }
            return tbl;
        }

        private void Form1_Load(object sender, EventArgs e) {
            gridControl1.DataSource = CreateTable(20);
            gridView1.Columns["Name6"].Visible = false;
            gridView1.Columns[4].Width = 300;
            gridControl1.ForceInitialize();
            ViewHandler.MergeCells(gridView1.GetRowCellValue(1, "Name2").ToString(), gridView1.GetDataSourceRowIndex(1), new GridColumn[] { gridView1.Columns[1], gridView1.Columns[2], gridView1.Columns[3], gridView1.Columns[4] });
         }

        private void gridView1_BeforePrintRow(object sender, DevExpress.XtraGrid.Views.Printing.CancelPrintRowEventArgs e) {
            if (e.RowHandle >= 0) {
                if (!(sender is GridView))
                    return;
                GridView gridView = ((GridView)sender);
                DataRowView dataRow = gridView.GetRow(e.RowHandle) as DataRowView;
                if (dataRow == null)
                    return;

                if ((bool)dataRow["Name6"]) {
                    GridViewInfo vi = (GridViewInfo)gridView.GetViewInfo();
                    PropertyInfo pi = typeof(BaseView).GetProperty("PrintInfo", BindingFlags.Instance | BindingFlags.NonPublic);
                    GridViewPrintInfo printInfo = (GridViewPrintInfo)pi.GetValue(gridView, null);
                    

                    GridRowInfo gridRowInfo = vi.GetGridRowInfo(e.RowHandle);
                    SizeF clientPageSize = (e.BrickGraphics as BrickGraphics).ClientPageSize;


                    //Draw other cells
                    TextBrick tb = new TextBrick();
                    tb.Style.Padding = new PaddingInfo(2,0,0,0);
                    tb.BackColor = gridView.Appearance.Row.BackColor;
                    tb.Text = (string)dataRow["Name1"];
                    tb.HorzAlignment = HorzAlignment.Near;
                    float textBrickHeight = vi.ColumnRowHeight;
                    GridCellInfo info = vi.GetGridCellInfo(e.RowHandle, gridView.Columns[0]);
                    RectangleF textBrickRect = new RectangleF(e.X, e.Y, printInfo.Columns[0].Bounds.Width, (int)textBrickHeight);
                    e.BrickGraphics.DrawBrick(tb, textBrickRect);
                    
                    //Draw merged cells
                    TextBrick mergedTextBreak = new TextBrick();
                    mergedTextBreak.Style.Padding = new PaddingInfo(2, 0, 0, 0);
                    mergedTextBreak.BackColor = gridView.Appearance.Row.BackColor;
                    mergedTextBreak.Text = (string)dataRow["Name2"];
                    mergedTextBreak.HorzAlignment = HorzAlignment.Near;

                    GridCellInfo mergedInfo = vi.GetGridCellInfo(e.RowHandle, gridView.Columns[1]);
                    RectangleF textMergedBrickRect = new RectangleF(printInfo.Columns[1].Bounds.X, e.Y, ((PrintColumnInfo)printInfo.Columns[printInfo.Columns.Count - 1]).Bounds.Right - printInfo.Columns[0].Bounds.Width, (int)textBrickHeight);
                    e.BrickGraphics.DrawBrick(mergedTextBreak, textMergedBrickRect);

                   
                    e.Y += (int)textBrickHeight;
                    e.Cancel = true;

                }
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e) {
            gridControl1.ShowRibbonPrintPreview();
        }
    }
}
