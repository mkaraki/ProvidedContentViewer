using CefSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProvidedContentViewer.Screen;

namespace ProvidedContentViewer
{
    public class CastPageInfo
    {
        public static void UpdateStatusInfo()
        {
            if (FirstBoot)
            {
                browser.ExecuteScriptAsync($"s_syswait();");

                System.Threading.Thread.Sleep(1000);

                FirstBoot = false;
            }

            if (pname != null)
                browser.ExecuteScriptAsync($"set_pname('{pname.Replace("'", "\'")}');");
            if (pnote != null)
                browser.ExecuteScriptAsync($"set_pnote('{pnote.Replace("'", "\'")}');");

            UpdateStatus();

            switch (stt)
            {
                case WaitPageStatus.Waiting:
                    browser.ExecuteScriptAsync($"s_wait();");
                    break;

                case WaitPageStatus.InformationFound:
                    browser.ExecuteScriptAsync($"s_waitinfo();");
                    break;

                case WaitPageStatus.Connecting:
                    browser.ExecuteScriptAsync($"s_con();");
                    break;

                case WaitPageStatus.WaitingStream:
                    browser.ExecuteScriptAsync($"s_waitstream();");
                    break;

                case WaitPageStatus.SystemWaiting:
                    browser.ExecuteScriptAsync($"s_syswait();");
                    break;

                case WaitPageStatus.Disabled:
                    browser.ExecuteScriptAsync($"s_disable();");
                    break;

                case WaitPageStatus.Error:
                default:
                    browser.ExecuteScriptAsync($"s_err();");
                    break;
            }
        }

        public static void UpdateStatus(bool newstatus = false)
        {
            // Ignore there are not listed status.
            if (!newstatus && (
                stt != WaitPageStatus.Waiting ||
                stt != WaitPageStatus.InformationFound
                ))
                return;

            if (Disabled)
            {
                stt = WaitPageStatus.Disabled;
                return;
            }
            else
            {
                stt = WaitPageStatus.Waiting;
            }
        }
    }

    public enum WaitPageStatus
    {
        Waiting,
        InformationFound,
        Connecting,
        WaitingStream,
        Error,
        SystemWaiting,
        Disabled
    }
}
