using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;

namespace BajadaDeArchivos
{
    public static class Logica
    {
        public static void bajarTRN()
        {
			//This next line for the logs
            string ahora = string.Format("TRN_{0:yyyy-MM-dd_hh-mm-ss_tt}.txt", DateTime.Now);
            string log = "PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + ahora;
            Stream crearLog = File.Create(log);
            crearLog.Close();

            string listadoCrudo;
            string[] filtroCAN;
            string[] filtroPAG;
            var acortado = "";

            List<string> listaTRNComp = new List<string>();
            List<string> logTRNDesc = new List<string>();

            //Acá me conecto a TRN
            FtpWebRequest pedirListado = (FtpWebRequest)WebRequest.Create("YOUR FTP PATH HERE");
            pedirListado.Method = WebRequestMethods.Ftp.ListDirectory;
            pedirListado.Credentials = new NetworkCredential("USER", "PASSWORD");

            //Acá listo los PAG y CAN
            using (FtpWebResponse respuestaListado = (FtpWebResponse)pedirListado.GetResponse())
            {
                using (StreamReader leeRespuestaListado = new StreamReader(respuestaListado.GetResponseStream()))
                {
                    listadoCrudo = leeRespuestaListado.ReadToEnd();
                    string[] listadoCrudoSeparado = listadoCrudo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] listadoLimpio = listadoCrudoSeparado.Select(x => x.Replace("WHATEVER YOU WANT TO TAKE OUT OF THE FILE NAME", "")).ToArray();

                    filtroPAG = Array.FindAll(listadoLimpio, x => x.StartsWith("WORD TO FILTER", StringComparison.Ordinal));
                    filtroCAN = Array.FindAll(listadoLimpio, x => x.StartsWith("WORD TO FILTER", StringComparison.Ordinal));
                }
            }

            //Acá listo lo de un directorio en el compartido
            foreach (String archivo in Directory.GetFiles("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC"))
            {
                //Acá limpio para quedarme con el nombre del archivo solo
                var completo = archivo.Split('\\');
                acortado = completo[completo.Length - 1];
                listaTRNComp.Add(acortado);
            }

            //Descargo los CAN que no están en el compartido (else)
            int i = 0;
            foreach (String archivo in filtroCAN)
            {
                if (listaTRNComp.Contains(archivo))
                {
                    
                }
                else
                {
                    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create("YOUR FTP PATH HERE"+archivo);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.UsePassive = false;
                    ftpRequest.UseBinary = false;
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Credentials = new NetworkCredential("USER", "PASSWORD");

                    using (Stream ftpStream = ftpRequest.GetResponse().GetResponseStream())
                    using (Stream fileStream = File.Create("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + archivo))
                    {
                        ftpStream.CopyTo(fileStream);
                    }
                    logTRNDesc.Add(archivo);
                    i++;
                }
            }
            File.WriteAllLines(log, logTRNDesc);
            logTRNDesc.Clear();
            //Descargo los PAG que no están en el compartido (else)
            foreach (String archivo in filtroPAG)
            {
                if (listaTRNComp.Contains(archivo))
                {

                }
                else
                {
                    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create("YOUR FTP PATH HERE" + archivo);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.UsePassive = false;
                    ftpRequest.UseBinary = false;
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Credentials = new NetworkCredential("USER", "PATH");

                    using (Stream ftpStream = ftpRequest.GetResponse().GetResponseStream())
                    using (Stream fileStream = File.Create("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + archivo))
                    {
                        ftpStream.CopyTo(fileStream);
                    }
                    logTRNDesc.Add(archivo);
                    i++;
                }
            }
            File.AppendAllLines(log, logTRNDesc);
            MessageBox.Show("Se descargaron " + i + " archivos\nPuede ver un detalle en el log generado", "Finalizado");
        }



        public static void bajarRPT()
        {
            string ahora = string.Format("RPT_{0:yyyy-MM-dd_hh-mm-ss_tt}.txt", DateTime.Now);
            string log = "PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + ahora;
            Stream crearLog = File.Create(log);
            crearLog.Close();

            string listadoCrudo;
            string[] filtroRPT;
            var acortado = "";

            List<string> listaRPTComp = new List<string>();
            List<string> logRPTDesc = new List<string>();

            //Acá me conecto a RPT
            FtpWebRequest pedirListado = (FtpWebRequest)WebRequest.Create("YOUR FTP PATH HERE");
            pedirListado.Method = WebRequestMethods.Ftp.ListDirectory;
            pedirListado.Credentials = new NetworkCredential("USER", "PASSWORD");

            //Acá listo los RPT
            using (FtpWebResponse respuestaListado = (FtpWebResponse)pedirListado.GetResponse())
            {
                using (StreamReader leeRespuestaListado = new StreamReader(respuestaListado.GetResponseStream()))
                {
                    listadoCrudo = leeRespuestaListado.ReadToEnd();
                    string[] listadoCrudoSeparado = listadoCrudo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] listadoLimpio = listadoCrudoSeparado.Select(x => x.Replace("WHATEVER YOU WANT TO TAKE OUT OF THE FILE NAME", "")).ToArray();

                    filtroRPT = Array.FindAll(listadoLimpio, x => x.StartsWith("FILTER WORD", StringComparison.Ordinal));
                }
            }

            //Acá listo lo de un directorio en el compartido
            foreach (String archivo in Directory.GetFiles("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC"))
            {
                //Acá limpio para quedarme con el nombre del archivo solo
                var completo = archivo.Split('\\');
                acortado = completo[completo.Length - 1];
                listaRPTComp.Add(acortado);
            }

            // Descargo los RPT que no están en el compartido(else)
            int i = 0;
            foreach (String archivo in filtroRPT)
            {
                if (listaRPTComp.Contains(archivo))
                {

                }
                else
                {
                    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create("YOUR FTP PATH HERE" + archivo);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.UsePassive = false;
                    ftpRequest.UseBinary = false;
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Credentials = new NetworkCredential("USER", "PASSWORD");


                    using (Stream ftpStream = ftpRequest.GetResponse().GetResponseStream())
                    using (Stream fileStream = File.Create("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + archivo))
                    {
                        ftpStream.CopyTo(fileStream);
                    }
                    logRPTDesc.Add(archivo);
                    i++;
                }
            }
            File.WriteAllLines(log, logRPTDesc);
            MessageBox.Show("Se descargaron " + i + " RPT\nPuede ver un detalle en el log generado", "Finalizado");
        }

        public static void bajarBancos()
        {
            string ahora = string.Format("Bancos_Tarjetas_{0:yyyy-MM-dd_hh-mm-ss_tt}.txt", DateTime.Now);
            string log = "PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + ahora;
            Stream crearLog = File.Create(log);
            crearLog.Close();

            string listadoCrudo;
            string[] filtroBancos;
            var acortado = "";

            List<string> listaBancosComp = new List<string>();
            List<string> logBancosDesc = new List<string>();

            //Acá me conecto a PAG_TRN
            FtpWebRequest pedirListado = (FtpWebRequest)WebRequest.Create("YOUR FTP PATH HERE");
            pedirListado.Method = WebRequestMethods.Ftp.ListDirectory;
            pedirListado.Credentials = new NetworkCredential("USER", "PASSWORD");

            //Acá listo los PAG_TRN
            using (FtpWebResponse respuestaListado = (FtpWebResponse)pedirListado.GetResponse())
            {
                using (StreamReader leeRespuestaListado = new StreamReader(respuestaListado.GetResponseStream()))
                {
                    listadoCrudo = leeRespuestaListado.ReadToEnd();
                    string[] listadoCrudoSeparado = listadoCrudo.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                    string[] listadoLimpio = listadoCrudoSeparado.Select(x => x.Replace("WHATEVER YOU WANT TO TAKE OUT OF THE FILE NAME", "")).ToArray();

                    filtroBancos = Array.FindAll(listadoLimpio, x => x.StartsWith("WORD FILTER", StringComparison.Ordinal));
                }
            }

            //Acá listo lo de un directorio en el compartido
            foreach (String archivo in Directory.GetFiles("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC"))
            {
                //Acá limpio para quedarme con el nombre del archivo solo
                var completo = archivo.Split('\\');
                acortado = completo[completo.Length - 1];
                listaBancosComp.Add(acortado);
            }

            // Descargo los PAG_TRN que no están en el compartido(else)
            int i = 0;
            foreach (String archivo in filtroBancos)
            {
                if (listaBancosComp.Contains(archivo))
                {

                }
                else
                {
                    FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create("YOUR FTP PATH HERE" + archivo);
                    ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                    ftpRequest.UsePassive = false;
                    ftpRequest.UseBinary = false;
                    ftpRequest.KeepAlive = false;
                    ftpRequest.Credentials = new NetworkCredential("USER", "PASSWORD");


                    using (Stream ftpStream = ftpRequest.GetResponse().GetResponseStream())
                    using (Stream fileStream = File.Create("PATH TO THE FOLDER WHERE YOU WANT TO UPDATE FILES IN YOUR PC" + archivo))
                    {
                        ftpStream.CopyTo(fileStream);
                    }
                    logBancosDesc.Add(archivo);
                    i++;
                }
            }
            File.WriteAllLines(log, logBancosDesc);
            MessageBox.Show("Se descargaron " + i + " archivos de bancos y tarjetas\nPuede ver un detalle en el log generado", "Finalizado");
        }
    }
}
