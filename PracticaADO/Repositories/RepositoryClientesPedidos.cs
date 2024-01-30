using PracticaADO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

# region PROCEDIMIENTOS ALMACENADOS

/*
CREATE OR ALTER PROCEDURE SP_CLIENTES
AS
	SELECT *
	FROM clientes
GO

CREATE OR ALTER PROCEDURE SP_DATOS_CLIENTE
(@NOM_CLIENTE NVARCHAR(MAX))
AS
	SELECT *
	FROM clientes
	WHERE Empresa = @NOM_CLIENTE
GO

CREATE OR ALTER PROCEDURE SP_PEDIDOS_CLIENTE
(@NOM_CLIENTE NVARCHAR(MAX))
AS
	DECLARE @COD_CLIENTE NVARCHAR(MAX)
	SELECT @COD_CLIENTE = CodigoCliente
	FROM clientes
	WHERE Empresa = @NOM_CLIENTE
	SELECT *
	FROM pedidos
	WHERE CodigoCliente = @COD_CLIENTE
GO
*/

# endregion

namespace PracticaADO.Repositories
{
    public class RepositoryClientesPedidos
    {
        private SqlConnection cn;
        private SqlCommand com;
        private SqlDataReader reader;

        public RepositoryClientesPedidos()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=NETCOREPRACTICA;Persist Security Info=True;User ID=SA;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<string> LoadClientes()
        {
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CLIENTES";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> clientes = new List<string>();
            while (this.reader.Read())
            {
                clientes.Add(this.reader["EMPRESA"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            return clientes;
        }

        public Cliente DatosCliente(string nomCliente)
        {
            SqlParameter paramCliente = new SqlParameter("@NOM_CLIENTE", nomCliente);
            this.com.Parameters.Add(paramCliente);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DATOS_CLIENTE";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            Cliente cliente = new Cliente();
            cliente.Empresa = this.reader["Empresa"].ToString();
            cliente.Telefono = int.Parse(this.reader["Telefono"].ToString());
            cliente.Ciudad = this.reader["Ciudad"].ToString();
            cliente.CodigoCliente = nomCliente;
            cliente.Contacto = this.reader["Contacto"].ToString();
            cliente.c
        }
    }
}