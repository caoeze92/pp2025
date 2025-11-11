using System;
using System.Data;
using System.Data.SqlClient;

public static class HistorialManager
{
    public static void RegistrarEvento(int tipoEvento,int codEntidad, string entidad, string usuario, string detalle)
    {
        try
        {
            using (SqlConnection conexion = ConnectionFactory.GetConnection())
            {
                conexion.Open();

                using (SqlCommand cmd = new SqlCommand(@"
                    INSERT INTO Historial (tipo_evento, entidad, usuario, fecha_solicitud, detalle, codEntidad)
                    VALUES (@tipo_evento, @entidad, @usuario, @fecha_solicitud, @detalle, @codEntidad)", conexion))
                {
                    cmd.Parameters.AddWithValue("@tipo_evento", tipoEvento);
                    cmd.Parameters.AddWithValue("@codEntidad", codEntidad);
                    cmd.Parameters.AddWithValue("@entidad", entidad);
                    cmd.Parameters.AddWithValue("@usuario", usuario);
                    cmd.Parameters.AddWithValue("@fecha_solicitud", DateTime.Now);
                    cmd.Parameters.AddWithValue("@detalle", detalle);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            
            throw new Exception("Error al registrar evento en Historial", ex);
        }
    }
}