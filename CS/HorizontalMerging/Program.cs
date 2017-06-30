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
using System.Linq;
using System.Windows.Forms;

namespace HorizontalMerging
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
