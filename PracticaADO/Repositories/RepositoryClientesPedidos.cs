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
	SELECT Empresa
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
	SELECT CodigoPedido
	FROM pedidos
	WHERE CodigoCliente = @COD_CLIENTE
GO

CREATE OR ALTER PROCEDURE SP_DATOS_PEDIDO
(@COD_PEDIDO NVARCHAR(MAX))
AS
	SELECT *
	FROM pedidos
	WHERE CodigoPedido = @COD_PEDIDO
GO

CREATE OR ALTER PROCEDURE SP_DELETE_PEDIDO
(@COD_PEDIDO NVARCHAR(MAX))
AS
	DELETE FROM pedidos
	WHERE CodigoPedido = @COD_PEDIDO
GO

CREATE OR ALTER PROCEDURE SP_CREATE_PEDIDO
(@COD_PEDIDO NVARCHAR(MAX), @COD_CLIENTE NVARCHAR(MAX),
@FECHA_ENTREGA DATETIME, @FORMA_ENVIO NVARCHAR(MAX),
@IMPORTE INT)
AS
	INSERT INTO pedidos
	VALUES (@COD_PEDIDO, @COD_CLIENTE, @FECHA_ENTREGA, @FORMA_ENVIO, @IMPORTE)
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
                clientes.Add(this.reader["Empresa"].ToString());
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
            cliente.Empresa = nomCliente;
            cliente.Telefono = int.Parse(this.reader["Telefono"].ToString());
            cliente.Ciudad = this.reader["Ciudad"].ToString();
            cliente.CodigoCliente = this.reader["CodigoCliente"].ToString();
            cliente.Contacto = this.reader["Contacto"].ToString();
            cliente.Cargo = this.reader["Cargo"].ToString();
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return cliente;
        }

        public List<string> GetPedidosCliente(string nomCliente)
        {
            SqlParameter paramCliente = new SqlParameter("@NOM_CLIENTE", nomCliente);
            this.com.Parameters.Add(paramCliente);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_PEDIDOS_CLIENTE";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            List<string> pedidos = new List<string>();
            while (this.reader.Read())
            {
                pedidos.Add(this.reader["CodigoPedido"].ToString());
            }
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedidos;
        }

        public Pedido GetPedido(string codPedido)
        {
            SqlParameter paramPedido = new SqlParameter("@COD_PEDIDO", codPedido);
            this.com.Parameters.Add(paramPedido);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DATOS_PEDIDO";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            Pedido pedido = new Pedido();
            pedido.CodigoPedido = codPedido;
            pedido.CodigoCliente = this.reader["CodigoCliente"].ToString();
            pedido.FormaEnvio = this.reader["FormaEnvio"].ToString();
            pedido.FechaEntrega = this.reader["FechaEntrega"].ToString();
            pedido.Importe = int.Parse(this.reader["Importe"].ToString());
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return pedido;
        }

        public string GetCodigoCliente(string nomCliente)
        {
            SqlParameter paramCliente = new SqlParameter("@NOM_CLIENTE", nomCliente);
            this.com.Parameters.Add(paramCliente);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DATOS_CLIENTE";
            this.cn.Open();
            this.reader = this.com.ExecuteReader();
            this.reader.Read();
            string codigo = this.reader["CodigoCliente"].ToString();
            this.reader.Close();
            this.cn.Close();
            this.com.Parameters.Clear();
            return codigo;
        }

        public int CreatePedido(Pedido pedido)
        {
            SqlParameter paramCodPedido = new SqlParameter("@COD_PEDIDO", pedido.CodigoPedido);
            SqlParameter paramCodCliente = new SqlParameter("@COD_CLIENTE", pedido.CodigoCliente);
            SqlParameter paramImporte = new SqlParameter("@IMPORTE", pedido.Importe);
            SqlParameter paramEnvio = new SqlParameter("@FORMA_ENVIO", pedido.FormaEnvio);
            SqlParameter paramFechaEntrega = new SqlParameter("@FECHA_ENTREGA", pedido.FechaEntrega);
            this.com.Parameters.Add(paramCodPedido);
            this.com.Parameters.Add(paramCodCliente);
            this.com.Parameters.Add(paramImporte);
            this.com.Parameters.Add(paramEnvio);
            this.com.Parameters.Add(paramFechaEntrega);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CREATE_PEDIDO";
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }

        public int DeletePedido(string codPedido)
        {
            SqlParameter paramPedido = new SqlParameter("@COD_PEDIDO", codPedido);
            this.com.Parameters.Add(paramPedido);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_DELETE_PEDIDO";
            this.cn.Open();
            int result = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return result;
        }
    }
}