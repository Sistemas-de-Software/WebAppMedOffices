using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppMedOffices.Shared
{
    // evitar acentos
    public enum Dia
    {
        DOM = 0,
        LUN = 1,
        MAR = 2,
        MIE = 3,
        JUE = 4,
        VIE = 5,
        SAB = 6,
    }

    // Evitar ñ
    public enum TrabajoTurno
    {
        M = 0, // Mañana
        T = 1, // Tarde
        G = 2, // Guardia
    }

    public enum Estado
    {
        Disponible = 0, 
        Reservado = 1, 
        Atendido = 2, 
    }
}