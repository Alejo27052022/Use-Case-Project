
using System;
using System.Data;

namespace Entidades.Usuarios
{
    public class ClsVentas
    {
        #region Atributos privados
        private byte _idVentas;
        private string _Cliente, _Producto;
        private int _Cantidad;
        private float _Total;

        //atributos de manejo de la base de datos
        private string _mensajeError, _valorScalar;
        private DataTable _dtResultados;

        #endregion

        #region Atributos publicos
        public byte IdVentas { get => _idVentas; set => _idVentas = value; }
        public string Cliente { get => _Cliente; set => _Cliente = value; }
        public string Producto { get => _Producto; set => _Producto = value; }
        public int Cantidad { get => _Cantidad; set => _Cantidad = value; }
        public float Total { get => _Total; set => _Total = value; }
        public string MensajeError { get => _mensajeError; set => _mensajeError = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public DataTable DtResultados { get => _dtResultados; set => _dtResultados = value; }
        #endregion
    }
}