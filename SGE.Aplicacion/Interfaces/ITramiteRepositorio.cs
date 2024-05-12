namespace SGE.Aplicacion;

public interface ITramiteRepositorio
{
    void Agregar(Tramite tramite);
    void Eliminar(int id, out bool ok);
    void Modificar(Tramite tramite, out bool ok);
    List<Tramite> ListarPorIdExpediente(int id);
    List<Tramite> ListarPorEtiqueta(EtiquetaTramite etiqueta);
    int ObtenerSiguienteId();
    void EliminarTramitesPorIdExpediente(int idExpediente);
    Tramite ObtenerPorId(int id);

}
