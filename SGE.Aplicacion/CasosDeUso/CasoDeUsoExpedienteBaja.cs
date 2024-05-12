namespace SGE.Aplicacion;

public class CasoDeUsoExpedienteBaja(IServicioAutorizacion servicioAutorizacion,IExpedienteRepositorio expedienteRepositorio,ITramiteRepositorio tramiteRepositorio)
{
     public void Ejecutar(int idExpediente, int idUsuario)
    {    
           // Verificar permisos
           if(servicioAutorizacion.PoseeElPermiso(idUsuario, Permiso.ExpedienteBaja)){
               // Eliminar expediente en el repositorio
               bool ok;
               expedienteRepositorio.Eliminar(idExpediente,out ok);
               if(ok){
                  //Al dar de baja el expediente, también doy de baja todos los trámites asociados
                  tramiteRepositorio.EliminarTramitesPorIdExpediente(idExpediente);
               }
               else{
                 throw new RepositorioException("El expediente que quiere dar de baja no existe ");
               }      
           }
           else{
               throw new AutorizacionException();
           }
    }
}
