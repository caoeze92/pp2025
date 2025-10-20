using System;
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

            string query = "INSERT INTO Componentes (Tipo, Marca, Modelo, Caracteristicas, Numero_Serie, Estado_id, Fecha_compra) " +
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
                using (var cmdCheck = new SqlCommand("SELECT COUNT(*) FROM Componentes WHERE Numero_Serie = @NumeroSerie", con, transaction))
                {
                    cmdCheck.Parameters.AddWithValue("@NumeroSerie", candidate);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());
                    if (count == 0)
                        break;
                }

                candidate = string.Format("{0}_{1}", baseNumeroSerie, sufijo++);
            }

            componente.Numero_Serie = candidate;

            string query = "INSERT INTO Componentes (Tipo, Marca, Modelo, Caracteristicas, Numero_Serie, Estado_id, Fecha_compra) " +
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

        public void VincularConComputadora(int idComputadora, int idComponente, String fecha)
        {
            if (idComputadora <= 0)
                throw new ArgumentException("idComputadora inválido");
            if (idComponente <= 0)
                throw new ArgumentException("idComponente inválido");

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

            string query = "SELECT COUNT(*) FROM Componentes WHERE Numero_Serie = @NumeroSerie";
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
