' Developer Express Code Central Example:
' How to merge cells horizontally in GridView via the CustomDrawCell event
' 
' This example illustrates an approach that is similar to that one described in
' the following thread: http://www.devexpress.com/scid=E2472.
' The mentioned
' functionality is implemented via the GridView.CustomDrawCell event handling.
' In
' addition, this example illustrates how to implement merged text scrolling.
' 
' You can find sample updates and versions for different programming languages here:
' http://www.devexpress.com/example=E4039

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Windows.Forms
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Drawing
Imports DevExpress.XtraGrid.Views.Printing
Imports DevExpress.XtraPrinting
Imports System.Reflection
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.Utils

Namespace HorizontalMerging
    Partial Public Class Form1
        Inherits Form

        Private ViewHandler As MyGridViewHandler = Nothing
        Public Sub New()
            InitializeComponent()
            ViewHandler = New MyGridViewHandler(gridView1)
        End Sub

        Private Function CreateTable(ByVal RowCount As Integer) As DataTable
            Dim tbl As New DataTable()
            tbl.Columns.Add("Name1", GetType(String))
            tbl.Columns.Add("Name2", GetType(String))
            tbl.Columns.Add("Name3", GetType(String))
            tbl.Columns.Add("Name4", GetType(String))
            tbl.Columns.Add("Name5", GetType(String))
            tbl.Columns.Add("Name6", GetType(Boolean))
            For i As Integer = 0 To RowCount - 1
                If i = 1 Then
                    tbl.Rows.Add(New Object() { String.Format("Name{0}", i), "This is a long long string, which is merged for several columns", "", "", "", True })

                Else
                    tbl.Rows.Add(New Object() { String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), String.Format("Name{0}", i), "", False })
                End If
            Next i
            Return tbl
        End Function

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            gridControl1.DataSource = CreateTable(20)
            gridView1.Columns("Name6").Visible = False
            gridView1.Columns(4).Width = 300
            gridControl1.ForceInitialize()
            ViewHandler.MergeCells(gridView1.GetRowCellValue(1, "Name2").ToString(), gridView1.GetDataSourceRowIndex(1), New GridColumn() { gridView1.Columns(1), gridView1.Columns(2), gridView1.Columns(3), gridView1.Columns(4) })
        End Sub

        Private Sub gridView1_BeforePrintRow(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Printing.CancelPrintRowEventArgs) Handles gridView1.BeforePrintRow
            If e.RowHandle >= 0 Then
                If Not(TypeOf sender Is GridView) Then
                    Return
                End If
                Dim gridView As GridView = (DirectCast(sender, GridView))
                Dim dataRow As DataRowView = TryCast(gridView.GetRow(e.RowHandle), DataRowView)
                If dataRow Is Nothing Then
                    Return
                End If

                If DirectCast(dataRow("Name6"), Boolean) Then
                    Dim vi As GridViewInfo = CType(gridView.GetViewInfo(), GridViewInfo)
                    Dim pi As PropertyInfo = GetType(BaseView).GetProperty("PrintInfo", BindingFlags.Instance Or BindingFlags.NonPublic)
                    Dim printInfo As GridViewPrintInfo = DirectCast(pi.GetValue(gridView, Nothing), GridViewPrintInfo)


                    Dim gridRowInfo As GridRowInfo = vi.GetGridRowInfo(e.RowHandle)
                    Dim clientPageSize As SizeF = (TryCast(e.BrickGraphics, BrickGraphics)).ClientPageSize


                    'Draw other cells
                    Dim tb As New TextBrick()
                    tb.Style.Padding = New PaddingInfo(2,0,0,0)
                    tb.BackColor = gridView.Appearance.Row.BackColor
                    tb.Text = DirectCast(dataRow("Name1"), String)
                    tb.HorzAlignment = HorzAlignment.Near
                    Dim textBrickHeight As Single = vi.ColumnRowHeight
                    Dim info As GridCellInfo = vi.GetGridCellInfo(e.RowHandle, gridView.Columns(0))
                    Dim textBrickRect As New RectangleF(e.X, e.Y, printInfo.Columns(0).Bounds.Width, CInt((textBrickHeight)))
                    e.BrickGraphics.DrawBrick(tb, textBrickRect)

                    'Draw merged cells
                    Dim mergedTextBreak As New TextBrick()
                    mergedTextBreak.Style.Padding = New PaddingInfo(2, 0, 0, 0)
                    mergedTextBreak.BackColor = gridView.Appearance.Row.BackColor
                    mergedTextBreak.Text = DirectCast(dataRow("Name2"), String)
                    mergedTextBreak.HorzAlignment = HorzAlignment.Near

                    Dim mergedInfo As GridCellInfo = vi.GetGridCellInfo(e.RowHandle, gridView.Columns(1))
                    Dim textMergedBrickRect As New RectangleF(printInfo.Columns(1).Bounds.X, e.Y, CType(printInfo.Columns(printInfo.Columns.Count - 1), PrintColumnInfo).Bounds.Right - printInfo.Columns(0).Bounds.Width, CInt((textBrickHeight)))
                    e.BrickGraphics.DrawBrick(mergedTextBreak, textMergedBrickRect)


                    e.Y += CInt((textBrickHeight))
                    e.Cancel = True

                End If
            End If
        End Sub

        Private Sub simpleButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles simpleButton1.Click
            gridControl1.ShowRibbonPrintPreview()
        End Sub
    End Class
End Namespace
