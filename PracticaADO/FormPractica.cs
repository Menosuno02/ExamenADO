﻿using PracticaADO.Repositories;
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
        }

        private void LoadClientes()
        {
        }

        private void btnnuevopedido_Click(object sender, EventArgs e)
        {
        }

        private void btneliminarpedido_Click(object sender, EventArgs e)
        {
        }
    }
}