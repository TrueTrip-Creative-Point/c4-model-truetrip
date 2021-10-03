using Structurizr;
using Structurizr.Api;

namespace c4_model_design
{
    class Program
    {
        static void Main(string[] args)
        {
            Banking();
        }

        static void Banking()
        {
            const long workspaceId = 69923;
            const string apiKey = "a1be5603-6d3f-4928-b67c-0db362dc21c6";
            const string apiSecret = "8a472170-60a1-43b9-8990-6e96c856a00a";

            StructurizrClient structurizrClient = new StructurizrClient(apiKey, apiSecret);
            Workspace workspace = new Workspace("C4 Model - Sistema Planificador de viajes y actividades turisticas", "Sistema Planificador de viajes y actividades turisticas True Trip");
            ViewSet viewSet = workspace.Views;
            Model model = workspace.Model;

            // 1. Diagrama de Contexto
            SoftwareSystem plannerSystem = model.AddSoftwareSystem("Sistema Planificador de viajes y actividades turisticas", "Permite planificar los viajes de inicio a fin considerando el traslado hospedaje y destino turisticos.");
            SoftwareSystem googleMaps = model.AddSoftwareSystem("Google Maps", "Plataforma que ofrece una REST API de informacion georeferencial para localizar destinos");
            
            Person empresario = model.AddPerson("Empresario", "Ciudadano peruano viajero.");
            Person viajero = model.AddPerson("Viajero", "Ciudadano peruano empresarion con un negocio en el rubro del turismo");
            
            viajero.Uses(plannerSystem, "Realiza consultas para mantenerse al tanto de las activiades que puede realizar en el destino seleccionado");
            empresario.Uses(plannerSystem, "Realiza consultas para mantenerse al tanto de los viajeros que seleccionan su negocio");
            plannerSystem.Uses(googleMaps, "Usa la API de google maps");
            
            SystemContextView contextView = viewSet.CreateSystemContextView(plannerSystem, "Contexto", "Diagrama de contexto");
            contextView.PaperSize = PaperSize.A3_Landscape;
            contextView.AddAllSoftwareSystems();
            contextView.AddAllPeople();

            // Tags
            empresario.AddTags("Ciudadano");
            viajero.AddTags("Ciudadano");
            plannerSystem.AddTags("SistemaPlanificadorActividades");
            googleMaps.AddTags("GoogleMaps");
            

            Styles styles = viewSet.Configuration.Styles;
            styles.Add(new ElementStyle("Ciudadano") { Background = "#0a60ff", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("SistemaPlanificadorActividades") { Background = "#008f39", Color = "#ffffff", Shape = Shape.RoundedBox });
            styles.Add(new ElementStyle("GoogleMaps") { Background = "#90714c", Color = "#ffffff", Shape = Shape.RoundedBox });
            

            // 2. Diagrama de Contenedores
            Container mobileApplication =       plannerSystem.AddContainer("Mobile App", "Permite a los usuarios visualizar las actividades turisticas que pueden realizar cercanas a su destino.", "Flutter");
            Container webApplication =          plannerSystem.AddContainer("Web App", "Permite a los usuarios visualizar las actividades turisticas que pueden realizar cercanas a su destino.", "Vue");
            Container landingPage =             plannerSystem.AddContainer("Landing Page", "", "Bootstrap");
            Container tripPlanContext =         plannerSystem.AddContainer("Trip Plan Context", "Bounded Context del Microservicio de Planificación de viajes y hospedajes en el destino seleccionado", "NodeJS (NestJS)");
            Container activitiesContext =       plannerSystem.AddContainer("Activities Context", "Bounded Context del Microservicio de actividades para el destino turistico", "NodeJS (NestJS)");
            Container promotionsContext =       plannerSystem.AddContainer("Promotions Context", "Bounded Context del Microservicio de promociones existentes", "NodeJS (NestJS)");
            Container partnerDetailsContext =   plannerSystem.AddContainer("Partner Details Context", "Bounded Context del Microservicio de información de los partners", "NodeJS (NestJS)");
            Container database =                plannerSystem.AddContainer("Database", "", "MySQL");
            
            empresario.Uses(mobileApplication, "Consulta");
            empresario.Uses(webApplication, "Consulta");
            empresario.Uses(landingPage, "Consulta");
            viajero.Uses(mobileApplication, "Consulta");
            viajero.Uses(webApplication, "Consulta");
            viajero.Uses(landingPage, "Consulta");                        

            mobileApplication.Uses(tripPlanContext,         "Request", "JSON/HTTPS");
            mobileApplication.Uses(activitiesContext,       "Request", "JSON/HTTPS");
            mobileApplication.Uses(promotionsContext,       "Request", "JSON/HTTPS");
            mobileApplication.Uses(partnerDetailsContext,   "Request", "JSON/HTTPS");
            webApplication.Uses(tripPlanContext,            "Request", "JSON/HTTPS");
            webApplication.Uses(activitiesContext,          "Request", "JSON/HTTPS");
            webApplication.Uses(promotionsContext,          "Request", "JSON/HTTPS");
            webApplication.Uses(partnerDetailsContext,      "Request", "JSON/HTTPS");      
            
            tripPlanContext.Uses(database, "", "JDBC");
            activitiesContext.Uses(database, "", "JDBC");
            promotionsContext.Uses(database, "", "JDBC");
            partnerDetailsContext.Uses(database, "", "JDBC");
                        
            activitiesContext.Uses(googleMaps, "API Request", "JSON/HTTPS");          

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            database.AddTags("Database");
            tripPlanContext.AddTags("BoundedContext");            
            activitiesContext.AddTags("BoundedContext");            
            promotionsContext.AddTags("BoundedContext");            
            partnerDetailsContext.AddTags("BoundedContext");            

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });            
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("BoundedContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });            

            ContainerView containerView = viewSet.CreateContainerView(plannerSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();
            
            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}