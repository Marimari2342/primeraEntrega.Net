namespace SGE.Repositorios;

using System.Collections.Generic;

using SGE.Aplicacion;
public class ExpedienteRepositorioTXT : IExpedienteRepositorio
{
    readonly string _nombreArch = "expedientes.txt";
    readonly string listaId = "identificadores.txt";
    


    //Retorno el id del archivo que se quiere dar de alta 
    public int ObtenerSiguienteId (){  
        int id=0; 
        if (File.Exists(_nombreArch)) {  
            using var sr = new StreamReader(listaId,true);
            while(!sr.EndOfStream){  
                id = int.Parse(sr.ReadLine()?? "");
            }
            sr.Close();
        }
        id+=1;
        using var sw = new StreamWriter(listaId,true);
        sw.WriteLine(id);
        sw.Close();
        return id;
    }

    //Método usado después de haber realizado una eliminacion o una modificación
    private void GuardarCambios(List<Expediente> list){
      File.Delete(_nombreArch);
      foreach(Expediente exp in list){
        Agregar(exp);
      }
    }

    //Caso de uso expediente ALTA
    public void Agregar(Expediente e){
        using var sw = new StreamWriter(_nombreArch, true);
        sw.WriteLine(e.Id);
        sw.WriteLine(e.Caratula);
        sw.WriteLine(e.FechaCreacion);
        sw.WriteLine(e.UltimaModificacion);
        sw.WriteLine(e.IdUsuarioUltimaModificacion);
        sw.WriteLine(e.Estado);
        sw.Close();
    }

    //Caso de uso expediente BAJA
    public void Eliminar(int id, out bool ok){
        ok=false;
        List<Expediente> lista = ObtenerTodos();
        int i=0;
        while( (i<lista.Count) && (!ok) ){
            if(lista[i].Id == id){
                ok=true;
                lista.RemoveAt(i);
                GuardarCambios(lista);
            }
            i++;
        }
    }

    //Caso de uso Consulta por Id
    public Expediente ObtenerPorId(int id){
        Expediente resultado = new Expediente();
        resultado.Id = -1;
        using var sr = new StreamReader(_nombreArch,true);
        while (!sr.EndOfStream && resultado.Id == -1){
            var expediente = new Expediente();
            expediente.Id = int.Parse(sr.ReadLine() ?? "");
            if(expediente.Id == id)
            { 
              expediente.Caratula = sr.ReadLine()?? "";
              expediente.FechaCreacion=DateTime.Parse(sr.ReadLine()?? "00/00/0000");  
              expediente.UltimaModificacion= DateTime.Parse(sr.ReadLine() ?? "00/00/0000");
              expediente.IdUsuarioUltimaModificacion= int.Parse(sr.ReadLine()?? "");
              expediente.Estado=Enum.Parse<EstadoExpediente>(sr.ReadLine()?? "");
              resultado=expediente;
            }
            else{
                for(int i=0; i<5; i++){ 
                    sr.ReadLine();
                }
            }
        }
        sr.Close(); 
        return resultado;   
    }

    //Caso de uso consulta TODOS
    public List<Expediente> ObtenerTodos(){
        List<Expediente> resultado = new List<Expediente>();
        using var sr = new StreamReader(_nombreArch);
        while (!sr.EndOfStream){
            var expediente = new Expediente();
            expediente.Id = int.Parse(sr.ReadLine() ?? "");
            expediente.Caratula = sr.ReadLine() ?? "";
            expediente.FechaCreacion=DateTime.Parse(sr.ReadLine() ?? "");
            expediente.UltimaModificacion=DateTime.Parse(sr.ReadLine() ?? "");
            expediente.IdUsuarioUltimaModificacion= int.Parse(sr.ReadLine() ?? "");
            expediente.Estado=Enum.Parse<EstadoExpediente>(sr.ReadLine()?? "");
            resultado.Add(expediente);
        }
        sr.Close();
        return resultado;  
    }


    //Caso de uso expediente MODIFICACION
    public void Modificar(Expediente e,out bool ok){
        ok=false;
        List<Expediente> lista=ObtenerTodos();
        int i=0;
        while( (i<lista.Count) && !ok ){
            if(lista[i].Id == e.Id){
                ok=true;
                e.FechaCreacion = lista[i].FechaCreacion;
                lista[i]=e;
                GuardarCambios(lista);
            }
            i++;
        }
    }
}
