using DevExpress.Skins;
using DevExpress.UserSkins;
using QuanlyThue.Forms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace QuanlyThue
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
            initCulturalFormattingChanges();
            Application.SetCompatibleTextRenderingDefault(false);
            DevComponents.DotNetBar.StyleManager.Style = DevComponents.DotNetBar.eStyle.Office2013;
            BonusSkins.Register();
            Application.Run(new frm_Login());
        }
        private static void initCulturalFormattingChanges()
        {
            CultureInfo cultureDefinition = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cultureDefinition.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cultureDefinition.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            cultureDefinition.NumberFormat.NumberDecimalSeparator = ",";
            //cultureDefinition.NumberFormat.CurrencyGroupSeparator = ".";
            cultureDefinition.NumberFormat.NumberGroupSeparator = ".";
            Thread.CurrentThread.CurrentCulture = cultureDefinition;
        }
    }
}
