using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;

namespace PracticaProfesional2025
{
    public class ComputadoraRepository
    {
        public int Insert(Computadora computadora)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConexionBD"].ConnectionString))
            {
                con.Open();
                SqlTransaction transaction = con.BeginTransaction();

                try
                {
                    // 1️⃣ Insertar la computadora y obtener el ID generado
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

                        System.Diagnostics.Debug.WriteLine("Parametros para la query de Relaciones");
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                            System.Diagnostics.Debug.WriteLine(result);
                        }

                        computadora.IdComputadora = (int)cmd.ExecuteScalar();


                    }

                    // 2️⃣ Insertar las relaciones en Computadora_Componentes
                    if (computadora.Componentes != null && computadora.Componentes.Count > 0)
                    {
                        string sqlRelacion = @"
                            INSERT INTO Computadora_Componentes (id_computadora, id_componente, fecha_asignacion)
                            VALUES (@IdComputadora, @IdComponente, @FechaAsignacion)";

                        foreach (var idComponente in computadora.Componentes)
                        {
                            using (SqlCommand cmdRelacion = new SqlCommand(sqlRelacion, con, transaction))
                            {
                                cmdRelacion.Parameters.AddWithValue("@IdComputadora", computadora.IdComputadora);
                                cmdRelacion.Parameters.AddWithValue("@IdComponente", idComponente);
                                cmdRelacion.Parameters.AddWithValue("@FechaAsignacion", DateTime.Now);


                                System.Diagnostics.Debug.WriteLine("Parametros para la query de Relaciones");
                                foreach (SqlParameter p in cmdRelacion.Parameters)
                                {
                                    string result = String.Format("{0} = {1}", p.ParameterName, p.Value);
                                    System.Diagnostics.Debug.WriteLine(result);
                                }

                                cmdRelacion.ExecuteNonQuery();

                                
                            }

                            
                        }
                        

                    }

                    // 3️⃣ Confirmar la transacción
                    transaction.Commit();
                    return computadora.IdComputadora;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
