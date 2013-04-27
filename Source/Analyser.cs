﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Heijden.DNS;
using VirusTotalNET;
using woanware;

namespace TargetAnalyser
{
    /// <summary>
    /// 
    /// </summary>
    public class Analyser
    {
        public delegate void ResultEvent(List<Result> results);

        public event woanware.Events.MessageEvent Message;
        public event ResultEvent ResultsIdentified;
        public event woanware.Events.DefaultEvent Complete;

        private string _apiKey;
        private int _retries;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="retries"></param>
        public Analyser(string apiKey, int retries)
        {
            _apiKey = apiKey;
            _retries = retries;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Analyse(Global.TargetType targetType, string data)
        {
            (new Thread(() =>
            {
                switch (targetType)
                {
                    case Global.TargetType.Ip:
                        RunIpVoid(data);
                        RunDns(data);
                        RunRobTex(data);
                        RunAlienVault(data);
                        RunFortiguard(data);
                        RunMalwareDomain(data);
                        RunBfk(data);
                        RunVirusTotalDns(data);
                        RunHurricaneElectric(data);
                        RunHpHosts(data);
                        break;
                    case Global.TargetType.Url:
                        RunUrlVoid(data);
                        RunDns(data);
                        RunRobTex(data);
                        RunAlienVault(data);
                        RunFortiguard(data);
                        RunMalwareDomain(data);
                        RunBfk(data);
                        RunVirusTotalDns(data);
                        RunHpHosts(data);
                        break;
                    case Global.TargetType.Md5:
                        RunThreatExpert(data);
                        RunVxVault(data);
                        RunMinotaurAnalysis(data);
                        RunVirusTotal(data);
                        break;
                }

                OnComplete();

            })).Start();
        }

        #region IP/URL Providers
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunIpVoid(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.IpVoid.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://ipvoid.com/scan/" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.IpVoid,
                                      "http://ipvoid.com/scan/" + data,
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regexNoMatch = new Regex(@"An\sError\soccurred", RegexOptions.IgnoreCase);
                if (regexNoMatch.Match(wcr.Response).Success == false)
                {
                    List<Result> results = new List<Result>();

                    Regex regex = new Regex(@"Detected\<\/font\>\<\/td..td..a.rel..nofollow..href.""(.{6,70})\""\stitle\=\""View", RegexOptions.IgnoreCase);
                    MatchCollection matches = regex.Matches(wcr.Response);
                    foreach (Match match in matches)
                    {
                        Result result = new Result();
                        result.Source = Global.Source.IpVoid;
                        result.Info = "Blacklisted: " + match.Groups[1].Value;
                        result.ParentUrl = "http://ipvoid.com/scan/" + data;
                        result.Url = match.Groups[1].Value;

                        results.Add(result);
                    }

                    if (results.Count > 0)
                    {
                        OnResultIdentified(results);
                    }
                    else
                    {
                        SendDefaultResult(Global.Source.IpVoid,
                                          "http://ipvoid.com/scan/" + data,
                                          "No record");
                    }
                }
                else
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("ip", data);
                    nvc.Add("go", "Scan Now");
                    wc.Headers.Add("Content-type", "application/x-www-form-urlencoded");
                    byte[] tempPost = wc.UploadValues("http://ipvoid.com", "POST", nvc);
                    string responsePost = Encoding.ASCII.GetString(tempPost);

                    List<Result> results = new List<Result>();

                    Regex regex = new Regex(@"Detected\<\/font\>\<\/td..td..a.rel..nofollow..href.""(.{6,70})\""\stitle\=\""View", RegexOptions.IgnoreCase);
                    MatchCollection matches = regex.Matches(responsePost);
                    foreach (Match match in matches)
                    {
                        Result result = new Result();
                        result.Source = Global.Source.IpVoid;
                        result.Info = "Blacklisted: " + match.Groups[1].Value;
                        result.ParentUrl = "http://ipvoid.com/scan/" + data;
                        result.Url = match.Groups[1].Value;

                        results.Add(result);
                    }

                    if (results.Count > 0)
                    {
                        OnResultIdentified(results);
                    }
                    else
                    {
                        SendDefaultResult(Global.Source.IpVoid,
                                          "http://ipvoid.com/scan/" + data,
                                          "No record");
                    }
                }
            }
            catch (Exception ex) 
            {
                SendDefaultResult(Global.Source.UrlVoid,
                                  "http://ipvoid.com/scan/" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunUrlVoid(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.UrlVoid.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://urlvoid.com/scan/" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.UrlVoid,
                                      "http://urlvoid.com/scan/" + data,
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regexNoMatch = new Regex(@"The website is not blacklisted", RegexOptions.IgnoreCase);
                if (regexNoMatch.Match(wcr.Response).Success == true)
                {
                    return;
                }

                regexNoMatch = new Regex(@"AN ERROR OCCURRED", RegexOptions.IgnoreCase);
                if (regexNoMatch.Match(wcr.Response).Success == false)
                {
                    List<Result> results = new List<Result>();

                    Regex regex = new Regex(@"DETECTED.{25,40}href\=\""(.{10,50})\""\stitle", RegexOptions.IgnoreCase);
                    MatchCollection matches = regex.Matches(wcr.Response);
                    foreach (Match match in matches)
                    {
                        Result result = new Result();
                        result.Source = Global.Source.UrlVoid;
                        result.Info = "Blacklisted: " + match.Groups[1].Value;
                        result.ParentUrl = "http://urlvoid.com/scan/" + data;
                        result.Url = match.Groups[1].Value;

                        results.Add(result);
                    }

                    if (results.Count > 0)
                    {
                        OnResultIdentified(results);
                    }
                    else
                    {
                        SendDefaultResult(Global.Source.UrlVoid,
                                          "http://urlvoid.com/scan/" + data,
                                          "No record");
                    }
                }
                else
                {
                    NameValueCollection nvc = new NameValueCollection();
                    nvc.Add("url", data);
                    nvc.Add("check", "Submit");
                    wc.Headers.Add("Content-type", "application/x-www-form-urlencoded");
                    byte[] tempPost = wc.UploadValues("http://urlvoid.com", "POST", nvc);
                    string responsePost = Encoding.ASCII.GetString(tempPost);

                    List<Result> results = new List<Result>();

                    Regex regex = new Regex(@"DETECTED.{25,40}href\=\""(.{10,50})\""\stitle", RegexOptions.IgnoreCase);
                    MatchCollection matches = regex.Matches(responsePost);
                    foreach (Match match in matches)
                    {
                        Result result = new Result();
                        result.Source = Global.Source.UrlVoid;
                        result.Info = "Blacklisted: " + match.Groups[1].Value;
                        result.ParentUrl = "http://urlvoid.com/scan/" + data;
                        result.Url = match.Groups[1].Value;

                        results.Add(result);
                    }

                    if (results.Count > 0)
                    {
                        OnResultIdentified(results);
                    }
                    else
                    {
                        SendDefaultResult(Global.Source.UrlVoid,
                                      "http://urlvoid.com/scan/" + data,
                                      "No record");
                    }
                }    
            }
            catch (Exception ex) 
            {
                SendDefaultResult(Global.Source.UrlVoid,
                                  "http://urlvoid.com/scan/" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunDns(string data)
        {
            try
            {
               // OnMessage("Analysing via " + Global.Source.Dn.GetEnumDescription());

                Resolver resolver = new Resolver("8.8.8.8");
                Response response = resolver.Query(data, QType.A);
                foreach (var a in response.Answers)
                {

                }
            }
            catch (Exception) { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunRobTex(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.Robtex.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://robtex.com/" + data + ".html", _retries);
                if (wcr.IsError == true)
                {
                    if (wcr.Response == HttpStatusCode.NotFound.ToString())
                    {
                        SendDefaultResult(Global.Source.Robtex,
                                          "http://robtex.com/" + data + ".html",
                                          "No record");
                    }
                    else
                    {
                        SendDefaultResult(Global.Source.Robtex,
                                          "http://robtex.com/" + data + ".html",
                                          "Error: " + wcr.Response);

                    }

                    return;
                }

                List<Result> results = new List<Result>();

                Regex regex = new Regex(@"<a href="".*#shared""\s?>(.*)</a>", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(wcr.Response);
                foreach (Match match in matches)
                {
                    Result result = new Result();
                    result.Source = Global.Source.Robtex;
                    result.Info = "A Record: " + match.Groups[1].Value;
                    result.ParentUrl = "http://robtex.com/" + data + ".html";
                    result.Url = "http://robtex.com/" + match.Groups[1].Value + ".html";

                    results.Add(result);
                }

                if (results.Count > 0)
                {
                    OnResultIdentified(results);
                }
                else
                {
                    SendDefaultResult(Global.Source.Robtex,
                                      "http://robtex.com/" + data + ".html",
                                      "No A records");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.Robtex,
                                  "http://robtex.com/" + data + ".html",
                                  "Error");
            }
        }

        /// <summary>
        /// //http://www.fortiguard.com/ip_rep/index.php?data=" + ipInput + "&lookup=Lookup"
        /// </summary>
        /// <param name="data"></param>
        private void RunFortiguard(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.Fortiguard.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://www.fortiguard.com/ip_rep/index.php?data=" + data + "&lookup=Lookup", _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.Fortiguard,
                                      "http://www.fortiguard.com/ip_rep/index.php?data=" + data + "&lookup=Lookup",
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regex = new Regex(@"Category:\s(.+)\<\/h3\>\s\<a", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == true)
                {
                    Result result = new Result();
                    result.Source = Global.Source.Fortiguard;
                    result.Info = "Classification: " + match.Groups[1].Value;
                    result.ParentUrl = "http://www.fortiguard.com/ip_rep/index.php?data=" + data + "&lookup=Lookup";
                    result.Url = "http://www.fortiguard.com/ip_rep/index.php?data=" + data + "&lookup=Lookup";

                    OnResultIdentified(new List<Result> { result });
                }
                else
                {
                    SendDefaultResult(Global.Source.Fortiguard,
                                      "http://www.fortiguard.com/ip_rep/index.php?data=" + data + "&lookup=Lookup",
                                      "No record");
                }
            }
            catch (Exception ex) 
            {
                SendDefaultResult(Global.Source.Fortiguard,
                                  "http://www.fortiguard.com/ip_rep/index.php?data=" + data + "&lookup=Lookup",
                                  "Error");
            }
        }

        /// <summary>
        /// http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + ipInput
        /// </summary>
        /// <param name="data"></param>
        private void RunAlienVault(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.AlienVault.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.AlienVault,
                                      "http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + data,
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regex = new Regex(".*IP not found.*", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == false)
                {
                    Result result = new Result();
                    result.Source = Global.Source.AlienVault;
                    result.Info = "Identified: " + data;
                    result.ParentUrl = "http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + data;
                    result.Url = "http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + data;

                    OnResultIdentified(new List<Result> { result });
                }
                else
                {
                    SendDefaultResult(Global.Source.AlienVault,
                                      "http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + data,
                                      "No record");
                }
            }
            catch (Exception ex) 
            {
                SendDefaultResult(Global.Source.AlienVault,
                                  "http://labs.alienvault.com/labs/index.php/projects/open-source-ip-reputation-portal/information-about-ip/?ip=" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// http://www.malwaredomainlist.com/mdl.php?search=freefblikes.net&colsearch=All&quantity=50
        /// </summary>
        /// <param name="data"></param>
        private void RunMalwareDomain(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.MalwareDomainlist.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50", _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.MalwareDomainlist,
                                      "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50",
                                      "Error: " + wcr.Response);
                    return;
                }

                List<Result> results = new List<Result>();

                Regex regex = new Regex(@"<td><nobr>\d\d\d\d/\d\d/\d\d_\d\d:\d\d</nobr></td><td>.*</td><td>\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b</td><td>.*</td><td>(.*)</td><td>(.*)<wbr>.*</td><td>.*</td><td align="".*""><img src="".*"" class="".*"" alt="".*"" title="".*""/></td></tr>", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(wcr.Response);
                foreach (Match match in matches)
                {
                    Result result = new Result();
                    result.Source = Global.Source.MalwareDomainlist;
                    result.Info = "Malware: " + match.Groups[1].Value + "#Registrant: " + match.Groups[2].Value;
                    result.ParentUrl = "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50";
                    result.Url = "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50";

                    results.Add(result);
                }

                if (results.Count > 0)
                {
                    OnResultIdentified(results);
                }
                else
                {
                    SendDefaultResult(Global.Source.MalwareDomainlist, 
                                      "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50", 
                                      "No record");
                }
            }
            catch (Exception ex) 
            {
                SendDefaultResult(Global.Source.MalwareDomainlist,
                                  "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50",
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunBfk(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.Bfk.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://www.bfk.de/bfk_dnslogger.html?query=" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.Bfk,
                                      "http://www.bfk.de/bfk_dnslogger.html?query=" + data,
                                      "Error: " + wcr.Response);
                    return;
                }

                List<Result> results = new List<Result>();

                Regex regex = new Regex(@"<a href=""(.*?)#result"" rel=""nofollow"">(.*?)</a>", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(wcr.Response);
                foreach (Match match in matches)
                {
                    Result result = new Result();
                    result.Source = Global.Source.Bfk;
                    result.Info = "Result: " + match.Groups[2].Value;
                    result.ParentUrl = "http://www.bfk.de/bfk_dnslogger.html?query=" + data;
                    result.Url = match.Groups[1].Value;

                    results.Add(result);
                }

                if (results.Count > 0)
                {
                    OnResultIdentified(results);
                }
                else
                {
                    SendDefaultResult(Global.Source.Bfk,
                                     "http://www.bfk.de/bfk_dnslogger.html?query=" + data,
                                      "No record");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.Bfk,
                                  "http://www.bfk.de/bfk_dnslogger.html?query=" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunVirusTotalDns(string data)
        {
            try
            {
                OnMessage("Analysing via " + Global.Source.VirusTotalDns.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("https://www.virustotal.com/en/ip-address/" + data + "/information/", _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.VirusTotalDns,
                                      "https://www.virustotal.com/latest-scan/" + data,
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regex = new Regex("Unknown IP!", RegexOptions.IgnoreCase);
                Match matchUnknown = regex.Match(wcr.Response);
                if (matchUnknown.Success == true)
                {
                    SendDefaultResult(Global.Source.VirusTotalDns,
                                      "https://www.virustotal.com/en/ip-address/" + data + "/information/",
                                      "No record");
                    return;
                }
                
                List<Result> results = new List<Result>();

                regex = new Regex(@" <a target=""_blank"" href=""/en/domain/(.*)"">(.*)</a>", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(wcr.Response);
                foreach (Match match in matches)
                {
                    Result result = new Result();
                    result.Source = Global.Source.VirusTotalDns;
                    result.Info = "Result: " + match.Groups[2].Value;
                    result.ParentUrl = "https://www.virustotal.com/en/ip-address/" + data + "/information/";
                    result.Url = "https://www.virustotal.com/en/domain/" + match.Groups[1].Value + "/information/";

                    results.Add(result);
                }


                if (results.Count > 0)
                {
                    OnResultIdentified(results);
                }
                else
                {
                    SendDefaultResult(Global.Source.VirusTotalDns,
                                     "https://www.virustotal.com/en/ip-address/" + data + "/information/",
                                      "No records");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.VirusTotalDns,
                                 "https://www.virustotal.com/en/ip-address/" + data + "/information/",
                                 "Error");
            }
        }

        /// <summary>
        /// http://hosts-file.net/default.asp?s=00cf9556.linkbucks.com
        /// </summary>
        /// <param name="data"></param>
        private void RunHpHosts(string data)
        {
            string url = "http://hosts-file.net/default.asp?s=" + data;

            try
            {
                OnMessage("Analysing via " + Global.Source.HpHosts.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download(url, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.HpHosts,
                                      url,
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regex = new Regex("This site is NOT currently listed in hpHosts", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == true)
                {
                    SendDefaultResult(Global.Source.HpHosts,
                                      url,
                                      "No record");
                    return;
                }

                SendDefaultResult(Global.Source.HpHosts,
                                  url,
                                  "Identified");
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.HpHosts,
                                  url,
                                  "Error");
            }
        }

        /// <summary>
        /// IP
        /// </summary>
        /// <param name="data"></param>
        private void RunHurricaneElectric(string data)
        {
            string url = "http://bgp.he.net/ip/" + data;

            try
            {
                OnMessage("Analysing via " + Global.Source.HurricaneElectric.GetEnumDescription());

                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download(url, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.HurricaneElectric,
                                      url,
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regex = new Regex("This site is NOT currently listed in hpHosts", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == true)
                {
                    SendDefaultResult(Global.Source.HurricaneElectric,
                                      url,
                                      "No record");
                    return;
                }

                SendDefaultResult(Global.Source.HurricaneElectric,
                                  url,
                                  "Identified");
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.HurricaneElectric,
                                  url,
                                  "Error");
            }
        }         
        #endregion

        #region Hash Provider Methods
        /// <summary>
        /// http://www.threatexpert.com/report.aspx?md5=" + md5
        /// </summary>
        /// <param name="data"></param>
        private void RunThreatExpert(string data)
        {
            try
            {
                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://www.threatexpert.com/report.aspx?md5=" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.ThreatExpert,
                                      "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50",
                                      "Error: " + wcr.Response);
                    return;
                }

                Regex regex = new Regex(@"<title>ThreatExpert Report:\s+(.*)</title>", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == true)
                {
                    Result result = new Result();
                    result.Source = Global.Source.ThreatExpert;
                    result.Info = "Malware: " + match.Groups[1].Value;
                    result.ParentUrl = "http://www.threatexpert.com/report.aspx?md5=" + data;
                    result.Url = "http://www.threatexpert.com/report.aspx?md5=" + data;

                    OnResultIdentified(new List<Result> { result });
                }
                else
                {
                    SendDefaultResult(Global.Source.ThreatExpert,
                                      "http://www.threatexpert.com/report.aspx?md5=" + data,
                                      "No record");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.ThreatExpert,
                                  "http://www.threatexpert.com/report.aspx?md5=" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunVxVault(string data)
        {
            try
            {
                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://vxvault.siri-urz.net/ViriList.php?MD5=" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.VxVault,
                                      "http://www.malwaredomainlist.com/mdl.php?search=" + data + "&colsearch=All&quantity=50",
                                      "Error: " + wcr.Response);
                    return;
                }

                List<Result> results = new List<Result>();

                Regex regex = new Regex(@"<a class=jaune href='virifiche.php\?ID=(\d*)'>\d\d\d\d-\d\d-\d\d</a></TD>", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(wcr.Response);
                foreach (Match match in matches)
                {
                    Result result = new Result();
                    result.Source = Global.Source.VxVault;
                    result.Info = "Malware: " + match.Groups[1].Value;
                    result.ParentUrl = "http://vxvault.siri-urz.net/ViriList.php?MD5=" + data;
                    result.Url = "http://vxvault.siri-urz.net/ViriFiche.php?ID=" + match.Groups[1].Value;

                    OnResultIdentified(new List<Result> { result });
                }

                if (results.Count > 0)
                {
                    OnResultIdentified(results);
                }
                else
                {
                    SendDefaultResult(Global.Source.VxVault, 
                                     "http://vxvault.siri-urz.net/ViriList.php?MD5=" + data, 
                                     "No record");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.VxVault,
                                  "http://vxvault.siri-urz.net/ViriList.php?MD5=" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunMinotaurAnalysis(string data)
        {
            try
            {
                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("http://minotauranalysis.com/search.aspx?q=" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.MinotaurAnalysis,
                                      "http://minotauranalysis.com/search.aspx?q=" + data,
                                      "Error: " + wcr.Response);
                    return;
                }

                List<Result> results = new List<Result>();

                Regex regex = new Regex(@"No sample found matching the MD5", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == false)
                {
                    Result result = new Result();
                    result.Source = Global.Source.MinotaurAnalysis;
                    result.Info = "Malware: " + data;
                    result.ParentUrl = "http://minotauranalysis.com/search.aspx?q=" + data;
                    result.Url = "http://minotauranalysis.com/search.aspx?q=" + data;

                    OnResultIdentified(new List<Result> { result });
                }
                else
                {
                    SendDefaultResult(Global.Source.MinotaurAnalysis,
                                     "http://minotauranalysis.com/search.aspx?q=" + data,
                                     "No record");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.MinotaurAnalysis,
                                  "http://minotauranalysis.com/search.aspx?q=" + data,
                                  "Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        private void RunVirusTotal(string data)
        {
            try
            {
                GZipWebClient wc = new GZipWebClient();
                WebClientResult wcr = wc.Download("https://www.virustotal.com/latest-scan/" + data, _retries);
                if (wcr.IsError == true)
                {
                    SendDefaultResult(Global.Source.VirusTotalHash,
                                      "https://www.virustotal.com/latest-scan/" + data,
                                      "Error: " + wcr.Response);
                    return;
                }
                List<Result> results = new List<Result>();

                Regex regex = new Regex(@"(\d+\s/\s\d+)", RegexOptions.IgnoreCase);
                Match match = regex.Match(wcr.Response);
                if (match.Success == true)
                {
                    Result result = new Result();
                    result.Source = Global.Source.VirusTotalHash;
                    result.Info = "Detection Ratio: " + match.Groups[1].Value;
                    result.ParentUrl = "https://www.virustotal.com/latest-scan/" + data;
                    result.Url = "https://www.virustotal.com/latest-scan/" + data;

                    OnResultIdentified(new List<Result> { result });
                }
                else
                {
                    SendDefaultResult(Global.Source.VirusTotalHash,
                                     "https://www.virustotal.com/latest-scan/" + data,
                                     "No record");
                }
            }
            catch (Exception ex)
            {
                SendDefaultResult(Global.Source.VirusTotalHash,
                                 "https://www.virustotal.com/latest-scan/" + data,
                                 "Error");
            }
        }
        #endregion

        #region Misc Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="url"></param>
        /// <param name="message"></param>
        private void SendDefaultResult(Global.Source source, 
                                       string url, 
                                       string message)
        {
            Result result = new Result();
            result.Source = source;
            result.Info = message;
            result.ParentUrl = url;
            result.Url = url;

            OnResultIdentified(new List<Result> { result });
        }
        #endregion

        #region Event Handler Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        private void OnResultIdentified(List<Result> result)
        {
            var handler = ResultsIdentified;
            if (handler != null)
            {
                handler(result);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void OnMessage(string message)
        {
            var handler = Message;
            if (handler != null)
            {
                handler(message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnComplete()
        {
            var handler = Complete;
            if (handler != null)
            {
                handler();
            }
        }
        #endregion
    }
}
