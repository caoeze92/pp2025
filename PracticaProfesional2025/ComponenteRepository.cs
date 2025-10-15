using System;
using System.Data.SqlClient;
using System.Configuration;



using System.Collections.Generic;namespace PracticaProfesional2025
{
    public class ComponenteRepository
    {


        public int Insert(Componente componente)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString))
            {
                string sql = @"
                    INSERT INTO Componentes (tipo, marca, modelo, numero_serie, estado_id, fecha_compra)
                    OUTPUT INSERTED.id_componente
                    VALUES (@tipo, @marca, @modelo, @numero_serie, @estado_id, @fecha_compra)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@tipo", componente.Tipo);
                    cmd.Parameters.AddWithValue("@marca", (object)componente.Marca ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@modelo", (object)componente.Modelo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@numero_serie", (object)componente.Numero_Serie ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@estado_id", 1); // Ej: estado "Activo"
                    cmd.Parameters.AddWithValue("@fecha_compra", DateTime.Now);

                    con.Open();

                    System.Diagnostics.Debug.WriteLine("Parametros para la query de Componente");
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                        System.Diagnostics.Debug.WriteLine(result);
                    }

                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void VincularConComputadora(int idComputadora, int idComponente)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString))
            {
                string sql = @"
                    INSERT INTO Computadora_Componentes (id_computadora, id_componente, fecha_asignacion)
                    VALUES (@idComputadora, @idComponente, @fechaAsignacion)";

                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idComputadora", idComputadora);
                    cmd.Parameters.AddWithValue("@idComponente", idComponente);
                    cmd.Parameters.AddWithValue("@fechaAsignacion", DateTime.Now);

                    con.Open();

                    System.Diagnostics.Debug.WriteLine("Parametros para la query de Relacion con Computadora");
                    foreach (SqlParameter p in cmd.Parameters)
                    {
                        string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                        System.Diagnostics.Debug.WriteLine(result);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
