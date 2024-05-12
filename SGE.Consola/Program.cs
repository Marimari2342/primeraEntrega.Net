using SGE.Aplicacion;
using SGE.Repositorios;

//Menú
bool fin=false; 
string? option;
while( !fin ){ 
  Console.WriteLine("MENÚ:");
  Console.WriteLine("Ingrese 1 si quiere dar de ALTA un expediente.");
  Console.WriteLine("Ingrese 2 si quiere dar de BAJA un expediente.");
  Console.WriteLine("Ingrese 3 si quiere consultar un expediente por su ID junto con todos sus trámites.");
  Console.WriteLine("Ingrese 4 si quiere consultar todos los expedientes.");
  Console.WriteLine("Ingrese 5 si quiere MODIFICAR un expediente.");
  Console.WriteLine("Ingrese 6 si quiere dar de ALTA un trámite.");
  Console.WriteLine("Ingrese 7 si quiere dar de BAJA un trámite.");
  Console.WriteLine("Ingrese 8 si quiere consultar todos los trámites con una etiqueta específica.");
  Console.WriteLine("Ingrese 9 si quiere MODIFICAR un trámite.");
  Console.WriteLine("Ingrese 10 para cerrar el menú");
  Console.Write("Opción: "); option = Console.ReadLine();
  Console.Clear();
  switch (option)
  {
    case "1":
      AltaExpediente();
      break;
    case ("2"):
      BajaExpediente();
      break;
    case "3":
      ConsultaPorID();
      break;
    case "4":
      ConsultarTodosLosExpedientes();
      break;
    case "5":
      ModificarExpediente();
      break;
    case "6":
      AltaTramite();
      break;
    case "7":
      BajaTramite();
      break;
    case "8":
      ConsultaTramitesPorEtiqueta();
      break;
    case "9":
      ModificarTramite();
      break;
    case "10":
      fin=true;
      break;
    default:
      Console.WriteLine("Opción ingresada inválida.");
      break;
  }
}

void AltaExpediente(){
  try{ 

    Expediente e=new Expediente();
    Console.WriteLine("Ingrese los siguientes datos para poder dar de alta al expediente: ");
    Console.Write("Ingrese su id de usuario: "); int idUsuario=int.Parse(Console.ReadLine()?? "");
    Console.Write("Carátula del expediente: "); e.Caratula = Console.ReadLine();

    IExpedienteRepositorio expRepo= new ExpedienteRepositorioTXT();
    IServicioAutorizacion autoProvisoria= new ServicioAutorizacionProvisorio(); 

    var casoAlta= new CasoDeUsoExpedienteAlta(expRepo, autoProvisoria);
    casoAlta.Ejecutar(e,idUsuario);
    Console.WriteLine("EL expediente fue agregado correctamente.");
  }
  catch(Exception msj){
    Console.WriteLine(msj.Message);
  }
}

void BajaExpediente(){
  try{  

  Console.WriteLine("Ingrese los siguientes datos para poder dar de baja al expediente: ");
  Console.Write("Ingrese su id de usuario: "); int idUsuario=int.Parse(Console.ReadLine() ?? "");
  Console.Write("Ingrese id del expediente que desea dar de baja: "); int idExpediente= int.Parse(Console.ReadLine()??"");

  IExpedienteRepositorio expRepo= new ExpedienteRepositorioTXT();
  ITramiteRepositorio traRepo= new TramiteRepositorioTXT();
  IServicioAutorizacion autoProvisoria= new ServicioAutorizacionProvisorio(); 
  var casoBaja= new CasoDeUsoExpedienteBaja(autoProvisoria,expRepo,traRepo);

  casoBaja.Ejecutar(idExpediente,idUsuario);
  Console.WriteLine($"El expediente con id {idExpediente} fue eliminado.");
  }
  catch(Exception e){
    Console.WriteLine(e.Message);
  }
}

void ConsultaPorID(){
  try{

    Console.Write("Ingrese el id del expediente que quiere consultar: "); int id= int.Parse(Console.ReadLine()?? "");
    
    IExpedienteRepositorio expedientes = new ExpedienteRepositorioTXT(); 
    ITramiteRepositorio tramites = new TramiteRepositorioTXT();
    var casoConsultaId = new CasoDeUsoExpedienteConsultaPorId(expedientes,tramites);
    Expediente ex = casoConsultaId.Ejecutar(id);
    
    Console.WriteLine(ex);
    foreach(Tramite t in ex.Tramites){
      Console.WriteLine(t);
    }

  }
  catch (Exception e){
    Console.WriteLine(e.Message);
  }
}

void ConsultarTodosLosExpedientes(){
  IExpedienteRepositorio expedientes = new ExpedienteRepositorioTXT(); 
  var casoConsultaTodos = new CasoDeUsoExpedienteConsultaTodos(expedientes);
  List<Expediente>lista = casoConsultaTodos.Ejecutar();
  foreach(Expediente e in lista){
    Console.WriteLine(e);
  }
}

void ModificarExpediente () {
  try{  
    Console.WriteLine("Ingrese los siguientes datos para poder modificar el expediente: ");
    Console.Write("Ingrese su id de usuario: "); int idUsuario=int.Parse(Console.ReadLine()?? "");
    Expediente expediente = new Expediente();
    Console.Write("Ingrese el id del expediente que desea modificar: "); expediente.Id = int.Parse(Console.ReadLine()?? "");
    Console.Write("Ingrese la nueva caratula: "); expediente.Caratula=Console.ReadLine();
    
    IExpedienteRepositorio expedienteRepo= new ExpedienteRepositorioTXT();
    IServicioAutorizacion autorizacionProvisoria= new ServicioAutorizacionProvisorio(); 
    var casoModificar= new CasoDeUsoExpedienteModificacion(expedienteRepo, autorizacionProvisoria);

    casoModificar.Ejecutar(expediente,idUsuario);
    Console.WriteLine($"El expediente con id {expediente.Id} fue modificado.");
  }
  catch(Exception e){
    Console.WriteLine(e.Message);
  }
}

void AltaTramite(){
  try{
    Tramite t=new Tramite();
    Console.WriteLine("Ingrese los siguientes datos para poder dar de alta al trámite: ");
    Console.Write("Ingrese su id de usuario: "); int idUsuario=int.Parse(Console.ReadLine()?? "");
    Console.Write("Ingrese el id del expediente al que pertenece: "); t.ExpedienteId = int.Parse(Console.ReadLine()?? "");
    Console.WriteLine("Ingrese el contenido del trámite: "); t.Contenido = Console.ReadLine();
    Console.WriteLine("Ingrese la etiqueta del trámite(EscritoPresentado,PaseAEstudio,Despacho,Resolucion,Notificacion,PaseAlArchivo): ");
    t.Etiqueta = Enum.Parse<EtiquetaTramite>(Console.ReadLine() ?? "");

    ITramiteRepositorio tramiteRepo= new TramiteRepositorioTXT();
    IExpedienteRepositorio expedienteRepo = new ExpedienteRepositorioTXT();
    IEspecificacionCambioEstado especificacion = new EspecificacionCambioEstado();
    ServicioActualizacionEstado servicioAE = new ServicioActualizacionEstado(especificacion,tramiteRepo,expedienteRepo);
    IServicioAutorizacion autorizacionProvisoria= new ServicioAutorizacionProvisorio(); 

    var casoAlta= new CasoDeUsoTramiteAlta(tramiteRepo, autorizacionProvisoria, servicioAE);
    casoAlta.Ejecutar(t,idUsuario);
    Console.WriteLine("El trámite fue agregado correctamente.");

  }
  catch (ValidacionException msj){
    Console.WriteLine(msj.Message);
  }
  catch(Exception msj){
    Console.WriteLine(msj.Message);
  }
}

void BajaTramite() {
  try{
    Tramite t=new Tramite();
    Console.WriteLine("Ingrese los siguientes datos para poder dar de baja un trámite: ");
    Console.Write("Ingrese su id de usuario: "); int idUsuario= int.Parse(Console.ReadLine()?? "");
    Console.Write("Ingrese el id del tramite que desea dar de baja: "); t.ExpedienteId = int.Parse(Console.ReadLine()?? "");

    ITramiteRepositorio tramiteRepo= new TramiteRepositorioTXT();
    IExpedienteRepositorio expedienteRepo = new ExpedienteRepositorioTXT();
    IEspecificacionCambioEstado especificacion = new EspecificacionCambioEstado();
    ServicioActualizacionEstado servicioAE = new ServicioActualizacionEstado(especificacion,tramiteRepo,expedienteRepo);
    IServicioAutorizacion autorizacionProvisoria= new ServicioAutorizacionProvisorio(); 
    var casoBaja= new CasoDeUsoTramiteBaja(tramiteRepo, autorizacionProvisoria,servicioAE);

    casoBaja.Ejecutar(t,idUsuario);
    Console.WriteLine($"Se eliminó el tramite con id {t.ExpedienteId}.");

  }
  catch (ValidacionException msj){
    Console.WriteLine(msj.Message);
  }
  catch(Exception msj){
    Console.WriteLine(msj.Message);
  }
}

void ConsultaTramitesPorEtiqueta(){
 try{
    Console.WriteLine("Ingrese etiqueta: ");
    EtiquetaTramite etiqueta = Enum.Parse<EtiquetaTramite>(Console.ReadLine() ?? "");
    ITramiteRepositorio tramiteRepo = new TramiteRepositorioTXT();
    var casoConsultarEtiqueta = new CasoDeUsoTramiteConsultaPorEtiqueta(tramiteRepo);
    List<Tramite> L= casoConsultarEtiqueta.Ejecutar(etiqueta);
    foreach(Tramite t in L){
      Console.WriteLine(t);
    }
 }
 catch (Exception e){
   Console.WriteLine(e.Message);
 }

}

void ModificarTramite(){
  try{
    Tramite t=new Tramite();
    Console.WriteLine("Ingrese los siguientes datos para poder modificar un trámite: ");
    Console.Write("Ingrese su id de usuario: "); string idUsuario=Console.ReadLine()?? "";
    Console.Write("Ingrese el id del tramite que desea modificar: "); t.Id = int.Parse(Console.ReadLine()?? "");
   
    Console.Write("Nueva etiqueta: "); t.Etiqueta = Enum.Parse<EtiquetaTramite>(Console.ReadLine() ?? "");
    Console.Write("Nuevo contenido: "); t.Contenido = Console.ReadLine();
    int idu=int.Parse(idUsuario);

    ITramiteRepositorio tramiteRepo= new TramiteRepositorioTXT();
    IExpedienteRepositorio expedienteRepo = new ExpedienteRepositorioTXT();
    IEspecificacionCambioEstado especificacion = new EspecificacionCambioEstado();
    ServicioActualizacionEstado servicioAE = new ServicioActualizacionEstado(especificacion,tramiteRepo,expedienteRepo);
    IServicioAutorizacion autorizacionProvisoria= new ServicioAutorizacionProvisorio(); 
    var casoModificar= new CasoDeUsoTramiteModificacion(tramiteRepo,autorizacionProvisoria,servicioAE);

    casoModificar.Ejecutar(t,idu);
    Console.WriteLine($"Se modificó el tramite con id {t.Id}.");
  }

  catch (ValidacionException msj){
    Console.WriteLine(msj.Message);
  }

  catch(Exception msj){
    Console.WriteLine(msj.Message);
  }  
}
