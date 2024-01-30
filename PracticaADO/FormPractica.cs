using PracticaADO.Models;
using PracticaADO.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PracticaADO
{
    public partial class FormPractica : Form
    {
        private RepositoryClientesPedidos repo;

        public FormPractica()
        {
            InitializeComponent();
            this.repo = new RepositoryClientesPedidos();
            foreach (string cliente in this.repo.LoadClientes())
            {
                this.cmbclientes.Items.Add(cliente);
            }
        }

        private void cmbclientes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cmbclientes.SelectedIndex != -1)
            {
                string codCliente = this.cmbclientes.SelectedItem.ToString();
                Cliente datosCliente = this.repo.DatosCliente(codCliente);
                this.txtcargo.Text = datosCliente.Cargo;
                this.txtempresa.Text = datosCliente.Empresa;
                this.txtcontacto.Text = datosCliente.Contacto;
                this.txtciudad.Text = datosCliente.Ciudad;
                this.txttelefono.Text = datosCliente.Telefono.ToString();
                this.lstpedidos.Items.Clear();
                foreach (string pedido in this.repo.GetPedidosCliente(codCliente))
                {
                    this.lstpedidos.Items.Add(pedido);
                }
            }
        }

        private void lstpedidos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lstpedidos.SelectedIndex != -1)
            {
                string codPedido = this.lstpedidos.SelectedItem.ToString();
                Pedido datosPedido = this.repo.GetPedido(codPedido);
                this.txtcodigopedido.Text = datosPedido.CodigoPedido;
                this.txtformaenvio.Text = datosPedido.FormaEnvio;
                this.txtimporte.Text = datosPedido.Importe.ToString();
                this.txtfechaentrega.Text = datosPedido.FechaEntrega;
            }
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
            if (this.cmbclientes.SelectedIndex != -1)
            {
                string nomEmpresa = this.cmbclientes.SelectedItem.ToString();
                Pedido nuevoPedido = new Pedido();
                nuevoPedido.CodigoCliente = this.repo.GetCodigoCliente(nomEmpresa);
                nuevoPedido.CodigoPedido = this.txtcodigopedido.Text;
                nuevoPedido.Importe = int.Parse(this.txtimporte.Text);
                nuevoPedido.FormaEnvio = this.txtformaenvio.Text;
                nuevoPedido.FechaEntrega = this.txtfechaentrega.Text;
                int result = this.repo.CreatePedido(nuevoPedido);
                this.lstpedidos.Items.Clear();
                foreach (string pedido in this.repo.GetPedidosCliente(nomEmpresa))
                {
                    this.lstpedidos.Items.Add(pedido);
                }
                MessageBox.Show("Pedidos creados: " + result);
            }
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
            if (this.lstpedidos.SelectedIndex != -1)
            {
                string codPedido = this.lstpedidos.SelectedItem.ToString();
                int result = this.repo.DeletePedido(codPedido);
                string codCliente = this.cmbclientes.SelectedItem.ToString();
                this.lstpedidos.Items.Clear();
                foreach (string pedido in this.repo.GetPedidosCliente(codCliente))
                {
                    this.lstpedidos.Items.Add(pedido);
                }
                MessageBox.Show("Pedidos eliminados: " + result);
            }
        }
    }
}