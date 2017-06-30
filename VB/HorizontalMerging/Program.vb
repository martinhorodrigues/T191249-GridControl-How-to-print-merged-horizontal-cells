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
Imports System.Linq
Imports System.Windows.Forms

Namespace HorizontalMerging
    Friend NotInheritable Class Program

        Private Sub New()
        End Sub

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread> _
        Shared Sub Main()
            Application.EnableVisualStyles()
            Application.SetCompatibleTextRenderingDefault(False)
            Application.Run(New Form1())
        End Sub
    End Class
End Namespace
