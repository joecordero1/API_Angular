using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Produnet.WebBFF.ApiJoe.Dominio.Interfaces;

namespace Produnet.WebBFF.ApiJoe.Infraestructura.Servicios
{
    public class InactividadServicio : IInactividadServicio
    {
        private Timer timer;
        private readonly int umbralDeTiempo = 5; // El umbral de inactividad en segundos
        private bool isSessionLocked = false;

        [HttpPost("iniciar")]
        public void IniciarMonitoreo()
        {
            if (timer == null)
            {
                Console.WriteLine("Monitoreando el estado de actividad del usuario...");
                timer = new Timer(CheckInactividadState, null, 0, 1000); // Verificar cada 1 segundo
            }
        }
        [HttpPost("finalizar")]
        public void DetenerMonitoreo()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
                Console.WriteLine("Monitoreo de inactividad detenido.");
            }
        }

        public (string UserName, string MachineName) ObtenerDetallesUsuarioEquipo()
        {
            string userName = WindowsIdentity.GetCurrent().Name;
            string machineName = Environment.MachineName;
            return (userName, machineName);
        }

        private void CheckInactividadState(object state)
        {
            if (isSessionLocked)
            {
                Console.WriteLine($"El usuario está inactivo debido a bloqueo de sesión a las {DateTime.Now}");
                return;
            }

            var idleTime = GetIdleTime();

            if (idleTime.TotalSeconds >= umbralDeTiempo)
            {
                Console.WriteLine($"El usuario está inactivo desde hace {idleTime.TotalSeconds} segundos a las {DateTime.Now}");
            }
            else
            {
                Console.WriteLine($"El usuario está activo a las {DateTime.Now}");
            }
        }

        private TimeSpan GetIdleTime()
        {
            var lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            if (GetLastInputInfo(ref lastInputInfo))
            {
                var idleTime = TimeSpan.FromMilliseconds(Environment.TickCount - lastInputInfo.dwTime);
                return idleTime;
            }
            else
            {
                throw new InvalidOperationException("No se pudo obtener el tiempo de inactividad del usuario.");
            }
        }

        [DllImport("user32.dll")]
        private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

        [StructLayout(LayoutKind.Sequential)]
        private struct LASTINPUTINFO
        {
            public uint cbSize;
            public uint dwTime;
        }
    }
}
