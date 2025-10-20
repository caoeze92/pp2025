using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace PracticaProfesional2025
{
    public class ComputadoraRepository
    {
        public int InsertarComputadoraConComponentes(Computadora computadora, List<Componente> componentes)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1️⃣ Insertar la computadora
                    string sqlComputadora = @"
                    INSERT INTO Computadoras (id_laboratorio, codigo_inventario, numero_serie, descripcion, estado_actual, fecha_alta)
                    OUTPUT INSERTED.id_computadora
                    VALUES (@Laboratorio, @CodigoInventario, @NumeroSerie, @Descripcion, @EstadoActual, @FechaAlta)";

                    using (SqlCommand cmd = new SqlCommand(sqlComputadora, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Laboratorio", computadora.IdLaboratorio);
                        cmd.Parameters.AddWithValue("@CodigoInventario", computadora.CodigoInventario ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NumeroSerie", computadora.NumeroSerie ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Descripcion", computadora.Descripcion ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@EstadoActual", computadora.EstadoActual);
                        cmd.Parameters.AddWithValue("@FechaAlta", computadora.FechaAlta);

                        computadora.IdComputadora = (int)cmd.ExecuteScalar();
                    }

                    // 2️⃣ Insertar los componentes y obtener sus IDs
                    List<int> idsComponentes = new List<int>();
                    var repoComponente = new ComponenteRepository();

                    foreach (var comp in componentes)
                    {
                        int idComponente = repoComponente.Insert(comp); // pasar conexión y transacción
                        idsComponentes.Add(idComponente);
                    }

                    // 3️⃣ Insertar las relaciones computadora-componente
                    string sqlRelacion = @"
                    INSERT INTO Computadora_Componentes (id_computadora, id_componente, fecha_asignacion)
                    VALUES (@IdComputadora, @IdComponente, @FechaAsignacion)";

                    foreach (var idComp in idsComponentes)
                    {
                        using (SqlCommand cmdRelacion = new SqlCommand(sqlRelacion, con, transaction))
                        {
                            cmdRelacion.Parameters.AddWithValue("@IdComputadora", computadora.IdComputadora);
                            cmdRelacion.Parameters.AddWithValue("@IdComponente", idComp);
                            cmdRelacion.Parameters.AddWithValue("@FechaAsignacion", DateTime.Now);

                            cmdRelacion.ExecuteNonQuery();
                        }
                    }

                    // 4️⃣ Confirmar la transacción
                    transaction.Commit();
                    return computadora.IdComputadora;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }


        }


        // Opcional: método para verificar si ya existe un Numero_Serie
        public bool ExisteNumeroSerie(string numeroSerie)
        {
            string query = "SELECT COUNT(*) FROM Computadoras WHERE Numero_Serie = @NumeroSerie";
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
