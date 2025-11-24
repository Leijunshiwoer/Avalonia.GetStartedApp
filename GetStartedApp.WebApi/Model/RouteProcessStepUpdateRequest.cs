using System.Collections.Generic;

namespace GetStartedApp.WebApi.Model
{
    public class RouteProcessStepUpdateRequest
    {
        public int RouteId { get; set; }

        public List<int> ProcessStepIds { get; set; } = new();
    }
}

