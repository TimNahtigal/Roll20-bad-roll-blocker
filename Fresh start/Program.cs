using System;
using System.Net;
using System.Threading.Tasks;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;

namespace Fresh_start
{
    class Program
    {
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(
        IntPtr hWnd,
        IntPtr hWndInsertAfter,
        int x,
        int y,
        int cx,
        int cy,
        int uFlags);


        private const int HWND_TOPMOST = -1;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOSIZE = 0x0001;


        public static class Globals
        {
            public static int targetValue = 15; // Modifiable
            public static bool onOff = true;
            public static string mode = "skill";
        }
        static void Main(string[] args)
        {
            IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
            SetWindowPos(hWnd,
                new IntPtr(HWND_TOPMOST),
                0, 0, 0, 0,
                SWP_NOMOVE | SWP_NOSIZE);

            Console.SetWindowSize(width: 31, height: 9);

            displayHelp();

            async Task OnResponse(object sender, SessionEventArgs e)
            {
                //Console.WriteLine(e.HttpClient.Request.Url);

                if (e.HttpClient.Request.Url.Contains("https://dice.roll20.net/doroll") && Globals.onOff)
                {
                    string bodyString = await e.GetResponseBodyAsString();
                    if (bodyString != "OK")
                    {
                        try
                        {
                            if (Globals.mode == "skill")
                            {

                                Regex rx = new Regex(@"total\\..[(-9)-9]{1,9}",
                                RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                MatchCollection matches = rx.Matches(bodyString);


                                var match = matches[1];
                                var totalValue = Int32.Parse(Regex.Match(match.ToString(), @"\d+").Value);

                                Console.WriteLine("__________________");
                                Console.WriteLine("Target value " + Globals.targetValue + " " + Globals.mode);
                                Console.WriteLine("Prediction: " + totalValue);

                                if (totalValue < Globals.targetValue)
                                {
                                    e.SetResponseBodyString(getDelateBody(bodyString));
                                    Console.WriteLine("Would been " + totalValue);
                                }
                            }
                            else if (Globals.mode == "misc")
                            {
                                Regex rx = new Regex(@"total\\..[(-9)-9]{1,9}",
                                RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                MatchCollection matches = rx.Matches(bodyString);


                                var match = matches[0];
                                var totalValue = Int32.Parse(Regex.Match(match.ToString(), @"\d+").Value);

                                Console.WriteLine("__________________");
                                Console.WriteLine("Target value " + Globals.targetValue + " " + Globals.mode);
                                Console.WriteLine("Prediction: " + totalValue);

                                if (totalValue < Globals.targetValue)
                                {
                                    e.SetResponseBodyString(getDelateBody(bodyString));
                                    Console.WriteLine("Would been " + totalValue);
                                }

                            }
                        }
                        catch
                        { Console.WriteLine("Funky"); }

                    }
                }
            }

            var proxyServer = new ProxyServer(userTrustRootCertificate: true);
            var httpProxy = new ExplicitProxyEndPoint(IPAddress.Any, 8080, decryptSsl: true);

            proxyServer.BeforeResponse += OnResponse;


            proxyServer.AddEndPoint(httpProxy);
            proxyServer.Start();

            string command;
            while ((command = Console.ReadLine()) != "quit")
            {
                if (command == "clear")
                {
                    Console.Clear();
                }
                else if (int.TryParse(command, out _))
                {
                    Globals.targetValue = int.Parse(command);
                }
                else if (command.ToLower() == "x")
                {
                    Globals.onOff = !Globals.onOff;
                    if (Globals.onOff)
                    {
                        Console.WriteLine("Turning ON");
                    }
                    else
                    {
                        Console.WriteLine("Turning OFF");
                    }
                }
                else if (command.ToLower() == "s")
                {
                    Globals.mode = "skill";
                }
                else if (command.ToLower() == "m")
                {
                    Globals.mode = "misc";
                }
                else if (command.ToLower() == "help")
                {
                    displayHelp();
                }
                else if (command.ToLower().Contains("color"))
                {
                    try
                    {
                        var list = command.Split(' ').ToList();
                        var numberFont = list[1];
                        var numberBack = list[2];

                        switch (numberFont)
                        {
                            case "0":
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;
                            case "1":
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;
                            case "2":
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;
                            case "3":
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;
                            case "4":
                                Console.ForegroundColor = ConsoleColor.Gray;
                                break;
                            case "5":
                                Console.ForegroundColor = ConsoleColor.White;
                                break;
                            case "6":
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                        }


                        switch (numberBack)
                        {
                            case "0":
                                Console.BackgroundColor = ConsoleColor.Red;
                                break;
                            case "1":
                                Console.BackgroundColor = ConsoleColor.Blue;
                                break;
                            case "2":
                                Console.BackgroundColor = ConsoleColor.Green;
                                break;
                            case "3":
                                Console.BackgroundColor = ConsoleColor.Magenta;
                                break;
                            case "4":
                                Console.BackgroundColor = ConsoleColor.Gray;
                                break;
                            case "5":
                                Console.BackgroundColor = ConsoleColor.White;
                                break;
                            case "6":
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;
                        }

                        Console.Clear();
                    }
                    catch { }
                }

                else if (command.ToLower() == "chelp")
                {
                    Console.WriteLine("0 = Red");
                    Console.WriteLine("1 = Blue");
                    Console.WriteLine("2 = Green");
                    Console.WriteLine("3 = Magenta");
                    Console.WriteLine("4 = Gray");
                    Console.WriteLine("5 = White");
                    Console.WriteLine("6 = Black");
                }
            }
                proxyServer.Stop();
            }

            static void displayHelp()
            {
                Console.WriteLine("Commands: ");
                Console.WriteLine("*number* - select minimal roll");
                Console.WriteLine("s - skill and attack roll mode");
                Console.WriteLine("m - misc and init roll mode");
                Console.WriteLine("clear - clear screen");
                Console.WriteLine("color x x - set color");
                Console.WriteLine("chelp - color help");
                Console.WriteLine("x - turn on/off");
            }

            static string getDelateBody(string bodystring)
            {

                Regex rx = new Regex(@"json",
                RegexOptions.Compiled | RegexOptions.IgnoreCase);
                string bodystring2 = rx.Replace(bodystring, "JSON69");
                return bodystring2;
            }
        }

    }