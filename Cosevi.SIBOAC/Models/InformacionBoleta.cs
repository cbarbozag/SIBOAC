using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cosevi.SIBOAC.Models
{

    public class Articulos
    {
        public string _codigoArticulo;
        public string _Descripcion;
        public double _Monto;
        public int _puntos;

        public Articulos(string codigoArticulo, string Descripcion, double Monto, int puntos) 
        {
            this._codigoArticulo = codigoArticulo;
            this._Descripcion = Descripcion;
            this._Monto = Monto;
            this._puntos = puntos;
        }
    }

    public class InformacionBoleta
    {
        private List<Articulos> newLista = new List<Articulos>();

        public string NumeroBoleta { get; set; }
        public DateTime FechaHoraBoleta { get; set; }

        public string DescripcionDelegacion { get; set; }
        public string DescripcionAutoridad { get; set; }
        public string DescripcionRol { get; set; }
        public string Usuario { get; set; }
        public string TipoDocumento { get; set; }
        public string Sexo { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string DireccionUsuario { get; set; }
        public string TipoLicencia { get; set; }
        public string Identificacion { get; set; }
        public string LugarHechos { get; set; }
        public int Km { get; set; }
        public string Placa { get; set; }
        public string DescripcionTipoAutomovil { get; set; }

        public string DescripcionCarroceria { get; set; }
        public string DescripcionMarca { get; set; }
        public string RevisionTecnica { get; set; }
        public string DescripcionOficinaImpugna { get; set; }
        public string NivelGases { get; set; }
        public int Velocidad { get; set; }

        public string CodigoInspector { get; set; }
        public string NombreInspector { get; set; }
        public string PiePagina { get; set; }
        public  List<Articulos> ListaArticulos {
            get { return newLista; }
            set { newLista = value; }

        }  
        public double TotalMulta {
            get {
                double Total = 0.0;
                for (int i=0;i<newLista.Count();i++)
                {
                    Total += newLista[i]._Monto;

                }
                return Total;
            }
        }

        public int TotalPuntos
        {
            get
            {
                int Total = 0;
                for (int i = 0; i < newLista.Count(); i++)
                {
                    Total += newLista[i]._puntos;

                }
                return Total;
            }
        }
    }
}  

