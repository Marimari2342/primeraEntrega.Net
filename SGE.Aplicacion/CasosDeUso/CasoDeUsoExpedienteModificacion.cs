namespace SGE.Aplicacion;

public class CasoDeUsoExpedienteModificacion(IExpedienteRepositorio expedienteRepositorio,IServicioAutorizacion _servicioAutorizacion)
{
    public void Ejecutar(Expediente expediente,int idUsuario)
    {
            if(_servicioAutorizacion.PoseeElPermiso(idUsuario, Permiso.ExpedienteModificacion)){
                expediente.IdUsuarioUltimaModificacion=idUsuario;
                // Asignar fecha de modificación
                expediente.UltimaModificacion = DateTime.Now;
                Expediente aux = expedienteRepositorio.ObtenerPorId(expediente.Id);
                expediente.Estado = aux.Estado;
                bool ok;
                expedienteRepositorio.Modificar(expediente,out ok);
                if(!ok){
                    throw new RepositorioException("El expediente con el id ingresado no existe");
                }
            }   
            else{
                throw new AutorizacionException();
            }
    }
}
