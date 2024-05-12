namespace SGE.Repositorios;
using SGE.Aplicacion;
public class TramiteRepositorioTXT : ITramiteRepositorio
{
    readonly string _nombreArch="tramites.txt";
    readonly string _nombreArchId = "identificadoresT.txt";

    //Retorno el id del archivo que se quiere dar de alta 
    public int ObtenerSiguienteId(){
        int id=0;
        if (File.Exists(_nombreArchId)){  
            using var sr = new StreamReader(_nombreArchId,true);
            while(!sr.EndOfStream){  
                id = int.Parse(sr.ReadLine()?? "");
            }
            sr.Close();
        }
        id+=1;
        using var sw = new StreamWriter(_nombreArchId,true);
        sw.WriteLine(id);
        sw.Close();
        return id;
    
    }

    //Método usado después de haber realizado una eliminacion o una modificación
    private void GuardarCambios(List<Tramite> list){
      File.Delete(_nombreArch);
      foreach(Tramite t in list){
        Agregar(t);
      }
    }

    //Caso de uso trámite ALTA
    public void Agregar(Tramite t){
        using var sw = new StreamWriter(_nombreArch, true);
        sw.WriteLine(t.Id);
        sw.WriteLine(t.ExpedienteId);
        sw.WriteLine(t.Etiqueta);
        sw.WriteLine(t.Contenido);
        sw.WriteLine(t.FechaCreacion);
        sw.WriteLine(t.UltimaModificacion);
        sw.WriteLine(t.IdUsuarioUltimaModificacion);
        sw.Close();
        
    }

    //Caso de uso trámite BAJA
    public void Eliminar(int id, out bool ok){
        ok=false;
        List<Tramite> lista=ListarTramites();
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
    //Caso de uso trámite MODIFICACIÓN
    public void Modificar(Tramite t, out bool ok){
        ok=false;
        List<Tramite> lista=ListarTramites();
        int i=0;
        while( (i<lista.Count) && !ok ){
            if(lista[i].Id == t.Id){
                ok=true;
                t.ExpedienteId = lista[i].ExpedienteId;
                t.FechaCreacion = lista[i].FechaCreacion;
                lista[i]=t;
                GuardarCambios(lista);
            }
            i++;
        }

    }

    //Metodo usado por el caso de uso expediente consulta por Id
    public List <Tramite> ListarPorIdExpediente(int id){
        var resultado = new List<Tramite>();
        using var sr = new StreamReader(_nombreArch);
        while (!sr.EndOfStream){
            var tramite = new Tramite();
            tramite.Id = int.Parse(sr.ReadLine() ?? "");
            tramite.ExpedienteId =  int.Parse(sr.ReadLine() ?? "");
            if(tramite.ExpedienteId == id){ 
              tramite.Etiqueta = Enum.Parse<EtiquetaTramite>(sr.ReadLine() ?? "");
              tramite.Contenido= sr.ReadLine();
              tramite.FechaCreacion=DateTime.Parse(sr.ReadLine() ?? "");
              tramite.UltimaModificacion= DateTime.Parse(sr.ReadLine()?? "");
              tramite.IdUsuarioUltimaModificacion= int.Parse(sr.ReadLine() ?? "");
              resultado.Add(tramite);
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


    public List<Tramite> ListarPorEtiqueta(EtiquetaTramite etiqueta){
        var resultado = new List<Tramite>();
        using var sr = new StreamReader(_nombreArch, true);
        while (!sr.EndOfStream){
            var tramite = new Tramite();
            tramite.Id = int.Parse(sr.ReadLine() ?? "");
            tramite.ExpedienteId =  int.Parse(sr.ReadLine() ?? "");
            tramite.Etiqueta = Enum.Parse<EtiquetaTramite>(sr.ReadLine() ?? "");
            if(tramite.Etiqueta == etiqueta){ 
              tramite.Contenido= sr.ReadLine();
              tramite.FechaCreacion=DateTime.Parse(sr.ReadLine() ?? "");
              tramite.UltimaModificacion= DateTime.Parse(sr.ReadLine()?? "");
              tramite.IdUsuarioUltimaModificacion= int.Parse(sr.ReadLine() ?? "");
              resultado.Add(tramite);
            }
            else{
                for(int i=0; i<4; i++){ 
                    sr.ReadLine();
                }
            }
        }
        sr.Close();
        return resultado; 

    }

    private List<Tramite> ListarTramites(){
        List<Tramite> resultado = new List<Tramite>();
        using var sr = new StreamReader(_nombreArch);
        while (!sr.EndOfStream){
            var tramite = new Tramite();
            tramite.Id = int.Parse(sr.ReadLine() ?? "");
            tramite.ExpedienteId = int.Parse(sr.ReadLine() ?? "");
            tramite.Etiqueta = Enum.Parse<EtiquetaTramite>(sr.ReadLine()?? "");
            tramite.Contenido=(sr.ReadLine()?? "");
            tramite.FechaCreacion=DateTime.Parse(sr.ReadLine() ?? "");
            tramite.UltimaModificacion=DateTime.Parse(sr.ReadLine() ?? "");
            tramite.IdUsuarioUltimaModificacion= int.Parse(sr.ReadLine() ?? "");
            resultado.Add(tramite);
        }
        sr.Close();
        return resultado;
    }

    public void EliminarTramitesPorIdExpediente(int idExpediente){
         List<Tramite> lista = ListarTramites();
         for(int i=0; i< lista.Count;i++){
            if(lista[i].ExpedienteId == idExpediente){
                lista.RemoveAt(i);
            }
         }
         GuardarCambios(lista);
    }

    public Tramite ObtenerPorId(int id){
        Tramite resultado = new Tramite();
        resultado.Id = -1;
        using var sr = new StreamReader(_nombreArch,true);
        while ((!sr.EndOfStream) && (resultado.Id == -1)){
            var tramite = new Tramite();
            tramite.Id = int.Parse(sr.ReadLine() ?? "");
            if(tramite.Id == id)
            { 
              tramite.ExpedienteId = int.Parse(sr.ReadLine() ?? "");
              tramite.Etiqueta=Enum.Parse<EtiquetaTramite>(sr.ReadLine()?? "");
              tramite.Contenido= sr.ReadLine()?? "";
              tramite.FechaCreacion=DateTime.Parse(sr.ReadLine()?? "00/00/0000");  
              tramite.UltimaModificacion= DateTime.Parse(sr.ReadLine() ?? "00/00/0000");
              tramite.IdUsuarioUltimaModificacion= int.Parse(sr.ReadLine()?? "");
              resultado=tramite;
            }
            else{
                for(int i=0; i<6; i++){ 
                    sr.ReadLine();
                }
            }
        }
        sr.Close(); 
        return resultado;  
    }
}
