namespace Kastra.Web.API.Models.ModuleDefinition
{
    public class ModuleInstallModel
    {
        public int ModuleDefinitionId { get; set; }
        public string AssemblyName { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }
}
