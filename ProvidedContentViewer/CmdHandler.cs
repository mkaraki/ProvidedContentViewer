using CefSharp;
using System.Collections.Generic;
using System.Linq;
using static ProvidedContentViewer.Screen;

namespace ProvidedContentViewer
{
    internal class CmdHandler
    {
        public static void CmdWork()
        {
            while (true)
            {
                var ri = TCPService.GetSession();
                string[] cmd = ri.Cmds;
                List<string> tmp = cmd.ToList();
                tmp.RemoveAt(0);
                string arg = string.Join(" ", tmp);

                WriteDbg($"Get command \"{cmd[0]}\" with option \"{arg}\" from \"{ri.ClientIP}\"");

                switch (cmd[0])
                {
                    case "view":
                        if (Disabled) break;
                        browser.ExecuteScriptAsync($"s_con();");
                        System.Threading.Thread.Sleep(1000);
                        browser.Load(arg);
                        break;

                    case "stop":
                        UpdateStatus(true);
                        browser.Load(MainPage);
                        break;

                    case "disable":
                        Disabled = true;
                        stt = WaitPageStatus.Disabled;
                        CastPageInfo.UpdateStatusInfo();
                        break;

                    case "enable":
                        Disabled = false;
                        CastPageInfo.UpdateStatus(true);
                        CastPageInfo.UpdateStatusInfo();
                        break;

                    case "pname":
                        pname = arg;
                        browser.ExecuteScriptAsync($"set_pname('{arg.Replace("'", "\'")}');");
                        break;

                    case "pnote":
                        pnote = arg;
                        browser.ExecuteScriptAsync($"set_pnote('{arg.Replace("'", "\'")}');");
                        break;

                    case "fill":
                        browser.ExecuteScriptAsync($"fill('{arg.Replace("'", "\'")}');");
                        break;

                    case "unfill":
                        browser.ExecuteScriptAsync($"unfill();");
                        break;

                    default:
                        stt = WaitPageStatus.Error;
                        browser.Load(MainPage);
                        break;
                }
            }
        }

        public static void UpdateStatus(bool newstatus = false)
        {
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
}