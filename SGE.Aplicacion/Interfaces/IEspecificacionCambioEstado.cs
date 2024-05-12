namespace SGE.Aplicacion;

public interface IEspecificacionCambioEstado
{
  EstadoExpediente ObtenerNuevoEstado(EtiquetaTramite etiquetaTramite, EstadoExpediente estadoActual);
}
