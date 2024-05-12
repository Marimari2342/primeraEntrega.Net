namespace SGE.Aplicacion;

public class CasoDeUsoTramiteModificacion (ITramiteRepositorio _tramiteRepositorio, IServicioAutorizacion servicioAutorizacion,ServicioActualizacionEstado actualizar)
{
     public void Ejecutar(Tramite tramite, int idUsuario)
    {
        
            // Verificar permisos
            if(servicioAutorizacion.PoseeElPermiso(idUsuario, Permiso.TramiteModificacion)){
                tramite.IdUsuarioUltimaModificacion = idUsuario;
                // Validar tramite
                string mensajeError;
                if(!TramiteValidador.Validar(tramite,out mensajeError)){
                    throw new ValidacionException(mensajeError);
                }
                else{

                    // Asignar fecha de modificación
                    tramite.UltimaModificacion = DateTime.Now;
                    // Guardar tramite en el repositorio
                    bool ok;
                    _tramiteRepositorio.Modificar(tramite, out ok);
                    if (ok){
                        Tramite aux = _tramiteRepositorio.ObtenerPorId(tramite.Id); 
                        //Actualizar estado del expediente
                        actualizar.ActualizarEstado(aux.ExpedienteId);
                    }
                    else{
                        throw new RepositorioException("El trámite con el id ingresado no existe.");
                    }
                    

                }
            }
             else{
                throw new AutorizacionException();
             }
    }

}
