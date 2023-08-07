using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Entidades.Usuarios;
using LogicaNegocio;
using LogicaNegocio.Usuarios;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Presentacion.Formulario
{
    public partial class FormCompraProducto : Form
    {
        private ClsProductoLn ObjProductoLn { get; set; } // Declara ObjProductoLn como una propiedad de lectura y escritura
        private ClsProducto ObjProducto = null;
        private ClsVentas ObjVentas = null;
        private readonly ClsVentasLn ObjVentasLn = new ClsVentasLn();

        public FormCompraProducto()
        {
            InitializeComponent();
            CargarVenta();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void FormCompraProducto_Load(object sender, EventArgs e)
        {
            Console.WriteLine("FormCompraProducto_Load event called."); // Mensaje de depuración

            ObjProductoLn = new ClsProductoLn(); // Verifica que el objeto ObjProductoLn esté inicializado correctamente

            CargarCombobox();
        }

        private void cbx_producto_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarProductoSeleccionado();
        }

        private void ActualizarProductoSeleccionado()
        {
            // Obtener el producto seleccionado del ComboBox
            DataRowView seleccion = cbx_producto.SelectedItem as DataRowView;

            // Verificar si se seleccionó un producto
            if (seleccion != null)
            {
                // Obtener los datos del producto seleccionado
                int stock = Convert.ToInt32(seleccion["Stock"]);
                float precio = Convert.ToSingle(seleccion["Precio"]);

                // Actualizar los controles del formulario con los datos del producto seleccionado
                lblStock.Text = stock.ToString();
                lblPrecio.Text = precio.ToString();

                // Mostrar el valor seleccionado en el TextBox
                txtProd.Text = cbx_producto.Text;
            }
        }


        private void CargarCombobox()
        {
            ObjProducto = new ClsProducto();
            ObjProductoLn.CargarBox(ref ObjProducto);
            if (ObjProducto.MensajeError == null)
            {
                cbx_producto.DisplayMember = "Nombre"; // Especifica la columna a mostrar
                cbx_producto.ValueMember = "IdProducto"; // Especifica la columna del valor subyacente
                cbx_producto.DataSource = ObjProducto.DtResultados;
                cbx_producto.Refresh(); // Actualiza la visualización de los datos en el ComboBox
            }
            else
            {
                MessageBox.Show(ObjProducto.MensajeError, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Convertir la cantidad a un valor entero
            int cantidad;
            bool esValido = int.TryParse(txbCantidad.Text, out cantidad);

            if (esValido)
            {
                // Obtener el precio y el stock del producto seleccionado
                DataRowView seleccion = cbx_producto.SelectedItem as DataRowView;
                float precio = Convert.ToSingle(seleccion["Precio"]);
                int stock = Convert.ToInt32(seleccion["Stock"]);

                // Calcular el precio total
                float total = cantidad * precio;

                // Crear el objeto ClsVentas con los valores adecuados
                ObjVentas = new ClsVentas()
                {
                    Producto = cbx_producto.Text,
                    Cliente = txtCliente.Text,
                    Cantidad = cantidad,
                    Total = total // Asignar el valor total calculado
                };

                // Crear el objeto ClsProducto con los valores adecuados
                ObjProducto = new ClsProducto()
                {
                    Stock = stock - cantidad
                };

                // Actualizar el stock en la base de datos
                ObjProductoLn.Update(ref ObjProducto);

                ObjVentasLn.Create(ref ObjVentas);
                // Resto del código para agregar la venta
                if (ObjVentas.MensajeError == null)
                {
                    MessageBox.Show("Su venta se ha generado correctamente y su valor ha sido de: " + total);
                    CargarVenta();
                }
                else
                {
                    MessageBox.Show(ObjVentas.MensajeError, "No se pudo guardar correctamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("La cantidad ingresada no es válida.", "Error de cantidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CargarVenta()
        {
            ObjVentas = new ClsVentas();
            ObjVentasLn.Index(ref ObjVentas);
            if(ObjVentas.MensajeError == null)
            {
                DvgDatos.DataSource = ObjVentas.DtResultados;
            }
            else
            {
                MessageBox.Show(ObjVentas.MensajeError, "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DvgDatos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DvgDatos.Columns[e.ColumnIndex].Name == "Editar")
                {
                    ObjVentas = new ClsVentas()
                    {
                        IdVentas = Convert.ToByte(DvgDatos.Rows[e.RowIndex].Cells["IdVentas"].Value.ToString())
                    };

                    lblID.Text = ObjVentas.IdVentas.ToString();

                    ObjVentasLn.Read(ref ObjVentas);

                    txtCliente.Text = ObjVentas.Cliente;
                    txbCantidad.Text = ObjVentas.Cantidad.ToString();

                    // Actualizar el valor de txtProd con el valor de la celda correspondiente a la columna "Producto"
                    txtProd.Text = DvgDatos.Rows[e.RowIndex].Cells["Producto"].Value.ToString();

                    // Buscar el valor seleccionado en el ComboBox
                    int index = cbx_producto.FindStringExact(txtProd.Text);

                    // Si se encuentra el valor, establecer el valor seleccionado del ComboBox en el elemento correspondiente
                    if (index != -1)
                    {
                        cbx_producto.SelectedIndex = index;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción
            }
        }

        private void btnGenerarVenta_Click(object sender, EventArgs e)
        {
            // Obtener los datos del producto seleccionado
            DataRowView seleccion = cbx_producto.SelectedItem as DataRowView;
            if (seleccion != null)
            {
                // Crear el objeto ClsVentas con los valores adecuados
                ObjVentas = new ClsVentas()
                {
                    IdVentas = Convert.ToByte(lblID.Text),
                    Producto = cbx_producto.Text,
                    Cliente = txtCliente.Text,
                    Cantidad = int.Parse(txbCantidad.Text),
                    Total = 0 // Inicializar en cero temporalmente
                };

                // Obtener el precio del producto seleccionado
                float precio = Convert.ToSingle(seleccion["Precio"]);

                // Calcular el valor total
                ObjVentas.Total = ObjVentas.Cantidad * precio;

                ObjVentasLn.Update(ref ObjVentas);
                if (ObjVentas.MensajeError == null)
                {
                    MessageBox.Show("La venta se ha actualizado correctamente. Total: " + ObjVentas.Total.ToString());
                    CargarVenta();
                }
                else
                {
                    MessageBox.Show(ObjVentas.MensajeError, "No se pudo actualizar correctamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado un producto válido.", "Error de producto", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            // Obtener los datos de la fila seleccionada en el DataGridView
            DataGridViewRow filaSeleccionada = DvgDatos.CurrentRow;
            if (filaSeleccionada != null)
            {
                // Obtener el valor del IdVentas de la fila seleccionada
                int idVentas = Convert.ToInt32(filaSeleccionada.Cells["IdVentas"].Value);

                // Crear el objeto ClsVentas con los valores adecuados
                ObjVentas = new ClsVentas()
                {
                    IdVentas = (byte)idVentas
                };

                ObjVentasLn.Delete(ref ObjVentas);
                if (ObjVentas.MensajeError == null)
                {
                    MessageBox.Show("La venta se ha eliminado correctamente.");
                    CargarVenta();
                }
                else
                {
                    MessageBox.Show(ObjVentas.MensajeError, "No se pudo eliminar correctamente", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtProd_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
