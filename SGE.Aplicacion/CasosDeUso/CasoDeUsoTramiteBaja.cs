namespace SGE.Aplicacion;

public class CasoDeUsoTramiteBaja(ITramiteRepositorio _tramiteRepositorio, IServicioAutorizacion _servicioAutorizacion,ServicioActualizacionEstado actualizar)
{
        public void Ejecutar(Tramite tramite, int idUsuario){
                // Verificar permisos
                if(_servicioAutorizacion.PoseeElPermiso(idUsuario, Permiso.TramiteBaja)){
                    tramite.IdUsuarioUltimaModificacion = idUsuario;
                    bool ok;
                    Tramite aux = _tramiteRepositorio.ObtenerPorId(tramite.Id);
                    _tramiteRepositorio.Eliminar(tramite.Id, out ok);
                    if(ok){
                        //Actualizar estado del expediente
                        actualizar.ActualizarEstado(aux.ExpedienteId);
                    }
                    else{
                        throw new RepositorioException("Id de trámite no encontrado");
                    }

                }
                else{
                    throw new AutorizacionException();
                } 
        }

}
