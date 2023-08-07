
using Datos.DataBase;
using Entidades.Usuarios;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LogicaNegocio.Usuarios
{
    public class ClsProductoLn
    {
        #region Variable privadas
        private ClsDataBase ObjDataBase = null; /* Estado inicial de una base de datos*/
        #endregion

        #region Metodo Index 
        /* Nos permite mostrar todas las operaciones que tenemos*/
        public void Index(ref ClsProducto ObjProducto)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Productos",
                NombreSP = "[SCH_GENERAL].[SP_Productos_Index]",
                Scalar = false,
            };
            Ejecutar(ref ObjProducto);
        }
        #endregion

        #region CRUD PRODUCTOS
        public void Create(ref ClsProducto ObjProducto)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Productos",
                NombreSP = "[SCH_GENERAL].[SP_Productos_Create]",
                Scalar = true
            };

            ObjDataBase.DtParametros.Rows.Add(@"@Nombre", "17", ObjProducto.Nombre);
            ObjDataBase.DtParametros.Rows.Add(@"@Stock", "4", ObjProducto.Stock);
            ObjDataBase.DtParametros.Rows.Add(@"@Precio", "9", ObjProducto.Precio);

            Ejecutar(ref ObjProducto);
        }

        public void Read(ref ClsProducto ObjProducto)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Productos",
                NombreSP = "[SCH_GENERAL].[SP_Productos_Read]",
                Scalar = false
            };

            ObjDataBase.DtParametros.Rows.Add(@"@IdProducto", "2", ObjProducto.IdProducto);
            Ejecutar(ref ObjProducto);
        }

        public void Update(ref ClsProducto ObjProducto)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Productos",
                NombreSP = "[SCH_GENERAL].[SP_Productos_Update]",
                Scalar = true
            };

            ObjDataBase.DtParametros.Rows.Add(@"@IdProducto", "2", ObjProducto.IdProducto);
            ObjDataBase.DtParametros.Rows.Add(@"@Nombre", "17", ObjProducto.Nombre);
            ObjDataBase.DtParametros.Rows.Add(@"@Stock", "4", ObjProducto.Stock);
            ObjDataBase.DtParametros.Rows.Add(@"@Precio", "9", ObjProducto.Precio);

            Ejecutar(ref ObjProducto);
        }


        public void Delete(ref ClsProducto ObjProducto)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Productos",
                NombreSP = "[SCH_GENERAL].[SP_Productos_Delete]",
                Scalar = true
            };

            ObjDataBase.DtParametros.Rows.Add(@"@IdProducto", "2", ObjProducto.IdProducto);

            Ejecutar(ref ObjProducto);
        }

        public void CargarBox(ref ClsProducto ObjProducto)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Productos",
                NombreSP = "[SCH_GENERAL].[SP_CARGARCOMBOBOX]",
                Scalar = false
            };

            Ejecutar_CargarCombo(ref ObjProducto);
        }

        #endregion

        #region Metodos privados
        private void Ejecutar(ref ClsProducto ObjProducto)
        {
            ObjDataBase.CRUD(ref ObjDataBase);

            if (ObjDataBase.MensajeErrorDB == null)
            {
                if (ObjDataBase.Scalar)
                {
                    ObjProducto.ValorScalar = ObjDataBase.ValorScalar;
                }
                else
                {
                    ObjProducto.DtResultados = ObjDataBase.DsResultados.Tables[0];
                    if (ObjProducto.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjProducto.DtResultados.Rows)
                        {
                            ObjProducto.IdProducto = Convert.ToByte(item["IdProducto"].ToString());
                            ObjProducto.Nombre = item["Nombre"].ToString();
                            ObjProducto.Stock = (int)Convert.ToInt64(item["Stock"].ToString());
                            ObjProducto.Precio = (float)Convert.ToDecimal(item["Precio"].ToString());

                        }
                    }
                }
            }
            else
            {
                ObjProducto.MensajeError = ObjDataBase.MensajeErrorDB;
            }
        }

        private void Ejecutar_CargarCombo(ref ClsProducto ObjProducto)
        {
            ObjDataBase.CRUD(ref ObjDataBase);

            if (ObjDataBase.MensajeErrorDB == null)
            {
                if (ObjDataBase.Scalar)
                {
                    ObjProducto.ValorScalar = ObjDataBase.ValorScalar;
                }
                else
                {
                    ObjProducto.DtResultados = ObjDataBase.DsResultados.Tables[0];
                    if (ObjProducto.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjProducto.DtResultados.Rows)
                        {
                            ObjProducto.Nombre = item["Nombre"].ToString();
                        }
                    }
                }
            }
            else
            {
                ObjProducto.MensajeError = ObjDataBase.MensajeErrorDB;
            }
        }
        #endregion
    }
}
