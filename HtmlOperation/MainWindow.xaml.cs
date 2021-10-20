using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows;
using HtmlAgilityPack;

namespace HtmlOperation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.Loaded += OnMainWinLoaded;
            
            //Tbx.Text = DateTime.Today.ToString("yyyy.MM.dd");
        }

        //version: eg. 1.7.1  2.0.0
        private readonly Regex _regex = new Regex("\\d+(?:\\.\\d+)+");

        private void OnMainWinLoaded(object sender, RoutedEventArgs e)
        {
            var rows = new List<List<string>>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var web = new HtmlWeb();
                var doc = web.Load("https://singer.xiaoice.com/updatelog");
                var i = 0;
                foreach (HtmlNode table in doc.DocumentNode.SelectNodes("//table"))
                {   
                    foreach (HtmlNode row in table.SelectNodes("tr"))
                    {
                        var temprow = new List<string>();
                        foreach (HtmlNode cell in row.SelectNodes("td"))
                        {
                            temprow.Add(cell.InnerText);
                        }
                        rows.Add(temprow);
                    }

                    i++;
                    if (i == 3)
                    {
                        break;
                    }
                }

                foreach (var row in rows)
                {
                    var tmpStr = "";
                    foreach (var r in row)
                    {
                        if (_regex.IsMatch(r))
                        {
                            continue;
                        }
                        tmpStr += r;
                    }

                    tmpStr += "\n";
                    Box.Text += tmpStr;
                }
            }
            catch (Exception ex)
            {
                Tbx.Text = ex.Message;
            }
        }
    }
}