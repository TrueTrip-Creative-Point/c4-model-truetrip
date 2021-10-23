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
            Container promotionsContext =       plannerSystem.AddContainer("Promotions Context", "Bounded Context del Microservicio de promociones existentes", "NodeJS (NestJS)");
            Container partnerDetailsContext =   plannerSystem.AddContainer("Partner Details Context", "Bounded Context del Microservicio de información de los partners", "NodeJS (NestJS)");
            Container database =                plannerSystem.AddContainer("Database", "", "MySQL");
            Container travelerContext =         plannerSystem.AddContainer("Traveler Context", "Bounded Context del microservicio de información del traveler", "NodeJS (NestJS)");
            Container authenticationContext =   plannerSystem.AddContainer("Authentication Context", "Bounded Context del microservicio de autenticación para traveler y partner", "NodeJS (NestJS)");
            
            empresario.Uses(mobileApplication, "Consulta");
            empresario.Uses(webApplication, "Consulta");
            empresario.Uses(landingPage, "Consulta");
            viajero.Uses(mobileApplication, "Consulta");
            viajero.Uses(webApplication, "Consulta");
            viajero.Uses(landingPage, "Consulta");                        

            mobileApplication.Uses(tripPlanContext,         "Request", "JSON/HTTPS");
            mobileApplication.Uses(travelerContext,         "Request", "JSON/HTTPS");
            mobileApplication.Uses(authenticationContext,   "Request", "JSON/HTTPS");
            mobileApplication.Uses(promotionsContext,       "Request", "JSON/HTTPS");
            mobileApplication.Uses(partnerDetailsContext,   "Request", "JSON/HTTPS");
            webApplication.Uses(tripPlanContext,            "Request", "JSON/HTTPS");
            webApplication.Uses(authenticationContext,      "Request", "JSON/HTTPS");
            webApplication.Uses(travelerContext,            "Request", "JSON/HTTPS");
            webApplication.Uses(promotionsContext,          "Request", "JSON/HTTPS");
            webApplication.Uses(partnerDetailsContext,      "Request", "JSON/HTTPS");      
            
            tripPlanContext.Uses(database, "", "JDBC");
            promotionsContext.Uses(database, "", "JDBC");
            partnerDetailsContext.Uses(database, "", "JDBC");
            authenticationContext.Uses(database, "", "JDBC");
            travelerContext.Uses(database, "", "JDBC");
                        
            tripPlanContext.Uses(googleMaps, "API Request", "JSON/HTTPS");          

            // Tags
            mobileApplication.AddTags("MobileApp");
            webApplication.AddTags("WebApp");
            landingPage.AddTags("LandingPage");
            database.AddTags("Database");
            tripPlanContext.AddTags("BoundedContext");            
            promotionsContext.AddTags("BoundedContext");            
            partnerDetailsContext.AddTags("BoundedContext");            
            travelerContext.AddTags("BoundedContext");            
            authenticationContext.AddTags("BoundedContext");            

            styles.Add(new ElementStyle("MobileApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.MobileDevicePortrait, Icon = "" });
            styles.Add(new ElementStyle("WebApp") { Background = "#9d33d6", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });
            styles.Add(new ElementStyle("LandingPage") { Background = "#929000", Color = "#ffffff", Shape = Shape.WebBrowser, Icon = "" });            
            styles.Add(new ElementStyle("Database") { Shape = Shape.Cylinder, Background = "#ff0000", Color = "#ffffff", Icon = "" });
            styles.Add(new ElementStyle("BoundedContext") { Shape = Shape.Hexagon, Background = "#facc2e", Icon = "" });            

            ContainerView containerView = viewSet.CreateContainerView(plannerSystem, "Contenedor", "Diagrama de contenedores");
            contextView.PaperSize = PaperSize.A4_Landscape;
            containerView.AddAllElements();            
            
            // 3. Diagrama de Componentes -> Promotions
            Component domainLayerPromotion = promotionsContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component promotionsController = promotionsContext.AddComponent("Promotion Controller", "", "NodeJS (NestJS)");
            Component promotionsApplicationService = promotionsContext.AddComponent("Promotion Application Service", "", "NodeJS (NestJS)");
            Component promotionsRepository = promotionsContext.AddComponent("Promotion Repository", "", "NodeJS (NestJS)");

            mobileApplication.Uses(promotionsController,"JSON");
            webApplication.Uses(promotionsController,"JSON");
            promotionsController.Uses(promotionsApplicationService,"Usa");
            promotionsApplicationService.Uses(promotionsRepository,"Usa");
            promotionsApplicationService.Uses(domainLayerPromotion,"");
            promotionsRepository.Uses(database,"","JDBC");
            //tags
            domainLayerPromotion.AddTags("Component");
            promotionsController.AddTags("Component");
            promotionsApplicationService.AddTags("Component");
            promotionsRepository.AddTags("Component");

            //style
            styles.Add(new ElementStyle("Component") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView promotionComponentView = viewSet.CreateComponentView(promotionsContext, "Promotion Components", "Component Diagram");
            promotionComponentView.PaperSize = PaperSize.A4_Landscape;
            promotionComponentView.Add(mobileApplication);   
            promotionComponentView.Add(webApplication);
            promotionComponentView.Add(database);
            promotionComponentView.AddAllComponents();
            
            // 3. Diagrama de Componentes -> Traveler
            Component domainLayerTraveler =         travelerContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component travelerController =          travelerContext.AddComponent("Traveler Controller", "REST Api endpoints de travelers", "NodeJS (NestJS)");
            Component travelerApplicationService =  travelerContext.AddComponent("Traveler Application Service", "Provee metodos para los datos de traveler", "NodeJS (NestJS)");
            Component travelerRepository =          travelerContext.AddComponent("Traveler Repository", "Informacion de traveler", "NodeJS (NestJS)");
            Component friendsRepository =           travelerContext.AddComponent("Friends Repository", "Informacion de los friends del traveler", "NodeJS (NestJS)");

            mobileApplication.Uses(travelerController,"JSON");
            webApplication.Uses(travelerController,"JSON");
            travelerController.Uses(travelerApplicationService,"Usa");
            travelerApplicationService.Uses(friendsRepository,"Usa");
            travelerApplicationService.Uses(travelerRepository,"Usa");
            travelerApplicationService.Uses(domainLayerTraveler,"Usa");
            friendsRepository.Uses(database,"","JDBC");
            travelerRepository.Uses(database,"","JDBC");
            
            //tags
            domainLayerTraveler.AddTags("Component");
            travelerRepository.AddTags("Component");
            travelerController.AddTags("Component");
            travelerApplicationService.AddTags("Component");
            friendsRepository.AddTags("Component");

            //style
            //styles.Add(new ElementStyle("Component") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView travelerComponentView = viewSet.CreateComponentView(travelerContext, "Traveler Components", "Component Diagram");
            travelerComponentView.PaperSize = PaperSize.A4_Landscape;
            travelerComponentView.Add(mobileApplication);   
            travelerComponentView.Add(webApplication);
            travelerComponentView.Add(database);
            travelerComponentView.AddAllComponents();


            // 3. Diagrama de Componentes -> Trip Plan
            Component domainLayerTripPlan =             tripPlanContext.AddComponent("Domain Layer", "", "NodeJS (NestJS)");
            Component tripPlanController =              tripPlanContext.AddComponent("Trip Plan Controller", "REST Api endpoints de plan de viaje", "NodeJS (NestJS)");
            Component agencyApplicationService =        tripPlanContext.AddComponent("Agency Application Service", "Provee metodos para la agencia de viaje", "NodeJS (NestJS)");
            Component destinationApplicationService =   tripPlanContext.AddComponent("Destination Application Service", "Provee metodos para el destino de viaje", "NodeJS (NestJS)");
            Component activityApplicationService =      tripPlanContext.AddComponent("Activity Application Service", "Provee metodos para la gestion de actividades", "NodeJS (NestJS)");
            Component activityRepository =              tripPlanContext.AddComponent("Activity Repository", "Informacion de actividades", "NodeJS (NestJS)");
            Component destinationRepository =           tripPlanContext.AddComponent("Destination Repository", "Informacion de la ubicacion de destino", "NodeJS (NestJS)");
            Component agencyRepository =                tripPlanContext.AddComponent("Agency Repository", "Informacion de agencia", "NodeJS (NestJS)");

            mobileApplication.Uses(tripPlanController,"JSON");
            webApplication.Uses(tripPlanController,"JSON");
            tripPlanController.Uses(agencyApplicationService,"Usa");
            tripPlanController.Uses(destinationApplicationService,"Usa");
            tripPlanController.Uses(activityApplicationService,"Usa");
            agencyApplicationService.Uses(domainLayerTripPlan,"Usa");
            destinationApplicationService.Uses(domainLayerTripPlan,"Usa");
            activityApplicationService.Uses(domainLayerTripPlan,"Usa");
            agencyApplicationService.Uses(agencyRepository,"Usa");
            destinationApplicationService.Uses(destinationRepository,"Usa");
            activityApplicationService.Uses(activityRepository,"Usa");
            activityRepository.Uses(database,"","JDBC");
            agencyRepository.Uses(database,"","JDBC");
            destinationRepository.Uses(database,"","JDBC");
            destinationRepository.Uses(googleMaps,"","JDBC");
            
            //tags
            domainLayerTripPlan.AddTags("Component");
            tripPlanController.AddTags("Component");
            destinationRepository.AddTags("Component");
            destinationApplicationService.AddTags("Component");
            agencyRepository.AddTags("Component");
            agencyApplicationService.AddTags("Component");
            activityRepository.AddTags("Component");
            activityApplicationService.AddTags("Component");
            

            //style
            //styles.Add(new ElementStyle("Component") { Shape = Shape.Component, Background = "#facc2e", Icon = "" });

            ComponentView tripPlanComponentView = viewSet.CreateComponentView(tripPlanContext, "Trip Plan Components", "Component Diagram");
            tripPlanComponentView.PaperSize = PaperSize.A4_Landscape;
            tripPlanComponentView.Add(mobileApplication);   
            tripPlanComponentView.Add(webApplication);
            tripPlanComponentView.Add(googleMaps);
            tripPlanComponentView.Add(database);
            tripPlanComponentView.AddAllComponents();

            structurizrClient.UnlockWorkspace(workspaceId);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}