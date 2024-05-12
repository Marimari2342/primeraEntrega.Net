namespace SGE.Aplicacion;

public class ServicioActualizacionEstado (IEspecificacionCambioEstado _especificacion, ITramiteRepositorio _tramiteRepositorio, IExpedienteRepositorio _expedienteRepositorio)
{
    public void ActualizarEstado(int id)
    {

        Expediente expediente = _expedienteRepositorio.ObtenerPorId(id);
        expediente.Tramites = _tramiteRepositorio.ListarPorIdExpediente(id);
        Tramite ultimoTramite = expediente.Tramites[expediente.Tramites.Count - 1];
        expediente.Estado = _especificacion.ObtenerNuevoEstado(ultimoTramite.Etiqueta, expediente.Estado);
        bool ok;
        _expedienteRepositorio.Modificar(expediente,out ok);

    }

}
