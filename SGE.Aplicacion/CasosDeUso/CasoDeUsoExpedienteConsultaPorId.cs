namespace SGE.Aplicacion;

public class CasoDeUsoExpedienteConsultaPorId(IExpedienteRepositorio _expedientes, ITramiteRepositorio _tramite)
{
    public Expediente Ejecutar(int id){
          Expediente ex =_expedientes.ObtenerPorId(id);
          if(ex.Id != -1){
             ex.Tramites = _tramite.ListarPorIdExpediente(id);
             return ex;
          }
          else{
            throw new RepositorioException("El expediente con el id ingresado no existe ");
          }
    }


}
