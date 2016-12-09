using System;
using System.Collections.Generic;
using System.Linq;

namespace Cosevi.SIBOAC.Models
{

    public class Articulos
    {
       
        public string codigo_articulo;
        public string descripcion;
        public decimal? multa;
        public int puntos;

        public Articulos(string codigo_articulo, string descripcion, decimal? multa, int puntos)
        {
            this.codigo_articulo = codigo_articulo;
            this.descripcion = descripcion;
            this.multa = multa;
            this.puntos = puntos;
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
        public string ParteOficial { get; set; }
        public string PiePagina { get; set; }
        public  List<Articulos> ListaArticulos {
            get { return newLista; }
            set { newLista = value; }

        }  
        public decimal TotalMulta {
            get {
                decimal Total = 0;
                for (int i=0;i<newLista.Count();i++)
                {
                    Total += newLista[i].multa == null ? 0: (decimal)newLista[i].multa;    
                                                       

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
                    Total += newLista[i].puntos;

                }
                return Total;
            }
        }
    }
}  

