
using System;
using System.Data;

namespace Entidades.Usuarios
{
    public class ClsProducto
    {
        #region Atributos privados
        private byte _idProducto;
        private string _nombre;
        private int _stock;
        private float _precio;

        //atributos de manejo de la base de datos
        private string _mensajeError, _valorScalar;
        private DataTable _dtResultados;

        #endregion

        #region Atributos publicos
        public byte IdProducto { get => _idProducto; set => _idProducto = value; }
        public string Nombre { get => _nombre; set => _nombre = value; }
        public int Stock { get => _stock; set => _stock = value; }
        public float Precio { get => _precio; set => _precio = value; }
        public string MensajeError { get => _mensajeError; set => _mensajeError = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public DataTable DtResultados { get => _dtResultados; set => _dtResultados = value; }

        #endregion
    }
}
