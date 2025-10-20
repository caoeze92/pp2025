using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;

namespace PracticaProfesional2025
{
    public class ComponenteRepository
    {
        public int Insert(Componente componente)
        {
            if (componente == null)
                throw new ArgumentNullException("componente");

            int idComponente = 0;

            // CORRECCIÓN: falta la coma entre @Caracteristicas y @NumeroSerie
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

                System.Diagnostics.Debug.WriteLine("Parametros para la query de Insert Componente");
                foreach (SqlParameter p in cmd.Parameters)
                {
                    string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                    System.Diagnostics.Debug.WriteLine(result);
                }

                object resultado = cmd.ExecuteScalar();
                if (resultado != null)
                    idComponente = Convert.ToInt32(resultado);
            }

            return idComponente;
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
                System.Diagnostics.Debug.WriteLine("Parametros para la query de VincularConComputadora");
                foreach (SqlParameter p in cmd.Parameters)
                {
                    string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                    System.Diagnostics.Debug.WriteLine(result);
                }
                cmd.ExecuteNonQuery();
            }
        }

        // Opcional: método para verificar si ya existe un Numero_Serie
        public bool ExisteNumeroSerie(string numeroSerie)
        {
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
