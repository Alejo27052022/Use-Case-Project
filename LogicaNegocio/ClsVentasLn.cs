using Datos.DataBase;
using Entidades.Usuarios;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LogicaNegocio
{
    public class ClsVentasLn
    {
        #region Variable privadas
        private ClsDataBase ObjDataBase = null; /* Estado inicial de una base de datos*/
        #endregion

        #region Metodo Index 
        /* Nos permite mostrar todas las operaciones que tenemos*/
        public void Index(ref ClsVentas ObjVentas)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Ventas",
                NombreSP = "[SCH_GENERAL].[SP_Ventas_Index]",
                Scalar = false,
            };
            Ejecutar(ref ObjVentas);
        }
        #endregion

        #region CRUD VENTAS
        public void Create(ref ClsVentas ObjVentas)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Ventas",
                NombreSP = "[SCH_GENERAL].[SP_Ventas_Create]",
                Scalar = true
            };

            ObjDataBase.DtParametros.Rows.Add(@"@Cliente", "17", ObjVentas.Cliente);
            ObjDataBase.DtParametros.Rows.Add(@"@Producto", "17", ObjVentas.Producto);
            ObjDataBase.DtParametros.Rows.Add(@"@Cantidad", "4", ObjVentas.Cantidad);
            ObjDataBase.DtParametros.Rows.Add(@"@Total", "9", ObjVentas.Total);

            Ejecutar(ref ObjVentas);
        }

        public void Read(ref ClsVentas ObjVentas)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Ventas",
                NombreSP = "[SCH_GENERAL].[SP_Ventas_Read]",
                Scalar = false
            };

            ObjDataBase.DtParametros.Rows.Add(@"@IdVentas", "2", ObjVentas.IdVentas);
            Ejecutar(ref ObjVentas);
        }

        public void Update(ref ClsVentas ObjVentas)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Ventas",
                NombreSP = "[SCH_GENERAL].[SP_Ventas_Update]",
                Scalar = true
            };

            ObjDataBase.DtParametros.Rows.Add(@"@IdVentas", "2", ObjVentas.IdVentas);
            ObjDataBase.DtParametros.Rows.Add(@"@Cliente", "17", ObjVentas.Cliente);
            ObjDataBase.DtParametros.Rows.Add(@"@Producto", "17", ObjVentas.Producto);
            ObjDataBase.DtParametros.Rows.Add(@"@Cantidad", "4", ObjVentas.Cantidad);
            ObjDataBase.DtParametros.Rows.Add(@"@Total", "9", ObjVentas.Total);

            Ejecutar(ref ObjVentas);
        }

        public void Delete(ref ClsVentas ObjVentas)
        {
            ObjDataBase = new ClsDataBase()
            {
                NombreTabla = "Ventas",
                NombreSP = "[SCH_GENERAL].[SP_Ventas_Delete]",
                Scalar = true
            };

            ObjDataBase.DtParametros.Rows.Add(@"@IdVentas", "2", ObjVentas.IdVentas);

            Ejecutar(ref ObjVentas);
        }
        #endregion

        #region Metodos privados
        private void Ejecutar(ref ClsVentas ObjVentas)
        {
            ObjDataBase.CRUD(ref ObjDataBase);

            if (ObjDataBase.MensajeErrorDB == null)
            {
                if (ObjDataBase.Scalar)
                {
                    ObjVentas.ValorScalar = ObjDataBase.ValorScalar;
                }
                else
                {
                    ObjVentas.DtResultados = ObjDataBase.DsResultados.Tables[0];
                    if (ObjVentas.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in ObjVentas.DtResultados.Rows)
                        {
                            ObjVentas.IdVentas = Convert.ToByte(item["IdVentas"].ToString());
                            ObjVentas.Cliente = item["Cliente"].ToString();
                            ObjVentas.Cantidad = (int)Convert.ToInt64(item["Cantidad"].ToString());
                            ObjVentas.Total = (float)Convert.ToDecimal(item["Total"].ToString());

                        }
                    }
                }
            }
            else
            {
                ObjVentas.MensajeError = ObjDataBase.MensajeErrorDB;
            }
        }
        #endregion
    }
}
