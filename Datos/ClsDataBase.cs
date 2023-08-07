
using System;
using System.Data;
using System.Data.SqlClient;

/* La capa de Datos esta cerca de la Base de Datos */

namespace Datos.DataBase
{
    public class ClsDataBase
    {
        #region Variables privadas 
        /* Son las variables que existen para la base de datos*/

        private SqlConnection _objSqlConnection;
        private SqlDataAdapter _objSqlDataAdapter;
        private SqlCommand _objSqlCommand;
        private DataSet _dsResultados;
        private DataTable _dtParametros;
        private string _nombreTabla, _nombreSP, _mensajeErrorDB, _valorScalar, _nombreDB;
        private bool _scalar;

        #endregion
        #region Variables públicas
        /* Estas variables privadas pasaran a publicas para hacer uso en el aplicativo*/

        public SqlConnection ObjSqlConnection { get => _objSqlConnection; set => _objSqlConnection = value; }
        public SqlDataAdapter ObjSqlDataAdapter { get => _objSqlDataAdapter; set => _objSqlDataAdapter = value; }
        public SqlCommand ObjSqlCommand { get => _objSqlCommand; set => _objSqlCommand = value; }
        public DataSet DsResultados { get => _dsResultados; set => _dsResultados = value; }
        public DataTable DtParametros { get => _dtParametros; set => _dtParametros = value; }
        public string NombreTabla { get => _nombreTabla; set => _nombreTabla = value; }
        public string NombreSP { get => _nombreSP; set => _nombreSP = value; }
        public string MensajeErrorDB { get => _mensajeErrorDB; set => _mensajeErrorDB = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public string NombreDB { get => _nombreDB; set => _nombreDB = value; }
        public bool Scalar { get => _scalar; set => _scalar = value; }
        #endregion

        #region Constructores
        /* Son las propiedades de las variables */
        public ClsDataBase()
        {
            DtParametros = new DataTable("SpParametros");
            DtParametros.Columns.Add("Nombre");
            DtParametros.Columns.Add("TipoDato");
            DtParametros.Columns.Add("Valor");

            NombreDB = "DB_IntComex";

        }

        #endregion

        #region Métodos privados
        /* Son metodos propios de la base de datos */
        private void CrearConexionBaseDatos(ref ClsDataBase ObjDataBase)
        {
            switch (ObjDataBase.NombreDB)
            {
                case "DB_IntComex": /* Es la llamada de la base de datos*/
                    ObjDataBase.ObjSqlConnection = new SqlConnection(Properties.Settings.Default.CadenaConexion);
                    break;
                default:
                    break;
            }

        }
        private void ValidarConexionBaseDatos(ref ClsDataBase ObjDataBase)
        {
            if (ObjDataBase.ObjSqlConnection.State == ConnectionState.Closed) //Se pregunta en que estado la base de datos y en el caso que lo este, se abra la base de datos
            {
                ObjDataBase.ObjSqlConnection.Open(); //Abre la base de datos
            }
            else
            {
                ObjDataBase.ObjSqlConnection.Close(); //Cierra la base de datos
                ObjDataBase.ObjSqlConnection.Dispose(); //Borra de la memoria cache
            }
        }
        private void AgregarParametros(ref ClsDataBase ObjDataBase)
        {
            if (ObjDataBase.DtParametros != null) //Pregunta si tiene parametros
            {
                SqlDbType TipoDatoSQL = new SqlDbType(); /* Nos permite recorrer la DB */

                foreach (DataRow item in ObjDataBase.DtParametros.Rows) /* Recorre por todos los elementos de la fila */
                {
                    switch (item[1])
                    {
                        case "1": /* Se va enlistar todos los tipos de datos que se esta trabajando en la base de datos*/
                            TipoDatoSQL = SqlDbType.Bit;
                            break;
                        case "2":
                            TipoDatoSQL = SqlDbType.TinyInt;
                            break;
                        case "3":
                            TipoDatoSQL = SqlDbType.SmallInt;
                            break;
                        case "4":
                            TipoDatoSQL = SqlDbType.Int;
                            break;
                        case "5":
                            TipoDatoSQL = SqlDbType.BigInt;
                            break;
                        case "6":
                            TipoDatoSQL = SqlDbType.Decimal;
                            break;
                        case "7":
                            TipoDatoSQL = SqlDbType.SmallMoney;
                            break;
                        case "8":
                            TipoDatoSQL = SqlDbType.Money;
                            break;
                        case "9":
                            TipoDatoSQL = SqlDbType.Float;
                            break;
                        case "10":
                            TipoDatoSQL = SqlDbType.Real;
                            break;
                        case "11":
                            TipoDatoSQL = SqlDbType.Date;
                            break;
                        case "12":
                            TipoDatoSQL = SqlDbType.Time;
                            break;
                        case "13":
                            TipoDatoSQL = SqlDbType.SmallDateTime;
                            break;
                        case "14":
                            TipoDatoSQL = SqlDbType.Char;
                            break;
                        case "15":
                            TipoDatoSQL = SqlDbType.NChar;
                            break;
                        case "16":
                            TipoDatoSQL = SqlDbType.VarChar;
                            break;
                        case "17":
                            TipoDatoSQL = SqlDbType.NVarChar;
                            break;
                        default:
                            break;
                    }

                    if (ObjDataBase.Scalar) /* Analiza parametros, si el item se encuentra vacio */
                    {
                        if (item[2].ToString().Equals(string.Empty))
                        {
                            ObjDataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value; /* Si es correcto se va devolver Nulo */
                        }
                        else
                        {
                            ObjDataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString(); /* Si no va devolver un valor*/
                        }
                    }
                    else /* Analiza un selectCommand (proceso que esta solicitando de acuerdo a los parametros) */
                    {
                        if (item[2].ToString().Equals(string.Empty))
                        {
                            ObjDataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value;
                        }
                        else
                        {
                            ObjDataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString();

                        }
                    }
                }

            }

        }
        private void PrepararConexionBaseDatos(ref ClsDataBase ObjDataBase) /* Llamamos a las funciones */
        {
            CrearConexionBaseDatos(ref ObjDataBase);
            ValidarConexionBaseDatos(ref ObjDataBase);
        }
        private void EjecutarDataAdapter(ref ClsDataBase ObjDataBase) /* */
        {
            try
            {
                /* Creamos el DataSet */

                PrepararConexionBaseDatos(ref ObjDataBase);

                ObjDataBase.ObjSqlDataAdapter = new SqlDataAdapter(ObjDataBase.NombreSP, ObjDataBase.ObjSqlConnection);
                ObjDataBase.ObjSqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;

                AgregarParametros(ref ObjDataBase);

                ObjDataBase.DsResultados = new DataSet();
                ObjDataBase.ObjSqlDataAdapter.Fill(ObjDataBase.DsResultados, ObjDataBase.NombreTabla);
            }
            catch (Exception ex)
            {
                /* Caso contrario muestra un mensaje de error*/
                ObjDataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                /* Pregunta en que estado se encuentra la base de datos, caso se encuentre abierta, se ejecutarà el metodo validar*/
                if (ObjDataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBaseDatos(ref ObjDataBase);
                }
            }
        }

        private void EjecutarCommand(ref ClsDataBase ObjDataBase)
        {
            try
            {
                PrepararConexionBaseDatos(ref ObjDataBase);
                ObjDataBase.ObjSqlCommand = new SqlCommand(ObjDataBase.NombreSP, ObjDataBase.ObjSqlConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                AgregarParametros(ref ObjDataBase);

                if (ObjDataBase.Scalar)
                {
                    var result = ObjDataBase.ObjSqlCommand.ExecuteScalar();
                    ObjDataBase.ValorScalar = result != null ? result.ToString().Trim() : string.Empty;
                }
                else
                {
                    ObjDataBase.ObjSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ObjDataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                if (ObjDataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    ValidarConexionBaseDatos(ref ObjDataBase);
                }
            }
        }


        #endregion 

        #region Métodos públicos
        /* Son los metodos propios de la Aplicacion */
        public void CRUD(ref ClsDataBase ObjDataBase) /* Permite hacer todas las operaciones CRUD*/
        {
            if (ObjDataBase.Scalar)
            {
                EjecutarCommand(ref ObjDataBase);
            }
            else
            {
                EjecutarDataAdapter(ref ObjDataBase);
            }
        }
        #endregion
    }
}
