using CefSharp;
using CefSharp.WinForms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProvidedContentViewer
{
    public partial class Screen : Form
    {
        public static ChromiumWebBrowser browser;
        public static string MainPage = $"file:///{AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/").TrimEnd('/')}/default/menu/menu.html";

        public static bool Disabled = false;

        public static string pname = null;
        public static string pnote = null;

        public static bool FirstBoot = true;

        public static WaitPageStatus stt = WaitPageStatus.Waiting;

#if DEBUG
        public static List<string> dbgstrs = new List<string>();
#endif

        [Obsolete]
        public Screen()
        {
            InitializeComponent();

            browser = new ChromiumWebBrowser();

            browser.FrameLoadEnd += Browser_FrameLoadEnd;
            browser.FrameLoadStart += Browser_FrameLoadStart;
            browser.LoadError += Browser_LoadError;

            browser.Load(MainPage);

            Controls.Add(browser);
            browser.Dock = DockStyle.Fill;
        }

        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            if (e.FailedUrl == MainPage) return;
            stt = WaitPageStatus.Error;
            browser.Load(MainPage);
        }

        private void Browser_FrameLoadStart(object sender, FrameLoadStartEventArgs e)
        {
        }

        private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Url == MainPage)
            {
                CastPageInfo.UpdateStatusInfo();

#if DEBUG
                browser.ExecuteScriptAsync($"dbgwrite('{string.Join("<br />", dbgstrs).Replace("'", "\\'")}');");
#endif

                return;
            }

            if (e.HttpStatusCode < 100) return;
            int ic = e.HttpStatusCode / 100;
            if (e.Url != MainPage && ic != 2)
            {
                stt = WaitPageStatus.Error;
                browser.Load(MainPage);
                return;
            }
        }

        private void Screen_Shown(object sender, EventArgs e)
        {
            Task.Run(() => CmdHandler.CmdWork());
        }

        public static void WriteDbg(string url)
        {
#if DEBUG
            dbgstrs.Add(url);

            browser.ExecuteScriptAsync($"dbgwrite('{string.Join("<br />", dbgstrs).Replace("'", "\\'")}');");
#endif
        }
    }
}