namespace SGE.Aplicacion;

public interface IExpedienteRepositorio
{
    void Agregar(Expediente expediente);
    void Eliminar(int id, out bool ok);
    void Modificar(Expediente expediente, out bool ok);
    Expediente ObtenerPorId(int id);
    List <Expediente> ObtenerTodos();
    int ObtenerSiguienteId();

}
