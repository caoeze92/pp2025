using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

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
                        // estado_actual en la BD parece ser FK / entero: enviar DBNull si no viene válido
                        int estadoInt;
                        object estadoParam = (!string.IsNullOrWhiteSpace(computadora.EstadoActual) && int.TryParse(computadora.EstadoActual, out estadoInt))
                            ? (object)estadoInt
                            : (object)DBNull.Value;
                        cmd.Parameters.AddWithValue("@EstadoActual", estadoParam);
                        cmd.Parameters.AddWithValue("@FechaAlta", computadora.FechaAlta);

                        Debug.WriteLine("---- Insert Computadora ----");
                        Debug.WriteLine(cmd.CommandText);
                        foreach (SqlParameter p in cmd.Parameters)
                        {
                            Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));
                        }

                        computadora.IdComputadora = (int)cmd.ExecuteScalar();
                    }

                    // 2️⃣ Insertar los componentes y obtener sus IDs usando la MISMA conexión/transaction
                    List<int> idsComponentes = new List<int>();
                    var repoComponente = new ComponenteRepository();

                    foreach (var comp in componentes)
                    {
                        int idComponente = repoComponente.Insert(comp, con, transaction); // ahora dentro de la misma transacción
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

                            Debug.WriteLine("---- Insert Computadora_Componentes ----");
                            Debug.WriteLine(cmdRelacion.CommandText);
                            foreach (SqlParameter p in cmdRelacion.Parameters)
                            {
                                Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));
                            }

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

        // Opcional: método para verificar si ya existe un Numero_serie
        public bool ExisteNumeroSerie(string numeroSerie)
        {
            string query = "SELECT COUNT(*) FROM Computadoras WHERE numero_serie = @NumeroSerie";
            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@NumeroSerie", numeroSerie);
                con.Open();
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        internal Computadora ObtenerPorId(int id)
        {
            if (id <= 0) return null;

            string sql = @"
                SELECT id_computadora, id_laboratorio, codigo_inventario, numero_serie, descripcion, estado_actual, fecha_alta, fecha_baja
                FROM Computadoras
                WHERE id_computadora = @Id";

            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();

                Debug.WriteLine("---- Select Computadora por Id ----");
                Debug.WriteLine(cmd.CommandText);
                foreach (SqlParameter p in cmd.Parameters)
                    Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));

                using (var rdr = cmd.ExecuteReader())
                {
                    if (!rdr.Read()) return null;

                    var compu = new Computadora
                    {
                        IdComputadora = rdr["id_computadora"] != DBNull.Value ? Convert.ToInt32(rdr["id_computadora"]) : 0,
                        IdLaboratorio = rdr["id_laboratorio"] != DBNull.Value ? Convert.ToInt32(rdr["id_laboratorio"]) : 0,
                        CodigoInventario = rdr["codigo_inventario"] != DBNull.Value ? rdr["codigo_inventario"].ToString() : null,
                        NumeroSerie = rdr["numero_serie"] != DBNull.Value ? rdr["numero_serie"].ToString() : null,
                        Descripcion = rdr["descripcion"] != DBNull.Value ? rdr["descripcion"].ToString() : null,
                        EstadoActual = rdr["estado_actual"] != DBNull.Value ? rdr["estado_actual"].ToString() : null,
                        FechaAlta = rdr["fecha_alta"] != DBNull.Value ? Convert.ToDateTime(rdr["fecha_alta"]) : DateTime.MinValue,
                        FechaBaja = rdr["fecha_baja"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(rdr["fecha_baja"]) : null
                    };

                    return compu;
                }
            }
        }

        internal void Actualizar(Computadora computadora)
        {
            if (computadora == null) throw new ArgumentNullException(nameof(computadora));
            if (computadora.IdComputadora <= 0) throw new ArgumentException("IdComputadora inválido", nameof(computadora.IdComputadora));

            string sql = @"
                UPDATE Computadoras
                SET codigo_inventario = @CodigoInventario,
                    numero_serie = @NumeroSerie,
                    descripcion = @Descripcion,
                    estado_actual = ISNULL(@EstadoActual, estado_actual),
                    fecha_baja = @FechaBaja
                WHERE id_computadora = @Id";

            using (SqlConnection con = ConnectionFactory.GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@CodigoInventario", computadora.CodigoInventario ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@NumeroSerie", computadora.NumeroSerie ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Descripcion", computadora.Descripcion ?? (object)DBNull.Value);

                int estadoVal;
                object estadoParam = (!string.IsNullOrWhiteSpace(computadora.EstadoActual) && int.TryParse(computadora.EstadoActual, out estadoVal))
                    ? (object)estadoVal
                    : (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@EstadoActual", estadoParam);

                cmd.Parameters.AddWithValue("@FechaBaja", computadora.FechaBaja.HasValue ? (object)computadora.FechaBaja.Value : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Id", computadora.IdComputadora);

                con.Open();

                Debug.WriteLine("---- Update Computadora ----");
                Debug.WriteLine(cmd.CommandText);
                foreach (SqlParameter p in cmd.Parameters)
                    Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));

                cmd.ExecuteNonQuery();
            }
        }

        internal void EliminarComputadora(int id)
        {
            if (id <= 0) throw new ArgumentException("id inválido", nameof(id));

            using (SqlConnection con = ConnectionFactory.GetConnection())
            {
                con.Open();
                using (var tx = con.BeginTransaction())
                {
                    try
                    {
                        // Eliminar vínculos en Computadora_Componentes
                        using (var cmd = new SqlCommand("DELETE FROM Computadora_Componentes WHERE id_computadora = @id", con, tx))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            Debug.WriteLine("---- DELETE Computadora_Componentes (por computadora) ----");
                            Debug.WriteLine(cmd.CommandText);
                            foreach (SqlParameter p in cmd.Parameters)
                                Debug.WriteLine(string.Format("{0} = {1}", p.ParameterName, p.Value ?? "NULL"));

                            cmd.ExecuteNonQuery();
                        }

                        // Eliminar registro en Computadoras
                        using (var cmd2 = new SqlCommand("DELETE FROM Computadoras WHERE id_computadora = @id", con, tx))
                        {
                            cmd2.Parameters.AddWithValue("@id", id);
                            Debug.WriteLine("---- DELETE Computadoras ----");
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
    }
}