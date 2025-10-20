using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;

namespace PracticaProfesional2025
{
    public class ComponenteRepository
    {
        public int Insert(Componente componente)
        {
            if (componente == null)
                throw new ArgumentNullException("componente");

            int idComponente = 0;

            string query = "INSERT INTO Componentes (tipo, marca, modelo, caracteristicas, numero_serie, estado_id, fecha_compra) " +
                           "VALUES (@Tipo, @Marca, @Modelo, @Caracteristicas, @NumeroSerie, @Estado, @Fecha); SELECT SCOPE_IDENTITY();";

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Tipo", componente.Tipo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Marca", componente.Marca ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Modelo", componente.Modelo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Caracteristicas", componente.Caracteristicas ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", componente.Estado_Id);
                cmd.Parameters.AddWithValue("@NumeroSerie", componente.Numero_Serie ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Fecha", componente.Fecha_Compra);

                con.Open();

                Debug.WriteLine("---- Insert Componente (sin transaction) ----");
                Debug.WriteLine(cmd.CommandText);
                foreach (SqlParameter p in cmd.Parameters)
                {
                    Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));
                }

                object resultado = cmd.ExecuteScalar();
                if (resultado != null)
                    idComponente = Convert.ToInt32(resultado);
            }

            return idComponente;
        }

        // Sobrecarga: inserta usando la conexión y transacción proporcionadas (evita condiciones de carrera)
        public int Insert(Componente componente, SqlConnection con, SqlTransaction transaction)
        {
            if (componente == null)
                throw new ArgumentNullException("componente");
            if (con == null)
                throw new ArgumentNullException("con");
            if (transaction == null)
                throw new ArgumentNullException("transaction");

            // Asegurar un Numero_Serie único usando la misma conexión/transaction
            string baseNumeroSerie = string.IsNullOrWhiteSpace(componente.Numero_Serie)
                ? "SN" + DateTime.Now.ToString("yyyyMMddHHmmss")
                : componente.Numero_Serie.Trim();

            string candidate = baseNumeroSerie;
            int sufijo = 1;

            while (true)
            {
                using (var cmdCheck = new SqlCommand("SELECT COUNT(*) FROM Componentes WHERE numero_serie = @NumeroSerie", con, transaction))
                {
                    cmdCheck.Parameters.AddWithValue("@NumeroSerie", candidate);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                    if (count == 0)
                        break;
                }

                candidate = string.Format("{0}_{1}", baseNumeroSerie, sufijo++);
            }

            componente.Numero_Serie = candidate;

            string query = "INSERT INTO Componentes (tipo, marca, modelo, caracteristicas, numero_serie, estado_id, fecha_compra) " +
                           "VALUES (@Tipo, @Marca, @Modelo, @Caracteristicas, @NumeroSerie, @Estado, @Fecha); SELECT SCOPE_IDENTITY();";

            using (var cmd = new SqlCommand(query, con, transaction))
            {
                cmd.Parameters.AddWithValue("@Tipo", componente.Tipo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Marca", componente.Marca ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Modelo", componente.Modelo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Caracteristicas", componente.Caracteristicas ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", componente.Estado_Id);
                cmd.Parameters.AddWithValue("@NumeroSerie", componente.Numero_Serie ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Fecha", componente.Fecha_Compra);

                Debug.WriteLine("---- Insert Componente (con transaction) ----");
                Debug.WriteLine(cmd.CommandText);
                foreach (SqlParameter p in cmd.Parameters)
                {
                    Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));
                }

                object resultado = cmd.ExecuteScalar();
                return resultado != null ? Convert.ToInt32(resultado) : 0;
            }
        }

        // Devuelve DataTable de componentes asociados a una computadora (aliases para DataField)
        public DataTable ObtenerPorComputadora(int idComputadora)
        {
            var dt = new DataTable();
            string sql = @"
                SELECT 
                    c.id_componente AS id_componente,
                    c.tipo AS Tipo,
                    c.marca AS Marca,
                    c.modelo AS Modelo,
                    c.caracteristicas AS Caracteristicas,
                    c.numero_serie AS Numero_Serie,
                    c.estado_id AS Estado_id,
                    c.fecha_compra AS Fecha_compra
                FROM Componentes c
                INNER JOIN Computadora_Componentes cc ON cc.id_componente = c.id_componente
                WHERE cc.id_computadora = @IdComputadora
                ORDER BY c.tipo, c.marca;";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@IdComputadora", idComputadora);
                using (var da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
            }

            return dt;
        }

        // Eliminar: quita vínculos y elimina componente
        public void EliminarComponente(int idComponente)
        {
            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    try
                    {
                        // Eliminar vínculos con computadoras
                        using (var cmd = new SqlCommand("DELETE FROM Computadora_Componentes WHERE id_componente = @id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", idComponente);
                            Debug.WriteLine("---- DELETE Computadora_Componentes ----");
                            Debug.WriteLine(cmd.CommandText);
                            foreach (SqlParameter p in cmd.Parameters)
                                Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));

                            cmd.ExecuteNonQuery();
                        }

                        // Eliminar registro de componente
                        using (var cmd2 = new SqlCommand("DELETE FROM Componentes WHERE id_componente = @id", con, tx))
                        {
                            cmd2.Parameters.AddWithValue("@id", idComponente);
                            Debug.WriteLine("---- DELETE Componentes ----");
                            Debug.WriteLine(cmd2.CommandText);
                            foreach (SqlParameter p in cmd2.Parameters)
                                Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));

                            cmd2.ExecuteNonQuery();
                        }

                        tx.Commit();
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Actualizar(Componente componente)
        {
            if (componente == null) throw new ArgumentNullException(nameof(componente));

            string sql = @"UPDATE Componentes
                           SET tipo = @Tipo,
                               marca = @Marca,
                               modelo = @Modelo,
                               caracteristicas = @Caracteristicas,
                               numero_serie = @NumeroSerie,
                               estado_id = @Estado,
                               fecha_compra = @Fecha
                           WHERE id_componente = @Id";

            using (var con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
            using (var cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Tipo", componente.Tipo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Marca", componente.Marca ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Modelo", componente.Modelo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Caracteristicas", componente.Caracteristicas ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroSerie", componente.Numero_Serie ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Estado", componente.Estado_Id);
                cmd.Parameters.AddWithValue("@Fecha", componente.Fecha_Compra);
                cmd.Parameters.AddWithValue("@Id", componente.Id_Componente);

                con.Open();
                Debug.WriteLine("---- Update Componente ----");
                Debug.WriteLine(cmd.CommandText);
                foreach (SqlParameter p in cmd.Parameters)
                    Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));

                cmd.ExecuteNonQuery();
            }
        }

        public void VincularConComputadora(int idComputadora, int idComponente, String fecha)
        {
            if (idComputadora <= 0) throw new ArgumentException("idComputadora inválido");
            if (idComponente <= 0) throw new ArgumentException("idComponente inválido");

            string query = "INSERT INTO Computadora_Componentes (id_computadora, id_componente, fecha_asignacion) " +
                           "VALUES (@idCompu, @idComponente, @fecha)";

            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idCompu", idComputadora);
                cmd.Parameters.AddWithValue("@idComponente", idComponente);
                cmd.Parameters.AddWithValue("@fecha", fecha ?? (object)DBNull.Value);

                con.Open();
                Debug.WriteLine("---- VincularConComputadora ----");
                Debug.WriteLine(cmd.CommandText);
                foreach (SqlParameter p in cmd.Parameters)
                {
                    Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));
                }
                cmd.ExecuteNonQuery();
            }
        }

        // Método para verificar si ya existe un Numero_Serie
        public bool ExisteNumeroSerie(string numeroSerie)
        {
            if (string.IsNullOrWhiteSpace(numeroSerie))
                return false;

            string query = "SELECT COUNT(*) FROM Componentes WHERE numero_serie = @NumeroSerie";
            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@NumeroSerie", numeroSerie);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }
    }
}